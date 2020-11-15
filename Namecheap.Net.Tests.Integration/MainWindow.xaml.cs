using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Namecheap.Net.Tests.Integration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _apiKey;
        public string ApiKey
        {
            get => _apiKey;
            set
            {
                _apiKey = value;
                OnPropertyChanged();
            }
        }

        private string _apiResponse;
        public string ApiResponse {
            get => _apiResponse;
            set
            {
                _apiResponse = value;
                OnPropertyChanged();
            } 
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            NamecheapApi api = new(ApiKey, "kbuzby", new System.Net.IPAddress(new byte[] { 97, 83, 134, 98 }));
            var getHostsResponse = await api.Domains.Dns.GetHosts("buzby", "dev");
            XmlSerializer xmlSerializer = new(typeof(ApiResponse<DnsCommands.GetHostsResponse>));

            using MemoryStream memStream = new();

            xmlSerializer.Serialize(memStream, getHostsResponse);

            ApiResponse = Encoding.UTF8.GetString(memStream.ToArray());
        }
    }
}
