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
        private Player owner;

        public Player Owner
        {
            get { return owner; }
            set { owner = value; }
        }

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

        public Ship(List<ShipPart> parts, Player owner)
        {
            this.shipParts = parts;
            this.owner = owner;
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

        public void PlaceShip(Square sq, Direction d)
        {
            switch (ShipType)
            {
                case ShipTypes.UBoat:
                    shipParts.AddRange(AddSquaresInDirection(sq.PositionX, sq.PositionY, 2, d));
                    break;
                case ShipTypes.Destroyer:
                    shipParts.AddRange(AddSquaresInDirection(sq.PositionX, sq.PositionY, 3, d));
                    break;
                case ShipTypes.Frigate:
                    shipParts.AddRange(AddSquaresInDirection(sq.PositionX, sq.PositionY, 4, d));
                    break;
                case ShipTypes.Battleship:
                    shipParts.AddRange(AddSquaresInDirection(sq.PositionX, sq.PositionY, 5, d));
                    break;
                case ShipTypes.AircraftCarrier:
                    shipParts.AddRange(AddSquaresInDirection(sq.PositionX, sq.PositionY, 6, d));
                    break;
                default:
                    break;
            }
        }

        private List<ShipPart> AddSquaresInDirection(int startX, int startY, int count, Direction d)
        {
            List<ShipPart> tempList = new List<ShipPart>();
            int Y = startY;
            int X = startX;
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

        public int GetShipLength()
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
    }
}
