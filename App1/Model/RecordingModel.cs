using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model.App1
{
    public class RecordingModel : INotifyPropertyChanged
    {
        public string ArtistName { get; set; }
        public string CompositionName { get; set; }
        public DateTime ReleaseDateTime { get; set; }

        private string endResult;
        public string EndResult
        {
            get { return endResult; }
            set
            {
                if (value != this.endResult)
                {
                    this.endResult = value;
                        NotifyPropertyChanged(); //raise the event.
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RecordingModel()
        {
            this.ArtistName = "Wolfgang Amadeus Mozart";
            this.CompositionName = "Andante in C for Piano";
            this.ReleaseDateTime = new DateTime(1761, 1, 1);
            EndResult = "ESTAAA";
        }

        public string OneLineSummary
        {
            get
            {
                return $"{this.CompositionName} by {this.ArtistName}, released: " + this.ReleaseDateTime.ToString("d");
            }
        }
    }
}
