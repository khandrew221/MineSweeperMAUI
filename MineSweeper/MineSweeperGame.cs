using static MineSweeper.MineSweeperGame;

namespace MineSweeper
{
    /// <summary>
    /// This class encapsulates and handles the mechanics of a Minesweeper game. It is purposefully
    /// decoupled from any interface coding, allowing for easy reuse of game code with different 
    /// interfaces, and acts as the Model of an MVC pattern. 
    /// </summary>
    public class MineSweeperGame
    {
        /// <summary>
        /// Handles the grid of cells that make up the game
        /// </summary>
        private CellGrid cellGrid;
        /// <summary>
        /// Reports on the state of the game: 0 for active, -1 for loss state, 1 for a win state 
        /// </summary>
        public int GameOver { get; private set; }

        /// <summary>
        /// Class constructor, initialising a grid of the specified width and height. Grid size can
        /// be changed later (!!!not currently)
        /// </summary>
        public MineSweeperGame(int width, int height)
        {
            cellGrid = new CellGrid(width, height);
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
        /// Encapsulates the storage and processing of individual cells. 
        /// Due to the cells being stored as a single dimensional array, a new CellGrid must be 
        /// created whenever the grid size is changed. 
        /// </summary>
        class CellGrid
        {
            /// <summary>
            /// Width of the grid.
            /// </summary>
            public int WIDTH { get; private set; }
            /// <summary>
            /// Height of the grid.
            /// </summary>
            public int HEIGHT { get; private set; }
            /// <summary>
            /// Array holding the Cell objects that make up the grid
            /// </summary>
            private Cell[] cells;

            /// <summary>
            /// Cell grid constructor. Note that this does not set up a new game state, only
            /// a grid of empty cells with neighbours set. The grid's size is fixed for its lifetime;
            /// a new CellGrid must be made to change grid size. This is a measure to prevent mismatches
            /// occuring between WIDTH, HEIGHT, cells, and cell neighbour settings at any point: they
            /// should all be fixed on creation and never change.
            /// </summary>
            /// <param name="width">The grid width. Precondition: >0.</param>
            /// <param name="height">The grid height. Precondition: >0.</param>
            public CellGrid(int width, int height)
            {
                //set width and height
                WIDTH = width;
                HEIGHT = height;
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

                        //check direct left for a cell, and add it as a neighbour if it exists
                        int neighbourIndex = XYToIndex(x - 1, y);
                        if (neighbourIndex != -1)
                        {
                            cells[cellIndex].neighbours.Add(cells[neighbourIndex]);
                        }

                        //same for lower left
                        neighbourIndex = XYToIndex(x - 1, y - 1);
                        if (neighbourIndex != -1)
                        {
                            cells[cellIndex].neighbours.Add(cells[neighbourIndex]);
                        }

                        //and upper left
                        neighbourIndex = XYToIndex(x - 1, y + 1);
                        if (neighbourIndex != -1)
                        {
                            cells[cellIndex].neighbours.Add(cells[neighbourIndex]);
                        }

                        //check direct right
                        neighbourIndex = XYToIndex(x + 1, y);
                        if (neighbourIndex != -1)
                        {
                            cells[cellIndex].neighbours.Add(cells[neighbourIndex]);
                        }

                        //same for lower right
                        neighbourIndex = XYToIndex(x + 1, y - 1);
                        if (neighbourIndex != -1)
                        {
                            cells[cellIndex].neighbours.Add(cells[neighbourIndex]);
                        }

                        //and upper right
                        neighbourIndex = XYToIndex(x + 1, y + 1);
                        if (neighbourIndex != -1)
                        {
                            cells[cellIndex].neighbours.Add(cells[neighbourIndex]);
                        }

                        //directly above
                        neighbourIndex = XYToIndex(x, y + 1);
                        if (neighbourIndex != -1)
                        {
                            cells[cellIndex].neighbours.Add(cells[neighbourIndex]);
                        }

                        //and directly below
                        neighbourIndex = XYToIndex(x, y - 1);
                        if (neighbourIndex != -1)
                        {
                            cells[cellIndex].neighbours.Add(cells[neighbourIndex]);
                        }
                    }
                }
            }

            /// <summary>
            /// Converts an (x,y) value to the index of the cell at that position, or -1 if the (x,y) value is not valid.
            /// </summary>
            /// <param name="x">The x value to be converted. Out of bounds values are caught and return an error.</param>
            /// <param name="y">The x value to be converted. Out of bounds values are caught and return an error.</param>
            /// <returns>The index of the cell at this x,y location, otherwise -1 as an error value.</returns>
            public int XYToIndex(int x, int y)
            {
                if (0 <= x && x < WIDTH && 0 <= y && y < HEIGHT)
                {
                    //if the x and y values are within the grid, return an index
                    return y * WIDTH + x;
                }
                else
                {
                    //otherwise, return an error value
                    return -1;
                }
            }

            /// <summary>
            /// TESTING. Returns the number of cells in the cell array.
            /// </summary>
            public int NumberOfCells()
            {
                try
                {
                    return cells.Length;
                } 
                catch (ArgumentNullException e) 
                {
                    return -1;
                }
            }
        }

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
            public HashSet<Cell> neighbours;
        }
    }

}