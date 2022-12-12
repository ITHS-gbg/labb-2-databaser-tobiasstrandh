using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2Databaser.Managers;

namespace Labb2Databaser.ViewModels;

public class NewBookViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly BookStoreManager _bookStoreManager;
    public NewBookViewModel(NavigationManager navigationManager, BookStoreManager bookStoreManager)
    {
        _bookStoreManager = bookStoreManager;
        _navigationManager = navigationManager;
        
    }
}