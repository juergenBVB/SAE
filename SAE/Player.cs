﻿using System;
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

        public GameBoard Board { get => board; set => board = value; }

        public Player()
        {
            this.HitLog = new ObservableCollection<Square>();
        }
        public Player(GameBoard board)
        {
            this.HitLog = new ObservableCollection<Square>();
            this.Board = board;
            rand = new Random(DateTime.Now.Millisecond);
            this.legalSquares = GetLegalSquaresOpp();
        }

        // updates the list of all targetable squares
        protected List<Square> GetLegalSquaresOpp()
        {
            List<Square> sqList = new List<Square>();
            foreach (Square sq in Board.Squares)
            {
                if (!sq.IsHit)
                {
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
                Board.Squares.Find(x => x == sq).IsHit = true;

                // if square is actually a shippart, destroy it
                if (sq is ShipPart)
                {
                    (sq as ShipPart).Destroy();
                    return true;
                }
            }
            return false;
        }

        public Boolean ShipFound()
        {
            foreach (Square sq in Board.Squares)
            {
                if (sq is ShipPart)
                {
                    return true;
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
                squareList = GetLegalSquaresOpp();
                sq = squareList[rand.Next(squareList.Count - 1)];
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

            foreach (Ship s in this.board.Ships)
            {

                sp = new ShipPart(GetRandomLegalSquare());
                shipLength = s.GetShipLength();
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

                s.PlaceShip(sp, d);
                legalSquares.Remove(sp);
                tempList.Add(s);
            }
            return tempList;
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
                        if (!legalSquares.Any(x => x.PositionX == X && x.PositionY == Y - 1))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Any(x => x.PositionX == X && x.PositionY == Y - 1))
                            DirectionIsLegal = false;
                        else
                            Y--;
                        break;
                    case Direction.DOWN:
                        if (!legalSquares.Any(x => x.PositionX == X && x.PositionY == Y + 1))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Any(x => x.PositionX == X && x.PositionY == Y + 1))
                            DirectionIsLegal = false;
                        else
                            Y++;
                        break;
                    case Direction.LEFT:
                        if (!legalSquares.Any(x => x.PositionX == X - 1 && x.PositionY == Y))
                            DirectionIsLegal = false;
                        else if (Opp && !GetLegalSquaresOpp().Any(x => x.PositionX == X - 1 && x.PositionY == Y))
                            DirectionIsLegal = false;
                        else
                            X--;
                        break;
                    case Direction.RIGHT:
                        if (!legalSquares.Any(x => x.PositionX == X + 1 && x.PositionY == Y))
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
