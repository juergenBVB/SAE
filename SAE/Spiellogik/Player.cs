using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class Player
    {
        protected ObservableCollection<Square> hitLog;
        protected List<Square> legalSquares;
        protected GameBoard board;
        protected Random rand;

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
        }

        // updates the list of all targetable squares
        protected List<Square> GetLegalSquaresOpp()
        {
            List<Square> sqList = new List<Square>();
            Boolean tooCloseToShip = false;
            foreach (Square sq in this.board.Squares)
            {
                if (!sq.IsHit)
                {
                    foreach (Ship ship in this.board.Ships)
                    {
                        foreach (Square shipsq in ship.ShipParts)
                        {
                            if (!(sq - shipsq))
                            {
                                tooCloseToShip = true;
                                break;
                            }
                        }
                        if (tooCloseToShip)
                            break;
                    }
                    if (!tooCloseToShip)
                        sqList.Add(sq);
                }
            }
            return sqList;
        }

        // target a specific square on the opponent's gameboard
        public Boolean TargetSquare(Square sq)
        {
            // if square isnt a legal target, do nothing and return false
            if (Board.Squares.Any(x => x == sq) && !sq.IsHit)
            {
                hitLog.Add(sq);
                this.board.Squares.Find(x => x == sq).IsHit = true;

                // if square is actually a shippart, destroy it
                if (sq.IsShipPart())
                {
                    foreach (Ship ship in board.Ships)
                        ship.DestroyShipPart(sq);

                    (sq as ShipPart).Destroy();
                    return true;
                }
            }
            return false;
        }

        public Boolean ShipFound()
        {
            foreach (Square sq in this.board.Squares)
            {
                if (sq.IsShipPart())
                {
                    foreach (Ship ship in this.board.Ships)
                    {
                        if (!ship.isDestroyed() && ship.ShipParts.Any(x => x == sq) && sq.IsHit)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

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

        // randomly places all ships for the ai
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

                // try to find a direction that fits the length of the ship
                while (!IsLegalDirection(sp, shipLength, d))
                {
                    // try finding a random direction the ship would fit in
                    int tries = 0;
                    while (!IsLegalDirection(sp, shipLength, d) && tries < 20)
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
                        sp = new ShipPart(GetRandomLegalSquare(false));
                    }
                }

                RemoveIllegalSquares(s.PlaceShip(sp, d));
                tempList.Add(s);
            }
            return tempList;
        }

        private void RemoveIllegalSquares(List<ShipPart> squares)
        {
            foreach (Square sq in squares)
            {
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

        protected Square GetNextSquareInDirection(Square sq, Direction d, List<Square> source)
        {
            Square tempSquare = sq;
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
