using Evolution.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using File = Google.Apis.Drive.v3.Data.File;
using System.IO;
using System.Threading.Tasks;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.DataSaveLoadServices;
using Evolution.Services.HelperServices;

namespace Evolution.Services.UserServices
{
    public static class CreateUserService
    {
        private static string PathToUserFolder;
        public static string PathToUserAuthFile;
        public static string EVOLUTIONFolder;

        private static ObservableCollection<File> FilesAndFolders = new ObservableCollection<File>();

        public static UserModel User = new();

        public static UserModel CreateUser(string? login, string? password, string? confirmPassword, string Email = "none")
        {

            if (IsValidUserData(login, password, confirmPassword, Email))
            {
                PathToUserAuthFile = CreateUserJSON(login);

                if (PathToUserAuthFile == "error: User already exists.")
                {
                    Debug.WriteLine("Ошибка. Пользователь существует.");
                    return null;
                }
                else
                {                   
                    User.Login = login;
                    User.Email = Email;
                    User.Password = password;
                    User.DateCreate = DateTime.Now;
                    
                    DataSaveLoad.Serialize(User);
                                        
                    return User;
                }
            }
            else
            {
                return null;
            }
        }

        private static bool IsValidUserData(string? login, string? password, string? confirmPassword, string Email = "none")
        {
            var email = new EmailAddressAttribute();

            if (
                    login == null ||
                    password == null ||
                    confirmPassword == null
               )
            {
                Debug.WriteLine("Ошибка, один из параметрова null!\nПользователь не был создан.");
                return false;
            }
            else if
                (
                    (login.Trim().Length < 3) || login.Contains(" ") ||
                    (password.Trim().Length < 3) || password.Contains(" ") || confirmPassword != password ||
                    (!email.IsValid(Email) && Email != "none" && Email != "")
                )
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private static string CreateUserJSON(string? login)
        {
            PathToUserFolder = $"{HelperService.pathToUsersFolder}\\{login}";
            PathToUserAuthFile = $"{PathToUserFolder}\\user_auth.json";

            if (!Directory.Exists(PathToUserFolder))
            {
                Directory.CreateDirectory(PathToUserFolder);
                var file = System.IO.File.Create($"{PathToUserFolder}\\user_auth.json");
                file.Close();
                return PathToUserAuthFile;
            }
            else
            {
                return "error: User already exists.";
            }
        }

        public static async void CreateUserFolderInGDrive(string login, bool isCreate = false)
        {
                                 
            FilesAndFolders = await Task.Run(() => GoogleDriveService.ListEntities()); //Получаем весь список файлов и папок в корне
            
            EVOLUTIONFolder = GetItemIDByName("EVOLUTION"); //айди папки EVOLUTION
            
            if(isCreate) await Task.Run(() => GoogleDriveService.CreateFolder(login, EVOLUTIONFolder)); //Создание папки пользователя        

            FilesAndFolders = await Task.Run(() => GoogleDriveService.ListEntities(EVOLUTIONFolder)); //Весь список файлов и папок в ПАПКЕ EVOLUTION            
            string UserFolder = GetItemIDByName(login); //Получение айпи папки пользователя           
            if (isCreate) await Task.Run(() => GoogleDriveService.uploadFile($"{AppDomain.CurrentDomain.BaseDirectory}\\Users\\{login}\\user_auth.json", UserFolder)); //Загрузка файла данных о пользователе в его папку           
        }

        public static string GetItemIDByName(string name)
        {
            for (int i = 0; i < FilesAndFolders.Count; i++)
            {
                if (FilesAndFolders[i].Name == name)
                {
                    Debug.WriteLine(FilesAndFolders[i].Name);
                    return FilesAndFolders[i].Id;
                }
            }

            return null;
        }
    }
}
