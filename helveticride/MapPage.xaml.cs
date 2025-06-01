using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace helveticride
{
  public partial class MapPage : Page
  {
    private Database _database;
    private Route _loadedRoute;

    private string _currentDistance;
    private string _currentDuration;

    public MapPage()
    {
      InitializeComponent();
      _database = new Database();
      InitializeWebView();
    }

    public MapPage(Route route)
    {
      InitializeComponent();
      _database = new Database();
      _loadedRoute = route;
      InitializeWebView();
    }

    private async void InitializeWebView()
    {
      try
      {
        await webView.EnsureCoreWebView2Async();

        string relativePath = "map.html";
        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        if (File.Exists(fullPath))
        {
          webView.Source = new Uri("file:///" + fullPath);
        }

        webView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;

        if (_loadedRoute != null)
        {
          webView.CoreWebView2.NavigationCompleted += (s, e) =>
          {
            string script = $@"
              showRouteFromDatabase(
                '{EscapeJs(_loadedRoute.Start)}',
                '{EscapeJs(_loadedRoute.End)}',
                '{EscapeJs(_loadedRoute.Waypoints)}'
              );";
            webView.CoreWebView2.ExecuteScriptAsync(script);
          };
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Fehler beim Initialisieren von WebView2: " + ex.Message);
      }
    }

    private string EscapeJs(string input)
    {
      return input.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
    }

    private void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
      try
      {
        string message = e.TryGetWebMessageAsString();
        dynamic json = JsonConvert.DeserializeObject(message);

        if (json?.type == "saveRoute")
        {
          string start = json.start;
          string end = json.end;
          string waypoints = json.waypoints;

          string distance = _currentDistance ?? "";
          string duration = _currentDuration ?? "";

          if (!string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end))
          {
            _database.SaveRoute(start, end, waypoints, distance, duration);
            MessageBox.Show($"✅ Route gespeichert\n📏 Distanz: {distance}\n🕒 Dauer: {duration}", "Erfolg");
          }
        }
        else if (json?.type == "routeInfo")
        {
          _currentDistance = json.distance;
          _currentDuration = json.duration;
        }
        else if (json?.type == "navigate")
        {
          string target = json.target;

          Application.Current.Dispatcher.Invoke(() =>
          {
            Page targetPage = target switch
            {
              "home" => new HomeWindow(),
              "routes" => new RoutesWindow(),
              "settings" => new SettingsWindow(),
              "help" => new HelpWindow(),
              "map" => new MapPage(),
              _ => null
            };

            if (targetPage != null)
            {
              ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(targetPage);
            }
          });
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Fehler beim Verarbeiten der Nachricht:\n" + ex.Message);
      }
    }
  }
}
