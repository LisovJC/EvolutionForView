using Evolution.Command;
using Evolution.Core;
using Evolution.Model;
using Evolution.Services.HelperServices;
using Evolution.Services.TaskServices;
using System;
using System.Collections.Generic;
using static Evolution.Model.TaskModel;

namespace Evolution.ViewModel.Pages
{
    internal class CreateViewModel : ObservableObject
    {
        #region TaskProperties
        private string _title;

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        /*=====================================================================*/
        private string _description;

        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
        /*=====================================================================*/
        private string _assigned = "Выбрать...";

        public string Assigned
        {
            get => _assigned;
            set => Set(ref _assigned, value);
        }
        /*=====================================================================*/
        private float _plannedTimeCosts;

        public float PlannedTimeCosts
        {
            get => _plannedTimeCosts;
            set => Set(ref _plannedTimeCosts, value);
        }
        /*=====================================================================*/
        private string _otherCategory;

        public string OtherCategory
        {
            get => _otherCategory;
            set => Set(ref _otherCategory, value);
        }
        /*=====================================================================*/
        private List<Category> _categories = new();

        public List<Category> Categories
        {
            get => _categories;
            set => Set(ref _categories, value);
        }
        /*=====================================================================*/
        private string _deadLine;

        public string DeadLine
        {
            get => _deadLine;
            set => Set(ref _deadLine, value);
        }
        /*=====================================================================*/
        public Priority Priority;
        public TypeTaskEdentity typeTask { get; set; }

        public List<UserModel> AllUsers { get; set; } = new();
        #endregion

        #region Commands
        public RelayCommand CreateTaskCommand { get; set; }
        public RelayCommand P0PriorityCommand { get; set; }
        public RelayCommand P1PriorityCommand { get; set; }
        public RelayCommand P2PriorityCommand { get; set; }
        public RelayCommand P3PriorityCommand { get; set; }
        public RelayCommand LocalTypeCommand { get; set; }
        public RelayCommand GlobalTypeCommand { get; set; }
        public RelayCommand AddCategoryCodeCommand { get; set; }
        public RelayCommand AddCategoryArtSpriteCommand { get; set; }
        public RelayCommand AddCategorySoundCommand { get; set; }
        public RelayCommand AddCategoryMusicCommand { get; set; }
        public RelayCommand AddCategoryCharacterCommand { get; set; }
        public RelayCommand AddCategoryDecorCommand { get; set; }
        public RelayCommand AddCategoryUICommand { get; set; }
        public RelayCommand AddCategoryEnvironmentCommand { get; set; }
        public RelayCommand AddCategoryActionsAnimationsCommand { get; set; }
        public RelayCommand AddCategorySettingsCommand { get; set; }
        public RelayCommand AddCategoryDialogCommand { get; set; }
        public RelayCommand SelectAssigneCommand { get; set; }
        #endregion

        public CreateViewModel()
        {
            if (Categories.Count == 0)
            { 
                CreateListCategory(); 
            }

            AllUsers = HelperService.AllUsersInApp;

            CreateTaskCommand = new(o => { CreateTask(); });
            SelectAssigneCommand = new(o => { Assigned = o.ToString(); });

            P0PriorityCommand = new(o => { SetPriority(Priority.ToDay); });
            P1PriorityCommand = new(o => { SetPriority(Priority.FewDays); });
            P2PriorityCommand = new(o => { SetPriority(Priority.Week); });
            P3PriorityCommand = new(o => { SetPriority(Priority.MoreWeek); });
            
            LocalTypeCommand = new(o => { SetType(TypeTaskEdentity.local); });
            GlobalTypeCommand = new(o => { SetType(TypeTaskEdentity.global); });

            //TODO убрать хард код из параметров!
            AddCategoryCodeCommand = new(o => { AddOrRemoveCategory("Code"); });
            AddCategoryArtSpriteCommand = new(o => { AddOrRemoveCategory("Art/Sprite"); });
            AddCategorySoundCommand = new(o => { AddOrRemoveCategory("Sound"); });
            AddCategoryMusicCommand = new(o => { AddOrRemoveCategory("Music"); });
            AddCategoryCharacterCommand = new(o => { AddOrRemoveCategory("Character"); });
            AddCategoryDecorCommand = new(o => { AddOrRemoveCategory("Decor"); });
            AddCategoryUICommand = new(o => { AddOrRemoveCategory("UI"); });
            AddCategoryEnvironmentCommand = new(o => { AddOrRemoveCategory("Environment"); });
            AddCategoryActionsAnimationsCommand = new(o => { AddOrRemoveCategory("Actions/Animations"); });
            AddCategorySettingsCommand = new(o => { AddOrRemoveCategory("Settings"); });
            AddCategoryDialogCommand = new(o => { AddOrRemoveCategory("Dialog"); });         
        }

        private void CreateTask()
        {
            TaskService.CreateTask
            (
                Title, 
                Assigned, 
                PlannedTimeCosts, 
                Description, 
                OtherCategory,
                DeadLine, 
                typeTask, 
                Categories
           );
            
           ClearFields();
        }

        private void SetPriority(Priority priority)
        {
            switch(priority)
            {
                case Priority.ToDay:
                    {
                        DeadLine = $"Сегодня ({DateTime.Now.ToString("d")})";
                        break;
                    }

                case Priority.FewDays:
                    {
                        DeadLine = $"Несколько дней ({DateTime.Now.AddDays(2).ToString("d")})";
                        break;
                    }

                case Priority.Week:
                    {
                        DeadLine = $"Неделя ({DateTime.Now.AddDays(7).ToString("d")})";
                        break;
                    }

                case Priority.MoreWeek:
                    {
                        DeadLine = $"Больше недели ({DateTime.Now.AddDays(7).ToString("d")})";
                        break;
                    }
            }
        }

        private void SetType(TypeTaskEdentity typeTask)
        {
            this.typeTask = typeTask;
        }

        private void CreateListCategory()
        {
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Code"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Art/Sprite"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Sound"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Music"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Character"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Decor"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "UI"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Environment"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Actions/Animations"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Settings"
            });
            Categories.Add(new()
            {
                isSelect = false,
                CategoryName = "Dialog"
            });
        }

        private void AddOrRemoveCategory(string category)
        {
            for (int i = 0; i < Categories.Count; i++)
            {
                if (Categories[i].CategoryName == category)
                {
                    Categories[i].isSelect = !Categories[i].isSelect;
                }
            }
        }       

        private void ClearFields()
        {
            Title = "";
            Description = "";
            Assigned = "";
            PlannedTimeCosts = 0;
            OtherCategory = "";           
        }
        
    }
}
