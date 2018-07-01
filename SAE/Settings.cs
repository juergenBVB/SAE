using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SAE
{
    [Serializable]
    class Settings
    {
        private int boardSize;
        private string playerColor;
        private string enemyColor;
        private AIDifficulty difficulty;
        private int shipCount;
        private GameModes gameMode;
        private string playerName;

        public Settings() {
            this.BoardSize = 20;
            this.PlayerColor = "blue";
            this.EnemyColor = "red";
            this.Difficulty = AIDifficulty.Easy;
            this.ShipCount = 5;
            this.GameMode = GameModes.Classic;
            this.PlayerName = "TestPlayer";
        }

        internal GameModes GameMode
        {
            get { return gameMode; }
            set { gameMode = value; }
        }

        public string PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }


        public int ShipCount
        {
            get { return shipCount; }
            set { shipCount = value; }
        }

        internal AIDifficulty Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }

        public string EnemyColor
        {
            get { return enemyColor; }
            set { enemyColor = value; }
        }

        public string PlayerColor
        {
            get { return playerColor; }
            set { playerColor = value; }
        }


        public int BoardSize
        {
            get { return boardSize; }
            set { boardSize = value; }
        }

        public void SaveSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter("../../config/settings.xml");
            serializer.Serialize(writer, this);
        }

    }
}
