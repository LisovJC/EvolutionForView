using Evolution.Command;
using Evolution.Core;
using Evolution.Model;
using Evolution.Services.HelperServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Evolution.ViewModel.Pages
{
    internal class AvailableTaskViewModel:ObservableObject
    {       
        private int _progress = 0;

        public int Progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }
        /*=====================================================================*/

        private string _directionProgressBar = "LeftToRight";

        public string DirectionProgressBar
        {
            get => _directionProgressBar;
            set => Set(ref _directionProgressBar, value);
        }
        /*=====================================================================*/

        private int _selectedIndex = -1;

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value);
        }
        /*=====================================================================*/
        private int _Index = -2;

        public int Index
        {
            get => _Index;
            set => Set(ref _Index, value);
        }
        /*=====================================================================*/
        private Visibility _progressVisibility = Visibility.Visible;

        public Visibility ProgressVisibility
        {
            get => _progressVisibility;
            set => Set(ref _progressVisibility, value);
        }
        /*=====================================================================*/
        public static TaskModel SelectTask { get; set; } = new();
        /*=====================================================================*/

        #region SelectTaskProperties
        private string _title = "none";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        /*=====================================================================*/
        private string _description = "none";

        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
        /*=====================================================================*/
        private string _deadLine;

        public string DeadLine
        {
            get => _deadLine;
            set => Set(ref _deadLine, value);
        }
        /*=====================================================================*/
        private string _assigned = "none";

        public string Assigned
        {
            get => _assigned;
            set => Set(ref _assigned, value);
        }
        /*=====================================================================*/
        private string _creator = "none";

        public string Creator
        {
            get => _creator;
            set => Set(ref _creator, value);
        }
        /*=====================================================================*/
        private double _plannedTimeCosts = 0.0;

        public double PlannedTimeCosts
        {
            get => _plannedTimeCosts;
            set => Set(ref _plannedTimeCosts, value);
        }
        /*=====================================================================*/
        private string _otherCategory = "";

        public string OtherCategory
        {
            get => _otherCategory;
            set => Set(ref _otherCategory, value);
        }
        /*=====================================================================*/
        private string _categories;

        public string Categories
        {
            get => _categories;
            set => Set(ref _categories, value);
        }
        /*=====================================================================*/
        private string _dateCreate;

        public string DateCreate
        {
            get => _dateCreate;
            set => Set(ref _dateCreate, value);
        }        
        #endregion

        #region Commands
        public RelayCommand OpenFullInformationOfTask { get; set; }
        public RelayCommand GetTaskCommand { get; set; }
        #endregion

        #region Collections
        public List<TaskModel> LoadedCommonTasks { get; set; } = new();
        public ObservableCollection<TaskModel> GlobalTasks { get; set; } = new();
        #endregion

        public AvailableTaskViewModel()
        {
            ProgressChange();
            Task.Run(() => LoadDatas());

            OpenFullInformationOfTask = new(o => { Task.Run(() => ShowTaskInformation()); });            
        }

        private void ShowTaskInformation()
        {
            while(true)
            {
                if (Index != SelectedIndex)
                {
                    Index = SelectedIndex;
                    Categories = String.Empty;
                    break;
                }

                if(SelectedIndex == -1)
                {                   
                    break;
                }
            }

            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
                Index = 0;
            }
            SelectTask = GlobalTasks[SelectedIndex];
            Title = GlobalTasks[SelectedIndex].Title;
            Assigned = GlobalTasks[SelectedIndex].Assigned;
            DeadLine = GlobalTasks[SelectedIndex].DeadLine;
            Description = GlobalTasks[SelectedIndex].Description;
            DateCreate = GlobalTasks[SelectedIndex].DateCreate;  

            if(GlobalTasks[SelectedIndex].Categories != null)
            {
                for (int i = 0; i < GlobalTasks[SelectedIndex].Categories.Count; i++)
                {
                    if (i != GlobalTasks[SelectedIndex].Categories.Count - 1)
                    {
                        Categories += GlobalTasks[SelectedIndex].Categories[i].CategoryName + ", ";
                    }
                    else
                    {
                        Categories += GlobalTasks[SelectedIndex].Categories[i].CategoryName;
                    }
                }
            }
            
        } //TODO рефакторнуть метод!

        public void LoadDatas()
        {
            while(true)
            {
                if (HelperService.GlobalTasksInCash.Count == 0 || HelperService.GlobalTasksInCash.Count == 0)
                {
                    Task.Run(() => HelperService.HelperUpdateData(HelperService.CurrentUser));
                }
                else
                {
                    for (int i = 0; i < HelperService.GlobalTasksInCash.Count; i++)
                    {
                        GlobalTasks.Add(HelperService.GlobalTasksInCash[i]);
                    }
                    break;
                }
            }    
        }   

        public async void ProgressChange()
        {
            await Task.Run(() =>
            {
                Random rnd = new();
                while (true)
                {
                    if (GlobalTasks.Count > 0)
                    {
                        ProgressVisibility = Visibility.Hidden;
                        break;
                    }

                    if (Progress > 99)
                    {
                        DirectionProgressBar = "RightToLeft";
                    }
                    else if(Progress < 1)
                    {
                        DirectionProgressBar = "LeftToRight";
                    }

                    if (Progress < 100 && DirectionProgressBar == "LeftToRight")
                    {
                        Progress++;
                    }
                    else
                    {
                        Progress--;
                    }

                    Thread.Sleep(10);
                }
            });
        }
    }
}
