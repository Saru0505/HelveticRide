using System.Windows;

namespace helveticride
{
  public partial class HomeWindow : Window
  {
    public HomeWindow()
    {
      InitializeComponent();  // Stellt sicher, dass das XAML korrekt initialisiert wird
    }


    // Der Event-Handler für den Button-Klick
    private void GoToRoutePlanner_Click(object sender, RoutedEventArgs e)
    {
      // Öffne das MainWindow
      MainWindow mainWindow = new MainWindow();
      mainWindow.Show();

      // Schließe das aktuelle HomeWindow
      this.Close();
    }
  }
}

