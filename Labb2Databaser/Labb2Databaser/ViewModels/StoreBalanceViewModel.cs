﻿using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Labb2Databaser.Managers;
using Labb2Databaser.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb2Databaser.ViewModels;

public class StoreBalanceViewModel : ObservableObject
{
    private readonly NavigationManager _navigationManager;
    private readonly BookStoreManager _bookStoreManager;
    public StoreBalanceViewModel(NavigationManager navigationManager, BookStoreManager bookStoreManager)
    {
        _bookStoreManager = bookStoreManager;
        _navigationManager = navigationManager;
        GetButikTbl();
     
        AddBookCommand = new RelayCommand(() => AddBook());

        RemoveBookCommand = new RelayCommand(() => RemoveBook());

        GoBackToStartCommand = new RelayCommand(() => _navigationManager.CurrentViewModel = new StartViewModel(_navigationManager, _bookStoreManager));
    }

    public ICommand AddBookCommand { get; }
    public ICommand RemoveBookCommand { get; }

    public ICommand GoBackToStartCommand { get; }

    private ObservableCollection<ButikTbl> _stores;

    public ObservableCollection<ButikTbl> Stores
    {
        get { return _stores; }
        set
        {
            SetProperty(ref _stores, value);
            
        }
    }

    private ButikTbl _selectedStore;

    public ButikTbl SelectedStore
    {
        get { return _selectedStore; }
        set
        {
            SetProperty(ref _selectedStore, value);
            GetBooksForAStore();
            GetBöckerTbl();
        }
    }


    private BöckerTbl _selectedBook;

    public BöckerTbl SelectedBook
    {
        get { return _selectedBook; }
        set
        {
            SetProperty(ref _selectedBook, value);
            CheckAmountBooks();
        }
    }


    public void CheckAmountBooks()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        var book = bokHandelDbContext.ButiksSaldoTbls.FirstOrDefault(b =>
            b.Isbn.Equals(SelectedBook.Isbn) && b.ButiksId.Equals(SelectedStore.Id));

        if (book != null)
        {
            AmountBooks = book.AntalBöcker;
            
        }

        else
        {
            AmountBooks = 0;
        }
    }

    private ObservableCollection<ButiksSaldoTbl> _books;

    public ObservableCollection<ButiksSaldoTbl> Books
    {
        get { return _books; }
        set
        {
            SetProperty(ref _books, value);
            
        }
    }

    private ObservableCollection<BöckerTbl> _allBooks;

    public ObservableCollection<BöckerTbl> AllBooks
    {
        get { return _allBooks; }
        set
        {
            SetProperty(ref _allBooks, value);

        }
    }

    private int _amountBooks;

    public int AmountBooks
    {
        get { return _amountBooks; }
        set { SetProperty(ref _amountBooks, value); }
    }

    public void GetBöckerTbl()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        AllBooks = new ObservableCollection<BöckerTbl>(bokHandelDbContext.BöckerTbls.ToList());
    }

    public void GetBooksForAStore()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        Books = new ObservableCollection<ButiksSaldoTbl>(bokHandelDbContext.ButiksSaldoTbls.Include(b =>b.IsbnNavigation).Where(s => s.ButiksId.Equals(SelectedStore.Id)));

       
    }

    

    public void GetButikTbl()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        Stores = new ObservableCollection<ButikTbl>(bokHandelDbContext.ButikTbls.ToList());
        
    }

    public void AddBook()
    {
        var bokHandelDbContext = new BokHandelDbContext();


        if (SelectedBook != null && AmountBooks >= 0)
        {
            var heja = bokHandelDbContext.ButiksSaldoTbls.FirstOrDefault(b => b.Isbn.Equals(SelectedBook.Isbn) && b.ButiksId.Equals(SelectedStore.Id));

            if (heja == null)
            {
                var addBook = new ButiksSaldoTbl() { Isbn = SelectedBook.Isbn, ButiksId = SelectedStore.Id, AntalBöcker = AmountBooks };

                bokHandelDbContext.ButiksSaldoTbls.Add(addBook);

            }

            else
            {
                heja.AntalBöcker = AmountBooks;
            }

            bokHandelDbContext.SaveChanges();
            GetBooksForAStore();
        }

    }

    private ButiksSaldoTbl _selectedBookToRemove;

    public ButiksSaldoTbl SelectedBookToRemove
    {
        get { return _selectedBookToRemove; }
        set
        {
            SetProperty(ref _selectedBookToRemove, value);
        }
    }
    public void RemoveBook()
    {
        var bokHandelDbContext = new BokHandelDbContext();

        if (SelectedBookToRemove != null)
        {
            var bookToRemove = bokHandelDbContext.ButiksSaldoTbls.FirstOrDefault(b =>
                b.Isbn.Equals(SelectedBookToRemove.Isbn) && b.ButiksId.Equals(SelectedBookToRemove.ButiksId));

            if (bookToRemove != null)
            {
                bokHandelDbContext.ButiksSaldoTbls.Remove(bookToRemove);
                bokHandelDbContext.SaveChanges();
                GetBooksForAStore();
            }
        }

        
    }

}