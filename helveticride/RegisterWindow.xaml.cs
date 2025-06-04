using System.Windows;
using System.Windows.Controls;

namespace helveticride
{
  public partial class RegisterWindow : Window
  {
    private Database _userDb;

    public RegisterWindow()
    {
      InitializeComponent();
      _userDb = new Database();
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
      string username = UsernameBox.Text.Trim();
      string password = PasswordBox.Password.Trim();

      if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
      {
        MessageBox.Show("Bitte Benutzername und Passwort eingeben.");
        return;
      }

      try
      {
        _userDb.AddUser(username, password);
        MessageBox.Show("Registrierung erfolgreich!");
        this.Close();
      }
      catch (System.Exception ex)
      {
        MessageBox.Show("Fehler bei der Registrierung:\n" + ex.Message);
      }
    }
    private void UsernameBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      UsernamePlaceholder.Visibility = string.IsNullOrEmpty(UsernameBox.Text)
          ? Visibility.Visible
          : Visibility.Collapsed;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
      PasswordPlaceholder.Visibility = string.IsNullOrEmpty(PasswordBox.Password)
          ? Visibility.Visible
          : Visibility.Collapsed;
    }

  }
}
