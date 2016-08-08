using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace RateApp.BackgroundTasks
{
    public class BackgroundTaskManager
    {
        public async Task<IBackgroundTaskRegistration> RegisterBackGroundTask(string taskName, string entryPoint)
        {
            //If the OneShot property is false, freshnessTime specifies the interval between
            //recurring tasks.If FreshnessTime is set to less than 15 minutes, an exception
            //is thrown when attempting to register the background task.

            //create time trigger
            var timeTrigger = new TimeTrigger(15, false);

            return await RegisterBackGroundTask(taskName, entryPoint, timeTrigger, null);
        }

        /// <summary>
        /// Register a background task with the specified taskEntryPoint, name, trigger,
        /// and condition (optional).
        /// </summary>
        /// <param name="taskName">A name for the background task.</param>
        /// <param name="taskEntryPoint">Task entry point for the background task.</param>
        /// <param name="trigger">The trigger for the background task.</param>
        /// <param name="condition">An optional conditional event that must be true for the task to fire.</param>
        public async static Task<IBackgroundTaskRegistration> RegisterBackGroundTask(string taskName, string taskEntryPoint, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            var appHasPermission = await IsAppHasPermission();
            
            if (!appHasPermission)
                return null;

            var backgorundTaskRegistered = GetBackgroundTaskIfAlreadyRegistered(taskName);

            if (backgorundTaskRegistered != null)
                return backgorundTaskRegistered;

            BackgroundTaskBuilder backgorundBuilder = CreateBackgroundTask(taskName, taskEntryPoint);

            backgorundBuilder.SetTrigger(trigger);

            SetBackgroundTaskCondtion(condition, backgorundBuilder);
                        
            return  backgorundBuilder.Register();            
        }

        private static void SetBackgroundTaskCondtion(IBackgroundCondition condition, BackgroundTaskBuilder backgorundBuilder)
        {
            if (condition != null)
            {
                backgorundBuilder.AddCondition(condition);

                // If the condition changes while the background task is executing then it will be canceled.
                backgorundBuilder.CancelOnConditionLoss = true;
            }
        }

        private static BackgroundTaskBuilder CreateBackgroundTask(string taskName, string taskEntryPoint)
        {
            return new BackgroundTaskBuilder()
            {
                Name = taskName,
                TaskEntryPoint = taskEntryPoint
            };
        }

        ///For Windows Phone Store apps, you must call RequestAccessAsync before attempting to register any background task.
        ///On Windows, this call is only required for the set of background tasks that require your app to be on the lock screen to run
        private static async Task<bool> IsAppHasPermission()
        {
            BackgroundAccessStatus access = default(BackgroundAccessStatus);
            try
            {
                ///Does not prompt the user, but must be called before registering any background tasks. 
                ///You do not need to add the app to the lock screen in order to use background tasks in Windows 10,
                ///but you still need to call RequestAccessAsync to request background access.
                ///
                ///On Windows 10, this call is only required for the set of background tasks that require your app to be on the lock screen to run!!!
                //required call
                access = await BackgroundExecutionManager.RequestAccessAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsBackgroundAccessValid(access);
        }

        private static bool IsBackgroundAccessValid(BackgroundAccessStatus bgStatus)
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
