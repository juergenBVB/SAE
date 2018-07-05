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

        public AIOpponent() : base()
        {

        }

        public AIOpponent(GameBoard board, AIDifficulty diff) : base(board)
        {
            this.difficulty = diff;
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
            Boolean shipPartHit = false;
            Boolean squareHit = false;
            int randomdir = rand.Next(3);

            foreach (Square sq in hitLog)
            {
                if (sq.IsShipPart())
                {
                    sp = new ShipPart(sq);

                    foreach (Ship ship in board.Ships)
                    {
                        if (ship.isDestroyed() && ship.ShipParts.Any(x => x == sp))
                            break;
                        else if (ship.isDestroyed())
                            continue;
                        else
                        {
                            do
                            {
                                if (TargetDirection == Direction.NONE)
                                {
                                    targetSquare = GetNextSquareInDirection(sp, (Direction)randomdir, board.Squares);

                                    while (ReferenceEquals(targetSquare, null))
                                    {
                                        randomdir++;
                                        if (randomdir > 3)
                                            randomdir = 0;
                                        targetSquare = GetNextSquareInDirection(sp, (Direction)randomdir, board.Squares);
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
                            } while (targetSquare.IsShipPart() && sp.Destroyed);

                            if (TargetDirection == Direction.NONE)
                            {
                                shipPartHit = TargetSquare(targetSquare);
                                squareHit = true;
                                if (shipPartHit)
                                    TargetDirection = (Direction)randomdir;
                            }
                            else
                            {
                                shipPartHit = TargetSquare(targetSquare);
                                squareHit = true;
                                if (!shipPartHit)
                                    TargetDirection = InvertDirection(TargetDirection);

                            }

                            if (shipPartHit || squareHit)
                            {
                                if (ship.isDestroyed())
                                    TargetDirection = Direction.NONE;
                                break;
                            }
                        }
                    }
                    if (shipPartHit || squareHit)
                        break;
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
