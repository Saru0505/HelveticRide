using System.Windows.Controls;
using System.Windows.Navigation;

namespace helveticride
{
  public partial class HelpWindow : Page
  {
    public HelpWindow()
    {
      InitializeComponent();
    }

    private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      NavigationService?.Navigate(new HomeWindow());
    }
  }
}
