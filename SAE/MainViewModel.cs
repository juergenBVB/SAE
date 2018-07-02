using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool startScreenVisible = false;
        private bool endScreenVisible = false;
        private bool mainScreenVisible = false;
        private Game mainGame;

        public bool StartScreenVisible
        {
            get { return startScreenVisible; }

            set
            {
                startScreenVisible = value;
                NotifyPropertyChanged("StartScreenVisible");
            }
        }

        public bool EndScreenVisible
        {
            get { return endScreenVisible; }

            set
            {
                endScreenVisible = value;
                NotifyPropertyChanged("EndScreenVisible");
            }
        }

        public bool MainScreenVisible {
            get { return mainScreenVisible; }

            set
            {
                mainScreenVisible = value;
                NotifyPropertyChanged("MainScreenVisible");
            }
        }

        internal Game MainGame { get => mainGame; set => mainGame = value; }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
