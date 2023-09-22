using AutostartManagement;
using Evolution.Model;
using Evolution.Services.DataSaveLoadServices;
using Evolution.Services.HelperServices;
using System;
using System.Diagnostics;
using System.IO;

namespace Evolution.Services.SettingsServices
{
    public static class SettingsService
    {
        public static SettingsModel sm { get; set; } = new()
        {
            RememberMeForAuth = false,
            Login = "",
            Password = "",
            AutoRun = false,
            AutoLogin = false
        };

        public static void CreateSettingsFile()
        {
            string pathToSettingsFile = HelperService.pathToSettingsFile;
            string pathToSettingsFolder = HelperService.pathToSettingsFolder;

            if(!Directory.Exists(pathToSettingsFolder))
            {
                Directory.CreateDirectory(pathToSettingsFolder);
            }

            if (!File.Exists(pathToSettingsFile))
            {
                var file = File.Create(pathToSettingsFile);
                file.Close();
               
                DataSaveLoad.Serialize(sm);

                Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** ВНИМАНИЕ.\n\t [SettingsService]: Файл настроек создан. ******\n");
            }
        }

        public static void SetRememberDataForAuthSettings(string login, string password, bool isRememberMe)
        {
            sm = DataSaveLoad.LoadDataSettings<SettingsModel>(HelperService.pathToSettingsFile);
            sm.Login = login;
            sm.Password = password;
            sm.RememberMeForAuth = isRememberMe;
             
            DataSaveLoad.Serialize(sm);

            Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** ВНИМАНИЕ.\n\t [SettingsService]: Установлена настройка ЗАПОМНИТЬ МЕНЯ {isRememberMe} ******\n");
        }

        public static void SetAutoRunSettings(bool isAutoRun)
        {
           sm = DataSaveLoad.LoadDataSettings<SettingsModel>(HelperService.pathToSettingsFile);
           if (isAutoRun) { AutoRunState(true); } else { AutoRunState(false); };
           sm.AutoRun = isAutoRun;

           DataSaveLoad.Serialize(sm);
        }

        public static SettingsModel GetSettings()
        {
            sm = DataSaveLoad.LoadDataSettings<SettingsModel>(HelperService.pathToSettingsFile);
            return sm;
        }

        private static void AutoRunState(bool autoRunState = false)
        {
            string shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\Evolution";
            string ExeFilePath = Environment.CurrentDirectory + "\\Evolution.exe";

            if(autoRunState)
            {
                var registerShortcutForAllUser = true;
                var autostartManager = new AutostartManager(shortCutPath, ExeFilePath, registerShortcutForAllUser);
                autostartManager.EnableAutostart();
                Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** ВНИМАНИЕ.\n\t [SettingsService]: Установлена настройка автозагрузки приложения с запуском системы. ******\n");
            }
            else
            {
                var registerShortcutForAllUser = true;
                var autostartManager = new AutostartManager(shortCutPath, ExeFilePath, registerShortcutForAllUser);
                autostartManager.DisableAutostart();
                Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** ВНИМАНИЕ.\n\t [SettingsService]: Отключена настройка автозагрузки приложения с запуском системы. ******\n");
            }
        }
    }
}
