using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class Game
    {
        private Settings settings;
        private GameBoard playerGameBoard;
        private GameBoard aiGameBoard;
        private AIOpponent ai;
        private Player player;

        public Game()
        {
            this.Settings = Settings.LoadSettings();                      
            this.AiGameBoard = new GameBoard(this.Settings.BoardSize, new List<Ship>(), null);
            this.Ai = new AIOpponent(this.AiGameBoard, this.AiGameBoard.Ships, this.Settings.Difficulty);
            this.PlayerGameBoard = new GameBoard(this.Settings.BoardSize, new List<Ship>(), null);
            this.Player = new Player(this.PlayerGameBoard, this.PlayerGameBoard.Ships);            
        }

        internal Settings Settings { get => settings; set => settings = value; }
        internal GameBoard PlayerGameBoard { get => playerGameBoard; set => playerGameBoard = value; }
        internal GameBoard AiGameBoard { get => aiGameBoard; set => aiGameBoard = value; }
        internal AIOpponent Ai { get => ai; set => ai = value; }
        internal Player Player { get => player; set => player = value; }
    }
}
