using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    class AIOpponent:Player
    {
        private List<Ship> playerShips;
        private AIDifficulty difficulty;

        internal List<Ship> PlayerShips
        {
            get { return playerShips; }
            set { playerShips = value; }
        }

        public AIOpponent(AIDifficulty diff)
        {
            this.difficulty = diff;
        }
    }
}
