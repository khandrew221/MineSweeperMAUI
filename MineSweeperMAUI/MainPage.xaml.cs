using MineSweeper;

namespace MineSweeperMAUI
{
    public partial class MainPage : ContentPage
    {
        //Variables required by the view
        int count = 0;
        Grid grid = new Grid();

        public MainPage()
        {
            InitializeComponent();

            // Make the controller. This encapsulates interface with the game code.
            MAUIController controller = new MAUIController();

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
                    GridButton btn = new GridButton(x, y);
                    btn.Text = controller.CellState(x, y).ToString();
                    btn.IsEnabled = true;
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
        int x;
        /// <summary>
        /// Y position of the button in the button grid
        /// </summary>
        int y;

        public GridButton(int x, int y)
        {
            //Sets the default style for GridButton objects.
            // !!! Figure out how to do with styles.xaml !!! 
            VerticalOptions = LayoutOptions.Fill;
            HorizontalOptions = LayoutOptions.Fill;
            BorderColor = Colors.Black;
            CornerRadius = 0;
            Clicked += new System.EventHandler(GridButtonClicked);
            this.x = x;
            this.y = y;
        }

        private void GridButtonClicked(object sender, EventArgs e)
        {
            // code goes here
        }
    };
}