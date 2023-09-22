using Evolution.Command;
using Evolution.Core;
using Evolution.Services.HelperServices;
using Evolution.Services.TaskServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution.Model
{
    public class TaskModel : ObservableObject
    {
        public enum TypeTaskEdentity
        {
            local,
            global
        }

        public enum Priority
        {
            ToDay,
            FewDays,
            Week,
            MoreWeek
        }
        /*=====================================================================*/
        private TypeTaskEdentity _typeTask = TypeTaskEdentity.local;

        public TypeTaskEdentity TypeTask
        {
            get => _typeTask;
            set => Set(ref _typeTask, value);
        }
        /*=====================================================================*/
        private int _id;

        public int ID
        {
            get => _id;
            set => Set(ref _id, value);
        }
        /*=====================================================================*/
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
        private List<Category> _categories;

        public List<Category> Categories
        {
            get => _categories;
            set { Set(ref _categories, value); SetCategories(); }
        }
        /*=====================================================================*/
        private string _dateCreate;

        public string DateCreate
        {
            get => _dateCreate;
            set => Set(ref _dateCreate, value);
        }
        /*=====================================================================*/
        private string _getCategories = "no categories set...";

        public string GetCategories
        {
            get => _getCategories;
            set => Set(ref _getCategories, value);
        }
        /*=====================================================================*/
        private string _taskButtonContent = "Начать выполнение";

        public string TaskButtonContent
        {
            get => _taskButtonContent;
            set => Set(ref _taskButtonContent, value);
        }
        /*=====================================================================*/
        private bool isTaskButtonEnable = true;

        public bool IsTaskButtonEnable

        {
            get => isTaskButtonEnable;
            set => Set(ref isTaskButtonEnable, value);
        }
        /*=====================================================================*/
        public RelayCommand GetTaskCommand { get; set; }

        public TaskModel()
        {
            GetTaskCommand = new(o =>
            {
                IsTaskButtonEnable = false;
                TaskButtonContent = "В работе";               
                TaskService.UpdateTaskList(this);
                TaskService.AddGlobalTaskToLocalTaskList(this);
            });
        }

        private void SetCategories()
        {
            GetCategories = "";
            for (int i = 0; i < Categories.Count; i++)
            {
                GetCategories += Categories[i].CategoryName + ", ";
            }
        }

    }
}
