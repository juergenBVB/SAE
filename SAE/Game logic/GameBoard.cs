using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    /*
     * The GameBoard class
     * contains a list of squares and a list of ships
     */
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
            this.squares = new List<Square>();
            GenerateSquares();
            GenerateShips();
        }

        // generates squares based on the boardsize
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

        // generates ships
        // the smaller the ship, the likelier it is to spawn
        private void GenerateShips(int count = -1)
        {
            //Random rand = new Random(DateTime.Now.Millisecond);
            //int temp = -1;

            //for (int i = 0; i < count; i++)
            //{
            //    temp = rand.Next(100);
            //    if (temp < 35)
            //        ships.Add(new Ship(ShipTypes.UBoat, new List<ShipPart>()));
            //    else if (temp < 60)
            //        ships.Add(new Ship(ShipTypes.Destroyer, new List<ShipPart>()));
            //    else if (temp < 75)
            //        ships.Add(new Ship(ShipTypes.Frigate, new List<ShipPart>()));
            //    else if (temp < 90)
            //        ships.Add(new Ship(ShipTypes.Battleship, new List<ShipPart>()));
            //    else if (true)
            //        ships.Add(new Ship(ShipTypes.AircraftCarrier, new List<ShipPart>()));
            //}

            ships.Add(new Ship(ShipTypes.UBoat, new List<ShipPart>()));
            ships.Add(new Ship(ShipTypes.Destroyer, new List<ShipPart>()));
            ships.Add(new Ship(ShipTypes.Frigate, new List<ShipPart>()));
            ships.Add(new Ship(ShipTypes.Battleship, new List<ShipPart>()));
            ships.Add(new Ship(ShipTypes.AircraftCarrier, new List<ShipPart>()));
        }

        // adds the ships to the gameboard
        public void AddShipsToBoard()
        {
            foreach (Ship ship in this.ships)
            {
                foreach (ShipPart sp in ship.ShipParts)
                {
                    this.Squares[GetIndexOfSquare(sp)] = sp;
                }
            }
        }

        // returns the index of a square
        private int GetIndexOfSquare(Square sq)
        {
            if (sq.PositionY > 0)
            {
                return sq.PositionX + sq.PositionY * this.size;
            }
            else
            {
                return sq.PositionX;
            }
        }

        // returns the index of a set of coordinates
        public static int GetIndexOfCoordinates(int x, int y, int size)
        {
            if (y > 0)
                return x + y * size;
            else
                return x;
        }

        // returns a square based on coordinates
        public Square GetSquareFromCoordinates(int x, int y)
        {
            Square tempSquare = new Square(x, y);
            foreach (Square sq in this.squares)
            {
                if (sq == tempSquare)
                {
                    return sq;
                }
            }
            return null;
        }
    }
}
