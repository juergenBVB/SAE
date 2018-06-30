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
        private Random rand;

        public AIOpponent(GameBoard board, List<Ship> ships, Difficulties diff) : base(board, ships)
        {
            this.difficulty = diff;
            rand = new Random(DateTime.Now.Millisecond);
            PlaceShips();
        }

        public ShipPart GetRandomLegalSquare()
        {
            ShipPart sp;
            do
            {
                sp = new ShipPart(rand.Next(board.Size - 1), rand.Next(board.Size - 1));
            }
            while (!legalSquares.Contains(sp));
            return sp;
        }

        // randomly places all ships for the ai
        public List<Ship> PlaceShips()
        {
            List<Ship> tempList = new List<Ship>();
            ShipPart sp;
            int shipLength = 0;
            Direction d;

            GetLegalSquares();
            foreach (Ship s in Ships)
            {
                sp = GetRandomLegalSquare();
                shipLength = s.GetShipLength();
                d = (Direction)rand.Next(3);

                // try to find a direction that fits the length of the ship
                while (!IsLegalDirection(sp, shipLength, d))
                {
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
                        sp = GetRandomLegalSquare();
                    }
                }

                s.PlaceShip(sp, d);
                tempList.Add(s);
            }
            return tempList;
        }

        private Boolean IsLegalDirection(Square sq, int shipLength, Direction d)
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
                        {
                            DirectionIsLegal = false;
                        }
                        else
                        {
                            Y--;
                        }
                        break;
                    case Direction.DOWN:
                        if (!legalSquares.Contains(new ShipPart(X, Y + 1)))
                        {
                            DirectionIsLegal = false;
                        }
                        else
                        {
                            Y++;
                        }
                        break;
                    case Direction.LEFT:
                        if (!legalSquares.Contains(new ShipPart(X - 1, Y)))
                        {
                            DirectionIsLegal = false;
                        }
                        else
                        {
                            X--;
                        }
                        break;
                    case Direction.RIGHT:
                        if (!legalSquares.Contains(new ShipPart(X + 1, Y)))
                        {
                            DirectionIsLegal = false;
                        }
                        else
                        {
                            X++;
                        }
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
    }
}
