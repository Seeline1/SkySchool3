using System;
using System.Windows;
using SkySchool3.Classes;
using SkySchool3.Pages;

namespace SkySchool3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.MainFrame = MainFrame;
            Manager.MainFrame.Navigate(new PageAuthorization());
        }
    }
}