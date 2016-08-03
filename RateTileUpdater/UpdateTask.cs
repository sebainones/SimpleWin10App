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

            //
            // TODO: Insert code to start one or more asynchronous methods using the
            //       await keyword, for example:
            //
            // await ExampleMethodAsync();
            //

            _deferral.Complete();
                        
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            
        }
    }
}
