using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace helveticride
{
  public partial class RoutesWindow : Page
  {
    private Database _database;
    private List<Route> _routeList;
    private Route selectedRoute;

    public RoutesWindow()
    {
      InitializeComponent();
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
        ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new MapPage(selectedRoute));

      }
      else
      {
        MessageBox.Show("Bitte wähle eine Route aus.");
      }
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new MapPage(selectedRoute));

    }
  }
}
