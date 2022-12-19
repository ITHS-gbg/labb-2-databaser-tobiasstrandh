using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Windows.Input;
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


        ClearCommand = new RelayCommand(() => ClearFields());

        NewBookCommand = new RelayCommand(() => AddNewBook());

    }

  

    public ICommand ClearCommand { get; }

    public ICommand NewBookCommand { get; }

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
            if(SelectedBook != null)
            {
                ISBN13 = SelectedBook.Isbn;
                Title = SelectedBook.Titel;
                DateForBook = SelectedBook.Utgivningsdatum;
                BookPrice = SelectedBook.Pris;
                Language = SelectedBook.SpråkNavigation.Spårk;
                BookGenre = SelectedBook.Genre.GenreNamn;
                BookPublisher = SelectedBook.Förlag.FörlagNamn;
                BookFormat = SelectedBook.BokFormat.BokFormat;
                BookAuthor = new ObservableCollection<FörfattareTbl>(SelectedBook.Författares);
            }

        }
    }

    private ObservableCollection<FörfattareTbl> _checkBox;

    public ObservableCollection<FörfattareTbl> CheckBox
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

    private ObservableCollection<FörfattareTbl> _bookAuthor = new ObservableCollection<FörfattareTbl>();
    /// <summary>
    /// //
    /// </summary>
    public ObservableCollection<FörfattareTbl> BookAuthor
    {
        get { return _bookAuthor; }
        set
        {
            SetProperty(ref _bookAuthor, value);
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

    private FörfattareTbl _author;

    public FörfattareTbl Author
    {
        get { return _author; }
        set
        {
            SetProperty(ref _author, value);

           
                foreach (var author in BookAuthor)
                {
                    if (author.Id == Author.Id)
                    {
                        return;
                    }
                }

                BookAuthor.Add(Author);
            
        }
    }

    public void AddNewBook()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        var books = bokHandelDbContext.BöckerTbls;

        foreach (var book in books)
        {
            if (book.Isbn == ISBN13)
            {
                return;
            }
        }

        var språk = bokHandelDbContext.SpårkTbls
            .FirstOrDefault(s => s.Spårk.Equals(Language));

        if (språk == null)
        {
            var numberLanguage = bokHandelDbContext.SpårkTbls.OrderBy(s => s.Id).Last();

            var newLanguage = new SpårkTbl() { Spårk = Language, Id = numberLanguage.Id + 1 };

            bokHandelDbContext.SpårkTbls.Add(newLanguage);
            bokHandelDbContext.SaveChanges();

        }

        var bokformat = bokHandelDbContext.BokFormatTbls.FirstOrDefault(b => b.BokFormat.Equals(BookFormat));
       
        if (bokformat == null)
        {
            var number = bokHandelDbContext.BokFormatTbls.OrderBy(b => b.Id).Last();

            var newBokformat = new BokFormatTbl() { BokFormat = BookFormat, Id = number.Id + 1 };

            bokHandelDbContext.BokFormatTbls.Add(newBokformat);
            bokHandelDbContext.SaveChanges();
        }

        var genre = bokHandelDbContext.GenreTbls.FirstOrDefault(g => g.GenreNamn.Equals(BookGenre));
        if (genre == null)
        {
            var number = bokHandelDbContext.GenreTbls.OrderBy(q => q.Id).Last();

            var newGenre = new GenreTbl() { GenreNamn = BookGenre, Id = number.Id + 1 };

            bokHandelDbContext.GenreTbls.Add(newGenre);
            bokHandelDbContext.SaveChanges();
        }

        var förlag = bokHandelDbContext.FörlagTbls.FirstOrDefault(f => f.FörlagNamn.Equals(BookPublisher));
        if (förlag == null)
        {
            var number = bokHandelDbContext.FörlagTbls.OrderBy(f => f.Id).Last();

            var newFörlag = new FörlagTbl()
            {
                FörlagNamn = BookPublisher, Id = number.Id + 1
            };

            bokHandelDbContext.FörlagTbls.Add(newFörlag);
            bokHandelDbContext.SaveChanges();
        }


        genre = bokHandelDbContext.GenreTbls.FirstOrDefault(g => g.GenreNamn.Equals(BookGenre));

        språk = bokHandelDbContext.SpårkTbls
            .FirstOrDefault(s => s.Spårk.Equals(Language));

        bokformat = bokHandelDbContext.BokFormatTbls.FirstOrDefault(b => b.BokFormat.Equals(BookFormat));

        förlag = bokHandelDbContext.FörlagTbls.FirstOrDefault(f => f.FörlagNamn.Equals(BookPublisher));

        

        var newBook = new BöckerTbl()
        {
            Isbn = ISBN13,
            Titel = Title,
            Utgivningsdatum = DateForBook,
            Pris = BookPrice,
            Språk = språk.Id,
            BokFormatId = bokformat.Id,
            GenreId = genre.Id,
            FörlagId = förlag.Id,
            Författares = BookAuthor //lagt till set

        };

        

        bokHandelDbContext.BöckerTbls.Add(newBook);
        
        bokHandelDbContext.SaveChanges();
        AllBooks();
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

    public void ClearFields()
    {
        ISBN13 = string.Empty;
        Title = string.Empty;
        DateForBook = DateTime.Today;
        BookPrice = null;
        Language = String.Empty;
        BookGenre = string.Empty;
        BookPublisher = string.Empty;
        BookFormat = string.Empty;
        BookAuthor = new ObservableCollection<FörfattareTbl>();
    }
}