using System.Windows;

namespace helveticride
{
  public partial class RegisterWindow : Window
  {
    private UserDatabase _userDb;

    public RegisterWindow()
    {
      InitializeComponent();
      _userDb = new UserDatabase();
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
  }
}
