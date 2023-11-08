using MineSweeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeperMAUI
{
    /// <summary>
    /// Controller for MineSweeperGame class. Encapsulates MineSweeperGame and provides an interface for a MAUI view setup. 
    /// </summary>
    internal class MAUIController
    {

        /// <summary>
        /// Holds the game class instance.
        /// </summary>
        private static MineSweeperGame game;


        public MAUIController()
        {
            game = new MineSweeperGame(20, 10);
        }

        /// <summary>
        /// Returns the current game grid height 
        /// </summary>
        /// <returns></returns>
        public int GameHeight()
        {
            return game.Height();
        }

        /// <summary>
        /// Returns the current game grid width 
        /// </summary>
        /// <returns></returns>
        public int GameWidth()
        {
            return game.Width();
        }

        /// <summary>
        /// Converts an (x,y) value to the index of the cell at that position, or the OOB const if the (x,y) value is not valid.
        /// </summary>
        /// <param name="x">The x value to be converted. Out of bounds values are caught and return an error.</param>
        /// <param name="y">The x value to be converted. Out of bounds values are caught and return an error.</param>
        /// <returns>The index of the cell at this x,y location, otherwise OOB const as an error value.</returns>
        public int XYToIndex(int x, int y)
        {
            return game.XYToIndex(x, y);
        }

    }
}
