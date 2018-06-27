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
    }
}
