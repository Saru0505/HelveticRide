using Microsoft.Web.WebView2.Core;
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

    public MainWindow()
    {
      InitializeComponent();
      _database = new Database(); // Initialisiere die Datenbankklasse
      InitializeWebView();        // Starte WebView2
    }

    private async void InitializeWebView()
    {
      try
      {
        await webView.EnsureCoreWebView2Async();

        string relativePath = "map.html";
        string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string fullPath = Path.Combine(projectDirectory, relativePath);

        if (File.Exists(fullPath))
        {
          webView.Source = new Uri("file:///" + fullPath);
        }
        else
        {
          MessageBox.Show("Fehler: Die Datei map.html wurde nicht gefunden.");
        }

        webView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
      }
      catch (Exception ex)
      {
        MessageBox.Show("Fehler beim Initialisieren von WebView2: " + ex.Message);
      }
    }

    private void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
      try
      {
        string message = e.TryGetWebMessageAsString();

        // Debug: Zeige die empfangene Nachricht an
        MessageBox.Show("Empfangene Nachricht:\n" + message);

        dynamic json = JsonConvert.DeserializeObject(message);

        if (json?.type == "saveRoute")
        {
          string start = json.start;
          string end = json.end;
          string waypoints = json.waypoints;

          if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end))
          {
            MessageBox.Show("Fehler: Start oder Endpunkt darf nicht leer sein.");
            return;
          }

          // Speichere Route in der Datenbank
          _database.SaveRoute(start, end, waypoints);
          MessageBox.Show("Route erfolgreich gespeichert.");


        }
        else
        {
          MessageBox.Show("Ungültige Nachricht erhalten.");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Fehler beim Verarbeiten der Nachricht:\n" + ex.Message);
      }
    }
  }
}
