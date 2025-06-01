using System.Windows.Controls;
using System.Windows.Navigation;

namespace helveticride
{
  public partial class SettingsWindow : Page
  {
    public SettingsWindow()
    {
      InitializeComponent();
    }

    private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      NavigationService?.Navigate(new HomeWindow());
    }
  }
}