using System.Diagnostics;
using System.Net.Http;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Creating_Elements_via_Github
{
    public partial class MainWindow : Window
    {
        private string serverDataLink = "https://raw.githubusercontent.com/J0nathan550/Creating-Elements-via-Github/master/serverData.json";
        private string versionApp = "0.2";
        private ServerData serverData = new(); 
        private HttpClient client = new();
        private Timer update = new Timer(); 

        public MainWindow()
        {
            InitializeComponent();
            applicationVersion.Content = $"Version: {versionApp}";
            string json = client.GetStringAsync(serverDataLink).Result;
            if (!string.IsNullOrEmpty(json))
            {
                serverData = JsonConvert.DeserializeObject<ServerData>(json);
                if (serverData != null)
                {
                    if (versionApp != serverData.ServerVersion)
                    {
                        MessageBox.Show($"New version is: {serverData.ServerVersion}", "New update of application", MessageBoxButton.OK, MessageBoxImage.Information);
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = serverData.GitHubLink,
                            UseShellExecute = true
                        });
                    }
                }
                else
                {
                    MessageBox.Show("There is no access to json file.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("There is no access to json file.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            update.Interval = 5000;
            update.Elapsed += Update_Elapsed;
            update.Start();
        }

        private async void Update_Elapsed(object? sender, ElapsedEventArgs e)
        {
            string json = client.GetStringAsync(serverDataLink).Result;
            if (!string.IsNullOrEmpty(json))
            {
                serverData = JsonConvert.DeserializeObject<ServerData>(json);
                if (serverData != null)
                {
                    await Dispatcher.BeginInvoke(() =>
                    {
                        WindowGrid.Children.Clear();
                    });
                    for (int i = 0; i < serverData.Buttons.Length; i++) 
                    {
                        await Dispatcher.BeginInvoke(() =>
                        {
                            Button button = new Button();
                            button.Content = serverData.Buttons[i].Title;
                            button.Width = 200;
                            button.Height = 200;
                            WindowGrid.Children.Add(button);
                        });
                    }
                    serverData = new ServerData();
                }
                else
                {
                    MessageBox.Show("There is no access to json file.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("There is no access to json file.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    public class ServerData
    {
        public string? ServerVersion { get; set; }
        public string? GitHubLink { get; set; }
        public struct ButtonData
        {
            public string? Title { get; set; }
        }
        public ButtonData[]? Buttons { get; set; }
    }
}