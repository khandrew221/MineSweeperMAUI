using MineSweeper;

namespace MineSweeperTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //extremely basic test to check size of the grid is correct for given values
            int width = 5;
            int height = 10;

            MineSweeperGame game = new MineSweeperGame(width, height);

            Assert.AreEqual(width*height, game.NumberOfCells());
            Assert.AreEqual(height, game.Height());
            Assert.AreEqual(width, game.Width());
        }
    }
}