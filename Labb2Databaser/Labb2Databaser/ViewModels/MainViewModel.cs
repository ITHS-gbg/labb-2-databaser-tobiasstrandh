using CommunityToolkit.Mvvm.ComponentModel;
using Labb2Databaser.Managers;

namespace Labb2Databaser.ViewModels;

public class MainViewModel: ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly BookStoreManager _bookStoreManager;

    public ObservableObject CurrentViewModel => _navigationManager.CurrentViewModel;


    public MainViewModel(NavigationManager navigationManager, BookStoreManager bookStoreManager)
    {
        _navigationManager = navigationManager;
        _bookStoreManager = bookStoreManager;

        _navigationManager.CurrentViewModelChanged += CurrentViewModelChanged;
    }

    private void CurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel)); ;
    }
}