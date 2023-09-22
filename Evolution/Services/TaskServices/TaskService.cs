using Evolution.Model;
using Evolution.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using static Evolution.Model.TaskModel;
using System.Diagnostics;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.DataSaveLoadServices;
using static Evolution.Enums.Enums;

namespace Evolution.Services.TaskServices
{
    /***СЕРВИС ДЛЯ РАБОТЫ С ЗАДАЧАМИ***/
    internal class TaskService 
    {      
        public static ObservableCollection<TaskModel> AllTasks { get; set; } = new();
        /*=====================================================================*/
        public static TaskModel NewTask = new();
        /*=====================================================================*/
        public static string PathToUserTasksFile = "";

        /***СОЗДАНИЕ ЗАДАЧИ***/
        public static async void CreateTask(
            string title,
            string assigned,
            double plannedTimeCosts,
            string description,
            string OtherCategory,
            string deadLine,
            TypeTaskEdentity typeTask,
            List<Category> categories)
        {
            NewTask = new TaskModel
            {
                Title = title,
                Description = description,
                Assigned = assigned,
                PlannedTimeCosts = plannedTimeCosts,
                OtherCategory = OtherCategory,
                DeadLine = deadLine,
                TypeTask = typeTask,
                DateCreate = DateTime.Now.ToString("d"),
                Categories = AddCategories(categories),
                Creator = "автор: " + HelperService.CurrentUser
            };

            /***ОПРЕДЕЛЕНИЕ ЛОКАЛЬНАЯ ИЛИ ПРОЕКТНАЯ***/
            switch (NewTask.TypeTask)
            {
                case TypeTaskEdentity.local:
                    {
                        AllTasks = GetAllMyTasks();
                        AllTasks.Add(NewTask);
                        DataSaveLoad.Serialize(AllTasks);
                    }
                    break;
                case TypeTaskEdentity.global:
                    {                       
                        try
                        {
                           int countOfTaskList = await Task.Run(() => FireBaseService.GetCounterFromDataBase(TypeDatas.GlobalTasks));
                           if(countOfTaskList == 0)
                            {                                
                                NewTask.ID = 0;
                                await Task.Run(() => FireBaseService.CreateCounterFromDataBase(TypeDatas.GlobalTasks));                               
                                await Task.Run(() => FireBaseService.PushToDataBase(TypeDatas.GlobalTasks, NewTask));
                                HelperService.CountTasksOfGlobalTaskList = 1;
                                await Task.Run(() => HelperService.HelperUpdateData(HelperService.CurrentUser));
                            }
                            else
                            {
                                NewTask.ID = countOfTaskList;
                                await Task.Run(() => FireBaseService.UpdateCounterFromDataBase(TypeDatas.GlobalTasks, (countOfTaskList + 1).ToString()));
                                await Task.Run(() => FireBaseService.PushToDataBase(TypeDatas.GlobalTasks, NewTask));
                                HelperService.CountTasksOfGlobalTaskList = countOfTaskList + 1;
                                await Task.Run(() => HelperService.HelperUpdateData(HelperService.CurrentUser));
                            }                                                                              
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine(ex.Message);
                        }
                    }
                    break;

            }       
        }

        /***ОБНОВЛЕНИЕ ЗАДАЧИ***/
        public static async void UpdateTaskList(TaskModel NewTask)
        {
            try
            {
                await Task.Run(() => FireBaseService.UpdateTaskFromDataBase(TypeDatas.GlobalTasks, NewTask));
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        /***ДОБАВЛЕНИЕ ПРОЕКТНОЙ ЗАДАЧИ В СПИСОК ВСЕХ СВОИХ ЗАДАЧ НА ЛОКАЛЬНОМ ДИСКЕ***/
        public static void AddGlobalTaskToLocalTaskList(TaskModel NewTask)
        {
            AllTasks = GetAllMyTasks();
            AllTasks.Add(NewTask);
            DataSaveLoad.Serialize(AllTasks);
        }

        /***ПОЛУЧЕНИЕ ВСЕХ ЗАДАЧ С ЛОКАЛЬНОГО ДИСКА***/
        public static ObservableCollection<TaskModel> GetAllMyTasks()
        {
            InitTaskJsonFiles(HelperService.CurrentUser);
            ObservableCollection<TaskModel> AllMyTasks = DataSaveLoad.LoadData<TaskModel>(PathToUserTasksFile);
            return AllMyTasks;
        }

        /***ТУТ ДОБАВЛЯЕМ КАТЕГОРИИ, ЧТОБЫ ОТДЕЛИТЬ И ДОБАВИТЬ ТОЛЬКО ТЕ, КОТОРЫЕ ОТМЕЧЕНЫ***/
        private static List<Category> AddCategories(List<Category> Categories)
        {
            List<Category> finalCategories = new();
            for (int i = 0; i < Categories.Count; i++)
            {
                if (Categories[i].isSelect)
                {
                    finalCategories.Add(Categories[i]);
                }
            }
            return finalCategories;
        }

        /***СОЗДАНИЕ JSON ФАЙЛА ДЛЯ ВСЕХ ЗАДАЧ***/
        private static string InitTaskJsonFiles(string? login)
        {
            string folderPath = $"{HelperService.pathToTasksFolder}\\{login}";
            PathToUserTasksFile = $"{folderPath}\\AllMyTasks.json";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                var file = File.Create($"{folderPath}\\AllMyTasks.json");
                file.Close();
                return folderPath;
            }
            else
            {
                return "error: This file exists!";
            }
        }

        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        //public static async Task<ObservableCollection<TaskModel>> GetCommonTasks()
        //{
        //    await HelperService.HelperUpdateData(HelperService.CurrentUser);

        //    if (!System.IO.File.Exists(PathToCommonTasksFile))
        //    {
        //        var file = System.IO.File.Create(PathToCommonTasksFile);
        //        file.Close();
        //    }

        //    FilesAndFoldersInCommonTasksFolder = HelperService.FilesAndFoldersInCommonTasksFolder;

        //    if (FilesAndFoldersInCommonTasksFolder.Count == 0)
        //    {
        //        return CommonTasks;
        //    }
        //    else
        //    {
        //        await GoogleDriveService.Download(HelperService.GetItemIDByName(
        //            FilesAndFoldersInCommonTasksFolder,
        //            "Common_Tasks.json"),
        //            PathToCommonTasksFile);
        //        CommonTasks = DataSaveLoad.LoadData<TaskModel>(PathToCommonTasksFile);
        //        return CommonTasks;
        //    }
        //}
    }
}
