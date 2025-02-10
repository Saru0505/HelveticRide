using System;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using static System.Net.Mime.MediaTypeNames;

namespace helveticride
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      InitializeWebView();
    }

    private async void InitializeWebView()
    {
      await webView.EnsureCoreWebView2Async();
      string relativePath = "map.html";
      string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
      string fullPath = System.IO.Path.Combine(projectDirectory, relativePath);
      webView.Source = new Uri("file:///" + fullPath);
    }


  }
}