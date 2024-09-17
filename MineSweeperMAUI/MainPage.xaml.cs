using MineSweeper;
using System.Xml;

namespace MineSweeperMAUI
{
    public partial class MainPage : ContentPage
    {

        int CellSize = 50;
        public MAUIController controller = ((App)Application.Current).controller;

        //Variables required by the view
        Grid grid = new Grid();
        VerticalStackLayout gridLayout;

        public MainPage()
        {
            InitializeComponent();

            //store the location of the grid so it can be manipulated by code later
            gridLayout = (VerticalStackLayout)FindByName("grid1");

            //sets up the initial game. Bomb density must be converted from % to decimal
            //MineSweeperGame.Settings settings = new MineSweeperGame.Settings(DefaultXSize, DefaultYSize, DefaultBombDensity / 100.0f, DefaultLives);
            ((App)Application.Current).NewGame();
            UpdateGameSummaryText();

            // Make the view's grid of buttons
            MakeButtonGrid(controller, CellSize);

            //and add it to the view
            gridLayout.Add(grid);
        }

        /// <summary>
        /// Updates the game summary label text
        /// </summary>
        public void UpdateGameSummaryText()
        {
            Label l = (Label)FindByName("GameSummary");
            l.Text = String.Format("Lives left {0}, Bombs Triggered {1}, Safe Cells Found {2}, Game State {3}", controller.LivesRemaining(), controller.BombsTriggered(), controller.SafeReveals(), controller.GameState());
        }

        /// <summary>
        /// Begins a new game on button press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGameButtonPressed(object sender, EventArgs e)
        {
            //resart the backend game component with the current settings
            ((App)Application.Current).NewGame();

            //reset the grid. Since the size may change the grid is removed, remade, and replaced in the layout. 
            gridLayout.Remove(grid);
            MakeButtonGrid(controller, CellSize);
            gridLayout.Add(grid);

            UpdateGameSummaryText();
        }


        /// <summary>
        /// Creates the interactive button grid that forms the main view of and method of with the game field.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="buttonSize">int, >0 (work on better constraints/recommended values later)</param>
        private void MakeButtonGrid(MAUIController controller, int buttonSize)
        {
            grid = new Grid();
            int width = controller.GameWidth();
            int height = controller.GameHeight();

            for (int i = 0; i < width; i++)
            {
                grid.AddColumnDefinition(new ColumnDefinition { Width = new GridLength(buttonSize) });
            }

            for (int i = 0; i < height; i++)
            {
                grid.AddRowDefinition(new RowDefinition { Height = new GridLength(buttonSize) });
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GridButton btn = new GridButton(x, y, this);
                    btn.SetButton();
                    grid.Add(btn, x, y);
                }
            }
        }

        /// <summary>
        /// Navigate to the settings pages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButtonPressed(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage());
        }

        /// <summary>
        /// Checks for and executes game end scenarios (win and lose) 
        /// </summary>
        public void GameEnd()
        {

            if (controller.GameState() == -1)
            {
                DisplayAlert("GAME OVER!", "", "OK");
            }
            if (controller.GameState() == 1)
            {
                DisplayAlert("YOU WIN!", "", "OK");
            }

        }
    }

    /// <summary>
    /// The buttons that make up the interactive cell grid in the view. 
    /// </summary>
    class GridButton : Button
    {
        /// <summary>
        /// Text colours for the numbers
        /// </summary>
        private static List<Color> NumberColors = new List<Color>
        { new Color(50,50,50), //dark grey 0
          new Color(0,0,100), //dark blue 1
          new Color(0,0,200), //bright blue 2
          new Color(0,150,150), //dark cyan 3
          new Color(0,200,0), //green 4
          new Color(100,150,0), //yellow-green 5
          new Color(150,150,0), //bright blue 6
          new Color(255,100,0), //orange 7
          new Color(150,0,0) //dull red 8
        }; 
        /// <summary>
        /// X position of the button in the button grid
        /// </summary>
        public int x { get; private set; }
        /// <summary>
        /// Y position of the button in the button grid
        /// </summary>
        public int y { get; private set; }
        /// <summary>
        /// Link to the host page. Includes game controller.
        /// </summary>
        public MainPage MainPage { get; private set; }

        public GridButton(int x, int y, MainPage page)
        {
            //Sets the default style for GridButton objects.
            // style found in resource/styles/styles.xaml
            // style should handle basic enabled/disabled appearance changes 
            if (App.Current.Resources.TryGetValue("GridButtonStyle", out object style))
                Style = (Style)style;

            Clicked += new System.EventHandler(GridButtonClicked);
            this.x = x;
            this.y = y;
            this.MainPage = page;
        }

        private void GridButtonClicked(object sender, EventArgs e)
        {
            //reveal cell in the game object
            MainPage.controller.RevealCell(x, y);

            //reset all grid squares, since multiple cells may be effected
            Grid g = this.Parent as Grid;
            foreach (GridButton btn in g)
            {
                btn.SetButton();
            }

            MainPage.UpdateGameSummaryText();

            //if game state is inactive after the reveal, run game end code
            if (MainPage.controller.GameState() != 0)
            {
                MainPage.GameEnd();
            }
        }

        /// <summary>
        /// Sets the properties of the cell based on its model state
        /// </summary>
        public void SetButton()
        {
            int state = MainPage.controller.CellState(x, y);
            this.Text = CellText(state);

            if (state == MAUIController.BOMB)
                BackgroundColor = new Color(255, 0, 0);

            if (state >= 0 && state < NumberColors.Count)
                TextColor = NumberColors[state];

            //disable button for revealed cells
            IsEnabled = state == MAUIController.HIDDEN;
        }

        /// <summary>
        /// Translates the model cell state to display cell text 
        /// </summary>
        private String CellText(int s)
        {
            switch(s) {
                case MAUIController.BOMB:
                    return "!";
                case MAUIController.OOB:
                    return "ERR";
                case MAUIController.HIDDEN:
                    return "";
                case 0:
                    return "";
                default:
                    return s.ToString();    
            }
        }

    };
}