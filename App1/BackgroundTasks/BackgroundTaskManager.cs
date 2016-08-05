using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace RateApp.BackgroundTasks
{
    public class BackgroundTaskManager
    {
        public  IBackgroundTaskRegistration RegisterBackGroundTask(string taskName, string entryPoint)
        {
            return RegisterBackGroundTask(taskName, entryPoint, new SystemTrigger(SystemTriggerType.InternetAvailable, false), null);
        }

        //
        // Register a background task with the specified taskEntryPoint, name, trigger,
        // and condition (optional).
        //
        // taskEntryPoint: Task entry point for the background task.
        // taskName: A name for the background task.
        // trigger: The trigger for the background task.
        // condition: Optional parameter. A conditional event that must be true for the task to fire.
        public static IBackgroundTaskRegistration RegisterBackGroundTask(string taskName, string entryPoint, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            var backgorundTaskRegistered = GetBackgroundTaskIfAlreadyRegistered(taskName);

            if (backgorundTaskRegistered != null)
                return backgorundTaskRegistered;

            BackgroundTaskBuilder backgorundBuilder = CreateBackgroundTask(taskName, entryPoint);

            backgorundBuilder.SetTrigger(trigger);

            if (condition != null)
            {
                backgorundBuilder.AddCondition(condition);
            }

            return backgorundBuilder.Register();
        }

        private static BackgroundTaskBuilder CreateBackgroundTask(string taskName, string entryPoint)
        {
            return new BackgroundTaskBuilder()
            {
                Name = taskName,
                TaskEntryPoint = entryPoint
            };
        }

        ///For Windows Phone Store apps, you must call RequestAccessAsync before attempting to register any background task.
        ///On Windows, this call is only required for the set of background tasks that require your app to be on the lock screen to run
        private async Task<bool> IsAppHasPermission()
        {
            BackgroundAccessStatus bgStatus = default(BackgroundAccessStatus);

            try
            {
                ///Does not prompt the user, but must be called before registering any background tasks. 
                ///You do not need to add the app to the lock screen in order to use background tasks in Windows 10,
                ///but you still need to call RequestAccessAsync to request background access.
                ///
                ///On Windows 10, this call is only required for the set of background tasks that require your app to be on the lock screen to run!!!

                bgStatus = await BackgroundExecutionManager.RequestAccessAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsBackgroundAccessValid(bgStatus);
        }

        private bool IsBackgroundAccessValid(BackgroundAccessStatus bgStatus)
        {
            var status = default(bool);

            switch (bgStatus)
            {
                case BackgroundAccessStatus.Denied:
                case BackgroundAccessStatus.Unspecified:
                    break;
                case BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity:
                case BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity:
                    status = true;
                    break;
            }

            return status;
        }

        private static IBackgroundTaskRegistration GetBackgroundTaskIfAlreadyRegistered(string bgTaskName)
        {
            return BackgroundTaskRegistration.AllTasks.Where(taskReg => taskReg.Value.Name == bgTaskName).Select(bgTask1 => bgTask1.Value).FirstOrDefault();
        }
    }
}
