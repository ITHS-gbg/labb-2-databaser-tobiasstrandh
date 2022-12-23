using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Labb2Databaser.Managers;
using Labb2Databaser.ViewModels;

namespace Labb2Databaser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationManager _navigationManager;
        public App()
        {
            _navigationManager = new NavigationManager();
            
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager);

          
            var mainWindow = new MainWindow() { DataContext = new MainViewModel(_navigationManager) };

            
            mainWindow.Show();
        }
    }
}
