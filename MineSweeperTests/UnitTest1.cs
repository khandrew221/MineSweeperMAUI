using MineSweeper;
using static MineSweeper.MineSweeperGame;

namespace MineSweeperTests
{
    [TestClass]
    public class UnitTest1
    {

        /// <summary>
        /// Tests that Settings structs can be accurately verified with VerifySettings(), and that instances of MineSweeperGame can be 
        /// correctly created (constructor) or restarted [with NewGame(settings)] with valid settings or throw an exception on invalid ones. 
        /// </summary>
        [TestMethod]
        public void TestVerifyAndApplySettings()
        {
            //minimum values testing
            Settings settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MIN, 1);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            //maximum values testing 
            settings = new Settings(WIDTH_MAX, HEIGHT_MAX, DENSITY_MAX, 100000);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            //Width values testing
            settings = new Settings(WIDTH_MIN + 1, HEIGHT_MIN, DENSITY_MIN, 1);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MIN + 2, HEIGHT_MIN, DENSITY_MIN, 1);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MIN + 10, HEIGHT_MIN, DENSITY_MIN, 1);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MIN - 1, HEIGHT_MIN, DENSITY_MIN, 1);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MIN - 2, HEIGHT_MIN, DENSITY_MIN, 1);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MIN - 10, HEIGHT_MIN, DENSITY_MIN, 1);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            //Height values testing
            settings = new Settings(WIDTH_MAX, HEIGHT_MAX + 1, DENSITY_MAX, 100000);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX + 2, DENSITY_MAX, 100000);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX + 10, DENSITY_MAX, 100000);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX - 1, DENSITY_MAX, 100000);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX - 2, DENSITY_MAX, 100000);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX - 10, DENSITY_MAX, 100000);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            //Bomb Density values testing
            // Tested to 0.0000001 precision; more precision starts to clash with float precision and produce unexpected logic values
            // Values DENSITY_MAX + (<)0.00000003f accepted, DENSITY_MAX + 0.0000003f fails to be set as expected
            settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MIN - 0.0000001f, 1);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MIN + 0.0000001f, 1);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MIN - 0.1f, 1);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MIN + 0.1f, 1);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MAX + 0.0000001f, 1);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MAX - 0.0000001f, 1);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MAX + 0.1f, 1);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_MAX - 0.1f, 1);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);


            //Maximium lives value testing
            settings = new Settings(WIDTH_MAX, HEIGHT_MAX, DENSITY_MAX, 2);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX, DENSITY_MAX, 10);
            TestVerifySettings(settings, true);
            TestApplySettings(settings, true);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX, DENSITY_MAX, 0);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX, DENSITY_MAX, -1);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX, DENSITY_MAX, -2);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);

            settings = new Settings(WIDTH_MAX, HEIGHT_MAX, DENSITY_MAX, -100000);
            TestVerifySettings(settings, false);
            TestApplySettings(settings, false);
        }

        public void TestVerifySettings(Settings settings, bool ExpectedResult)
        {
            Assert.IsTrue(VerifySettings(settings) == ExpectedResult);
        }

        public void TestApplySettings(Settings settings, bool ExpectedResult)
        {

            try
            {
                //test on a new instance
                MineSweeperGame game = new(settings);

                //Exception should be thrown for invalid settings
                Assert.IsTrue(ExpectedResult);

                //test application of grid size
                Assert.AreEqual(settings.Width * settings.Height, game.NumberOfCells());
                Assert.AreEqual(settings.Height, game.Height());
                Assert.AreEqual(settings.Width, game.Width());

                //checks that the correct number of bombs are placed for the settings
                int expectedBombs = (int)Math.Floor(settings.Height * settings.Width * settings.BombDensity);
                Assert.AreEqual(expectedBombs, game.AllBombs().Count);

                //check application of maximum lives settings
                Assert.AreEqual(settings.MaxLives, game.MaxLives);

                //test applying new settings on a new game on an instance begun with other settings 
                game = new(new Settings(WIDTH_MIN, HEIGHT_MIN, DENSITY_DEFAULT, 1));
                game.NewGame(settings);

                //Exception should be thrown for invalid settings
                Assert.IsTrue(ExpectedResult);

                //test application of grid size
                Assert.AreEqual(settings.Width * settings.Height, game.NumberOfCells());
                Assert.AreEqual(settings.Height, game.Height());
                Assert.AreEqual(settings.Width, game.Width());

                //checks that the correct number of bombs are placed for the settings
                Assert.AreEqual(expectedBombs, game.AllBombs().Count);

                //check application of maximum lives settings
                Assert.AreEqual(settings.MaxLives, game.MaxLives);

            }
            catch (ArgumentException e)
            {
                //ArgumentException should be thrown for invalid settings
                Assert.IsFalse(ExpectedResult);
            }

        }

        /// <summary>
        /// !!! old, rework
        /// </summary>
        [TestMethod]
        public void TestGameCreation()
        {

            TestCellStateReporting(10, 15);

        }

        /// <summary>
        /// Includes testing for 
        /// MineSweeperGame.NeighboursOf()
        /// CellGrid.GenerateNeighboursSet()
        /// </summary>
        [TestMethod]
        public void TestNeighbourAssignment()
        {
            //tests nieghbour assignment on a 5x5 grid (minimum size to cover all possible neighbour types is 3x3).
            MineSweeperGame game = new MineSweeperGame(new Settings(5, 5, DENSITY_DEFAULT, 1));

            //Corner tests
            HashSet<int> values = game.NeighboursOf(0, 0);
            HashSet<int> expectedValues = new HashSet<int>();
            expectedValues.Add(1);
            expectedValues.Add(5);
            expectedValues.Add(6);
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(0, 4);
            expectedValues = new HashSet<int>();
            expectedValues.Add(15);
            expectedValues.Add(16);
            expectedValues.Add(21);
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(4, 0);
            expectedValues = new HashSet<int>();
            expectedValues.Add(3);
            expectedValues.Add(8);
            expectedValues.Add(9);
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(4, 4);
            expectedValues = new HashSet<int>();
            expectedValues.Add(18);
            expectedValues.Add(19);
            expectedValues.Add(23);
            Assert.IsTrue(values.SetEquals(expectedValues));

            //Central test
            values = game.NeighboursOf(2, 2);
            expectedValues = new HashSet<int>();
            expectedValues.Add(6);
            expectedValues.Add(7);
            expectedValues.Add(8);
            expectedValues.Add(11);
            expectedValues.Add(13);
            expectedValues.Add(16);
            expectedValues.Add(17);
            expectedValues.Add(18);
            Assert.IsTrue(values.SetEquals(expectedValues));

            //Edge tests
            values = game.NeighboursOf(3, 0);
            expectedValues = new HashSet<int>();
            expectedValues.Add(2);
            expectedValues.Add(4);
            expectedValues.Add(7);
            expectedValues.Add(8);
            expectedValues.Add(9);
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(0, 1);
            expectedValues = new HashSet<int>();
            expectedValues.Add(0);
            expectedValues.Add(1);
            expectedValues.Add(6);
            expectedValues.Add(10);
            expectedValues.Add(11);
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(4, 3);
            expectedValues = new HashSet<int>();
            expectedValues.Add(13);
            expectedValues.Add(14);
            expectedValues.Add(18);
            expectedValues.Add(23);
            expectedValues.Add(24);
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(4, 3);
            expectedValues = new HashSet<int>();
            expectedValues.Add(13);
            expectedValues.Add(14);
            expectedValues.Add(18);
            expectedValues.Add(23);
            expectedValues.Add(24);
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(1, 4);
            expectedValues = new HashSet<int>();
            expectedValues.Add(15);
            expectedValues.Add(16);
            expectedValues.Add(17);
            expectedValues.Add(20);
            expectedValues.Add(22);
            Assert.IsTrue(values.SetEquals(expectedValues));

            //out of bounds tests
            values = game.NeighboursOf(-1, 0);
            Assert.IsTrue(values.Count == 0);
            values = game.NeighboursOf(-1, -1);
            Assert.IsTrue(values.Count == 0);
            values = game.NeighboursOf(0, -1);
            Assert.IsTrue(values.Count == 0);
            values = game.NeighboursOf(5, 0);
            Assert.IsTrue(values.Count == 0);
            values = game.NeighboursOf(5, 5);
            Assert.IsTrue(values.Count == 0);
            values = game.NeighboursOf(0, 5);
            Assert.IsTrue(values.Count == 0);
            values = game.NeighboursOf(100, -40);
            Assert.IsTrue(values.Count == 0);
        }

        public void TestCellStateReporting(int width, int height)
        {
            MineSweeperGame game = new MineSweeperGame(new Settings(width, height, DENSITY_DEFAULT, 1));
            game.NewGame();

            //test OOB index values
            Assert.AreNotEqual(game.CellState(0), MineSweeperGame.OOB);
            Assert.AreNotEqual(game.CellState(width * height - 1), MineSweeperGame.OOB);
            Assert.AreEqual(game.CellState(width * height), MineSweeperGame.OOB);
            Assert.AreEqual(game.CellState(width * height * 2), MineSweeperGame.OOB);
            Assert.AreEqual(game.CellState(-1), MineSweeperGame.OOB);
            Assert.AreEqual(game.CellState(-100), MineSweeperGame.OOB);

            //initial setup; all cells should be hidden
            for (int i = 0; i < width * height; i++)
            {
                Assert.AreEqual(game.CellState(i), MineSweeperGame.HIDDEN);
            }

            //test reveal random cell on fresh board 20 times
            for (int i = 0; i < 20; i++)
            {
                game.NewGame();
                Random r = new Random();
                int rX = r.Next(0, width);
                int rY = r.Next(0, height);
                game.RevealCell(rX, rY);
                Assert.AreNotEqual(game.CellState(game.XYToIndex(rX, rY)), MineSweeperGame.HIDDEN);
                Assert.AreNotEqual(game.CellState(game.XYToIndex(rX, rY)), MineSweeperGame.OOB);
            }
        }

        [TestMethod]
        public void TestAllBombsMethod()
        {
            MineSweeperGame game = new MineSweeperGame(new Settings(10, 10, DENSITY_DEFAULT, 1));
            game.NewGame();

            HashSet<int> allBombs = game.AllBombs();

            //assert that the expected number of bombs were found
            Assert.AreEqual(allBombs.Count, (int)(game.NumberOfCells() * game.BombDensity()));

            //assert that all values in allBombs are valid cell indices
            foreach (int n in allBombs)
            {
                bool inGrid = (n >= 0 && n < game.NumberOfCells());
                Assert.IsTrue(inGrid);
            }

            //assert that all the returned indices are bombs, and none of the other cells are bombs
            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    game.RevealCell(x, y);
                    int i = game.XYToIndex(x, y);
                    bool isBomb = game.CellState(i) == MineSweeperGame.BOMB;
                    if (allBombs.Contains(i))
                    {
                        Assert.IsTrue(isBomb);
                    }
                    else
                    {
                        Assert.IsFalse(isBomb);
                    }
                }
            }
        }

        [TestMethod]
        public void TestCellStateInitialHidden()
        {
            MineSweeperGame game = new MineSweeperGame(new Settings(20, 7, DENSITY_DEFAULT, 1));
            game.NewGame();

            for (int i = 0;  i > game.NumberOfCells(); i++) 
            {
                int t = game.CellState(0);
                Assert.IsTrue(t == MineSweeperGame.HIDDEN);
            }
        }




    }

}