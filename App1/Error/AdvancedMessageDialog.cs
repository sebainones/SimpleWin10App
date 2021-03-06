﻿using System;
using Windows.UI.Popups;

namespace RateApp.Error
{
    internal class AdvancedMessageDialog : IMessageDialog
    {
        public async void SimpleMessageDialogAsync(string message, string title)
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
