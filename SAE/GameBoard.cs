using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class GameBoard
    {
        private int size;

        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        private List<Ship> ships;

        internal List<Ship> Ships
        {
            get { return ships; }
            set { ships = value; }
        }
        private List<Square> squares;

        internal List<Square> Squares
        {
            get { return squares; }
            set { squares = value; }
        }

        public GameBoard(int size, List<Ship> ships)
        {
            this.size = size;
            this.ships = ships;
            GenerateSquares();
            GenerateShips();
        }

        private void GenerateSquares()
        {
            int linex = 0;
            int liney = 0;
            for (int i = 0; i < Math.Pow(size, 2); i++)
            {
                if (linex == this.size)
                {
                    linex = 0;
                    liney++;
                }
                this.squares.Add(new Square(linex, liney));
                linex++;
            }
        }

        private void GenerateShips()
        {
            ships.Add(new Ship(ShipTypes.UBoat, new List<ShipPart>()));
            ships.Add(new Ship(ShipTypes.Destroyer, new List<ShipPart>()));
            ships.Add(new Ship(ShipTypes.Frigate, new List<ShipPart>()));
            ships.Add(new Ship(ShipTypes.Battleship, new List<ShipPart>()));
            ships.Add(new Ship(ShipTypes.AircraftCarrier, new List<ShipPart>()));

        }
    }
}
