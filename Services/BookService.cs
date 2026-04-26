using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Exceptions;
using LibraryBookBorrowingSystem.Models;
using LibraryBookBorrowingSystem.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryBookBorrowingSystem.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IMemoryCache _cache;
    private const string BooksCacheKey = "all_books_cache";
    private void InvalidateBooksCache()
    {
        _cache.Remove(BooksCacheKey);
    }

    public BookService(
        IBookRepository bookRepository,
        IBorrowRecordRepository borrowRecordRepository,
        IMemoryCache cache)
    {
        _bookRepository = bookRepository;
        _borrowRecordRepository = borrowRecordRepository;
        _cache = cache;
    }

    // create a book
    public async Task<BookDto> CreateBookAsync(CreateBookRequest request)
    {
        if (request.TotalCopies <= 0)
            throw new BadRequestException("Total copies must be greater than 0.");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Title is required.");
        if (string.IsNullOrWhiteSpace(request.Author))
            throw new BadRequestException("Author is required.");
        if (string.IsNullOrWhiteSpace(request.ISBN))
            throw new BadRequestException("ISBN is required.");

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
        InvalidateBooksCache();
        return new BookDto(book);
    }

    public async Task<BookDto> UpdateBookAsync(Guid bookId, UpdateBookRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new BadRequestException("Title is required.");
        if (string.IsNullOrWhiteSpace(request.Author))
            throw new BadRequestException("Author is required.");
        if (string.IsNullOrWhiteSpace(request.ISBN))
            throw new BadRequestException("ISBN is required.");

        if (request.TotalCopies <= 0)
            throw new BadRequestException("Total copies must be greater than 0.");

        if (request.AvailableCopies < 0)
            throw new BadRequestException("Available copies must be greater than or equal to 0.");
        if (request.AvailableCopies > request.TotalCopies)
            throw new BadRequestException("Available copies must not exceed total copies.");

        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new NotFoundException("Book not found.");

        book.Title = request.Title;
        book.Author = request.Author;
        book.ISBN = request.ISBN;
        book.TotalCopies = request.TotalCopies;
        book.AvailableCopies = request.AvailableCopies;

        await _bookRepository.UpdateAsync(book);
        InvalidateBooksCache();
        return new BookDto(book);
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        if (_cache.TryGetValue(BooksCacheKey, out IEnumerable<BookDto>? cachedBooks))
        {
            Console.WriteLine("Returning books from cache.");
            return cachedBooks!;
        }
        
        Console.WriteLine("Fetching books from database.");
        
        var books = await _bookRepository.GetAllAsync();
        var dtoList = books.Select(r => new BookDto(r));

        _cache.Set(
            BooksCacheKey,
            dtoList,
            TimeSpan.FromMinutes(5)
        );

        return dtoList;
    }

    public async Task<BookDto> GetByIdAsync(Guid bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new NotFoundException("Book not found.");
        return new BookDto(book);
    }

    public async Task DeleteBookAsync(Guid bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new NotFoundException("Book not found.");

        var activeBorrow = await _borrowRecordRepository.GetActiveBorrowByBookIdAsync(bookId);
        if (activeBorrow != null)
            throw new ConflictException("Cannot delete a book that is currently borrowed.");

        await _bookRepository.DeleteAsync(book);
        InvalidateBooksCache();
    }

    public async Task<IEnumerable<BookDto>> SearchBookAsync(string request)
    {
        var books = await _bookRepository.SearchAsync(request);
        return books.Select(r => new BookDto(r));
    }
}
