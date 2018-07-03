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
        private Direction TargetDirection = Direction.NONE;

        public AIOpponent() : base()
        {

        }

        public AIOpponent(GameBoard board, AIDifficulty diff) : base(board)
        {
            this.difficulty = diff;
            legalSquares = GetLegalSquaresOpp();
        }

        public Square TargetRandomSquare()
        {
            Square sq = GetRandomLegalSquare(true);
            TargetSquare(sq);
            return sq;
        }

        private Square TargetShip()
        {
            ShipPart sp = null;
            Boolean shipFits = false;
            Direction d = (Direction)0;
            Ship targetShip = null;

            foreach (Square sq in hitLog)
            {
                if (sq.IsShipPart())
                {
                    sp = new ShipPart(sq);

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

            if (shipFits)
            {
                if (TargetDirection != Direction.NONE)
                {
                    d = TargetDirection;
                }
                if (TargetSquare(FindNextSquareInDirection(sp, d)))
                {
                    TargetDirection = d;
                    if (targetShip.isDestroyed())
                    {
                        Board.Ships.Remove(targetShip);
                        TargetDirection = Direction.NONE;
                    }
                }
                else
                {
                    TargetDirection = Direction.NONE;
                }
                return FindNextSquareInDirection(sp, d);
            }
            else
                return null;
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

        public Square MakeMove()
        {
            Square sq = null;
            if (!ShipFound() || difficulty == AIDifficulty.Easy)
            {
                sq = TargetRandomSquare();
            }
            else if (!(difficulty == AIDifficulty.Easy))
            {
                do
                {
                    sq = TargetShip();
                }
                while (sq == null);
            }
            return sq;
        }
    }
}
