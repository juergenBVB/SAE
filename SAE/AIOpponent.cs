using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class AIOpponent : Player
    {
        private Difficulties difficulty;
        private List<Square> legalSquares;
        private Random rand;

        public AIOpponent(GameBoard board, List<Ship> ships, Difficulties diff) : base(board, ships)
        {
            this.difficulty = diff;
            rand = new Random(DateTime.Now.Millisecond);
            legalSquares = GetLegalSquaresOpp();
            PlaceShips();
        }

        public Square GetRandomLegalSquare(Boolean isTarget)
        {
            Square sq;
            if (isTarget)
            {
                sq = GetLegalSquaresOpp()[rand.Next(legalSquares.Count - 1)];
            }
            else
                sq = legalSquares[rand.Next(legalSquares.Count - 1)];
            return sq;
        }

        // randomly places all ships for the ai
        public List<Ship> PlaceShips()
        {
            List<Ship> tempList = new List<Ship>();
            ShipPart sp;
            int shipLength = 0;
            Direction d;

            foreach (Ship s in Ships)
            {
                sp = (ShipPart)GetRandomLegalSquare(false);
                shipLength = s.GetShipLength();
                d = (Direction)rand.Next(3);

                // try to find a direction that fits the length of the ship
                while (!IsLegalDirection(sp, shipLength, d))
                {
                    // try finding a random direction the ship would fit in
                    int tries = 0;
                    while (!IsLegalDirection(sp, shipLength, d) && tries < 10)
                    {
                        d = (Direction)rand.Next(3);
                        tries++;
                    }

                    if (IsLegalDirection(sp, shipLength, d))
                    {
                        break;
                    }
                    else
                    {
                        sp = (ShipPart)GetRandomLegalSquare(false);
                    }
                }

                s.PlaceShip(sp, d);
                legalSquares.Remove(sp);
                tempList.Add(s);
            }
            return tempList;
        }

        private Boolean IsLegalDirection(Square sq, int shipLength, Direction d, Boolean Opp = false)
        {
            Boolean DirectionIsLegal = true;
            int X = sq.PositionX;
            int Y = sq.PositionY;
            for (int i = 0; i < shipLength; i++)
            {
                switch (d)
                {
                    case Direction.UP:
                        if (!legalSquares.Contains(new ShipPart(X, Y - 1)))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Contains(new ShipPart(X, Y - 1)))
                            DirectionIsLegal = false;
                        else
                            Y--;
                        break;
                    case Direction.DOWN:
                        if (!legalSquares.Contains(new ShipPart(X, Y + 1)))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Contains(new ShipPart(X, Y + 1)))
                            DirectionIsLegal = false;
                        else
                            Y++;
                        break;
                    case Direction.LEFT:
                        if (!legalSquares.Contains(new ShipPart(X - 1, Y)))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Contains(new ShipPart(X - 1, Y)))
                            DirectionIsLegal = false;
                        else
                            X--;
                        break;
                    case Direction.RIGHT:
                        if (!legalSquares.Contains(new ShipPart(X + 1, Y)))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Contains(new ShipPart(X + 1, Y)))
                            DirectionIsLegal = false;
                        else
                            X++;
                        break;
                    default:
                        break;
                }
                if (!DirectionIsLegal)
                {
                    break;
                }
            }
            return DirectionIsLegal;
        }

        public Square TargetRandomSquare()
        {
            Square sq = GetRandomLegalSquare(true);
            TargetSquare(sq);
            return sq;
        }

        private void Destroy()
        {
            ShipPart sp = null;
            Boolean shipFits = false;
            Direction d = (Direction)0;
            Ship targetShip;

            foreach (Square sq in hitLog)
            {
                if (sq.CheckHit())
                {
                    sp = (ShipPart)sq;

                    if (!sp.Destroyed)
                    {
                        foreach (Ship ship in board.Ships)
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
                        {
                            break;
                        }
                    }
                }
            }

            if (shipFits)
            {
                TargetSquare(FindNextSquareInDirection(sp, d));
            }
        }

        private Square FindNextSquareInDirection(Square sq, Direction d)
        {
            Square tempSquare;
            switch (d)
            {
                case Direction.UP:
                    tempSquare = board.Squares.Find(x => x.PositionX == sq.PositionX && x.PositionY == sq.PositionY - 1);
                    break;
                case Direction.DOWN:
                    tempSquare = board.Squares.Find(x => x.PositionX == sq.PositionX && x.PositionY == sq.PositionY + 1);
                    break;
                case Direction.LEFT:
                    tempSquare = board.Squares.Find(x => x.PositionX == sq.PositionX - 1 && x.PositionY == sq.PositionY);
                    break;
                case Direction.RIGHT:
                    tempSquare = board.Squares.Find(x => x.PositionX == sq.PositionX + 1 && x.PositionY == sq.PositionY - 1);
                    break;
                default:
                    tempSquare = null;
                    break;
            }
            return tempSquare;
        }
    }
}
