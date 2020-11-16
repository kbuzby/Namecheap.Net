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
        private Type? _requestType;

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
            if (UserName == null) { return; }
            if (_requestType == null) { return; }

            // Build the request
            var request = Activator.CreateInstance(_requestType);
            if (RequestProperties == null) { return; }
            foreach (var prop in RequestProperties)
            {
                prop.PropertyInfo.SetValue(request, prop.PropertyValue);
            }

            // Setup handling the response
            var responseType = GetResponseTypeForRequest(request);
            if (responseType == null) { return; }
            XmlSerializer xmlSerializer = new(responseType);

            // Execute the request
            NamecheapApi api = new(ApiKey, UserName, new System.Net.IPAddress(new byte[] { 97, 83, 134, 98 }));
            var response = await ExecuteCommandForRequest(api, request);
            if (response == null) { return; }

            // write output
            using MemoryStream memStream = new();
            xmlSerializer.Serialize(memStream, response);
            ApiResponse = Encoding.UTF8.GetString(memStream.ToArray());
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Always reset when it's changed
            RequestProperties = null;

            if (e.NewValue is TreeViewItem treeViewItem)
            {
                var path = GetItemPath(treeViewItem);
                _requestType = GetRequestTypeForPath(path);
                if (_requestType == null) { return; }
                var commandInfo = ApiRequestBuilder.FindApiCommandInterface(_requestType);
                if (commandInfo == null) { return; }
                HashSet<string> interfacePropNames = commandInfo.Value.iface
                    .GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(QueryParamAttribute), true))
                    .Select(prop => prop.Name).ToHashSet();
                // TODO get the property info for the interface props from the request type since we'll need the setter
                var requestProps = _requestType.GetProperties().Where(prop => interfacePropNames.Contains(prop.Name));
                var requestProperties = new ObservableCollection<PropertyDisplayInfo>();
                foreach (var propInfo in requestProps)
                {
                    requestProperties.Add(new PropertyDisplayInfo(propInfo));
                }
                RequestProperties = requestProperties;
            }
            else
            {
                _requestType = null;
            }
        }

        private static string GetItemPath(TreeViewItem treeViewItem)
        {
            return (treeViewItem.Parent is TreeViewItem parentItem ? GetItemPath(parentItem) + "." : "") + treeViewItem.Header;
        }

        private static Type? GetRequestTypeForPath(string itemPath)
        {
            return itemPath switch
            {
                "Domains.DNS.GetHosts" => typeof(DnsGetHostsRequest),
                _ => null
            };
        }
        private static Type? GetResponseTypeForRequest(object? request)
        {
            return request switch
            {
                DnsGetHostsRequest => typeof(ApiResponse<DnsCommands.GetHostsResponse>),
                _ => null
            };
        }
        private static async Task<object?> ExecuteCommandForRequest(NamecheapApi api, object? request)
        {
            return request switch
            {
                DnsGetHostsRequest => await api.Domains.Dns.GetHosts((DnsGetHostsRequest)request),
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
