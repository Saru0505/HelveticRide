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

      RoutesList.SelectionChanged += RoutesList_SelectionChanged;

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
    private void RoutesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var route = (Route)RoutesList.SelectedItem;
      if (route != null)
      {
        DetailsStart.Text = $"🚩 Start: {route.Start}";
        DetailsEnd.Text = $"🏁 Ziel: {route.End}";
        DetailsWaypoints.Text = $"🛑 Stopps: {(string.IsNullOrWhiteSpace(route.Waypoints) ? "–" : route.Waypoints)}";
        DetailsFavorite.Text = $"⭐ Favorit: {(route.IsFavorite ? "Ja" : "Nein")}";
        DetailsCreatedAt.Text = $"🕓 Erstellt am: {route.CreatedAt}";
        DetailsDistance.Text = $"📏 Distanz: {route.Distance}";
        DetailsDuration.Text = $"🕒 Dauer: {route.Duration}";


      }
      else
      {
        DetailsStart.Text = "";
        DetailsEnd.Text = "";
        DetailsWaypoints.Text = "";
        DetailsFavorite.Text = "";
        

      }
    }


    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(new MapPage(selectedRoute));

    }

    private void ToggleFavorite_Click(object sender, RoutedEventArgs e)
    {
      var selected = (Route)RoutesList.SelectedItem;
      if (selected != null)
      {
        selected.IsFavorite = !selected.IsFavorite;
        _database.UpdateFavoriteStatus(selected.Id, selected.IsFavorite);
        LoadRoutes();
      }
    }

  }
}
