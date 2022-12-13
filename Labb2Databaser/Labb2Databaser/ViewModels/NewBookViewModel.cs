using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2Databaser.Managers;
using Labb2Databaser.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb2Databaser.ViewModels;

public class NewBookViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly BookStoreManager _bookStoreManager;
    public NewBookViewModel(NavigationManager navigationManager, BookStoreManager bookStoreManager)
    {
        _bookStoreManager = bookStoreManager;
        _navigationManager = navigationManager;
        AllBooks();
        GetFörfattareTbl();

        
        Check();

    }

    public void Check()
    {
        for (int i = 0; i < CheckBox.Count; i++)
        {
            
                CheckBox[i] = true;
            
        }
    }

    private ObservableCollection<BöckerTbl> _books;

    public ObservableCollection<BöckerTbl> Books
    {
        get { return _books; }
        set { SetProperty(ref _books, value); }
    }


    private BöckerTbl _selectedBook;

    public BöckerTbl SelectedBook
    {
        get { return _selectedBook; }
        set
        {
            SetProperty(ref _selectedBook, value);
            ISBN13 = SelectedBook.Isbn;
            Title = SelectedBook.Titel;
            DateForBook = SelectedBook.Utgivningsdatum;
            BookPrice = SelectedBook.Pris;
            Language = SelectedBook.SpråkNavigation.Spårk;
            BookGenre = SelectedBook.Genre.GenreNamn;
            BookPublisher = SelectedBook.Förlag.FörlagNamn;
            BookFormat = SelectedBook.BokFormat.BokFormat;
            BookAuthor= new ObservableCollection<FörfattareTbl>(SelectedBook.Författares);
            

            //CheckBox = false;
            //BookAuthor = temp.Förnamn + " " + temp.Efternamn;

        }
    }

    private ObservableCollection<bool> _checkBox = new ObservableCollection<bool>(){false, false, true, false,true};

    public ObservableCollection<bool> CheckBox
    {
        get { return _checkBox; }
        set { SetProperty(ref _checkBox, value); }
    }

    private string _isbn13;

    public string ISBN13
    {
        get { return _isbn13; }
        set { SetProperty(ref _isbn13, value); }
    }

    private string _title;

    public string Title
    {
        get { return _title; }
        set { SetProperty(ref _title, value); }
    }

    private DateTime? _dateForBook;

    public DateTime? DateForBook
    {
        get { return _dateForBook; }
        set { SetProperty(ref _dateForBook, value); }
    }

    private int? _bookPrice;

    public int? BookPrice
    {
        get { return _bookPrice; }
        set { SetProperty(ref _bookPrice, value); }
    }

    private string _language;

    public string Language
    {
        get { return _language; }
        set { SetProperty(ref _language, value); }
    }

    private string? _bookGenre;

    public string? BookGenre
    {
        get { return _bookGenre; }
        set { SetProperty(ref _bookGenre, value); }
    }

    private string _bookPublisher;

    public string BookPublisher
    {
        get { return _bookPublisher; }
        set { SetProperty(ref _bookPublisher, value); }
    }

    private string _bookFormat;

    public string BookFormat
    {
        get { return _bookFormat; }
        set { SetProperty(ref _bookFormat, value); }
    }

    private ObservableCollection<FörfattareTbl> _bookAuthor;
    /// <summary>
    /// //
    /// </summary>
    public ObservableCollection<FörfattareTbl> BookAuthor
    {
        get { return _bookAuthor; }
        set { SetProperty(ref _bookAuthor, value); }
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

    private ObservableCollection<FörfattareTbl> _author;

    public ObservableCollection<FörfattareTbl> Author
    {
        get { return _author; }
        set
        {
            SetProperty(ref _author, value);
            //BookAuthor = Author.Förnamn + " " + Author.Efternamn;

        }
    }

    public void AllBooks()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        Books = new ObservableCollection<BöckerTbl>(bokHandelDbContext.BöckerTbls
            .Include(b => b.SpråkNavigation)
            .Include(b => b.Genre)
            .Include(b => b.Förlag)
            .Include(b => b.BokFormat)
            .Include(b => b.Författares));
    }

    public void GetFörfattareTbl()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        Authors = new ObservableCollection<FörfattareTbl>(bokHandelDbContext.FörfattareTbls);

    }
}