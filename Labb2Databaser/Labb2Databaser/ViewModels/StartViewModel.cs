using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Input;
using Labb2Databaser.Managers;

namespace Labb2Databaser.ViewModels;

public class StartViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
   private readonly BookStoreManager _bookStoreManager;
    public StartViewModel(NavigationManager navigationManager, BookStoreManager bookStoreManager)
    {
        _bookStoreManager = bookStoreManager;
        _navigationManager = navigationManager;

        GoToBalanceViewCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StoreBalanceViewModel(_navigationManager, _bookStoreManager));
    }

    public ICommand GoToBalanceViewCommand { get; }
}