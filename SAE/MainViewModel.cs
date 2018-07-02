using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SAE
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool startScreenVisible = false;
        private bool endScreenVisible = false;
        private bool mainScreenVisible = false;
        private string timerValue;
        private DateTime timerStart;
        private Game mainGame;

        public MainViewModel()
        {
            this.timerStart = DateTime.Now;
            Timer timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(this.OnTimerTick);
            timer.Interval = 1000;
            timer.Enabled = true;            
        }

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

        public string TimerValue
        {
            get { return timerValue; }

            set
            {
                timerValue = value;
                NotifyPropertyChanged("TimerValue");
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

        private void OnTimerTick(object source, ElapsedEventArgs e)
        {
            DateTime endTime = DateTime.Now;

            TimeSpan span = endTime.Subtract(this.timerStart);
            this.TimerValue = span.ToString();
           
        }
    }
}
