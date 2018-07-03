using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SAE
{
    class SquareView : INotifyPropertyChanged
    {
        private Color backgroundColor;

        public SquareView()
        {
            this.BackgroundColor = Colors.White;
        }

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; this.NotifyPropertyChanged("BackgroundColor"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
