
using System.Windows;
using System.Windows.Controls;

namespace helveticride
{
  public partial class LoginWindow : Window
  {
    private UserDatabase _userDb;

    public LoginWindow()
    {
      InitializeComponent();
      _userDb = new UserDatabase();
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
      string username = UsernameBox.Text.Trim();
      string password = PasswordBox.Password.Trim();

      if (_userDb.ValidateUser(username, password))
      {
        int userId = _userDb.GetUserId(username);
        UserSession.CurrentUserId = userId;
        MessageBox.Show("Login erfolgreich!");
        this.DialogResult = true;
        this.Close();
      }
      else
      {
        MessageBox.Show("Benutzername oder Passwort ist falsch.");
      }
    }

    private void Register_Click(object sender, RoutedEventArgs e)
    {
      var registerWindow = new RegisterWindow();
      registerWindow.ShowDialog();
    }
  }
}
