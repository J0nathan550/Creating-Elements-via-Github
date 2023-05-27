using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;

namespace Creating_Elements_via_Github
{
    public partial class MainWindow : Window
    {
        private string serverDataLink = "link";
        private string versionApp = "0.1";
        private ServerData serverData = new(); 
        private HttpClient client = new();
        public MainWindow()
        {
            InitializeComponent();
            applicationVersion.Content = $"Version: {versionApp}";
            string json = client.GetStringAsync(serverDataLink).Result;
            if (!string.IsNullOrEmpty(json))
            {
                serverData = JsonConvert.DeserializeObject<ServerData>(json);
                if (versionApp != serverData.ServerVersion)
                {
                    MessageBox.Show($"New version is: {serverData.ServerVersion}", "New update of application", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}