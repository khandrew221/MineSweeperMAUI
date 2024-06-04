using MineSweeper;
using System.Collections.Generic;
using static MineSweeper.MineSweeperGame;

namespace MineSweeperTests
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Tests the basic validity of const values 
        /// </summary>
        [TestMethod]
        public void TestConstValidity() 
        {
            //test flag values uniqueness
            Assert.IsFalse(OOB == BOMB);
            Assert.IsFalse(OOB == HIDDEN);
            Assert.IsFalse(BOMB == HIDDEN);

            //test flag values do not clash with >=0 cell state returns
            Assert.IsTrue(OOB < 0);
            Assert.IsTrue(BOMB < 0); 
            Assert.IsTrue(HIDDEN < 0);

            //WIDTH_MIN & WIDTH_MAX
            Assert.IsTrue(WIDTH_MIN > 0);
            Assert.IsTrue(WIDTH_MIN <= WIDTH_MAX);

            //HEIGHT_MIN & HEIGHT_MAX
            Assert.IsTrue(HEIGHT_MIN > 0);
            Assert.IsTrue(HEIGHT_MIN <=  HEIGHT_MAX);

            //DENSITY MIN, MAX and DEFAULT
            Assert.IsTrue(DENSITY_MIN >= 0);
            Assert.IsTrue(DENSITY_MIN <= DENSITY_MAX);
            Assert.IsTrue(DENSITY_MAX <= 1);
            Assert.IsTrue(DENSITY_DEFAULT <= DENSITY_MAX);
            Assert.IsTrue(DENSITY_DEFAULT >= DENSITY_MIN);

            //LIVES_DEFAULT
            Assert.IsTrue(LIVES_DEFAULT > 0);

        }

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
            catch (ArgumentException)
            {
                //ArgumentException should be thrown for invalid settings
                Assert.IsFalse(ExpectedResult);
            }
        }

        /// <summary>
        /// Tests if grid neighbours are being assigned correctly. 
        /// !!! only one test case
        /// </summary>
        [TestMethod]
        public void TestNeighbourAssignment()
        {
            //tests nieghbour assignment on a 5x5 grid (minimum size to cover all possible neighbour types is 3x3).
            MineSweeperGame game = new MineSweeperGame(new Settings(5, 5, DENSITY_DEFAULT, 1));

            //Corner tests
            HashSet<int> values = game.NeighboursOf(0, 0);
            HashSet<int> expectedValues = new HashSet<int>() { 1, 5, 6 };
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(0, 4);
            expectedValues = new HashSet<int>() { 15, 16, 21 };
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(4, 0);
            expectedValues = new HashSet<int>() { 3, 8, 9 };
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(4, 4);
            expectedValues = new HashSet<int>() { 18, 19, 23 };
            Assert.IsTrue(values.SetEquals(expectedValues));

            //Central test
            values = game.NeighboursOf(2, 2);
            expectedValues = new HashSet<int>() { 6, 7, 8, 11, 13, 16, 17, 18 };
            Assert.IsTrue(values.SetEquals(expectedValues));

            //Mid Edge tests
            values = game.NeighboursOf(3, 0);
            expectedValues = new HashSet<int>() { 2, 4, 7, 8, 9 };
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(0, 1);
            expectedValues = new HashSet<int>() { 0, 1, 6, 10, 11 };
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(4, 3);
            expectedValues = new HashSet<int> {13, 14, 18, 23, 24};
            Assert.IsTrue(values.SetEquals(expectedValues));

            values = game.NeighboursOf(1, 4);
            expectedValues = new HashSet<int> {15, 16, 17, 20, 22};
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
        /// Checks that the underlying state of the grid is valid. Includes:
        /// Correct number of bombs placed
        /// Correct number of neighbouring bombs reported
        /// </summary>
        [TestMethod]
        public void TestUnderlyingState()
        {
            MineSweeperGame game = new MineSweeperGame(new Settings(11, 11, DENSITY_DEFAULT, LIVES_DEFAULT));

            //store full underlying state grid
            int[,] grid = new int[11, 11];
            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    grid[x, y] = game.CellUnderlyingState(x, y);
                }
            }

            //check for validity: should contain the correct number of bombs, and 
            //correct values for cells based on number of bomb neighbours
            int totalBombs = 0;
            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    //if this cell is a bomb incremement total bombs 
                    if (grid[x, y] == BOMB)
                    {
                        totalBombs++;
                    }
                    else
                    {
                        //find the correct number of neighbouring bombs
                        int nBombs = 0;

                        List<(int, int)> neighbours = NeighborsOfCell(x, y, game.Width(), game.Height());

                        foreach (var n in neighbours)
                        {
                            if (grid[n.Item1, n.Item2] == BOMB) nBombs++;
                        }

                        //test against state reported number
                        Assert.AreEqual(grid[x, y], nBombs);
                    }
                }
            }

            //check the correct number of bombs were found
            Assert.AreEqual(totalBombs, game.AllBombs().Count);
        }

        /// <summary>
        /// Tests CellState().
        /// </summary>
        [TestMethod]
        public void TestCellStateReporting()
        {
            int width = 5;
            int height = 5;

            MineSweeperGame game = new MineSweeperGame(new Settings(width, height, DENSITY_DEFAULT, 1));
            game.NewGame();

            //test OOB index values are reported correctly
            Assert.AreNotEqual(game.CellState(0, 0), OOB);
            Assert.AreNotEqual(game.CellState(width-1, height-1), OOB);
            Assert.AreNotEqual(game.CellState(width / 2, height / 2), OOB);
            Assert.AreEqual(game.CellState(width, height), OOB);
            Assert.AreEqual(game.CellState(width -1, height), OOB);
            Assert.AreEqual(game.CellState(width, height - 1), OOB);
            Assert.AreEqual(game.CellState(width * 2, height * 2), OOB);
            Assert.AreEqual(game.CellState(-1, 0), OOB);
            Assert.AreEqual(game.CellState(-100, 0), OOB);
            Assert.AreEqual(game.CellState(0, -1), OOB);
            Assert.AreEqual(game.CellState(0, -100), OOB);
            Assert.AreEqual(game.CellState(-1, -1), OOB);
            Assert.AreEqual(game.CellState(-100, -100), OOB);

            //initial setup; all cells should be hidden
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Assert.AreEqual(game.CellState(x, y), HIDDEN);
                }                
            }

            //test reveal random cell on fresh board 20 times
            for (int i = 0; i < 20; i++)
            {
                //initialise test game and data
                game.NewGame();
                HashSet<int> allBombs = game.AllBombs();

                //pick random cell and reveal
                Random r = new Random();
                int rX = r.Next(0, width);
                int rY = r.Next(0, height);
                game.RevealCell(rX, rY);

                //check state of revealed cell
                int index = game.XYToIndex(rX, rY);
                int state = game.CellState(rX, rY);
                Assert.AreNotEqual(state, OOB); //should not be outside the grid; note this could be issue with random cell picks above
                Assert.AreNotEqual(state, HIDDEN); //has been revealed, should not register as hidden
                if (allBombs.Contains(index))
                {
                    //a bomb should register as a bomb
                    Assert.AreEqual(state, BOMB);
                }
                else
                {
                    //a revealed non-bomb should report an integer between 0 and 8 inclusive
                    //testing of precise values based on neighbouring bombs in TestUnderlyingState()
                    Assert.IsTrue(state >= 0);
                    Assert.IsTrue(state <= 8);
                }
            }
        }

        /// <summary>
        /// Tests the AllBombs() method to be certain that it is reporting accurately.
        /// </summary>
        [TestMethod]
        public void TestAllBombsMethod()
        {
            MineSweeperGame game = new MineSweeperGame(new Settings(10, 10, DENSITY_DEFAULT, 1));
            game.NewGame();
            game.ZenMode = true;

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
                    bool isBomb = game.CellState(x, y) == BOMB;
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

        /// <summary>
        /// Tests that SafeReveals and BombsTriggered (and by extension, GameState) are updating correctly.
        /// </summary>
        [TestMethod]
        public void TestGameStateReporting()
        {
            MineSweeperGame game = new MineSweeperGame(new Settings(11, 11, DENSITY_DEFAULT, LIVES_DEFAULT));
            game.ZenMode = true; //required for free testing

            //test initial game state
            Assert.AreEqual(game.SafeReveals, 0);
            Assert.AreEqual(game.BombsTriggered, 0);
            Assert.AreEqual(game.GameState, 0);

            int oldSafeReveals = game.SafeReveals;
            int newSafeReveals = game.SafeReveals;
            int oldBombsTriggered = game.BombsTriggered;
            int newBombsTriggered = game.BombsTriggered;
            int trueBombsTriggered = 0;
            int trueSafeReveals = 0;

            //reveal entire grid, checking as we go
            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    bool isHidden = game.CellState(x, y) == HIDDEN;

                    if (isHidden)
                    {
                        //reveal cell and record old and new values
                        oldSafeReveals = game.SafeReveals;
                        oldBombsTriggered = game.BombsTriggered;
                        game.RevealCell(x, y);
                        newSafeReveals = game.SafeReveals;
                        newBombsTriggered = game.BombsTriggered;

                        int[] trueVals = GameStateData(game);
                        trueBombsTriggered = trueVals[2];
                        trueSafeReveals = trueVals[1];

                        int cs = game.CellState(x, y);

                        //no safe reveal change; assert bomb reveal
                        if (oldSafeReveals == newSafeReveals)
                        {
                            Assert.AreEqual(cs, BOMB);
                            Assert.AreEqual(newBombsTriggered - oldBombsTriggered, 1);
                        } 
                        else
                        {
                            ///valid cell reveal
                            Assert.IsTrue(cs >= 0 && cs <= 8);
                            if (cs == 0)
                            {
                                //check that extra cells were revealed on a no-neighbour cell being revealed
                                //!!! should not run into issues with current setup but outlyers possible!!!!
                                Assert.IsTrue(newSafeReveals - oldSafeReveals > 1);
                            } 
                            else
                            {
                                //non-zero states should reveal a single cell only 
                                Assert.AreEqual(newSafeReveals - oldSafeReveals, 1);
                            }
                        }
                    }
                    else
                    {
                        //no change should take place on reveal
                        oldSafeReveals = game.SafeReveals;
                        oldBombsTriggered = game.BombsTriggered;
                        game.RevealCell(x, y);
                        newSafeReveals = game.SafeReveals;
                        newBombsTriggered = game.BombsTriggered;

                        int[] trueVals = GameStateData(game);
                        trueBombsTriggered = trueVals[2];
                        trueSafeReveals = trueVals[1];

                        Assert.AreEqual(oldSafeReveals, newSafeReveals);
                        Assert.AreEqual(oldBombsTriggered, newBombsTriggered);
                    }

                    //check that inner tally and brute force count return the same value
                    Assert.AreEqual(trueSafeReveals, newSafeReveals);
                    Assert.AreEqual(trueBombsTriggered, newBombsTriggered);
                }
            }
        }


        /// <summary>
        /// Returns brute force counts of hidden cells, revealed safe cells, and revealed bombs, as reported by CellState(). 
        /// Counts returned in an int array
        /// [0] hidden cells
        /// [1] revealed safe cells
        /// [2] revealed bombs
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public int[] GameStateData(MineSweeperGame game)
        {
            int[] output = { 0, 0, 0 };

            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    int cs = game.CellState(x, y);
                    if (cs == HIDDEN)
                    {
                        output[0]++;
                    }
                    else if (cs == BOMB)
                    {
                        output[2]++;
                    }
                    else if (cs >= 0 && cs < 9)
                    {
                        output[1]++;
                    }
                }
            }

            //check that the correct number of cells have been counted
            Assert.AreEqual(output.Sum(), game.NumberOfCells());

            return output;
        }

        /// <summary>
        /// Runs the multireveal test multiple times with random grid sizes
        /// </summary>
        [TestMethod]
        public void TestMultipleReveal()
        {
            int iterations = 20;
            Random rnd = new Random();

            for (int i = 0; i < iterations; i++)
            {
                TestMultipleRevealSub(rnd.Next(WIDTH_MIN, WIDTH_MAX), rnd.Next(HEIGHT_MIN, HEIGHT_MAX));
            }
        }


        /// <summary>
        /// Tests the reveal of multiple cells on revealing a cell with no bomb neighbours
        /// </summary>
        public void TestMultipleRevealSub(int xsize, int ysize)
        {
            MineSweeperGame game = new MineSweeperGame(new Settings(xsize, ysize, 0.1f, LIVES_DEFAULT));
            game.ZenMode = true;

            //store full underlying state grid
            int[,] grid = new int[xsize, ysize];
            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    grid[x, y] = game.CellUnderlyingState(x, y);
                }
            }

            bool MultiRevealFound = false;
            List<(int, int)> processReveals = new List<(int, int)> ();

            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    int safeReveals = game.SafeReveals;
                    game.RevealCell(x, y);
                    processReveals.Add((x, y));

                    if (game.SafeReveals - safeReveals > 1)
                    {

                        int numReveals = game.SafeReveals - safeReveals;
                        List<(int, int)> reveals = CheckMultiRevealOnUnderlyingGridRecursion(new List<(int, int)>(), grid, x, y);

                        //multireveal only occurs on a 0 state cell
                        Assert.AreEqual(game.CellState(x, y), 0);

                        //Console.WriteLine("Multireveal found at " + x + ", " + y);
                        //Console.WriteLine("Raw numReveals: " + numReveals);

                        //due to search, some cells that should be revealed according to the raw count provided from the underlying grid 
                        //may already have been revealed. Adjusts numReveals appropriately before comparison.
                        if (x > 0)
                        {
                            numReveals+=3;
                        }
                        if (y > 0)
                        {
                            numReveals++;
                        }

                        //the (adjusted) number of reveals from game state data should match the number found by analysisng the 
                        //underlying state grid seperately in test methods
                        Assert.AreEqual(numReveals, reveals.Count);

                        //all the cells in processReveals and reveals should be unhidden, all others should be hidden
                        for (int y1 = 0; y1 < game.Height(); y1++)
                        {
                            for (int x1 = 0; x1 < game.Width(); x1++)
                            {
                                int a = game.CellState(x1, y1);
                                var l = (x1, y1);
                                if (reveals.Contains(l) || processReveals.Contains(l))
                                {
                                    Assert.AreNotEqual(a, HIDDEN);
                                } 
                                else
                                {
                                    Assert.AreEqual(a, HIDDEN);
                                }
                            }
                        }

                        /*
                        //console out allows some manual checking of results
                        Console.WriteLine("Full Grid:");
                        for (int y1 = 0; y1 < game.Height(); y1++)
                        {
                            for (int x1 = 0; x1 < game.Width(); x1++)
                            {
                                int a = grid[x1, y1];
                                if (a == BOMB)
                                    Console.Write("B");
                                else
                                    Console.Write(grid[x1, y1]);    
                            }
                            Console.WriteLine("");
                        }

                        Console.WriteLine(numReveals);
                        Console.WriteLine(reveals.Count);

                        foreach (var reveal in reveals)
                        {
                            Console.WriteLine(reveal.Item1 +  ", " + reveal.Item2);
                        }*/

                        MultiRevealFound = true;
                        break;
                    }
                }
                if (MultiRevealFound)
                    break;
            }
        }


        /// <summary>
        /// Returns the list of positions that should have been revealed if the given cell was revealed on the given grid. Enter with empty currList.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public List<(int, int)> CheckMultiRevealOnUnderlyingGridRecursion(List<(int, int)> currList, int[,] grid, int x, int y)
        {
            List<(int, int)> newList = currList;

            var t = (x, y);
            if (!newList.Contains(t))
            {
                newList.Add(t);
            }

            //multireveal 
            if (grid[x, y] == 0)
            {
                int XSize = grid.GetLength(0);
                int YSize = grid.GetLength(1);

                List<(int, int)> neighbors = NeighborsOfCell(x, y, XSize, YSize);

                foreach ( var n in neighbors )
                {
                    if (!newList.Contains(n))
                    {
                        newList.Add(n);
                        if (grid[n.Item1, n.Item2] == 0)
                        {
                            newList = CheckMultiRevealOnUnderlyingGridRecursion(newList, grid, n.Item1, n.Item2);
                        }
                    }
                }
            }

            return newList;
        }

        /// <summary>
        /// Returns the neighbour set of the given x,y position given a grid with width w and height h
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public List<(int, int)> NeighborsOfCell(int x, int y, int w, int h)
        {
            List<(int, int)> output = new List<(int, int)>();

            bool xEdgeL = (x - 1) < 0;
            bool xEdgeR = (x + 1) >= w;
            bool yEdgeU = (y - 1) < 0;
            bool yEdgeB = (y + 1) >= h;


            if (!xEdgeL)
            {
                var t = (x - 1, y);
                output.Add(t);

                if (!yEdgeU)
                {
                    t = (x - 1, y -1);
                    output.Add(t);
                }

                if (!yEdgeB)
                {
                    t = (x - 1, y + 1);
                    output.Add(t);
                }
            }

            if (!xEdgeR)
            {
                var t = (x + 1, y);
                output.Add(t);

                if (!yEdgeU)
                {
                    t = (x + 1, y - 1);
                    output.Add(t);
                }

                if (!yEdgeB)
                {
                    t = (x + 1, y + 1);
                    output.Add(t);
                }
            }

            if (!yEdgeU)
            {
                var t = (x, y - 1);
                output.Add(t);
            }

            if (!yEdgeB)
            {
                var t = (x, y + 1);
                output.Add(t);
            }

            return output;

        }

        [TestMethod]
        public void TestGameEndAndZenMode()
        {

            //create a bomb dense game
            MineSweeperGame game = new MineSweeperGame(new Settings(10, 10, DENSITY_MAX, LIVES_DEFAULT));
            game.ZenMode = false;

            //test normal mode
            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    //if cell is hidden, try a reveal
                    if (game.CellState(x, y) == HIDDEN)
                    {
                        //if game is active, cell should be revealed
                        if (game.GameState == 0)
                        {
                            game.RevealCell(x, y);
                            Assert.AreEqual(game.CellState(x, y), game.CellUnderlyingState(x, y));
                        }
                        else
                        {
                            game.RevealCell(x, y);
                            //otherwise, the cell should remain hidden
                            Assert.AreEqual(game.CellState(x, y), HIDDEN);
                        }
                    }
                }
            }

            
            game.NewGame();
            game.ZenMode = true;

            //test zen mode
            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    //if cell is hidden, try a reveal
                    if (game.CellState(x, y) == HIDDEN)
                    {
                        //cell should be revealed regardless of other factors
                        game.RevealCell(x, y);
                        Assert.AreEqual(game.CellState(x, y), game.CellUnderlyingState(x, y));
                    }
                }
            }


            game.NewGame();
            game.ZenMode = false;

            //test entering zen mode after game loss
            for (int x = 0; x < game.Width(); x++)
            {
                for (int y = 0; y < game.Height(); y++)
                {
                    //if cell is hidden, try a reveal
                    if (game.CellState(x, y) == HIDDEN)
                    {
                        //if game is active, cell should be revealed
                        if (game.GameState == 0 || game.ZenMode)
                        {
                            game.RevealCell(x, y);
                            Assert.AreEqual(game.CellState(x, y), game.CellUnderlyingState(x, y));
                        }
                        else
                        {
                            game.RevealCell(x, y);
                            //otherwise, the cell should remain hidden
                            Assert.AreEqual(game.CellState(x, y), HIDDEN);
                        }
                    }

                    //check for fail state and turn on zen mode if so
                    if (game.GameState == -1 && !game.ZenMode)
                    {
                        game.ZenMode = true;
                    }
                }
            }


        }

    }
}