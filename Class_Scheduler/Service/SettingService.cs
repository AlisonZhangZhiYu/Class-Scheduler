﻿using Class_Scheduler.Common.Models;
using Class_Scheduler.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Class_Scheduler.Service
{
    public class SettingService : ISettingService
    {
        private UserSetting userSetting;
        private List<ISavableSettings> settings;

        public UserSetting UserSetting
        {
            get { return userSetting; }
            set { userSetting = value; }
        }

        public List<ISavableSettings> Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public SettingService(IRepositoryService repositoryService) 
        {
            UserSetting = repositoryService.UserSetting;
            settings = new List<ISavableSettings>();
        }

        public void RegisterData<T>(string MemberName, T value)
        {
            PropertyInfo? property = userSetting.GetType().GetProperty(MemberName);
            if (property == null)
                System.Diagnostics.Debug.WriteLine("Reflection Error: Property gotten is null {" +  MemberName + "}");

            property?.SetValue(userSetting, value);
            // System.Diagnostics.Debug.WriteLine(userSetting.GlobalUnavailableTimeSots.Count);
        }

        public void RegisterSettingInstance(ISavableSettings savableSettings)
        {
            foreach (var setting in Settings)
            {
                if (setting.ToString() == savableSettings.ToString())
                    return;
            }
            Settings.Add(savableSettings);
        }

        public void Save()
        {
            foreach (var setting in Settings)
            {
                setting.OnSave();
                System.Diagnostics.Debug.WriteLine(setting.ToString());
            }
            settings.Clear();
        }

        public void Reset(UserSetting userSetting)
        {
            foreach (var setting in Settings)
            {
                setting.Reset(userSetting);
            }
        }
    }
}
