using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace helveticride
{
  public partial class MainWindow : Window
  {
    private Database _database;
    private Route _loadedRoute;

    public MainWindow()
    {
      InitializeComponent();
      WindowState = WindowState.Maximized;
      _database = new Database();
      InitializeWebView();
    }

    public MainWindow(Route route) : this()
    {
      _loadedRoute = route;
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

        // Falls Route übergeben wurde → anzeigen
        if (_loadedRoute != null)
        {
          webView.CoreWebView2.NavigationCompleted += (s, e) =>
          {
            string script = $@"
              showRouteFromDatabase(
                '{EscapeJs(_loadedRoute.Start)}',
                '{EscapeJs(_loadedRoute.End)}',
                '{EscapeJs(_loadedRoute.Waypoints)}'
              );
            ";
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

          if (!string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end))
          {
            _database.SaveRoute(start, end, waypoints);
          }
        }
        else if (json?.type == "navigate")
        {
          string target = json.target;

          Application.Current.Dispatcher.Invoke(() =>
          {
            Window window = target switch
            {
              "home" => new HomeWindow(),
              "routes" => new RoutesWindow(),
              "settings" => new SettingsWindow(),
              "help" => new HelpWindow(),
              "map" => new MainWindow(),
              _ => null
            };

            if (window != null)
            {
              window.Show();
              this.Close();
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
