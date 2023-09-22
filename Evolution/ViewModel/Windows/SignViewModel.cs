using Evolution.Command;
using Evolution.Core;
using Evolution.Model;
using Evolution.Services.CloudStoreServices;
using Evolution.Services.HelperServices;
using Evolution.Services.SettingsServices;
using Evolution.Services.UserServices;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using static Evolution.Enums.Enums;

namespace Evolution.ViewModel.Windows
{
    internal class SignViewModel : ObservableObject
    {

        #region VisibilityProperties
        private Visibility _signUpVisibility = Visibility.Collapsed;

        public Visibility SignUpVisibility
        {
            get => _signUpVisibility;
            set => Set(ref _signUpVisibility, value);
        }

        private string _labelSign = "Вход";

        public string LabelSign
        {
            get => _labelSign;
            set => Set(ref _labelSign, value);
        }

        private bool _isEnableSignIn = true;

        public bool isEnableSignIn
        {
            get => _isEnableSignIn;
            set => Set(ref _isEnableSignIn, value);
        }

        private bool _isEnableSignUp = true;

        public bool isEnableSignUp
        {
            get => _isEnableSignUp;
            set => Set(ref _isEnableSignUp, value);
        }

        private bool _isEnableGetStarted = false;

        public bool isEnableGetStarted
        {
            get => _isEnableGetStarted;
            set => Set(ref _isEnableGetStarted, value);
        }

        private Visibility _reqired = Visibility.Collapsed;

        public Visibility Reqired
        {
            get => _reqired;
            set => Set(ref _reqired, value);
        }

        private Visibility _iconVisibility = Visibility.Visible;

        public Visibility IconVisibility
        {
            get => _iconVisibility;
            set => Set(ref _iconVisibility, value);
        }
        #endregion

        #region Fields
        private string _login = "";

        public string Login
        {
            get => _login;
            set { Set(ref _login, value); IsValidData(); }
        }

        private string _password = "";

        public string Password
        {
            get => _password;
            set { Set(ref _password, value); IsValidData(); }
        }

        private string _confrimPassword = "";

        public string ConfirmPassword
        {
            get => _confrimPassword;
            set { Set(ref _confrimPassword, value); IsValidData(); }
        }

        private string _email = "";

        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private string _contentRegButton = "Регистрация";

        public string ContentRegButton
        {
            get => _contentRegButton;
            set => Set(ref _contentRegButton, value);
        }

        private bool _isRememberMe;

        public bool isRememberMe
        {
            get => _isRememberMe;
            set => Set(ref _isRememberMe, value);
        }

        #endregion

        #region Commands
        public RelayCommand SignInCommand { get; set; }
        public RelayCommand SignUpCommand { get; set; }
        public RelayCommand GetStartedCommand { get; set; }
        public RelayCommand CloseAppCommand { get; set; }
        public RelayCommand RememberMeCommand { get; set; }
        #endregion        
        public SignViewModel()
        {
            SettingsService.CreateSettingsFile();
            AutoLogin();

            #region Commands
            /*=====================================================================*/
            SignInCommand = new(o => //TODO предусмотреть отсутствие интернета!
            {
                if (Task.Run(() => AuthUserService.SignIn(Login, Password)).Result)
                {
                    Task.Run(() => HelperService.HelperUpdateData(Login)); //Содаём необходимые данные в хелпере для их дальнейшего получения и работы с ними.

                    Application.Current.MainWindow.Hide();
                    MainWindow mainWindow = new();
                    mainWindow.Owner = Application.Current.MainWindow;
                    Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                }
                else
                {
                    Debug.WriteLine($"\n^^^^^^^^^^^^^^^^^^^^^^^^^\n\t***** ОШИБКА.\n\t [SignViewModel]: Попытка входа завершилась провалом. ******\n");
                }
            });
            /*=====================================================================*/
            SignUpCommand = new(o =>
            {
                if (SignUpVisibility == Visibility.Collapsed)
                {
                    SignUpVisibility = Visibility.Visible;
                    IconVisibility = Visibility.Collapsed;
                    Reqired = Visibility.Visible;
                    
                    LabelSign = "Регистрация";
                    ContentRegButton = "Отмена";

                    isEnableSignIn = false;
                }
                else
                {
                    SignUpVisibility = Visibility.Collapsed;
                    Reqired = Visibility.Collapsed;
                    Reqired = Visibility.Collapsed;

                    LabelSign = "Вход";
                    ContentRegButton = "Регистрация";

                    isEnableSignIn = true;
                }

            });
            /*=====================================================================*/
            GetStartedCommand = new(o =>
            {
                SignUpVisibility = Visibility.Collapsed;
                Reqired = Visibility.Collapsed;
                IconVisibility = Visibility.Visible;
                
                LabelSign = "Вход";
                ContentRegButton = "Регистрация";

                isEnableSignIn = true;

                var User = CreateUserService.CreateUser(Login, Password, ConfirmPassword, Email);
                if (User != null)
                {
                    FireBaseService.PushToDataBase(TypeDatas.UserAuthData, User);
                    ConfirmPassword = string.Empty;
                    Email = string.Empty;                    
                }
                else
                {
                    Debug.WriteLine("Ошибка регистрации. User = null. Строка 191.");
                }
            });
            /*=====================================================================*/
            CloseAppCommand = new(o =>
            {
                Application.Current.Shutdown();
            });
            /*=====================================================================*/
            RememberMeCommand = new(o =>
            {
                isRememberMe = !isRememberMe;
                SettingsService.SetRememberDataForAuthSettings(Login, Password, isRememberMe);
            });
            #endregion                      
        }

        private void IsValidData()
        {
            try
            {
                if (Login.Length > 2 && Password.Length > 2 && Password == ConfirmPassword)
                {
                    isEnableGetStarted = true;
                }
                else
                {
                    isEnableGetStarted = false;
                }
            }
            catch (System.Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }            
        }            

        private void AutoLogin() //TODO исправить файл настроек, так как для разных пользователей должно быть поразному.
        {
            SettingsModel sm = SettingsService.GetSettings();

            if(sm.RememberMeForAuth)
            {
                Login = sm.Login;
                Password = sm.Password;
                isRememberMe = sm.RememberMeForAuth;
            }
        }
    }
}