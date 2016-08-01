using System;
using Windows.UI.Popups;

namespace App1.Error
{
    internal class AdvancedMessageDialog : IMessageDialog
    {
        public async void SimpleMessageDialog(string message, string title)
        {
            MessageDialog dialog;

            if (string.IsNullOrEmpty(title))
            {
                dialog = new MessageDialog(message);
            }
            else
                dialog = new MessageDialog(message, title);

            await dialog.ShowAsync();
        }
    }
}
