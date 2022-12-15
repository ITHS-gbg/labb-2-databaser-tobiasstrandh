using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2Databaser.Managers;
using Labb2Databaser.Models;

namespace Labb2Databaser.ViewModels;

public class NewAuthorViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly BookStoreManager _bookStoreManager;
    public NewAuthorViewModel(NavigationManager navigationManager, BookStoreManager bookStoreManager)
    {
        _bookStoreManager = bookStoreManager;
        _navigationManager = navigationManager;

        GetFörfattareTbl();

        

        ClearCommand = new RelayCommand(() => ClearFields());

        SaveAuthorCommand = new RelayCommand(() => SaveAuthor());

        RemoveAuthorCommand = new RelayCommand(() => RemoveAuthor());

        EditAuthorCommand = new RelayCommand(() => EditAuthor());
    }

    public ICommand ClearCommand { get; }

    public ICommand SaveAuthorCommand { get; }

    public ICommand RemoveAuthorCommand { get; }
    public ICommand EditAuthorCommand { get; }

    private FörfattareTbl _selectedAuthor;

    public FörfattareTbl SelectedAuthor
    {
        get { return _selectedAuthor; }
        set
        {
            SetProperty(ref _selectedAuthor, value);
            if (SelectedAuthor != null)
            {
                FirstName = SelectedAuthor.Förnamn;
                LastName = SelectedAuthor.Efternamn;
                DayOfBirth = SelectedAuthor.Födelsedatum;
            }
        }
    }

    private string _firstName;

    public string FirstName
    {
        get { return _firstName; }
        set { SetProperty(ref _firstName, value); }
    }

    private string _lastName;

    public string LastName
    {
        get { return _lastName; }
        set { SetProperty(ref _lastName, value); }
    }

    private DateTime? _DayOfBirth;

    public DateTime? DayOfBirth
    {
        get { return _DayOfBirth; }
        set
        {
            SetProperty(ref _DayOfBirth, value);
        }
    }

    private ObservableCollection<FörfattareTbl> _authors;

    public ObservableCollection<FörfattareTbl> Authors
    {
        get { return _authors; }
        set
        {
            SetProperty(ref _authors, value); 

        }
    }

    public void GetFörfattareTbl()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        Authors = new ObservableCollection<FörfattareTbl>(bokHandelDbContext.FörfattareTbls);

    }

    public void SaveAuthor()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        var authors = bokHandelDbContext.FörfattareTbls;

        foreach (var author in authors)
        {
            if (author.Förnamn == FirstName && author.Efternamn == LastName)
            {
                return;
            }
        }


        if (FirstName != String.Empty && LastName != String.Empty && DayOfBirth != null)
        {
            var NewAuthor = new FörfattareTbl() { Förnamn = FirstName, Efternamn = LastName, Födelsedatum = DayOfBirth};
           

            bokHandelDbContext.FörfattareTbls.Add(NewAuthor);

            bokHandelDbContext.SaveChanges();

            GetFörfattareTbl();
        }
    }

    public void RemoveAuthor()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        var hej = bokHandelDbContext.FörfattareTbls.FirstOrDefault(a => a.Förnamn.Equals(FirstName) && a.Efternamn.Equals(LastName));

        if (hej == null || FirstName == null || LastName == null)
        {
            return;
        }

        

        bokHandelDbContext.FörfattareTbls.Remove(hej);

        bokHandelDbContext.SaveChanges();

        GetFörfattareTbl();
    }

    public void EditAuthor()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        var author = bokHandelDbContext.FörfattareTbls.FirstOrDefault(a => a.Id.Equals(SelectedAuthor.Id));

        if (author != null)
        {
            author.Förnamn = FirstName;
            author.Efternamn = LastName;
            author.Födelsedatum = DayOfBirth;
            bokHandelDbContext.SaveChanges();
            GetFörfattareTbl();
        }

    }

    public void ClearFields()
    {
        FirstName = string.Empty; 
        LastName = string.Empty;
        DayOfBirth = DateTime.Today;
       // SelectedAuthor = null;
    }
}