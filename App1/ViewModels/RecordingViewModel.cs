using Commands.App1;
using Model.App1;
using System.Windows.Input;

namespace App1.ViewModels
{
    public class RecordingViewModel
    {
        private RecordingModel defaultRecording = new RecordingModel();

        public RecordingModel DefaultRecording { get { return this.defaultRecording; } }

        public ICommand MyCommand
        {
            get;
            private set;
        }

        public RecordingViewModel()
        {
            MyCommand = new DelegateCommand<string>(ChangeStringAction);
        }

        private void ChangeStringAction(string obj)
        {
            defaultRecording.EndResult = "CAMBIO";
        }
    }
}
