using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace helveticride
{
  public partial class RoutesWindow : Window
  {
    private Database _database;

    public RoutesWindow()
    {
      InitializeComponent();
      _database = new Database();
      LoadRoutes();
    }

    private void LoadRoutes()
    {
      string routes = _database.GetAllRoutes();
      RoutesTextBox.Text = routes;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      new MainWindow().Show();
      this.Close();
    }
  }
}
