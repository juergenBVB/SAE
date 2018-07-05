using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class Ship
    {
        private ShipTypes shipType;
        private List<ShipPart> shipParts;
        private Boolean destroyed = false;
        private int length;

        public Boolean Destroyed
        {
            get { return destroyed; }
            set { destroyed = value; }
        }

        internal List<ShipPart> ShipParts
        {
            get { return shipParts; }
            set { shipParts = value; }
        }

        internal ShipTypes ShipType
        {
            get { return shipType; }
            set { shipType = value; }
        }

        public int Length { get => length; set => length = value; }

        public Ship(ShipTypes st, List<ShipPart> parts)
        {
            this.shipType = st;
            this.shipParts = parts;
        }

        public Boolean isDestroyed()
        {
            foreach (ShipPart part in this.shipParts)
            {
                if (!part.Destroyed)
                {
                    return false;
                }
            }
            return true;
        }

        public List<ShipPart> PlaceShip(Square sq, Direction d)
        {
            switch (ShipType)
            {
                case ShipTypes.UBoat:
                    shipParts.AddRange(AddSquaresInDirection(sq, 2, d));
                    break;
                case ShipTypes.Destroyer:
                    shipParts.AddRange(AddSquaresInDirection(sq, 3, d));
                    break;
                case ShipTypes.Frigate:
                    shipParts.AddRange(AddSquaresInDirection(sq, 4, d));
                    break;
                case ShipTypes.Battleship:
                    shipParts.AddRange(AddSquaresInDirection(sq, 5, d));
                    break;
                case ShipTypes.AircraftCarrier:
                    shipParts.AddRange(AddSquaresInDirection(sq, 6, d));
                    break;
                default:
                    break;
            }
            return shipParts;
        }

        private List<ShipPart> AddSquaresInDirection(Square sq, int count, Direction d)
        {
            List<ShipPart> tempList = new List<ShipPart>();
            int Y = sq.PositionY;
            int X = sq.PositionX;
            for (int i = 0; i < count; i++)
            {
                switch (d)
                {
                    case Direction.UP:
                        tempList.Add(new ShipPart(X, Y - 1));
                        Y--;
                        break;
                    case Direction.DOWN:
                        tempList.Add(new ShipPart(X, Y + 1));
                        Y++;
                        break;
                    case Direction.LEFT:
                        tempList.Add(new ShipPart(X - 1, Y));
                        X--;
                        break;
                    case Direction.RIGHT:
                        tempList.Add(new ShipPart(X + 1, Y));
                        X++;
                        break;
                    default:
                        break;
                }
            }
            return tempList;
        }

        public int GetInitialShipLength()
        {
            int length;
            switch (shipType)
            {
                case ShipTypes.UBoat:
                    length = 2;
                    break;
                case ShipTypes.Destroyer:
                    length = 3;
                    break;
                case ShipTypes.Frigate:
                    length = 4;
                    break;
                case ShipTypes.Battleship:
                    length = 5;
                    break;
                case ShipTypes.AircraftCarrier:
                    length = 6;
                    break;
                default:
                    length = 0;
                    break;
            }
            return length;
        }

        public Boolean DestroyShipPart(Square sq)
        {
            if (this.shipParts.Any(x => x == sq))
            {
                this.shipParts.Find(x => x == sq).Destroy();
                return true;
            }
            return false;
        }
    }
}
