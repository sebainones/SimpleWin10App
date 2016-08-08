using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace RateTileUpdater
{
    public sealed class UpdateTask : IBackgroundTask
    {

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (taskInstance == null) return;

            taskInstance.Canceled += TaskInstance_Canceled;
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");

            // TODO: Insert code to start one or more asynchronous methods using the
            //       await keyword, for example:
            //
            // await ExampleMethodAsync();

            //TODO: Remove this.
            //var toast = NotificationsExtensions.ToastContent.ToastContentFactory.CreateToastText01();
            //toast.TextBodyWrap.Text = "ACTUALIZANDOOOOOOOOOOO!";
            //Windows.UI.Notifications.ToastNotificationManager.CreateToastNotifier().Show(toast.CreateNotification());

            _deferral.Complete();

            Debug.WriteLine("Background " + taskInstance.Task.Name + " Completed");
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {

        }
    }
}
