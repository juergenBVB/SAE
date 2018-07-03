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
        private Random rand;
        private int turn;

        internal Settings Settings {  get { return settings; } set { settings = value; } }
        internal GameBoard PlayerGameBoard { get { return playerGameBoard; } set { playerGameBoard = value; } }
        internal GameBoard AiGameBoard { get { return aiGameBoard; } set { aiGameBoard = value; } }
        internal AIOpponent Ai { get { return ai; } set { ai = value; } }
        internal Player Player { get { return player; } set { player = value; } }

        public Game()
        {
            this.Settings = Settings.LoadSettings();
            this.AiGameBoard = new GameBoard(this.Settings.BoardSize, new List<Ship>());
            this.Ai = new AIOpponent(this.AiGameBoard, this.Settings.Difficulty);
            this.PlayerGameBoard = new GameBoard(this.Settings.BoardSize, new List<Ship>());
            this.Player = new Player(this.PlayerGameBoard);
            this.AiGameBoard.Ships = this.Player.PlaceShips();
            this.PlayerGameBoard.Ships = this.Ai.PlaceShips();
            this.Player.Board.AddShipsToBoard();
            this.Ai.Board.AddShipsToBoard();
            this.rand = new Random(DateTime.Now.Millisecond);
            this.turn = this.rand.Next(1);
        }

        public Boolean ExecuteMove(int x = -1, int y = -1)
        {
            Boolean hit = false;
            switch (turn)
            {
                case 0:
                    hit = this.player.TargetSquare(this.player.Board.GetSquareFromCoordinates(x, y));
                    turn = 1;
                    break;
                case 1:
                   hit = this.ai.MakeMove() is ShipPart;
                    turn = 0;
                    break;
                default:
                    break;
            }
            return hit;
        }

        public Boolean IsGameOver()
        {
            return this.player.Board.Ships.Count == 0 || this.ai.Board.Ships.Count == 0;
        }

        public Boolean CalculateWinner()
        {
            return this.player.Board.Ships.Count == 0;
        }
    }
}
