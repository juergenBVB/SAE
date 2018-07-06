using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    /*
     * The Game class
     * does Game related things (who would've thought, am I right?!)
     */
    class Game
    {
        private Settings settings;
        private GameBoard playerGameBoard;
        private GameBoard aiGameBoard;
        private AIOpponent ai;
        private Player player;

        internal Settings Settings { get { return settings; } set { settings = value; } }
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
            this.AiGameBoard.Ships = this.Ai.PlaceShips();
            this.playerGameBoard.Ships = this.player.PlaceShips();
            this.Player.Board.AddShipsToBoard();
            this.Ai.Board.AddShipsToBoard();
        }

        // executes a Move of either the player or the ai
        public Boolean ExecuteMove(int x = -1, int y = -1)
        {
            Boolean hit = false;
            if (x != -1 && y != -1)
                hit = this.player.TargetSquare(this.player.Board.GetSquareFromCoordinates(x, y));
            else
                hit = this.ai.MakeMove().IsShipPart();

            return hit;
        }

        // checks if a player has lost all ships (yes, the ai is also considered a player and no, mayonnaise is NOT considered a player)
        public Boolean IsGameOver()
        {
            Boolean remainingPlayer = true;
            Boolean remainingAi = true;
            foreach (Square sq in this.player.Board.Squares)
            {
                if (sq.IsShipPart() && !((ShipPart)sq).Destroyed)
                {
                    remainingPlayer = false;
                }
            }

            foreach (Square sq in this.ai.Board.Squares)
            {
                if (sq.IsShipPart() && !((ShipPart)sq).Destroyed)
                {
                    remainingAi = false;
                }
            }

            return (remainingPlayer || remainingAi);
        }


        // calculases the winner based on who has lost all their ships
        public Boolean CalculateWinner()
        {
            foreach (Square sq in this.player.Board.Squares)
            {
                if (sq.IsShipPart() && !((ShipPart)sq).Destroyed)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
