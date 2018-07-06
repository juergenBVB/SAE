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

        // stores the direction we determined contains a ship
        private Direction TargetDirection = Direction.NONE;

        public AIOpponent() : base()
        {

        }

        public AIOpponent(GameBoard board, AIDifficulty diff) : base(board)
        {
            this.difficulty = diff;
            ResetDirections();
        }

        // targets a random viable/legal square
        public Square TargetRandomSquare()
        {
            Square sq = GetRandomLegalSquare(true);
            TargetSquare(sq);
            return sq;
        }

        // if the ai has hit a ShipPart in the previous turn, try destroying the ship it belongs to
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
                            // check if that ship contains the current square
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
                            // when we aren't sure where the ship actually is
                            if (TargetDirection == Direction.NONE)
                            {
                                targetSquare = GetNextSquareInDirection(sp, randomDir, board.Squares);
                                
                                // while we haven't found a targetable square, try targeting squares in different directions
                                while (ReferenceEquals(targetSquare, null) || (!targetSquare.IsShipPart() && targetSquare.IsHit))
                                {
                                    ViableDirections.Remove(randomDir);
                                    randomDir = ViableDirections[rand.Next(ViableDirections.Count - 1)];
                                    targetSquare = GetNextSquareInDirection(sp, randomDir, board.Squares);
                                }
                            }
                            // when we are sure where (at least part of) the ship is
                            else
                            {
                                targetSquare = GetNextSquareInDirection(sp, TargetDirection, board.Squares);

                                // if the targeted square isn't targetable, invert the targetdirection and pursue it
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

                        // if we haven't yet figured out where the ship is, target the previously chosen square, which might actually be a miss
                        if (TargetDirection == Direction.NONE)
                        {
                            // if it hit, set the TargetDirection to the current direction
                            if (TargetSquare(targetSquare))
                                TargetDirection = randomDir;
                            // if it was a miss, remove the current direction from the ViableDirections list and get a new random direction from the list
                            else
                            {
                                ViableDirections.Remove(randomDir);
                                randomDir = ViableDirections[rand.Next(ViableDirections.Count - 1)];
                            }
                            squareHit = true;
                        }
                        // when we have a targetdirection
                        else
                        {
                            // if we've missed, invert the targetdirection
                            if (!TargetSquare(targetSquare))
                                TargetDirection = InvertDirection(TargetDirection);
                            squareHit = true;
                        }

                        if (squareHit)
                        {
                            foreach (Ship ship in this.board.Ships)
                            {
                                // if we destroyed the ship, reset the targetdirection and the viabledirections list
                                if (ship.isDestroyed() && ship.ShipParts.Any(x => x == targetSquare))
                                {
                                    TargetDirection = Direction.NONE;
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

        // executes a move, based on the chosen difficulty
        // an easy ai only targets (partially) random squares, while the normal ai uses the targetship algorithm
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

        // inverts a direction
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
