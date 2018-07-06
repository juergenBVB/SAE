using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    /*
     * The Player class
     * Contains all player based logic, like placing ships and targeting enemy ships 
     */
    class Player
    {
        // ObservableCollections enable GUI components to observe the list
        protected ObservableCollection<Square> hitLog;
        
        // a list that is used for placing ships
        protected List<Square> legalSquares;
        protected GameBoard board;
        protected Random rand;
        protected List<Direction> ViableDirections;

        internal ObservableCollection<Square> HitLog
        {
            get { return hitLog; }
            set { hitLog = value; }
        }

        public GameBoard Board
        {
            get { return board; }
            set { board = value; }
        }

        public Player()
        {
            this.HitLog = new ObservableCollection<Square>();
        }
        public Player(GameBoard board)
        {
            this.HitLog = new ObservableCollection<Square>();
            this.board = board;
            rand = new Random(DateTime.Now.Millisecond);
            this.legalSquares = new List<Square>(board.Squares);
            ResetDirections();
        }

        // Checks if two squares are within a distance of 1
        public Boolean SquaresInProximity(Square sq1, Square sq2)
        {
            int distance = (int)Math.Sqrt(Math.Pow(sq2.PositionX - sq1.PositionX, 2) + Math.Pow(sq2.PositionY - sq1.PositionY, 2));
            return distance <= 1;
        }

        // Refills the ViableDirections list, which usually contains the directions UP, DOWN, LEFT and RIGHT
        protected void ResetDirections()
        {
            this.ViableDirections = new List<Direction>();
            for (int i = 0; i < 4; i++)
                ViableDirections.Add((Direction)i);
        }

        // returns the list of all targetable squares on the opponent's gameboard
        protected List<Square> GetLegalSquaresOpp()
        {
            List<Square> sqList = new List<Square>();
            Boolean tooCloseToShip = false;
            int shortestShipLength = int.MaxValue;
            foreach (Square sq in this.board.Squares)
            {
                tooCloseToShip = false;
                if (!sq.IsHit)
                {
                    // find destroyed ships and check if the target square is within distance of 1 to that ship
                    // if it is, then ignore that square
                    foreach (Ship ship in this.board.Ships)
                    {
                        if (ship.isDestroyed())
                        {
                            foreach (Square shipsq in ship.ShipParts)
                            {
                                if (SquaresInProximity(shipsq, sq))
                                {
                                    tooCloseToShip = true;
                                    break;
                                }
                            }
                        }
                        else
                            if (ship.Length < shortestShipLength)
                            shortestShipLength = ship.Length;

                        if (tooCloseToShip)
                            break;
                    }

                    // only target squares that aren't blocked off by already hit squares and board limits
                    // add them if the proximity could fit the smallest not destroyed ship
                    Boolean legalDirExists = false;
                    foreach (Direction d in ViableDirections)
                    {
                        Boolean shipFits = true;
                        for (int i = 0; i < shortestShipLength - 1; i++)
                        {
                            Square tempSquare = GetNextSquareInDirection(sq, d, this.board.Squares);
                            if (ReferenceEquals(tempSquare, null))
                                shipFits = false;
                            else if (tempSquare.IsHit)
                                shipFits = false;

                            if (shipFits)
                                break;
                        }
                        if (shipFits)
                        {
                            legalDirExists = true;
                            break;
                        }
                    }
                    // add the square if it's neither too close to a destroyed ship, nor blocked off
                    if (!tooCloseToShip && legalDirExists)
                        sqList.Add(sq);
                }
            }
            return sqList;
        }

        // target a specific square on the opponent's gameboard
        public Boolean TargetSquare(Square sq)
        {
            // if square is already hit or doesn't exist at all, return false and target nothing
            if (this.board.Squares.Any(x => x == sq) && !sq.IsHit)
            {
                hitLog.Add(sq);
                this.board.Squares.Find(x => x == sq).IsHit = true;

                // if square is actually a shippart, destroy it
                if (sq.IsShipPart())
                {
                    foreach (Ship ship in board.Ships)
                        if (ship.DestroyShipPart(sq))
                        {
                            (sq as ShipPart).Destroy();
                            return true;
                        }
                }
            }
            return false;
        }

        // checks if we have found a ship on the gameboard
        public Boolean ShipFound()
        {
            foreach (Square sq in this.board.Squares)
            {
                if (sq.IsShipPart())
                {
                    foreach (Ship ship in this.board.Ships)
                    {
                        // only return true if the ship wasnt destroyed already and contains the square
                        if (!ship.isDestroyed() && ship.ShipParts.Any(x => x == sq) && sq.IsHit)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // returns a random 'legal' square, belonging to either the player itself or the opponent 
        protected Square GetRandomLegalSquare(Boolean isTarget = false)
        {
            Square sq;
            if (isTarget)
            {
                List<Square> squareList;
                squareList = new List<Square>(GetLegalSquaresOpp());
                sq = squareList[rand.Next(0, squareList.Count - 1)];
            }
            else
                sq = legalSquares[rand.Next(0, legalSquares.Count - 1)];
            return sq;
        }

        // randomly places all ships on the gameboard
        public List<Ship> PlaceShips()
        {
            List<Ship> tempList = new List<Ship>();
            ShipPart sp;
            int shipLength = 0;
            Direction d;

            foreach (Ship s in this.board.Ships)
            {
                sp = new ShipPart(GetRandomLegalSquare());
                shipLength = s.GetInitialShipLength();
                d = (Direction)rand.Next(3);
                int tries = 0;

                // try to find a direction that fits the length of the ship
                // if we haven't found a direction after x tries, get a new square
                while (!IsLegalDirection(sp, shipLength, d) && tries < 20)
                {
                    if (IsLegalDirection(sp, shipLength, d))
                        break;
                    else
                    {
                        d = (Direction)rand.Next(3);
                        tries++;
                        if (tries == 19)
                        {
                            sp = new ShipPart(GetRandomLegalSquare(false));
                            tries = 0;
                        }
                    }
                }      

                RemoveIllegalSquares(s.PlaceShip(sp, d));
                tempList.Add(s);
            }
            return tempList;
        }

        // removes a list of illegal squares from the legalSquares list
        private void RemoveIllegalSquares(List<ShipPart> squares)
        {
            foreach (Square sq in squares)
            {
                // remove all bordering squares, since those aren't legal for placing other ships
                Boolean leftsq, rightsq, upsq, downsq, upleftsq, downleftsq, uprightsq, downrighsq;
                leftsq = legalSquares.Any(x => x.PositionX == sq.PositionX - 1 && x.PositionY == sq.PositionY);
                rightsq = legalSquares.Any(x => x.PositionX == sq.PositionX + 1 && x.PositionY == sq.PositionY);
                upsq = legalSquares.Any(x => x.PositionX == sq.PositionX && x.PositionY == sq.PositionY - 1);
                downsq = legalSquares.Any(x => x.PositionX == sq.PositionX && x.PositionY == sq.PositionY + 1);
                upleftsq = legalSquares.Any(x => x.PositionX == sq.PositionX - 1 && x.PositionY == sq.PositionY - 1);
                downleftsq = legalSquares.Any(x => x.PositionX == sq.PositionX - 1 && x.PositionY == sq.PositionY + 1);
                uprightsq = legalSquares.Any(x => x.PositionX == sq.PositionX + 1 && x.PositionY == sq.PositionY - 1);
                downrighsq = legalSquares.Any(x => x.PositionX == sq.PositionX + 1 && x.PositionY == sq.PositionY + 1);
                if (leftsq)
                {
                    legalSquares.Remove(legalSquares.Find(x => x == GetNextSquareInDirection(sq, Direction.LEFT, legalSquares)));
                }
                if (rightsq)
                {
                    legalSquares.Remove(legalSquares.Find(x => x == GetNextSquareInDirection(sq, Direction.RIGHT, legalSquares)));
                }
                if (upsq)
                {
                    legalSquares.Remove(legalSquares.Find(x => x == GetNextSquareInDirection(sq, Direction.UP, legalSquares)));
                }
                if (downsq)
                {
                    legalSquares.Remove(legalSquares.Find(x => x == GetNextSquareInDirection(sq, Direction.DOWN, legalSquares)));
                }
                if (upleftsq)
                {
                    legalSquares.Remove(legalSquares.Find(x => x == GetNextSquareInDirection(sq, Direction.UPLEFT, legalSquares)));
                }
                if (downleftsq)
                {
                    legalSquares.Remove(legalSquares.Find(x => x == GetNextSquareInDirection(sq, Direction.DOWNLEFT, legalSquares)));
                }
                if (uprightsq)
                {
                    legalSquares.Remove(legalSquares.Find(x => x == GetNextSquareInDirection(sq, Direction.UPRIGHT, legalSquares)));
                }
                if (downrighsq)
                {
                    legalSquares.Remove(legalSquares.Find(x => x == GetNextSquareInDirection(sq, Direction.DOWNRIGHT, legalSquares)));
                }
                legalSquares.Remove(sq);
            }
        }

        // returns the next square in a direction d
        // returns null if the square would be outside the board
        protected Square GetNextSquareInDirection(Square sq, Direction d, List<Square> source)
        {
            Square tempSquare = null;
            switch (d)
            {
                case Direction.NONE:
                    break;
                case Direction.UP:
                    tempSquare = source.Find(x => x.PositionX == sq.PositionX && x.PositionY == sq.PositionY - 1);
                    break;
                case Direction.DOWN:
                    tempSquare = source.Find(x => x.PositionX == sq.PositionX && x.PositionY == sq.PositionY + 1);
                    break;
                case Direction.LEFT:
                    tempSquare = source.Find(x => x.PositionX == sq.PositionX - 1 && x.PositionY == sq.PositionY);
                    break;
                case Direction.RIGHT:
                    tempSquare = source.Find(x => x.PositionX == sq.PositionX + 1 && x.PositionY == sq.PositionY);
                    break;
                case Direction.UPLEFT:
                    tempSquare = source.Find(x => x.PositionX == sq.PositionX - 1 && x.PositionY == sq.PositionY - 1);
                    break;
                case Direction.DOWNLEFT:
                    tempSquare = source.Find(x => x.PositionX == sq.PositionX - 1 && x.PositionY == sq.PositionY + 1);
                    break;
                case Direction.UPRIGHT:
                    tempSquare = source.Find(x => x.PositionX == sq.PositionX + 1 && x.PositionY == sq.PositionY - 1);
                    break;
                case Direction.DOWNRIGHT:
                    tempSquare = source.Find(x => x.PositionX == sq.PositionX + 1 && x.PositionY == sq.PositionY + 1);
                    break;
                default:
                    break;
            }
            return tempSquare;
        }

        // checks if direction d is a viable direction for the given length of a ship

        protected Boolean IsLegalDirection(Square sq, int shipLength, Direction d, Boolean Opp = false)
        {
            Boolean DirectionIsLegal = true;
            int X = sq.PositionX;
            int Y = sq.PositionY;
            for (int i = 0; i < shipLength; i++)
            {
                switch (d)
                {
                    case Direction.UP:
                        if (!Opp && !legalSquares.Any(x => x.PositionX == X && x.PositionY == Y - 1))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Any(x => x.PositionX == X && x.PositionY == Y - 1))
                            DirectionIsLegal = false;
                        else
                            Y--;
                        break;
                    case Direction.DOWN:
                        if (!Opp && !legalSquares.Any(x => x.PositionX == X && x.PositionY == Y + 1))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Any(x => x.PositionX == X && x.PositionY == Y + 1))
                            DirectionIsLegal = false;
                        else
                            Y++;
                        break;
                    case Direction.LEFT:
                        if (!Opp && !legalSquares.Any(x => x.PositionX == X - 1 && x.PositionY == Y))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Any(x => x.PositionX == X - 1 && x.PositionY == Y))
                            DirectionIsLegal = false;
                        else
                            X--;
                        break;
                    case Direction.RIGHT:
                        if (!Opp && !legalSquares.Any(x => x.PositionX == X + 1 && x.PositionY == Y))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Any(x => x.PositionX == X + 1 && x.PositionY == Y))
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
    }
}
