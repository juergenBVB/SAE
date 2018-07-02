﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class AIOpponent : Player
    {
        private AIDifficulty difficulty;

        public AIOpponent() : base()
        {

        }

        public AIOpponent(GameBoard board, List<Ship> ships, AIDifficulty diff) : base(board, ships)
        {
            this.difficulty = diff;
            rand = new Random(DateTime.Now.Millisecond);
            LegalSquares = GetLegalSquaresOpp();
            PlaceShips();
        }

        public Square TargetRandomSquare()
        {
            Square sq = GetRandomLegalSquare(true);
            TargetSquare(sq);
            return sq;
        }

        private void TargetShip()
        {
            ShipPart sp = null;
            Boolean shipFits = false;
            Direction d = (Direction)0;
            Ship targetShip = null;

            foreach (Square sq in hitLog)
            {
                if (sq.IsShipPart())
                {
                    sp = (ShipPart)sq;

                    if (!sp.Destroyed)
                    {
                        foreach (Ship ship in Board.Ships)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                shipFits = IsLegalDirection(sp, ship.GetShipLength(), (Direction)i, true);
                                if (shipFits)
                                {
                                    d = (Direction)i;
                                    targetShip = ship;
                                    break;
                                }
                            }
                            if (shipFits)
                                break;
                        }

                        if (shipFits)
                            break;
                    }
                }
            }

            if (shipFits)
            {
                TargetSquare(FindNextSquareInDirection(sp, d));
                if (targetShip.isDestroyed())
                    ships.Remove(targetShip);
            }
        }

        private Square FindNextSquareInDirection(Square sq, Direction d)
        {
            Square tempSquare;
            switch (d)
            {
                case Direction.UP:
                    tempSquare = Board.Squares.Find(x => x.PositionX == sq.PositionX && x.PositionY == sq.PositionY - 1);
                    break;
                case Direction.DOWN:
                    tempSquare = Board.Squares.Find(x => x.PositionX == sq.PositionX && x.PositionY == sq.PositionY + 1);
                    break;
                case Direction.LEFT:
                    tempSquare = Board.Squares.Find(x => x.PositionX == sq.PositionX - 1 && x.PositionY == sq.PositionY);
                    break;
                case Direction.RIGHT:
                    tempSquare = Board.Squares.Find(x => x.PositionX == sq.PositionX + 1 && x.PositionY == sq.PositionY - 1);
                    break;
                default:
                    tempSquare = null;
                    break;
            }
            return tempSquare;
        }

        public void MakeMove()
        {
            if (!ShipFound() || difficulty == AIDifficulty.Easy)
            {
                TargetRandomSquare();
            }
            else if (!(difficulty == AIDifficulty.Easy))
            {
                TargetShip();
            }
        }
    }
}
