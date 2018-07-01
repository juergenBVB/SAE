using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class ShipPart:Square
    {
        
        Boolean destroyed = false;

        public Boolean Destroyed
        {
            get { return destroyed; }
            set { Destroy(); }
        }

        public ShipPart(int x, int y):base(x,y)
        {
           
        }

        public void Destroy()
        {
            destroyed = true;
        }
    }
}
