
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

            // Make the game. TEMPORARY, NEEDS CONTROLLER
            MineSweeper game = new MineSweeper(20, 20);

            // Make the view's grid of buttons
            MakeButtonGrid(20, 20, 30);

            //and add it to the view
            VerticalStackLayout lay = (VerticalStackLayout)FindByName("grid1");
            lay.Add(grid);

        }

        /// <summary>
        /// Creates the interactive button grid that forms the main view of and method of with the game field.
        /// </summary>
        /// <param name="width">int, >0 (work on better constraints/recommended values later)</param>
        /// <param name="height">int, >0 (work on better constraints/recommended values later)</param>
        /// <param name="buttonSize">int, >0 (work on better constraints/recommended values later)</param>
        /// <param name="game"> TEMPORARY, should link to MAUI adapter/controller not direct to model class</param>
        private void MakeButtonGrid(int width, int height, int buttonSize)
        {
            grid = new Grid();

            for (int i = 0; i < width; i++)
            {
                grid.AddColumnDefinition(new ColumnDefinition { Width = new GridLength(buttonSize) });
            }

            for (int i = 0; i < height; i++)
            {
                grid.AddRowDefinition(new RowDefinition { Height = new GridLength(buttonSize) });
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    GridButton btn = new GridButton(j * width + i);
                    btn.Text = "?";
                    btn.IsEnabled = true;
                    grid.Add(btn, i, j);
                }
            }
        }
    }

    /// <summary>
    /// The buttons that make up the interactive cell grid in the view. 
    /// </summary>
    class GridButton : Button
    {
        int cellNum;

        public GridButton(int cellnum)
        {
            //Sets the default style for GridButton objects.
            // !!! Figure out how to do with styles.xaml !!! 
            VerticalOptions = LayoutOptions.Fill;
            HorizontalOptions = LayoutOptions.Fill;
            BorderColor = Colors.Black;
            CornerRadius = 0;
            Clicked += new System.EventHandler(GridButtonClicked);
            this.cellNum = cellnum;
        }

        private void GridButtonClicked(object sender, EventArgs e)
        {
            // code goes here
        }
    };
}