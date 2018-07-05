using System;
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
        private List<Direction> ViableDirections;

        public AIOpponent() : base()
        {

        }

        public AIOpponent(GameBoard board, AIDifficulty diff) : base(board)
        {
            this.difficulty = diff;
            this.ViableDirections = new List<Direction>();
            for (int i = 0; i < 4; i++)
                ViableDirections.Add((Direction)i);
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
            Square targetSquare = null;
            Boolean squareHit = false;
            Boolean isTargetedShip = false;
            Direction randomDir = ViableDirections[rand.Next(ViableDirections.Count - 1)];

            foreach (Square sq in hitLog)
            {
                if (sq.IsShipPart())
                {
                    sp = new ShipPart(sq);

                    foreach (Ship ship in board.Ships)
                    {
                        if (!ship.isDestroyed())
                        {
                            if (ship.ShipParts.Any(x => x == sp))
                                isTargetedShip = true;
                        }
                    }

                    if (isTargetedShip)
                    {
                        // analyse next square in a direction until its not a destroyed shippart
                        do
                        {
                            if (TargetDirection == Direction.NONE)
                            {
                                targetSquare = GetNextSquareInDirection(sp, randomDir, board.Squares);
                                if (ReferenceEquals(targetSquare, null))
                                {
                                    ViableDirections.Remove(randomDir);

                                    foreach (Direction d in ViableDirections)
                                    {
                                        targetSquare = GetNextSquareInDirection(sp, d, board.Squares);
                                        if (ReferenceEquals(targetSquare, null))
                                            ViableDirections.Remove(d);
                                        else
                                        {
                                            randomDir = d;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                targetSquare = GetNextSquareInDirection(sp, TargetDirection, board.Squares);

                                if (ReferenceEquals(targetSquare, null))
                                {
                                    TargetDirection = InvertDirection(TargetDirection);
                                    targetSquare = GetNextSquareInDirection(sp, TargetDirection, board.Squares);
                                }
                            }

                            if (targetSquare.IsShipPart())
                                sp = new ShipPart(targetSquare);
                            else if (targetSquare.IsHit)
                            {
                                randomDir = ViableDirections[rand.Next(ViableDirections.Count - 1)];
                                continue;
                            }
                        } while (targetSquare.IsShipPart() && sp.Destroyed);

                        if (TargetDirection == Direction.NONE)
                        {
                            if (TargetSquare(targetSquare))
                                TargetDirection = randomDir;
                            else
                                ViableDirections.Remove(randomDir);
                            squareHit = true;
                        }
                        else
                        {
                            if (!TargetSquare(targetSquare))
                                TargetDirection = InvertDirection(TargetDirection);
                            squareHit = true;
                        }

                        if (squareHit)
                        {
                            foreach (Ship ship in this.board.Ships)
                            {
                                if (ship.isDestroyed() && ship.ShipParts.Any(x => x == targetSquare))
                                {
                                    TargetDirection = Direction.NONE;
                                    ViableDirections = new List<Direction>();
                                    for (int i = 0; i < 4; i++)
                                        ViableDirections.Add((Direction)i);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            return targetSquare;
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
                sq = TargetShip();
            }
            return sq;
        }

        private Direction InvertDirection(Direction d)
        {
            switch (d)
            {
                case Direction.UP:
                    return Direction.DOWN;
                case Direction.DOWN:
                    return Direction.UP;
                case Direction.LEFT:
                    return Direction.RIGHT;
                case Direction.RIGHT:
                    return Direction.LEFT;
                default:
                    return Direction.NONE;
            }
        }
    }
}
