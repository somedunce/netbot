using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace net.bot.beta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool loggedIn = false;

        private static IInstaApi api;
        private static UserSessionData user;

        private static SolidColorBrush selectedColorBlue = new SolidColorBrush(Color.FromRgb(48,65,81));
        private static SolidColorBrush notSelected = new SolidColorBrush(Color.FromRgb(84,84,84));
        public MainWindow()
        {
            InitializeComponent();

            if(Properties.Settings.Default.rememberMe == true)
            {
                RememberMeCheckBox.IsChecked = true;
                UsernameTextbox.Text = Properties.Settings.Default.rememberedUsername;
                PasswordTextbox.Password = Properties.Settings.Default.rememberedPassword;
                LoginOnStartCheckBox.IsEnabled = true;
            }
            if(Properties.Settings.Default.loginOnStart == true)
            {
                LoginButton.IsEnabled = false;
                LoginOnStartCheckBox.IsChecked = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AboutButton.Background = notSelected;
            HomeButton.Background = selectedColorBlue;
            UtilsButton.Background = notSelected;

            OutputButton.Visibility = Visibility.Hidden;
            OutputTextbox.Visibility = Visibility.Hidden;
            OutputButton.Visibility = Visibility.Hidden;

            UsernameLabel.Visibility = Visibility.Visible;
            UsernameTextbox.Visibility = Visibility.Visible;
            PasswordLabel.Visibility = Visibility.Visible;
            PasswordTextbox.Visibility = Visibility.Visible;
            StatusBox.Visibility = Visibility.Visible;
            LoginButton.Visibility = Visibility.Visible;
            RememberMeCheckBox.Visibility = Visibility.Visible;
            LoginOnStartCheckBox.Visibility = Visibility.Visible;

            GenerateAmount.Visibility = Visibility.Hidden;
            GenerateButton.Visibility = Visibility.Hidden;

            MadeByLabel1.Visibility = Visibility.Hidden;
            MadeByLabel2.Visibility = Visibility.Hidden;
            ThanksLabel.Visibility = Visibility.Hidden;
            ThanksLabel2.Visibility = Visibility.Hidden;
            ThanksUserLabel.Visibility = Visibility.Hidden;
            ProjectUrl.Visibility = Visibility.Hidden;
            ThanksLabel3.Visibility = Visibility.Hidden;
            ThanksUserLabel2.Visibility = Visibility.Hidden;
            ThanksLabel4.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AboutButton.Background = selectedColorBlue;
            HomeButton.Background = notSelected;
            UtilsButton.Background = notSelected;

            OutputButton.Visibility = Visibility.Hidden;
            OutputTextbox.Visibility = Visibility.Hidden;
            GenerateAmount.Visibility = Visibility.Hidden;
            GenerateButton.Visibility = Visibility.Hidden;
            OutputButton.Visibility = Visibility.Hidden;

            UsernameLabel.Visibility = Visibility.Hidden;
            UsernameTextbox.Visibility = Visibility.Hidden;
            PasswordLabel.Visibility = Visibility.Hidden;
            PasswordTextbox.Visibility = Visibility.Hidden;
            StatusBox.Visibility = Visibility.Hidden;
            LoginButton.Visibility = Visibility.Hidden;
            RememberMeCheckBox.Visibility = Visibility.Hidden;
            LoginOnStartCheckBox.Visibility = Visibility.Hidden;

            MadeByLabel1.Visibility = Visibility.Visible;
            MadeByLabel2.Visibility = Visibility.Visible;
            ThanksLabel.Visibility = Visibility.Visible;
            ThanksLabel2.Visibility = Visibility.Visible;
            ThanksUserLabel.Visibility = Visibility.Visible;
            ProjectUrl.Visibility = Visibility.Visible;
            ThanksLabel3.Visibility = Visibility.Visible;
            ThanksUserLabel2.Visibility = Visibility.Visible;
            ThanksLabel4.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            FileStream fs = File.Create(Directory.GetCurrentDirectory() + "\\botnames.txt");
            fs.Close();

            List<string> lines = new List<string>();
            int i = 0;
            for(i = 0; i < Convert.ToInt32(GenerateAmount.Text); i++)
            {
                int random = rand.Next(1000000, 10000000);
                lines.Add("notbot" + random.ToString().Substring(0, 6));
            }
            File.WriteAllLines(Directory.GetCurrentDirectory() + "\\botnames.txt", lines.ToArray());

            MessageBoxResult dl = MessageBox.Show("Would you like to see the output?","See output", MessageBoxButton.YesNo);
            if(dl == MessageBoxResult.Yes)
            {
                OutputButton.Visibility = Visibility.Visible;
                OutputTextbox.Visibility = Visibility.Visible;
                GenerateAmount.Visibility = Visibility.Hidden;
                GenerateButton.Visibility = Visibility.Hidden;

                AboutButton.Background = notSelected;
                HomeButton.Background = notSelected;
                UtilsButton.Background = notSelected;
                OutputButton.Background = selectedColorBlue;

                HomeButton.Background = new SolidColorBrush(Color.FromRgb(84, 84, 84));
                OutputTextboxText.Text = File.ReadAllText(Directory.GetCurrentDirectory() + "\\botnames.txt");
                TotalGeneratedText.Text = "Total Generated: " + File.ReadAllLines(Directory.GetCurrentDirectory() + "\\botnames.txt").Length + "\n";
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
            catch
            {

            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if(RememberMeCheckBox.IsChecked == false)
            {
                Properties.Settings.Default.rememberMe = false;
            }
            if(LoginOnStartCheckBox.IsChecked == false)
            {
                Properties.Settings.Default.loginOnStart = false;
            }

            Statustext.Text = "Loading...";
            Statustext.Foreground = new SolidColorBrush(Colors.LightBlue);
            user = new UserSessionData();
            user.UserName = UsernameTextbox.Text;
            user.Password = PasswordTextbox.Password;

            api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .Build();

            if (!api.IsUserAuthenticated == true)
            {
                try
                {
                    var logInResult = await api.LoginAsync();
                    if (logInResult.Succeeded == true)
                    {
                        Statustext.Text = "Logged In!";
                        Statustext.Foreground = new SolidColorBrush(Colors.Cyan);
                        Logoutbutton.IsEnabled = true;
                        loggedIn = true;
                        LoginButton.IsEnabled = false;

                        if (RememberMeCheckBox.IsChecked == true)
                        {
                            Properties.Settings.Default.rememberMe = true;
                            Properties.Settings.Default.rememberedUsername = UsernameTextbox.Text;
                            Properties.Settings.Default.rememberedPassword = PasswordTextbox.Password;
                        }
                        if (LoginOnStartCheckBox.IsChecked == true)
                            Properties.Settings.Default.loginOnStart = true;
                    }
                    else
                    {
                        LoginButton.IsEnabled = true;
                        Statustext.Text = "Failed Login";
                        Statustext.Foreground = new SolidColorBrush(Colors.Orange);
                    }
                }
                catch
                {
                    LoginButton.IsEnabled = true;
                    Statustext.Text = "Failed Login";
                    Statustext.Foreground = new SolidColorBrush(Colors.Orange);
                }
            }

            PasswordTextbox.Clear();
            Properties.Settings.Default.Save();
        }

        private void Label_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/somedunce");
        }

        private void UtilsButton_Click(object sender, RoutedEventArgs e)
        {
            AboutButton.Background = notSelected;
            HomeButton.Background = notSelected;
            UtilsButton.Background = selectedColorBlue;

            OutputButton.Visibility = Visibility.Hidden;
            OutputTextbox.Visibility = Visibility.Hidden;
            OutputButton.Visibility = Visibility.Hidden;

            UsernameLabel.Visibility = Visibility.Hidden;
            UsernameTextbox.Visibility = Visibility.Hidden;
            PasswordLabel.Visibility = Visibility.Hidden;
            PasswordTextbox.Visibility = Visibility.Hidden;
            StatusBox.Visibility = Visibility.Hidden;
            LoginButton.Visibility = Visibility.Hidden;
            RememberMeCheckBox.Visibility = Visibility.Hidden;
            LoginOnStartCheckBox.Visibility = Visibility.Hidden;

            GenerateAmount.Visibility = Visibility.Visible;
            GenerateButton.Visibility = Visibility.Visible;

            MadeByLabel1.Visibility = Visibility.Hidden;
            MadeByLabel2.Visibility = Visibility.Hidden;
            ThanksLabel.Visibility = Visibility.Hidden;
            ThanksLabel2.Visibility = Visibility.Hidden;
            ThanksUserLabel.Visibility = Visibility.Hidden;
            ProjectUrl.Visibility = Visibility.Hidden;
            ThanksLabel3.Visibility = Visibility.Hidden;
            ThanksUserLabel2.Visibility = Visibility.Hidden;
            ThanksLabel4.Visibility = Visibility.Hidden;
        }

        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if(loggedIn == true)
            {
                var logoutResult = await api.LogoutAsync();
                if(logoutResult.Succeeded == true)
                {
                    Statustext.Text = "Logged out";
                    Statustext.Foreground = new SolidColorBrush(Colors.Red);
                    Logoutbutton.IsEnabled = false;
                    loggedIn = false;
                    LoginButton.IsEnabled = true;
                }
            }
        }

        private void ThanksUserLabel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/ramtinak");
        }

        private void ProjectUrl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/ramtinak/InstagramApiSharp");
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(Properties.Settings.Default.loginOnStart == true && Properties.Settings.Default.rememberMe == true)
            {
                Statustext.Text = "Loading...";
                Statustext.Foreground = new SolidColorBrush(Colors.LightBlue);
                user = new UserSessionData();
                user.UserName = UsernameTextbox.Text;
                user.Password = PasswordTextbox.Password;

                api = InstaApiBuilder.CreateBuilder()
                    .SetUser(user)
                    .Build();
                try
                {
                    var logInResult = await api.LoginAsync();
                    if (logInResult.Succeeded == true)
                    {
                        LoginButton.IsEnabled = false;
                        Statustext.Text = "Logged In!";
                        Statustext.Foreground = new SolidColorBrush(Colors.Cyan);
                        Logoutbutton.IsEnabled = true;
                        loggedIn = true;
                        LoginButton.IsEnabled = false;

                        if (RememberMeCheckBox.IsChecked == true)
                        {
                            Properties.Settings.Default.rememberMe = true;
                            Properties.Settings.Default.rememberedUsername = UsernameTextbox.Text;
                            Properties.Settings.Default.rememberedPassword = PasswordTextbox.Password;
                        }
                    }
                    else
                    {
                        LoginButton.IsEnabled = true;
                        Statustext.Text = "Failed Login";
                        Statustext.Foreground = new SolidColorBrush(Colors.Orange);
                    }
                }
                catch
                {
                    LoginButton.IsEnabled = true;
                    Statustext.Text = "Failed Login";
                    Statustext.Foreground = new SolidColorBrush(Colors.Orange);
                }
            }
        }

        private void RememberMeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (RememberMeCheckBox.IsChecked == true)
            {
                LoginOnStartCheckBox.IsEnabled = true;
            }
            else
            {
                LoginOnStartCheckBox.IsEnabled = false;
                LoginOnStartCheckBox.IsChecked = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (RememberMeCheckBox.IsChecked == true)
            {
                Properties.Settings.Default.rememberMe = true;
                Properties.Settings.Default.rememberedUsername = UsernameTextbox.Text;
                Properties.Settings.Default.rememberedPassword = PasswordTextbox.Password;
            }
            else
                Properties.Settings.Default.rememberMe = false;

            if (LoginOnStartCheckBox.IsChecked == true && RememberMeCheckBox.IsChecked == true)
                Properties.Settings.Default.loginOnStart = true;
            else
                Properties.Settings.Default.loginOnStart = false;

            Properties.Settings.Default.Save();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings s = new Settings();
            s.ShowDialog();
        } 

        private void ExitButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ThanksUserLabel2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit");
        }
    }
}
