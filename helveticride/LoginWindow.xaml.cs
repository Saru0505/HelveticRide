
using System.Windows;
using System.Windows.Controls;

namespace helveticride
{
  public partial class LoginWindow : Window
  {
    private Database _userDb;

    public LoginWindow()
    {
      InitializeComponent();
      _userDb = new Database();
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
