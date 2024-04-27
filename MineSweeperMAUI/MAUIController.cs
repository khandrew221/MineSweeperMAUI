﻿using MineSweeper;
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
        /// <summary>
        /// Bomb state tag
        /// </summary>
        public const int BOMB = MineSweeperGame.BOMB;
        /// <summary>
        /// Hidden state tag
        /// </summary>
        public const int HIDDEN = MineSweeperGame.HIDDEN;
        /// <summary>
        /// Out of bounds state tag
        /// </summary>
        public const int OOB = MineSweeperGame.OOB;


        public MAUIController()
        {
            game = new MineSweeperGame(20, 10);
        }

        /// <summary>
        /// Begins a new game with the current settings.
        /// </summary>
        public void BeginGame()
        {
            game.SetupGame();
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

        /// <summary>
        /// Returns an int representing the current state of the given cell.
        /// MineSweeperGame.HIDDEN const represents hidden cells 
        /// Revealed non-bomb cells are represented by their number of bomb neighbours (0-8).
        /// MineSweeperGame.BOMB const represents a revealed bomb. 
        /// MineSweeperGame.OOB const represents an out of bounds cell index
        /// See MineSweeperGame
        /// </summary>
        /// <param name="x">the x position of the cell</param>
        /// <param name="y">the y position of the cell</param>
        /// <returns>An int representing the current state of the given cell.</returns>
        public int CellState(int x, int y)
        {
            return game.CellState(game.XYToIndex(x,y));
        }

        /// <summary>
        /// Sends a request to reveal the cell at the given location. If a hidden cell is found at that location, it is revealed. Does nothing if the location is not within the grid or the cell at that location is already revealed.
        /// If the cell is revealed and has no bomb neighbours, it reveals all adjacent cells. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void RevealCell(int x, int y)
        {
            game.RevealCell(x, y);
        }

    }
}