using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SAE
{
    class SquareView : Square, INotifyPropertyChanged
    {
        private SolidColorBrush backgroundColor;

        public SquareView(int x, int y, SolidColorBrush color) : base(x, y)
        {            
            this.BackgroundColor = color;
        }

        public SolidColorBrush BackgroundColor
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

        public override string ToString()
        {
            return "";
        }
    }
}
