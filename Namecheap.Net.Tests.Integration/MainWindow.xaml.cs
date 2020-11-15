using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private string? _apiKey;
        public string? ApiKey
        {
            get => _apiKey;
            set
            {
                _apiKey = value;
                OnPropertyChanged();
            }
        }
        private string? _userName;
        public string? UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string? _apiResponse;
        public string? ApiResponse {
            get => _apiResponse;
            set
            {
                _apiResponse = value;
                OnPropertyChanged();
            } 
        }

        private ObservableCollection<PropertyDisplayInfo>? _requestProperties;
        public ObservableCollection<PropertyDisplayInfo>? RequestProperties { 
            get => _requestProperties;
            set
            {
                _requestProperties = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ApiKey == null) { return; }

            NamecheapApi api = new(ApiKey, "kbuzby", new System.Net.IPAddress(new byte[] { 97, 83, 134, 98 }));
            var getHostsResponse = await api.Domains.Dns.GetHosts("buzby", "dev");
            XmlSerializer xmlSerializer = new(typeof(ApiResponse<DnsCommands.GetHostsResponse>));

            using MemoryStream memStream = new();

            xmlSerializer.Serialize(memStream, getHostsResponse);

            ApiResponse = Encoding.UTF8.GetString(memStream.ToArray());
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem treeViewItem)
            {
                var path = GetItemPath(treeViewItem);
                var requestType = GetRequestTypeForPath(path);
                if (requestType == null) { return; }
                var requestProps = requestType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(QueryParamAttribute)));
                var requestProperties = new ObservableCollection<PropertyDisplayInfo>();
                foreach (var propInfo in requestProps)
                {
                    requestProperties.Add(new PropertyDisplayInfo(propInfo));
                }
                RequestProperties = requestProperties;
            }
        }

        private static string GetItemPath(TreeViewItem treeViewItem)
        {
            return (treeViewItem.Parent is TreeViewItem parentItem ? GetItemPath(parentItem) + "." : "") + treeViewItem.Header;
        }

        private Type? GetRequestTypeForPath(string itemPath)
        {
            return itemPath switch
            {
                "Domains.DNS.GetHosts" => typeof(DnsCommands.GetHostsRequest),
                _ => null
            };
        }
    }

    public record PropertyDisplayInfo
    {
        public PropertyInfo PropertyInfo { get; init; }
        public string PropertyName => PropertyInfo.Name; 
        public string? PropertyValue { get; init; }
        public PropertyDisplayInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }
}
