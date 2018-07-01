using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class Mapper
    {
        public static AIDifficulty TextToDiffictulty(string Difficulty)
        {
            switch (Difficulty)
            {
                case "Easy":
                    return AIDifficulty.Easy;
                case "Normal":
                    return AIDifficulty.Normal;
                default:
                    return AIDifficulty.Normal;
            }
        }
    }
}
