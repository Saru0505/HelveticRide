using System.Windows;
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

    private void SubmitFeedback_Click(object sender, RoutedEventArgs e)
    {
      string subject = SubjectBox.Text.Trim();
      string description = DescriptionBox.Text.Trim();
      string email = EmailBox.Text.Trim();

      if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(description))
      {
        MessageBox.Show("Bitte gib einen Betreff und eine Beschreibung ein.", "Unvollständig", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      // Platzhalter für Datenbank oder Logging
      MessageBox.Show("Vielen Dank für dein Feedback!", "Erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information);

      // Eingaben leeren
      SubjectBox.Clear();
      DescriptionBox.Clear();
      EmailBox.Clear();
    }

    private void Back_Click(object sender, RoutedEventArgs e)
    {
      NavigationService?.GoBack(); // Geht zur vorherigen Seite zurück
    }
  }
}
