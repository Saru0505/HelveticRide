using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace helveticride
{
  public partial class HelpWindow : Page
  {
    private FeedbackDatabase _feedbackDb;

    public HelpWindow()
    {
      InitializeComponent();
      _feedbackDb = new FeedbackDatabase();
    }

    private void SendFeedback_Click(object sender, RoutedEventArgs e)
    {
      string message = FeedbackBox.Text.Trim();

      if (string.IsNullOrWhiteSpace(message))
      {
        MessageBox.Show("Bitte gib eine Nachricht ein.");
        return;
      }

      int userId = UserSession.CurrentUserId;

      try
      {
        _feedbackDb.AddFeedback(userId, message);
        MessageBox.Show("Vielen Dank für dein Feedback!");
        FeedbackBox.Text = "";
      }
      catch (System.Exception ex)
      {
        MessageBox.Show("Fehler beim Speichern des Feedbacks:\n" + ex.Message);
      }
    }

    private void Back_Click(object sender, System.Windows.RoutedEventArgs e)
    {
      NavigationService?.Navigate(new HomeWindow());
    }
  }
}
