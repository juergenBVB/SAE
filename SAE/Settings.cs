using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SAE
{
    class Settings
    {
        private int boardSize;
        private Color playerColor;
        private Color enemyColor;
        private AIDifficulty difficulty;
        private int shipCount;
        private GameModes gameMode;
        private string playerName;

        public Settings() {
            this.BoardSize = 20;
            this.PlayerColor = Colors.Blue;
            this.EnemyColor = Colors.Red;
            this.Difficulty = AIDifficulty.Easy;
            this.ShipCount = 5;
            this.GameMode = GameModes.Classic;
            this.PlayerName = "TestPlayer";
        }

        public GameModes GameMode
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

        public AIDifficulty Difficulty
        {
            get { return difficulty; }
            set { difficulty = value; }
        }

        public Color EnemyColor
        {
            get { return enemyColor; }
            set { enemyColor = value; }
        }

        public Color PlayerColor
        {
            get { return playerColor; }
            set { playerColor = value; }
        }


        public int BoardSize
        {
            get { return boardSize; }
            set { boardSize = value; }
        }

        public static Settings LoadSettings()
        {
            using (StreamReader file = File.OpenText(@"../../config/settings.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (Settings)serializer.Deserialize(file, typeof(Settings));
            }
        }

        public void SaveSettings()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.Converters.Add(new StringEnumConverter());

            using (StreamWriter sw = new StreamWriter(@"../../config/settings.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, this);
            }
        }

    }
}
