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
using Microsoft.IdentityModel.Tokens;

namespace Labb2Databaser.ViewModels;

public class NewBookViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    
    public NewBookViewModel(NavigationManager navigationManager)
    {
        
        _navigationManager = navigationManager;
        AllBooks();
        GetFörfattareTbl();


        ClearCommand = new RelayCommand(() => ClearAndUpdate());

        NewBookCommand = new RelayCommand(() => AddNewBook());

        RemoveAuthorFromTheBookCommand = new RelayCommand(() => RemoveAuthorFromBook());

        EditBookCommand = new RelayCommand(() => EditBook());

        RemoveBookCommand = new RelayCommand(() => RemoveBook());

        GoBackToStartCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager));
    }

    public ICommand RemoveAuthorFromTheBookCommand { get; }

    public ICommand ClearCommand { get; }

    public ICommand NewBookCommand { get; }

    public ICommand EditBookCommand { get; }

    public ICommand RemoveBookCommand { get; }

    public ICommand GoBackToStartCommand { get; }

    private ObservableCollection<BöckerTbl>? _books;

    public ObservableCollection<BöckerTbl>? Books
    {
        get { return _books; }
        set { SetProperty(ref _books, value); }
    }


    private BöckerTbl? _selectedBook;

    public BöckerTbl? SelectedBook
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
                BookGenre = SelectedBook.Genre!.GenreNamn;
                BookPublisher = SelectedBook.Förlag!.FörlagNamn;
                BookFormat = SelectedBook.BokFormat!.BokFormat;
                BookAuthor = new ObservableCollection<FörfattareTbl>(SelectedBook.Författares);
                Author = null!;
                CanUseSaveButton = false;
                CanChangeISBN13 = false;
                CanUseEditButton = true;
            }

        }
    }

  
    private string? _isbn13;

    public string? ISBN13
    {
        get { return _isbn13; }
        set
        {
            SetProperty(ref _isbn13, value);
            CheckFields();
        }
    }

    private string? _title;

    public string? Title
    {
        get { return _title; }
        set
        {
            SetProperty(ref _title, value);
            CheckFields();
        }
    }

    private DateTime? _dateForBook;

    public DateTime? DateForBook
    {
        get { return _dateForBook; }
        set
        {
            SetProperty(ref _dateForBook, value);
            CheckFields();
        }
    }

    private int? _bookPrice;

    public int? BookPrice
    {
        get { return _bookPrice; }
        set
        {
            SetProperty(ref _bookPrice, value);
            CheckFields();
        }
    }

    private string? _language;

    public string? Language
    {
        get { return _language; }
        set
        {
            SetProperty(ref _language, value);
            CheckFields();
        }
    }

    private string? _bookGenre;

    public string? BookGenre
    {
        get { return _bookGenre; }
        set
        {
            SetProperty(ref _bookGenre, value);
            CheckFields();
        }
    }

    private string? _bookPublisher;

    public string? BookPublisher
    {
        get { return _bookPublisher; }
        set
        {
            SetProperty(ref _bookPublisher, value);
            CheckFields();
        }
    }

    private string? _bookFormat;

    public string? BookFormat
    {
        get { return _bookFormat; }
        set
        {
            SetProperty(ref _bookFormat, value);
            CheckFields();
        }
    }

    private ObservableCollection<FörfattareTbl>? _bookAuthor = new ObservableCollection<FörfattareTbl>();

    public ObservableCollection<FörfattareTbl>? BookAuthor
    {
        get { return _bookAuthor; }
        set
        {
            SetProperty(ref _bookAuthor, value);
        }
    }

    private ObservableCollection<FörfattareTbl>? _authors;

    public ObservableCollection<FörfattareTbl>? Authors
    {
        get { return _authors; }
        set
        {
            SetProperty(ref _authors, value);

        }
    }

    private FörfattareTbl? _author;

    public FörfattareTbl? Author
    {
        get { return _author; }
        set
        {
            SetProperty(ref _author, value);


            if (Author != null)
            {
                foreach (var author in BookAuthor!)
                {
                    if (author.Id == Author.Id)
                    {
                        return;
                    }
                }

                BookAuthor.Add(Author);
            }
            
        }
    }

    private FörfattareTbl? _removeAuthor;

    public FörfattareTbl? RemoveAuthor
    {
        get { return _removeAuthor; }
        set
        {
            SetProperty(ref _removeAuthor, value);

        }
    }

    private bool _canUseEditButton = false;

    public bool CanUseEditButton
    {
        get { return _canUseEditButton; }
        set { SetProperty(ref _canUseEditButton, value); }
    }

    private bool _canChangeIsbn13 = true;

    public bool CanChangeISBN13
    {
        get { return _canChangeIsbn13; }
        set { SetProperty(ref _canChangeIsbn13, value); }
    }

    public void RemoveAuthorFromBook()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        if (RemoveAuthor != null)
        {
            var author = bokHandelDbContext.BöckerTbls.Include(a => a.Författares).FirstOrDefault(b => b.Isbn.Equals(ISBN13));

            if (author == null)
            {
                BookAuthor = null;
                return;
            }

            var authors = author.Författares;
            

            foreach (var aut in authors.ToList())
            {
                if (aut.Id == RemoveAuthor.Id)
                {
                    authors.Remove(aut);
                }
            }

            author.Författares = authors;

            bokHandelDbContext.SaveChanges();
            AllBooks();
            var bookAuthor = bokHandelDbContext.BöckerTbls.Include(a => a.Författares).FirstOrDefault(b => b.Isbn.Equals(ISBN13));
            BookAuthor = new ObservableCollection<FörfattareTbl>(bookAuthor!.Författares);
        }

    }

    public void EditBook()
    {
        
        using (var bokHandelDbContext = new BokHandelDbContext())
        {
            var editBook = bokHandelDbContext.BöckerTbls
            .Include(a => a.Författares)
            .FirstOrDefault(b => b.Isbn.Equals(ISBN13));

            if (editBook == null)
            {
                return;
            }



            editBook.Titel = Title!;
            editBook.Pris = BookPrice;
            editBook.Utgivningsdatum = DateForBook;


            var authors = editBook.Författares;
            if (authors.Count != 0)
            {
                foreach (var bAuthor in BookAuthor!)
                {
                    foreach (var author in authors.ToList())
                    {
                        if (author.Id != bAuthor.Id)
                        {
                            authors.Add(bAuthor);
                        }
                    }
                }

                editBook.Författares = authors;
            }

            else
            {
                editBook.Författares = BookAuthor!;
            }





            var språk = bokHandelDbContext.SpårkTbls
                .FirstOrDefault(s => s.Spårk.Equals(Language));

            if (språk == null)
            {
                var numberLanguage = bokHandelDbContext.SpårkTbls.OrderBy(s => s.Id).Last();

                var newLanguage = new SpårkTbl() { Spårk = Language!, Id = numberLanguage.Id + 1 };

                bokHandelDbContext.SpårkTbls.Add(newLanguage);
                bokHandelDbContext.SaveChanges();

                språk = bokHandelDbContext.SpårkTbls
                    .First(s => s.Spårk.Equals(Language));
            }

            editBook.Språk = språk.Id;

            var bokformat = bokHandelDbContext.BokFormatTbls.FirstOrDefault(b => b.BokFormat.Equals(BookFormat));

            if (bokformat == null)
            {
                var number = bokHandelDbContext.BokFormatTbls.OrderBy(b => b.Id).Last();

                var newBokformat = new BokFormatTbl() { BokFormat = BookFormat!, Id = number.Id + 1 };

                bokHandelDbContext.BokFormatTbls.Add(newBokformat);
                bokHandelDbContext.SaveChanges();

                bokformat = bokHandelDbContext.BokFormatTbls.First(b => b.BokFormat.Equals(BookFormat));
            }

            editBook.BokFormatId = bokformat.Id;

            var genre = bokHandelDbContext.GenreTbls.FirstOrDefault(g => g.GenreNamn.Equals(BookGenre));
            if (genre == null)
            {
                var number = bokHandelDbContext.GenreTbls.OrderBy(q => q.Id).Last();

                var newGenre = new GenreTbl() { GenreNamn = BookGenre!, Id = number.Id + 1 };

                bokHandelDbContext.GenreTbls.Add(newGenre);
                bokHandelDbContext.SaveChanges();

                genre = bokHandelDbContext.GenreTbls.First(g => g.GenreNamn.Equals(BookGenre));
            }

            editBook.GenreId = genre.Id;

            var förlag = bokHandelDbContext.FörlagTbls.FirstOrDefault(f => f.FörlagNamn.Equals(BookPublisher));
            if (förlag == null)
            {
                var number = bokHandelDbContext.FörlagTbls.OrderBy(f => f.Id).Last();

                var newFörlag = new FörlagTbl()
                {
                    FörlagNamn = BookPublisher!,
                    Id = number.Id + 1
                };

                bokHandelDbContext.FörlagTbls.Add(newFörlag);
                bokHandelDbContext.SaveChanges();

                förlag = bokHandelDbContext.FörlagTbls.First(f => f.FörlagNamn.Equals(BookPublisher));
            }

            editBook.FörlagId = förlag.Id;

            bokHandelDbContext.SaveChanges();
            editBook = null;
            ClearAndUpdate();
        }
    }

    public void AddNewBook()
    {
        

        using (var bokHandelDbContext = new BokHandelDbContext())
        {
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

                var newLanguage = new SpårkTbl() { Spårk = Language!, Id = numberLanguage.Id + 1 };

                bokHandelDbContext.SpårkTbls.Add(newLanguage);
                bokHandelDbContext.SaveChanges();

            }

            var bokformat = bokHandelDbContext.BokFormatTbls.FirstOrDefault(b => b.BokFormat.Equals(BookFormat));

            if (bokformat == null)
            {
                var number = bokHandelDbContext.BokFormatTbls.OrderBy(b => b.Id).Last();

                var newBokformat = new BokFormatTbl() { BokFormat = BookFormat!, Id = number.Id + 1 };

                bokHandelDbContext.BokFormatTbls.Add(newBokformat);
                bokHandelDbContext.SaveChanges();
            }

            var genre = bokHandelDbContext.GenreTbls.FirstOrDefault(g => g.GenreNamn.Equals(BookGenre));
            if (genre == null)
            {
                var number = bokHandelDbContext.GenreTbls.OrderBy(q => q.Id).Last();

                var newGenre = new GenreTbl() { GenreNamn = BookGenre!, Id = number.Id + 1 };

                bokHandelDbContext.GenreTbls.Add(newGenre);
                bokHandelDbContext.SaveChanges();
            }

            var förlag = bokHandelDbContext.FörlagTbls.FirstOrDefault(f => f.FörlagNamn.Equals(BookPublisher));
            if (förlag == null)
            {
                var number = bokHandelDbContext.FörlagTbls.OrderBy(f => f.Id).Last();

                var newFörlag = new FörlagTbl()
                {
                    FörlagNamn = BookPublisher!,
                    Id = number.Id + 1
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
                Isbn = ISBN13!,
                Titel = Title!,
                Utgivningsdatum = DateForBook,
                Pris = BookPrice,
                Språk = språk!.Id,
                BokFormatId = bokformat!.Id,
                GenreId = genre!.Id,
                FörlagId = förlag!.Id


            };


            bokHandelDbContext.BöckerTbls.Add(newBook);
            bokHandelDbContext.SaveChanges();
            

            var recentlyAddedBook = bokHandelDbContext.BöckerTbls.First(b => b.Isbn.Equals(ISBN13));
            recentlyAddedBook.Författares = BookAuthor!;


            bokHandelDbContext.SaveChanges();


            
            ClearAndUpdate();
        }
    }

    public void RemoveBook()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        if (SelectedBook == null)
        {
            return;
        }

        var book = bokHandelDbContext.BöckerTbls.First(b => b.Isbn.Equals(SelectedBook.Isbn));

        bokHandelDbContext.BöckerTbls.Remove(book);
        bokHandelDbContext.SaveChanges();
        
        ClearAndUpdate();
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

    public void CheckFields()
    {
        if (ISBN13 == null)
        {
            CanUseSaveButton = false;
            return;
        }

        if (ISBN13.Length == 13)
        {
            if (Title != String.Empty)
            {
                if (DateForBook != null)
                {
                    if (BookGenre != String.Empty)
                    {
                        if (BookPublisher != String.Empty)
                        {
                            if (BookPrice != null)
                            {
                                if (Language != String.Empty)
                                {
                                    if (BookFormat != String.Empty)
                                    {
                                        CanUseSaveButton = true;
                                    }

                                    else
                                    {
                                        CanUseSaveButton = false;
                                    }
                                }

                                else
                                {
                                    CanUseSaveButton = false;
                                }
                            }

                            else
                            {
                                CanUseSaveButton = false;
                            }
                        }

                        else
                        {
                            CanUseSaveButton = false;
                        }
                    }

                    else
                    {
                        CanUseSaveButton = false;
                    }
                }

                else
                {
                    CanUseSaveButton = false;
                }
            }

            else
            {
                CanUseSaveButton = false;
            }
        }

        else
        {
            CanUseSaveButton = false;
        }

    }

    public void ClearAndUpdate()
    {
        Books!.Clear();
        Authors!.Clear();
        ISBN13 = null;
        Title = null;
        DateForBook = DateTime.Today;
        BookPrice = null;
        Language = null;
        BookGenre = null;
        BookPublisher = null;
        BookFormat = null;
        BookAuthor!.Clear();
        SelectedBook = null!;
        CanChangeISBN13 = true;
        CanUseEditButton = false;
        Author = null;
        AllBooks();
        GetFörfattareTbl();
    }

    private bool _canUseSaveButton;

    public bool CanUseSaveButton
    {
        get { return _canUseSaveButton; }
        set { SetProperty(ref _canUseSaveButton, value); }
    }
}