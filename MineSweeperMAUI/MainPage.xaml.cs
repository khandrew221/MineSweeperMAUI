using MineSweeper;
using System.Xml;

namespace MineSweeperMAUI
{
    public partial class MainPage : ContentPage
    {
        const int DefaultXSize = 10;
        const int DefaultYSize = 10;
        const int DefaultBombDensity = 20;

        //Variables required by the view
        Grid grid = new Grid();

        public MainPage()
        {
            InitializeComponent();

            //initialise setting sliders. Has to be done here since xaml Value does not work
            Slider s = (Slider)FindByName("XSlider");
            s.Value = DefaultXSize;
            s = (Slider)FindByName("YSlider");
            s.Value = DefaultYSize;
            s = (Slider)FindByName("BombDensity");
            s.Value = DefaultBombDensity;

            // Make the controller. This encapsulates interface with the game code.
            MAUIController controller = new MAUIController();

            //sets up the initial game. Bomb density must be converted from % to decimal
            controller.BeginGame(DefaultXSize, DefaultYSize, DefaultBombDensity/100.0f);

            // Make the view's grid of buttons
            MakeButtonGrid(controller, 50);

            //and add it to the view
            VerticalStackLayout lay = (VerticalStackLayout)FindByName("grid1");
            lay.Add(grid);

        }

        /// <summary>
        /// Controls slider code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSliderValueChanged(object sender, EventArgs e)
        {
            Slider s = (Slider)sender;
            String id = s.StyleId;

            switch (id)
            {
                case "BombDensity":
                    Label l = (Label)FindByName("BombDensityValue");
                    l.Text = String.Format("{0}%", (int)s.Value);
                    break;
                case "XSlider":
                    l = (Label)FindByName("XSliderValue");
                    l.Text = String.Format("{0}", (int)s.Value);
                    break;
                case "YSlider":
                    l = (Label)FindByName("YSliderValue");
                    l.Text = String.Format("{0}", (int)s.Value);
                    break;
            }

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