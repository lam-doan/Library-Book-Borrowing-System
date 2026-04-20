using System.Data.Common;
using System.Runtime.Serialization;
using LibraryBookBorrowingSystem.Data;
using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Repositories;
using LibraryBookBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookBorrowingSystem.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;

    public BookService(
        IBookRepository bookRepository,
        IBorrowRecordRepository borrowRecordRepository)
    {
        _bookRepository = bookRepository;
        _borrowRecordRepository = borrowRecordRepository;
    }

    // create a book
    public async Task<BookDto> CreateBookAsync(CreateBookRequest request)
    {
        // validate TotalCopies
        if (request.TotalCopies <= 0)
            throw new Exception("Total copies must be greater than 0.");
            
        // validate empty strings
        if (string.IsNullOrEmpty(request.Title))
            throw new Exception("Title must be required.");
        if (string.IsNullOrEmpty(request.Author))
            throw new Exception("Author must be required.");    
        if (string.IsNullOrEmpty(request.ISBN))
            throw new Exception("ISBN must be required.");

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            ISBN = request.ISBN,
            TotalCopies = request.TotalCopies,
            AvailableCopies = request.TotalCopies
        };

        await _bookRepository.CreateAsync(book);
        return new BookDto(book);
    }

    public async Task<BookDto> UpdateBookAsync(Guid bookId, UpdateBookRequest request)
    {
        // validate Title, Author, ISBN
        if (string.IsNullOrEmpty(request.Title))
            throw new Exception("Title is required.");
        if (string.IsNullOrEmpty(request.Author))
            throw new Exception("Author is required.");
        if (string.IsNullOrEmpty(request.ISBN))
            throw new Exception("ISBN is required.");

        // validate TotalCopies
        if (request.TotalCopies <= 0)
            throw new Exception("Total copies must be greater than 0.");
        
        // validate AvailableCopies
        if (request.AvailableCopies < 0)
            throw new Exception("Available copies must be greater than 0.");
        if (request.AvailableCopies > request.TotalCopies)
            throw new Exception("Available copies must not exceed Total copoies.");
        
        // fetch existing book
        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new Exception("Book not found.");
        
        // update info
        book.Title = request.Title;
        book.Author = request.Author;
        book.ISBN = request.ISBN;
        book.TotalCopies = request.TotalCopies;
        book.AvailableCopies = request.AvailableCopies;

        // save
        await _bookRepository.UpdateAsync(book);
        return new BookDto(book);
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(r => new BookDto(r));
    }

    public async Task<BookDto> GetByIdAsync(Guid bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new Exception("Book not found.");
        return new BookDto(book);
    }

    public async Task DeleteBookAsync(Guid bookId)
    {
        // fetch existing book
        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new Exception("Book not found.");
        
        // if the book is currently borrowed
        var activeBorrow = await _borrowRecordRepository.GetActiveBorrowByBookIdAsync(bookId);
        if (activeBorrow != null)
            throw new Exception("Cannot delete a book that is currently borrowed.");

        await _bookRepository.DeleteAsync(book);
    }
}