using System.Windows.Controls;

namespace helveticride
{
  public partial class HomeWindow : Page
  {
    public HomeWindow()
    {
      InitializeComponent();
    }

    private void ToMap_Click(object sender, System.Windows.RoutedEventArgs e) => ((MainWindow)System.Windows.Application.Current.MainWindow).MainFrame.Navigate(new MapPage());
    private void ToRoutes_Click(object sender, System.Windows.RoutedEventArgs e) => ((MainWindow)System.Windows.Application.Current.MainWindow).MainFrame.Navigate(new RoutesWindow());
    private void ToSettings_Click(object sender, System.Windows.RoutedEventArgs e) => ((MainWindow)System.Windows.Application.Current.MainWindow).MainFrame.Navigate(new SettingsWindow());
    private void ToHelp_Click(object sender, System.Windows.RoutedEventArgs e) => ((MainWindow)System.Windows.Application.Current.MainWindow).MainFrame.Navigate(new HelpWindow());
  }
}