using System;
using Windows.UI.Popups;

namespace App1.Error
{
    internal class AdvancedMessageDialog : IMessageDialog
    {
        public async void SimpleMessageDialog(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}
