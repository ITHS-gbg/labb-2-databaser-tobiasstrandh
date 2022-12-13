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
    }

    public ICommand ClearCommand { get; }

    public ICommand SaveAuthorCommand { get; }

    public ICommand RemoveAuthorCommand { get; }

    private FörfattareTbl _author;

    public FörfattareTbl Author
    {
        get { return _author; }
        set
        {
            SetProperty(ref _author, value);
            FirstName = Author.Förnamn;
            LastName = Author.Efternamn;
            DayOfBirth = Author.Födelsedatum;
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
        set { SetProperty(ref _DayOfBirth, value); }
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
               // var hej = bokHandelDbContext.FörfattareTbls.Where(a => a.Förnamn.Equals(FirstName) && a.Efternamn.Equals(LastName));
               return;
            }
        }


        if (FirstName != String.Empty && LastName != String.Empty)
        {
            var NewAuthor = new FörfattareTbl() { Förnamn = FirstName, Efternamn = LastName, Födelsedatum = DayOfBirth};
           

            bokHandelDbContext.FörfattareTbls.Add(NewAuthor);

            bokHandelDbContext.SaveChanges();
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
    }

    public void ClearFields()
    {
        FirstName = string.Empty; 
        LastName = string.Empty;
        DayOfBirth = DateTime.Today;
    }
}