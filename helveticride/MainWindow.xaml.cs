using System.Windows;
using System.Windows.Controls;
using helveticride; // für Route-Definition

namespace helveticride
{
  public partial class MainWindow : Window
  {
    private Route _loadedRoute;

    public MainWindow()
    {
      InitializeComponent();
      MainFrame.Navigate(new HomeWindow()); // Startseite setzen
    }

    public MainWindow(Route route) : this()
    {
      _loadedRoute = route;
      MainFrame.Navigate(new MapPage(route)); // initial Route laden
    }

    public void NavigateTo(Page page)
    {
      MainFrame.Navigate(page);
    }
    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
      WindowState = WindowState.Minimized;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

  }
}