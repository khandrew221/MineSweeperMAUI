using MineSweeper;

namespace MineSweeperMAUI
{
    public partial class MainPage : ContentPage
    {
        //Variables required by the view
        Grid grid = new Grid();

        public MainPage()
        {
            InitializeComponent();

            // Make the controller. This encapsulates interface with the game code.
            MAUIController controller = new MAUIController();

            //sets up the initial game
            controller.BeginGame();


            // Make the view's grid of buttons
            MakeButtonGrid(controller, 50);

            //and add it to the view
            VerticalStackLayout lay = (VerticalStackLayout)FindByName("grid1");
            lay.Add(grid);

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
                    GridButton btn = new GridButton(x, y, controller);
                    btn.SetButton();
                    grid.Add(btn, x, y);
                }
            }
        }
    }

    /// <summary>
    /// The buttons that make up the interactive cell grid in the view. 
    /// </summary>
    class GridButton : Button
    {
        /// <summary>
        /// X position of the button in the button grid
        /// </summary>
        public int x { get; private set; }
        /// <summary>
        /// Y position of the button in the button grid
        /// </summary>
        public int y { get; private set; }
        /// <summary>
        /// Link to the game controller
        /// </summary>
        public MAUIController controller { get; private set; }

        public GridButton(int x, int y, MAUIController controller)
        {
            //Sets the default style for GridButton objects.
            // style found in resource/styles/styles.xaml
            // style should handle basic enabled/disabled appearance changes 
            if (App.Current.Resources.TryGetValue("GridButtonStyle", out object style))
                Style = (Style)style;

            Clicked += new System.EventHandler(GridButtonClicked);
            this.x = x;
            this.y = y;
            this.controller = controller;
        }

        private void GridButtonClicked(object sender, EventArgs e)
        {
            controller.RevealCell(x, y);

            //reset all grid squares, since multiple cells may be effected
            Grid g = this.Parent as Grid;
            foreach (GridButton btn in g)
            {
                btn.SetButton();
            }
        }

        /// <summary>
        /// Sets the properties of the cell based on its model state
        /// </summary>
        public void SetButton()
        {
            int state = controller.CellState(x, y);
            this.Text = CellText(state);
            if (state == MAUIController.BOMB)
                this.BackgroundColor = new Color(255, 0, 0);

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
                default:
                    return s.ToString();    
            }
        }

    };
}