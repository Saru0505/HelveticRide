using System.Windows;

namespace helveticride
{
  public partial class HomeWindow : Window
  {
    public HomeWindow()
    {
      InitializeComponent();  // Stellt sicher, dass das XAML korrekt initialisiert wird
    }

    private void ToMap_Click(object sender, RoutedEventArgs e) => Navigate(new MainWindow());
    private void ToRoutes_Click(object sender, RoutedEventArgs e) => Navigate(new RoutesWindow());
    private void ToSettings_Click(object sender, RoutedEventArgs e) => Navigate(new SettingsWindow());
    private void ToHelp_Click(object sender, RoutedEventArgs e) => Navigate(new HelpWindow());

    private void Navigate(Window window)
    {
      window.Show();
      this.Close();
    }
  }
}

