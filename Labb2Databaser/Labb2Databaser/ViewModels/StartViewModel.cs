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
   
    public StartViewModel(NavigationManager navigationManager)
    {
        
        _navigationManager = navigationManager;

        GoToBalanceViewCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StoreBalanceViewModel(_navigationManager));

        GoToNewAuthorViewCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new NewAuthorViewModel(_navigationManager));

        GoToNewBookViewCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new NewBookViewModel(_navigationManager));
    }

    public ICommand GoToBalanceViewCommand { get; }

    public ICommand GoToNewAuthorViewCommand { get; }

    public ICommand GoToNewBookViewCommand { get; }
}