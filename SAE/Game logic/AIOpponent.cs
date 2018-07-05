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
            ResetDirections();
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
            
            // loop through all squares we have already hit
            foreach (Square sq in hitLog)
            {
                // check if the square is a shippart and belongs to an undestroyed ship
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
                        // analyse the next square in a direction until we find a not already hit square
                        // the direction is chosen randomly until we find a viable direction which yields undestroyed shipparts
                        do
                        {
                            if (TargetDirection == Direction.NONE)
                            {
                                targetSquare = GetNextSquareInDirection(sp, randomDir, board.Squares);
                                while (ReferenceEquals(targetSquare, null) || (!targetSquare.IsShipPart() && targetSquare.IsHit))
                                {
                                    ViableDirections.Remove(randomDir);
                                    randomDir = ViableDirections[rand.Next(ViableDirections.Count - 1)];
                                    targetSquare = GetNextSquareInDirection(sp, randomDir, board.Squares);
                                }
                            }
                            else
                            {
                                targetSquare = GetNextSquareInDirection(sp, TargetDirection, board.Squares);

                                if (ReferenceEquals(targetSquare, null) || (!targetSquare.IsShipPart() && targetSquare.IsHit))
                                {
                                    TargetDirection = InvertDirection(TargetDirection);
                                    targetSquare = GetNextSquareInDirection(sp, TargetDirection, board.Squares);
                                }
                            }

                            if (targetSquare.IsShipPart())
                                sp = new ShipPart(targetSquare);
                        }
                        while (targetSquare.IsShipPart() && sp.Destroyed);

                        // if we have
                        if (TargetDirection == Direction.NONE)
                        {
                            if (TargetSquare(targetSquare))
                                TargetDirection = randomDir;
                            else
                            {
                                ViableDirections.Remove(randomDir);
                                randomDir = ViableDirections[rand.Next(ViableDirections.Count - 1)];
                            }
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
                                    ResetDirections();
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

        private void ResetDirections()
        {
            for (int i = 0; i < 4; i++)
                ViableDirections.Add((Direction)i);
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
