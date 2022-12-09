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
        private readonly BookStoreManager _bookStoreManager;
        public App()
        {
            _navigationManager = new NavigationManager();
            _bookStoreManager = new BookStoreManager();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);


            _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _bookStoreManager);

          
            var mainWindow = new MainWindow() { DataContext = new MainViewModel(_navigationManager, _bookStoreManager) };

            
            mainWindow.Show();
        }
    }
}
