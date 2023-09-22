using Evolution.Model;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.DataSaveLoadServices;
using Evolution.Services.HelperServices;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static Evolution.Enums.Enums;
using File = System.IO.File;

namespace Evolution.Services.UserServices
{
    public class AuthUserService
    {
        private static string PathToUserFile = "";

        public static UserModel User = new();

        public static async Task<bool> SignIn(string login, string password)
        {          
            UserModel userExists = new();
            userExists = await Task.Run(() => UserExists(login));
            if (userExists.Login != null)
            {                
                if (IsCorrectUserData(login, password, userExists))
                {
                    CreateUserService.CreateUser(User.Login, User.Password, User.Password, User.Email);
                    Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** УСПЕШНО.\n\t [AuthUserService]: Вход пользавателем {login}. ******\n");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** ОШИБКА.\n\t [AuthUserService]: Данные входа не корректны. ******\n");
                    return false;
                }
            }
            else
            {
                Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** ОШИБКА.\n\t [AuthUserService]: Данные входа не корректны. ******\n");
                return false;
            }
        }

        public static async Task<UserModel> UserExists(string login)
        {
            PathToUserFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\Users\\{login}\\user_auth.json";            
            if (File.Exists(PathToUserFile))
            {
                User = DataSaveLoad.LoadDataUser<UserModel>(PathToUserFile);
                return User;
            }
            else
            {
                HelperService.AllUsersInApp = await FireBaseService.GetDataFromDataBase<UserModel>(TypeDatas.UserAuthData, -1);
                List<UserModel> AllUsersInApp = new();
                AllUsersInApp = HelperService.AllUsersInApp;
                if (AllUsersInApp.Count > 0)
                {
                    foreach (var user in AllUsersInApp)
                    {
                        if (user.Login == login)
                        {
                            User = user;
                            return User;
                        }             
                    }
                    return User;
                }
                else return User;
            }
        }

        private static bool IsCorrectUserData(string? login, string? password, UserModel user)
        {
            if (
                    login == null ||
                    password == null
               )
            {
                return false;
            }
            else if
                (
                    (login.Trim().Length >= 2) && !login.Contains(" ") && login == user.Login &&
                    (password.Trim().Length >= 2) && !password.Contains(" ") && password == user.Password
                )
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
