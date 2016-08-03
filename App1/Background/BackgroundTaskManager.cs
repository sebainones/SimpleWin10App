﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace App1.Background
{
    public class BackgroundTaskManager
    {
        public async Task<IBackgroundTaskRegistration> RegisterBackGroundTask()
        {
            //var hasPermission = await IsAppHasPermission();

            //if (hasPermission)
            //{
                //TODO: The name of the task and the TaskEntry point should be in some way paramters
                var backgorundTaskRegistered = IsBackgroundTaskRegistered("UpdateTask");

                if (backgorundTaskRegistered != null)
                    return backgorundTaskRegistered;

                var objBg = new BackgroundTaskBuilder()
                {
                    Name = "UpdateTask",
                    TaskEntryPoint = "RateTileUpdater.UpdateTask"
                };

                objBg.SetTrigger(new SystemTrigger(SystemTriggerType.InternetAvailable, false));
                //objBg.AddCondition(new SystemCondition(SystemConditionType.UserPresent)); //Just in case we disable it.

                backgorundTaskRegistered = objBg.Register();
                return backgorundTaskRegistered;
            //}

            //return null;
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

        private static IBackgroundTaskRegistration IsBackgroundTaskRegistered(string bgTaskName)
        {
            var bgTask = BackgroundTaskRegistration.AllTasks.Where(taskReg => taskReg.Value.Name == bgTaskName).Select(bgTask1 => bgTask1.Value).FirstOrDefault();
            return bgTask;
        }
    }
}
