using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp;
using System.ComponentModel.DataAnnotations;
using static Evolution.Services.TaskServices.TaskService;
using Evolution.Model;
using System.Drawing.Drawing2D;
using static Evolution.Enums.Enums;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Markup;

namespace Evolution.Services.CloudStoreServices
{
    internal class FireBaseService
    {
        private static FirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "7ObrgEm3lNF073bppXmCYsboUhdVSBqonvvLqIyp",
            BasePath = "https://evolution-5d406-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        private static FirebaseClient client;

        static FireBaseService()
        {
            client = new FirebaseClient(config);
        }

        public static async void PushToDataBase<T>(TypeDatas dataType, T? data)
        {
            string dataPath = "";

            UserModel? user = new();
            TaskModel? task = new();

            if (data != null)
            {
                switch (dataType)
                {
                    case TypeDatas.UserAuthData:
                        {
                            user = data as UserModel;
                            dataPath = "UserAuthData\\Users";
                            await client.PushAsync(dataPath, user);
                        }
                        break;
                    case TypeDatas.GlobalTasks:
                        {
                            task = data as TaskModel;
                            dataPath = $"GlobalTasks\\Tasks\\{task?.ID}";
                            await client.SetAsync(dataPath, task);
                        }
                        break;
                    default:
                        {
                            Debug.WriteLine("Ошибка при попытке отправить данные в сервис.");
                        }
                        break;

                }
            }
        }

        public static async void UpdateTaskFromDataBase<T>(TypeDatas dataType, T data)
        {
            string dataPath = "";

            UserModel? user = new();
            TaskModel? task = new();

            switch (dataType)
            {
                case TypeDatas.UserAuthData:
                    break;
                case TypeDatas.GlobalTasks:
                    {
                        task = data as TaskModel;
                        dataPath = $"GlobalTasks\\Tasks\\{task?.ID}";
                        await client.SetAsync(dataPath, task);;
                    }
                    break;
                default:
                    break;
            }
        }

        public static async Task<List<T>> GetDataFromDataBase<T>(TypeDatas dataType, int count = -1)
        {
            List<T> Data = new();
            string dataPath = "";

            try
            {
                switch (dataType)
                {
                    case TypeDatas.UserAuthData:
                        {
                            dataPath = "UserAuthData/Users";
                            FirebaseResponse response = await client.GetAsync(dataPath);

                            Dictionary<string, T> datas = JsonConvert.DeserializeObject<Dictionary<string, T>>(response.Body.ToString());
                            Data = datas.Select(x => x.Value).ToList();
                        }
                        break;
                    case TypeDatas.GlobalTasks:
                        {
                            List<T> Temp = new();
                            dataPath = @"GlobalTasks/Tasks/";

                            for (int i = 0; i < count; i++)
                            {
                                FirebaseResponse response = await client.GetAsync(dataPath + i);
                                Data.Add(response.ResultAs<T>());                                                                                            
                            }
                        }
                        break;
                    default:
                        break;
                }


                return Data;
            }
            catch (System.Exception ex)
            {
               Debug.WriteLine(ex.Message + " Ошибка при попытке получить данные!");
               return null;
            }
        }

        public static async void CreateCounterFromDataBase(TypeDatas dataType)
        {
            switch (dataType)
            {
                case TypeDatas.UserAuthData:                   
                    break;
                case TypeDatas.GlobalTasks:
                    {
                        await client.SetAsync("GlobalTasks/Counter", "1");
                    }
                    break;
                default:
                    break;
            }
        }

        public static async Task<int> GetCounterFromDataBase(TypeDatas dataType)
        {
            int count = 0;
            switch (dataType)
            {
                case TypeDatas.UserAuthData:
                    {
                        return count;
                    }                    
                case TypeDatas.GlobalTasks:
                    {
                        try
                        {
                            FirebaseResponse response = await client.GetAsync("GlobalTasks/Counter");
                            count = int.Parse(response.ResultAs<string>());

                            return count;
                        }
                        catch (System.Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            return count;
                        }                        
                    }                    
                default:
                    {
                        return count;
                    }                  
            }
        }

        public static async void UpdateCounterFromDataBase(TypeDatas dataType, string newCount)
        {
            switch (dataType)
            {
                case TypeDatas.UserAuthData:
                    {                       
                        break;
                    }
                case TypeDatas.GlobalTasks:
                    {
                        try
                        {
                            await client.SetAsync("GlobalTasks/Counter", newCount);                 
                        }
                        catch (System.Exception ex)
                        {
                            Debug.WriteLine(ex.Message);                           
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
