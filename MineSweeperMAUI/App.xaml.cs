using MineSweeper;

namespace MineSweeperMAUI
{
    public partial class App : Application
    {

        //interface level defaults can be set without messing with model level defaults
        const int DefaultXSize = 10;
        const int DefaultYSize = 10;
        const int DefaultBombDensity = 20;
        const int DefaultLives = 3;

        //global settings object for the model
        public MineSweeperGame.Settings settings = new MineSweeperGame.Settings(DefaultXSize, DefaultYSize, DefaultBombDensity / 100f, DefaultLives);
        //This encapsulates interface with the game code. 
        public MAUIController controller = new MAUIController();      

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

        }

        /// <summary>
        /// Begins a new game with current settings
        /// </summary>
        public void NewGame()
        {
            controller.BeginGame(settings);
        }

    }
}