using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

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

        public void StartTimer()
        {
            this.timerStart = DateTime.Now;
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Send);
            timer.Tick += new EventHandler(this.OnTimerTick);
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
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

        private void OnTimerTick(object source, EventArgs e)
        {
            DateTime endTime = DateTime.Now;

            TimeSpan span = endTime.Subtract(this.timerStart);
            this.TimerValue = span.ToString(@"hh\:mm\:ss");                       
        }
    }
}
