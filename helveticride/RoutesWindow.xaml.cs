using System.Collections.Generic;
using System.Windows;

namespace helveticride
{
  public partial class RoutesWindow : Window
  {
    private Database _database;
    private List<Route> _routeList;

    public RoutesWindow()
    {
      InitializeComponent();
      WindowState = WindowState.Maximized;
      _database = new Database();
      LoadRoutes();
    }

    private void LoadRoutes()
    {
      _routeList = _database.GetRouteList();
      RoutesList.ItemsSource = _routeList;
    }

    private void ShowRoute_Click(object sender, RoutedEventArgs e)
    {
      var selectedRoute = (Route)RoutesList.SelectedItem;
      if (selectedRoute != null)
      {
        var mainWindow = new MainWindow(selectedRoute);
        mainWindow.Show();
        this.Close();
      }
      else
      {
        MessageBox.Show("Bitte wähle eine Route aus.");
      }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      new MainWindow().Show();
      this.Close();
    }
  }
}
