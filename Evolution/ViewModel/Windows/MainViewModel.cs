using Evolution.Command;
using Evolution.Core;
using Evolution.Services.HelperServices;
using Evolution.View.Pages;
using Evolution.ViewModel.Pages;
using System.Windows;
using System.Windows.Controls;

namespace Evolution.ViewModel.Windows
{
    internal class MainViewModel : ObservableObject
    {
        enum AppPages
        {
            Home,
            MyTasks,
            AvailableTasks,
            Create,
            Storage
        }
        
        #region VisibilityLineSelect
        private Visibility _isHomeSelect = Visibility.Hidden;

        public Visibility isHomeSelect
        {
            get => _isHomeSelect;
            set => Set(ref _isHomeSelect, value);
        }

        private Visibility _isStorageSelect = Visibility.Hidden;

        public Visibility isStorageSelect
        {
            get => _isStorageSelect;
            set => Set(ref _isStorageSelect, value);
        }

        private Visibility _isAvailableTSelect = Visibility.Hidden;

        public Visibility isAvailableTSelect
        {
            get => _isAvailableTSelect;
            set => Set(ref _isAvailableTSelect, value);
        }

        private Visibility _isMyTSelect = Visibility.Hidden;

        public Visibility isMyTSelect
        {
            get => _isMyTSelect;
            set => Set(ref _isMyTSelect, value);
        }

        private Visibility _isCreateSelect = Visibility.Hidden;

        public Visibility isCreateSelect
        {
            get => _isCreateSelect;
            set => Set(ref _isCreateSelect, value);
        }
        #endregion
       
        private Page _selectMainPage;
        public Page SelectMainPage
        {
            get => _selectMainPage;
            set => Set(ref _selectMainPage, value);
        }
        
        private string _currentUser;
        public string CurrentUser
        {
            get => _currentUser;
            set => Set(ref _currentUser, value);
        }
        
        public static MainViewModel Instance { get; set; } = new();
        
        #region Commands
        public RelayCommand SelectHomeCommand { get; set; }
        public RelayCommand SelectAvailableTCommand { get; set; }
        public RelayCommand SelectMyTCommand { get; set; }
        public RelayCommand SelectCreateCommand { get; set; }
        public RelayCommand SelectStorageCommand { get; set; }
        public RelayCommand CloseAppCommand { get; set; }
        public RelayCommand MinimizeCommand { get; set; }
        #endregion

        public static readonly string selectButtonColor = "#ff3c38";
        public static readonly string unSelectButtonColor = "#fefefe";
        public MainViewModel()
        {
            GoSelectPage(AppPages.Home);

            CurrentUser = HelperService.CurrentUser;

            SelectHomeCommand = new(o =>{GoSelectPage(AppPages.Home);});

            SelectAvailableTCommand = new(o => { GoSelectPage(AppPages.AvailableTasks); });

            SelectMyTCommand = new(o => { GoSelectPage(AppPages.MyTasks); });

            SelectCreateCommand = new(o => { GoSelectPage(AppPages.Create); });           

            SelectStorageCommand = new(o => { GoSelectPage(AppPages.Storage); });

            CloseAppCommand = new(o => { Application.Current.Shutdown(); });

            MinimizeCommand = new RelayCommand(o => {Application.Current.MainWindow.WindowState = WindowState.Minimized; });
        }

        private void GoSelectPage(AppPages page)
        {
           
            if(page == AppPages.Home)
            {
                isHomeSelect = Visibility.Visible;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Hidden;
                isCreateSelect = Visibility.Hidden;               
                isStorageSelect = Visibility.Hidden;
                
                SelectMainPage = new HomePage();
            }

            if (page == AppPages.AvailableTasks)
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Visible;
                isMyTSelect = Visibility.Hidden;                
                isCreateSelect = Visibility.Hidden;               
                isStorageSelect = Visibility.Hidden;
                
                SelectMainPage = new AvailableTaskPage();
            }

            if (page == AppPages.MyTasks)
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Visible;                
                isCreateSelect = Visibility.Hidden;               
                isStorageSelect = Visibility.Hidden;
                
                SelectMainPage = new MyTasksPage();
            }

            if (page == AppPages.Create)
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Hidden;                
                isCreateSelect = Visibility.Visible;                
                isStorageSelect = Visibility.Hidden;
                
                SelectMainPage = new CreatePage();
            }                     

            if (page == AppPages.Storage)
            {
                isHomeSelect = Visibility.Hidden;
                isAvailableTSelect = Visibility.Hidden;
                isMyTSelect = Visibility.Hidden;
                isCreateSelect = Visibility.Hidden;                
                isStorageSelect = Visibility.Visible;
                
                SelectMainPage = new StoragePage();
            }
        }
    }
}
