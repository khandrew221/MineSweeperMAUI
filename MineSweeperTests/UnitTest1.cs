using MineSweeper;

namespace MineSweeperTests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestGameCreation()
        {
            // Test valid size values

            /** code to test every value combo in a range
            int maxWidth = 50;
            int maxHeight = 50;

            for (int x = 0; x < maxWidth; x++)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    TestSizeValues(x, y);
                }
            }**/

            //min values
            TestSizeValues(1, 1);
            TestSizeValues(2, 2);

            //mid values
            TestSizeValues(25, 25);

            //high values
            TestSizeValues(50, 50);

            //min + mid values
            TestSizeValues(25, 1);
            TestSizeValues(1, 25);

            //min + high values
            TestSizeValues(50, 1);
            TestSizeValues(1, 50);

            //high + mid values
            TestSizeValues(25, 50);
            TestSizeValues(50, 25);

            TestCellStateReporting(10, 15);

        }

        /// <summary>
        /// Includes testing for 
        /// MineSweeperGame.NumberOfCells()
        /// MineSweeperGame.Height()
        /// MineSweeperGame.Width()
        /// </summary>
        public void TestSizeValues(int width, int height)
        {
            //extremely basic test to check size of the grid is correct for given values
            MineSweeperGame game = new MineSweeperGame(width, height, 0, 1);

            Assert.AreEqual(width * height, game.NumberOfCells());
            Assert.AreEqual(height, game.Height());
            Assert.AreEqual(width, game.Width());
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
            MineSweeperGame game = new MineSweeperGame(5, 5, 0, 1);

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

        /// <summary>
        /// DENSITY_MIN assumed to be >=0 and <= DENSITY_MAX - 0.1
        /// Tested to 0.0000001 precision; more precision starts to clash with float precision and produce unexpected logic values
        /// Values DENSITY_MAX + (<)0.00000003f accepted, DENSITY_MAX + 0.0000003f fails to be set as expected
        /// Includes testing for 
        /// MineSweeperGame.SetBombDensity()
        /// MineSweeperGame.BombDensity()
        /// </summary>
        [TestMethod]
        public void TestBombDensitySetting()
        {
            /// tests on constant values to make sure they are still within bounds 0 to 1 and logically consistent
            Assert.IsTrue(MineSweeperGame.DENSITY_MIN >= 0);
            Assert.IsTrue(MineSweeperGame.DENSITY_MAX <= 1);
            Assert.IsTrue(MineSweeperGame.DENSITY_MIN <= MineSweeperGame.DENSITY_MAX);
            Assert.IsTrue(MineSweeperGame.DENSITY_DEFAULT >= MineSweeperGame.DENSITY_MIN);
            Assert.IsTrue(MineSweeperGame.DENSITY_DEFAULT <= MineSweeperGame.DENSITY_MAX);

            /// set up game and test density value is default
            MineSweeperGame game = new MineSweeperGame(10, 10, MineSweeperGame.DENSITY_DEFAULT, 1);
            Assert.IsTrue(game.BombDensity() == MineSweeperGame.DENSITY_DEFAULT);

            ///test setting density values (DENSITY_MIN to DENSITY_MAX inclusive expected)
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MIN, true, game);
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MIN - 0.0000001f, false, game);
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MIN + 0.0000001f, true, game);
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MIN - 0.1f, false, game);
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MIN + 0.1f, true, game);

            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MAX, true, game);
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MAX - 0.0000001f, true, game);
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MAX + 0.0000001f, false, game);
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MAX - 0.1f, true, game);
            BombDensitySettingSubTest(MineSweeperGame.DENSITY_MAX + 0.1f, false, game);

            BombDensitySettingSubTest(MineSweeperGame.DENSITY_DEFAULT, true, game);

            BombDensitySettingSubTest(-1, false, game);

        }

        public void BombDensitySettingSubTest(float TestValue, bool SuccessExpected, MineSweeperGame game)
        {
            float StartingDensity = game.BombDensity();

            //checks that the attempt to set the test value produces the expected true/false return
            Assert.AreEqual(game.SetBombDensity(TestValue), SuccessExpected);

            //checks the the correct change to the value has taken place
            if (SuccessExpected)
            {
                Assert.IsTrue(game.BombDensity() == TestValue);
            } else
            {
                Assert.IsTrue(game.BombDensity() == StartingDensity);
            }

            //checks that on a new game, the correct number of bombs are placed for the test value
            game.NewGame();
            int expectedBombs = (int)Math.Floor(game.Height() * game.Width() * game.BombDensity());
            Assert.AreEqual(expectedBombs, game.AllBombs().Count);
        }


        public void TestCellStateReporting(int width, int height)
        {
            MineSweeperGame game = new MineSweeperGame(width, height, 0.2f, 1);
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
            MineSweeperGame game = new MineSweeperGame(10, 10, 0.2f, 1);
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
            MineSweeperGame game = new MineSweeperGame(20, 7, 0.2f, 1);
            game.NewGame();

            for (int i = 0;  i > game.NumberOfCells(); i++) 
            {
                int t = game.CellState(0);
                Assert.IsTrue(t == MineSweeperGame.HIDDEN);
            }
        }


    }

}