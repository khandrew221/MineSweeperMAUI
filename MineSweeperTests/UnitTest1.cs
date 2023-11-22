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

            //
            TestNeighbourAssignment();

            TestBombDistribution(5,5);


            TestCellStateReporting(10, 15);


        }

        public void TestSizeValues(int width, int height)
        {
            //extremely basic test to check size of the grid is correct for given values
            MineSweeperGame game = new MineSweeperGame(width, height);

            Assert.AreEqual(width*height, game.NumberOfCells());
            Assert.AreEqual(height, game.Height());
            Assert.AreEqual(width, game.Width());
        }

        public void TestNeighbourAssignment()
        {
            //tests nieghbour assignment on a 5x5 grid (minimum size to cover all possible neighbour types is 3x3).
            MineSweeperGame game = new MineSweeperGame(5, 5);

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

        public void TestBombDistribution(int width, int height)
        {
            MineSweeperGame game = new MineSweeperGame(width, height);
            float defaultDensity = MineSweeperGame.DENSITY_DEFAULT;

            ///test setting density values (0 to 0.5 exclusive expected)
            Assert.IsFalse(game.SetBombDensity(0));
            Assert.IsTrue(game.BombDensity() == defaultDensity);

            Assert.IsFalse(game.SetBombDensity(0.5f));
            Assert.IsTrue(game.BombDensity() == defaultDensity);

            Assert.IsTrue(game.SetBombDensity(0.000001f));
            Assert.IsTrue(game.BombDensity() == 0.000001f);

            Assert.IsTrue(game.SetBombDensity(0.4999999f));
            Assert.IsTrue(game.BombDensity() == 0.4999999f);

            Assert.IsTrue(game.SetBombDensity(0.1f));
            Assert.IsTrue(game.BombDensity() == 0.1f);

            Assert.IsTrue(game.SetBombDensity(0.4f));
            Assert.IsTrue(game.BombDensity() == 0.4f);

            Assert.IsFalse(game.SetBombDensity(1));
            Assert.IsTrue(game.BombDensity() == 0.4f);

            Assert.IsFalse(game.SetBombDensity(-1));
            Assert.IsTrue(game.BombDensity() == 0.4f);

            float testDensity = 0.3f;

            Assert.IsTrue(game.SetBombDensity(testDensity));

            game.SetupGame();

            Assert.AreEqual((int)Math.Floor(width * height * testDensity), game.AllBombs().Count);
        }

        public void TestCellStateReporting(int width, int height)
        {
            MineSweeperGame game = new MineSweeperGame(width, height);
            game.SetupGame();

            //test OOB index values
            Assert.AreNotEqual(game.CellState(0), MineSweeperGame.OOB);
            Assert.AreNotEqual(game.CellState(width*height-1), MineSweeperGame.OOB);
            Assert.AreEqual(game.CellState(width * height), MineSweeperGame.OOB);
            Assert.AreEqual(game.CellState(width * height * 2), MineSweeperGame.OOB);
            Assert.AreEqual(game.CellState(-1), MineSweeperGame.OOB);
            Assert.AreEqual(game.CellState(-100), MineSweeperGame.OOB);

            //initial setup; all cells should be hidden
            for (int i = 0; i < width*height; i++)
            {
                Assert.AreEqual(game.CellState(i), MineSweeperGame.HIDDEN);
            }

            //test reveal random cell on fresh board 20 times
            for (int i = 0; i < 20; i++)
            {
                game.SetupGame();
                Random r = new Random();
                int rX = r.Next(0, width);
                int rY = r.Next(0, height);
                game.RevealCell(rX, rY);
                Assert.AreNotEqual(game.CellState(game.XYToIndex(rX, rY)), MineSweeperGame.HIDDEN);
                Assert.AreNotEqual(game.CellState(game.XYToIndex(rX, rY)), MineSweeperGame.OOB);
            }
        } 

    }
}