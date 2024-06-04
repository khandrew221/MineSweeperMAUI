namespace MineSweeper
{
    /// <summary>
    /// This class encapsulates and handles the mechanics of a Minesweeper game. It is purposefully
    /// decoupled from any interface coding, allowing for easy reuse of game code with different 
    /// interfaces, and acts as the Model of an MVC pattern. Defensive programming to catch bad 
    /// values being passed into functions should take place here if at all possible.
    /// </summary>
    public class MineSweeperGame
    {
        public const int BOMB = -2;
        public const int HIDDEN = -1;
        public const int OOB = -3;

        public const float DENSITY_DEFAULT = 0.2f;
        public const float DENSITY_MIN = 0f;
        public const float DENSITY_MAX = 0.5f;
        public const int LIVES_DEFAULT = 3;

        /// <summary>
        /// Maximum grid width allowed by the game code itself. Interfaces may enforce lower maximum values on what settings they pass. 
        /// </summary>
        public const int WIDTH_MAX = 32;
        /// <summary>
        /// Maximum grid height allowed by the game code itself. Interfaces may enforce lower maximum values on what settings they pass. 
        /// </summary>
        public const int HEIGHT_MAX = 32;
        /// <summary>
        /// Minimum grid width allowed by the game code itself. Interfaces may externally enforce higher minimum values on what settings they pass. 
        /// </summary>
        public const int WIDTH_MIN = 3;
        /// <summary>
        /// Maximum grid height allowed by the game code itself. Interfaces may externally enforce higher minimum values on what settings they pass. 
        /// </summary>
        public const int HEIGHT_MIN = 3;

        /// <summary>
        /// Handles the grid of cells that make up the game
        /// </summary>
        private CellGrid cellGrid;
        /// <summary>
        /// Reports on the state of the game: 0 for active, -1 for loss state, 1 for a win state 
        /// </summary>
        public int GameState { get; private set; }
        /// <summary>
        /// Tracks the number of bombs triggered
        /// </summary>
        public int BombsTriggered { get; private set; }
        /// <summary>
        /// Tracks the number of clear cells revealed
        /// </summary>
        public int SafeReveals { get; private set; }
        /// <summary>
        /// Stores the maximum lives for this game
        /// </summary>
        public int MaxLives { get; private set; }
        /// <summary>
        /// Tracks if the game is in Zen Mode (true = no endgame state for losing all lives). 
        /// This is held outside the usual settings structure because it can be freely changed during play.
        /// </summary>
        public bool ZenMode = false;

        /// <summary>
        /// Class constructor, initialising with given settings. Throws an ArgumentException if the settings instance cannot be verified as valid values. 
        /// </summary>
        public MineSweeperGame(Settings settings)
        {
            if (VerifySettings(settings))
            {
                cellGrid = new CellGrid(settings.Width, settings.Height, settings.BombDensity);
                MaxLives = settings.MaxLives;
                NewGame();
            } else
            {
                throw new ArgumentException("Attempted to create instance of MineSweeperGame with invalid MineSweeperSettings");
            }
        }

        /// <summary>
        /// Sets up a new game with current settings.
        /// </summary>
        public void NewGame()
        {
            cellGrid.SetGrid();
            ResetStats();
        }

        /// <summary>
        /// Sets up a new game with the provided settings. Throws an ArgumentException if the settings instance cannot be verified as valid values. 
        /// </summary>
        public void NewGame(Settings settings)
        {
            if (VerifySettings(settings))
            {
                cellGrid = new CellGrid(settings.Width, settings.Height, settings.BombDensity);
                cellGrid.SetGrid();
                MaxLives = settings.MaxLives;
                ResetStats();
            }
            else
            {
                throw new ArgumentException("Attempted to begin a new game with invalid MineSweeperSettings");
            }
        }

        /// <summary>
        /// Resets game state, BombsTriggered, and SafeReveals to 0
        /// </summary>
        private void ResetStats()
        {
            GameState = 0;
            BombsTriggered = 0;
            SafeReveals = 0;
        }

        /// <summary>
        /// Returns the number of cells in the cell grid, or -1 if there is an error.
        /// </summary>
        public int NumberOfCells()
        {
            return cellGrid.NumberOfCells();
        }

        /// <summary>
        /// Returns the width of the cell grid.
        /// </summary>
        public int Width()
        {
            return cellGrid.WIDTH;
        }

        /// <summary>
        /// Returns the height of the cell grid.
        /// </summary>
        public int Height()
        {
            return cellGrid.HEIGHT;
        }

        /// <summary>
        /// Returns the set of indices for cells neighbouring the given location, or an empty
        /// set if the location is invalid. 
        /// </summary>
        /// <param name="x">The x location</param>
        /// <param name="y">The y location</param>
        /// <returns>A set of ints representing the indices of neighbouring cells, or an empty set if the given location is invalid.</returns>
        /// Unit Tests: TestNeighbourAssignment()
        public HashSet<int> NeighboursOf(int x, int y)
        {
            return cellGrid.GenerateNeighboursSet(x, y);
        }

        /// <summary>
        /// Returns the set of indices for all bombs in the grid.
        /// </summary>
        /// <returns></returns>
        public HashSet<int> AllBombs()
        {
            return cellGrid.FindBombs();
        }

        /// <summary>
        /// Returns the bomb density as a float.
        /// </summary>
        /// <returns></returns>
        public float BombDensity() 
        { 
            return cellGrid.bombDensity; 
        }

        /// <summary>
        /// Returns an int representing the current state of the given cell.
        /// HIDDEN const represents hidden cells 
        /// Revealed non-bomb cells are represented by their number of bomb neighbours (0-8).
        /// BOMB const represents a revealed bomb. 
        /// OOB const represents an out of bounds cell index
        /// </summary>
        /// <param name="x">The x value to be converted. Out of bounds values are caught and return an error.</param>
        /// <param name="y">The y value to be converted. Out of bounds values are caught and return an error.</param>
        /// <returns>An int representing the current state of the given cell.</returns>
        public int CellState(int x, int y)
        {
            int i = XYToIndex(x, y);
            if(i >= 0 && i < NumberOfCells())
            {
                return cellGrid.State(i);
            }
            else { return OOB; }
        }

        /// <summary>
        /// Returns an int representing the current underlying state of the given cell, regardless of whether it is currently hidden.
        /// Revealed non-bomb cells are represented by their number of bomb neighbours (0-8).
        /// BOMB const represents a revealed bomb. 
        /// OOB const represents an out of bounds cell index
        /// </summary>
        /// <param name="x">The x position of the cell. Out of bounds values are caught and return an error.</param>
        /// <param name="y">The x position of the cell. Out of bounds values are caught and return an error.</param>
        /// <returns>An int representing the current state of the given cell.</returns>
        public int CellUnderlyingState(int x, int y)
        {
            int i = XYToIndex(x, y);
            if (i >= 0 && i < NumberOfCells())
            {
                return cellGrid.UnderlyingState(i);
            }
            else { return OOB; }
        }

        /// <summary>
        /// Converts an (x,y) value to the index of the cell at that position, or OOB const if the (x,y) value is not valid.
        /// </summary>
        /// <param name="x">The x value to be converted. Out of bounds values are caught and return an error.</param>
        /// <param name="y">The y value to be converted. Out of bounds values are caught and return an error.</param>
        /// <returns>The index of the cell at this x,y location, otherwise OOB as an error value.</returns>
        public int XYToIndex(int x, int y)
        {
            return cellGrid.XYToIndex(x, y);
        }

        /// <summary>
        /// Reveals the given cell. Does nothing if the location is not within the grid, if the given cell is already revealed, or if the game is not in a play state or Zen Mode.
        /// If the cell is revealed and has no bomb neighbours, it reveals all adjacent cells, and adds the number revealed to the count of safely revealed cells. 
        /// If the cell was a bomb, increments BombsTriggered.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void RevealCell(int x, int y)
        {
            if (GameState == 0 || ZenMode == true)
            {
                //reveal and store number of cells revealed / bomb value
                int r = cellGrid.RevealCell(x, y);

                if (r == BOMB)
                {
                    BombsTriggered++;
                    UpdateGameState();
                }
                else if (r > 0)
                {
                    SafeReveals += r;
                    UpdateGameState();
                }
            }
        }

        /// <summary>
        /// Updates the game state based on current count of lives, revealed cells, and bombs triggered.
        /// </summary>
        private void UpdateGameState()
        {
            //game ends in loss if out of lives
            if (LivesRemaining() < 1)
            {
                GameState = -1;
            }
            //lives remain and there are no more safe cells to reveal: win state
            else if (SafeCellsRemaining() == 0)
            {
                GameState = 1;
            }
            //otherwise, game is still active
            else
            {
                GameState = 0;
            }
        }

        /// <summary>
        /// Returns the number of lives remaining.
        /// </summary>
        /// <returns></returns>
        public int LivesRemaining() 
        {
            return MaxLives - BombsTriggered;
        }

        /// <summary>
        /// Returns the number of safe cells still hidden.
        /// </summary>
        /// <returns></returns>
        public int SafeCellsRemaining()
        {
            return cellGrid.SafeCells() - SafeReveals;
        }

        /// <summary>
        /// Encapsulates the storage and processing of individual cells. 
        /// Due to the cells being stored as a single dimensional array, a new CellGrid must be 
        /// created whenever the grid size is changed. 
        /// </summary>
        public class CellGrid
        {
            /// <summary>
            /// Width of the grid.
            /// </summary>
            public readonly int WIDTH;
            /// <summary>
            /// Height of the grid.
            /// </summary>
            public readonly int HEIGHT;
            /// <summary>
            /// Density of bombs.
            /// </summary>
            public float bombDensity { get; set; }
            /// <summary>
            /// Array holding the Cell objects that make up the grid
            /// </summary>
            private Cell[] cells;


            /// <summary>
            /// Handles the state of individual cells in the game grid
            /// </summary>
            public class Cell
            {
                /// <summary>
                /// Whether or not the cell is hidden.
                /// </summary>
                public bool isHidden = true;
                /// <summary>
                ///  Whether or not the cell is a bomb.
                /// </summary>
                public bool isBomb = false;
                /// <summary>
                /// A reference to each directly neighbouring cell, including diagonals
                /// </summary>
                public HashSet<Cell> neighbours = new HashSet<Cell>();

                /// <summary>
                /// Returns the number of bombs neighbouring this cell
                /// </summary>
                /// <returns>The number of bombs neighbouring this cell as an int</returns>
                public int NeighborBombs()
                {
                    int output = 0;
                    foreach (Cell cell in neighbours)
                    {
                        if (cell.isBomb)
                            output++;
                    }
                    return output;
                }

                /// <summary>
                /// Returns an int representing the current state of the cell.
                /// HIDDEN const represents hidden cells 
                /// Revealed non-bomb cells are represented by their number of bomb neighbours (0-8).
                /// BOMB const represents a revealed bomb. 
                /// </summary>
                /// <returns>An int representing the current state of the cell.</returns>
                public int State()
                {
                    if (!isHidden)
                    {
                        return UnderlyingState();
                    }
                    return HIDDEN;
                } 

                /// <summary>
                /// Returns the underlying state of the cell, regardless of whether it is currently hidden. The
                /// state is an int representing the number of neighbouring cells which are bombs, or the BOMB 
                /// flag constant if the cell itself contains a bomb.
                /// </summary>
                /// <returns></returns>
                public int UnderlyingState()
                {
                    return isBomb ? BOMB : NeighborBombs();
                }

                /// <summary>
                /// Reveals the cell if it is hidden. Does nothing if the cell is not hidden.
                /// Returns BOMB if the revealed cell was a bomb, otherwise the number of cells revealed
                /// </summary>
                public int Reveal()
                {
                    int numRevealed = 0;

                    if (isHidden)
                    {
                        isHidden = false;

                        if (isBomb)
                        {
                            return BOMB;
                        }

                        numRevealed++;

                        //reveal all neighbouring cells. This works recursively to reveal areas with no bombs.
                        if (State() == 0)
                        {
                            foreach (Cell cell in neighbours)
                            {
                                numRevealed += cell.Reveal();
                            }
                        }
                    }
                    return numRevealed;
                }
            }


            /// <summary>
            /// Cell grid constructor. Note that this does not set up a new game state, only
            /// a grid of empty cells with neighbours set. The grid's size is fixed for its lifetime;
            /// a new CellGrid must be made to change grid size. This is a measure to prevent mismatches
            /// occuring between WIDTH, HEIGHT, cells, and cell neighbour settings at any point: they
            /// should all be fixed on creation and never change.
            /// </summary>
            /// <param name="width">The grid width. Precondition: >0.</param>
            /// <param name="height">The grid height. Precondition: >0.</param>
            public CellGrid(int width, int height, float density)
            {
                //set width and height
                WIDTH = width;
                HEIGHT = height;
                bombDensity = density;
                cells = new Cell[WIDTH * HEIGHT];

                //create the array of cells
                for (int i = 0; i < HEIGHT * WIDTH; i++)
                {
                    cells[i] = new Cell();
                }

                //set cell neighbours
                for (int x = 0; x < WIDTH; x++)
                {
                    for (int y = 0; y < HEIGHT; y++)
                    {
                        //get the index of the cell in the array
                        int cellIndex = XYToIndex(x, y);

                        //assign an empty neighbour set for the cell
                        cells[cellIndex].neighbours = new HashSet<Cell>();

                        //assign all neighbours
                        foreach (int i in GenerateNeighboursSet(x, y))
                        {
                            cells[cellIndex].neighbours.Add(cells[i]);
                        }
                    }
                }
            }

            /// <summary>
            /// Sets or resets the grid for a game.
            /// </summary>
            public void SetGrid()
            {
                HashSet<int> bombs = GenerateBombSet(NumberOfBombs());

                for (int i = 0; i < cells.Length; i++)
                {
                    cells[i].isHidden = true;
                    cells[i].isBomb = bombs.Contains(i);
                }
            }

            /// <summary>
            /// Returns an int representing the current state of a cell in the game grid. 
            /// HIDDEN const represents hidden cells 
            /// Revealed non-bomb cells are represented by their number of bomb neighbours (0-8).
            /// BOMB const represents a revealed bomb. 
            /// </summary>
            /// <returns>An int representing the current state of the given cell.</returns>
            public int State(int i)
            {
                return cells[i].State();
            }

            /// <summary>
            /// Returns an int representing the current underlying state of a cell in the game grid, ignoring the hidden status
            /// Non-bomb cells are represented by their number of bomb neighbours (0-8).
            /// BOMB const represents a revealed bomb. 
            /// </summary>
            /// <returns>An int representing the current state of the given cell.</returns>
            public int UnderlyingState(int i)
            {
                return cells[i].UnderlyingState();
            }

            /// <summary>
            /// Converts an (x,y) value to the index of the cell at that position, or OOB const if the (x,y) value is not valid.
            /// </summary>
            /// <param name="x">The x value to be converted. Out of bounds values are caught and return an error.</param>
            /// <param name="y">The x value to be converted. Out of bounds values are caught and return an error.</param>
            /// <returns>The index of the cell at this x,y location, otherwise -1 as an error value.</returns>
            public int XYToIndex(int x, int y)
            {
                if (InGrid(x,y))
                {
                    //if the x and y values are within the grid, return an index
                    return y * WIDTH + x;
                }
                else
                {
                    //otherwise, return an error value
                    return OOB;
                }
            }

            /// <summary>
            /// Returns the number of cells in the cell array.
            /// </summary>
            public int NumberOfCells()
            {
                return cells.Length;
            }

            /// <summary>
            /// Checks if the given x,y value is within the grid. Returns true if it is,
            /// false otherwise.
            /// </summary>
            /// <param name="x">The x location</param>
            /// <param name="y">The y location</param>
            /// <returns>True if within the grid, false otherwise</returns>
            public bool InGrid(int x, int y)
            {
                return 0 <= x && x < WIDTH && 0 <= y && y < HEIGHT;
            }

            /// <summary>
            /// Calculates the set of indices for the neighbours of a given cell location. 
            /// <param name="x">The x position of the cell. Out of bounds values are caught and return an empty set.</param>
            /// <param name="y">The y position of the cell. Out of bounds values are caught and return an empty set.</param>
            /// <returns>The set of cells that are neighbours of this one, otherwise an empty set.
            /// </summary>
            /// Unit Tests: TestNeighbourAssignment()
            public HashSet<int> GenerateNeighboursSet(int x, int y)
            {
                HashSet<int> output = new HashSet<int>();

                if (InGrid(x, y)) //if valid location
                {
                    //Add index values for all possible neighbour cells. None-existent neighbours
                    //at edges and corners will add the value -1 to the set. 
                    output.Add(XYToIndex(x - 1, y));
                    output.Add(XYToIndex(x - 1, y - 1));
                    output.Add(XYToIndex(x - 1, y + 1));
                    output.Add(XYToIndex(x + 1, y));
                    output.Add(XYToIndex(x + 1, y - 1));
                    output.Add(XYToIndex(x + 1, y + 1));
                    output.Add(XYToIndex(x, y + 1));
                    output.Add(XYToIndex(x, y - 1));

                    //Remove OOB from the set if it exists. The remaining set is the set of
                    //neighbour indices.
                    output.Remove(OOB);
                }
                return output;
            }

            /// <summary>
            /// Generates a random set of bomb locations of the given size.
            /// </summary>
            /// <param name="size">The size of the set to be generated</param>
            /// <returns>A set of random integers between 0(inclusive) and WIDTH * HEIGHT(exclusive) of the given length.</returns>
            private HashSet<int> GenerateBombSet(int size)
            {
                Random rnd = new Random();
                HashSet<int> output = new HashSet<int>();

                //catch values that would lead to an infinite while loop below
                if (size > WIDTH * HEIGHT) size = WIDTH * HEIGHT;

                //add values to the set until it reaches the required size 
                while (output.Count < size)
                {
                    output.Add(rnd.Next(0, WIDTH * HEIGHT));
                }

                return output;
            }

            /// <summary>
            /// Brute force searches for all bombs.
            /// </summary>
            /// <returns>A set of all cell indices that are bombs.</returns>
            public HashSet<int> FindBombs()
            {
                HashSet<int> output = new HashSet<int>();
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i].isBomb)
                        output.Add(i);

                }
                return output;
            }

            /// <summary>
            /// Reveals the cell at the given location if the location is the grid and the cell is hidden. Does nothing if the location is not in the grid or the given cell is not hidden.
            /// If the cell is revealed and has no bomb neighbours, it recursively reveals all adjacent cells. 
            /// Returns BOMB if the revealed cell was a bomb, OOB for an out of bounds value, otherwise the number of cells revealed.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public int RevealCell(int x, int y)
            {
                if (InGrid(x, y))
                {
                    int i = XYToIndex(x, y);
                    return cells[i].Reveal();
                }

                return OOB;
            }

            /// <summary>
            /// Returns the total number of bombs in the grid
            /// </summary>
            /// <returns></returns>
            public int NumberOfBombs()
            {
                return (int)Math.Floor(WIDTH * HEIGHT * bombDensity);
            }

            /// <summary>
            /// Returns the total number of safe cells in the grid
            /// </summary>
            /// <returns></returns>
            public int SafeCells()
            {
                return NumberOfCells() - NumberOfBombs();
            }

        }

        /// <summary>
        /// Contains all settings data required to configure a new game. An interface can create an instance of this to freely manipulate values 
        /// and only pass them to the game itself on creating a new game. Verification of values can be performed by the main game class.
        /// Zen Mode setting is held outside the usual settings structure because it can be freely changed during play.
        /// </summary>
        public struct Settings
        {
            public int Width;
            public int Height;
            public float BombDensity;
            public int MaxLives;

            public Settings(int Width, int Height, float BombDensity, int MaxLives)
            {
                this.Width = Width;
                this.Height = Height;
                this.BombDensity = BombDensity;
                this.MaxLives = MaxLives;
            }
        }

        /// <summary>
        /// Verifies that an instance of Settings is not null and contains valid values.
        /// </summary>
        /// <param name="s"></param>
        /// <returns>True if all values are valid, false otherwise</returns>
        public static bool VerifySettings(Settings s)
        {
            if (s.Equals(null)) return false;

            bool widthTest = (s.Width >= WIDTH_MIN && s.Width <= WIDTH_MAX);
            bool heightTest = (s.Height >= HEIGHT_MIN && s.Height <= HEIGHT_MAX);
            bool densTest = (s.BombDensity >= DENSITY_MIN && s.BombDensity <= DENSITY_MAX);
            bool livesTest = s.MaxLives > 0;

            if (widthTest && heightTest && densTest && livesTest) { return true; }

            return false;
        }

    }




}