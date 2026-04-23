using LibraryBookBorrowingSystem.Data;
using LibraryBookBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookBorrowingSystem.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;
    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Book> CreateAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid bookId)
    {
        return await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == bookId);
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(Book book)
    {
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }

    public Task<bool> ExistsAsync(Guid bookId)
    {
        return _context.Books.AnyAsync(r => r.Id == bookId); 
    }

    public async Task<bool> TryDecrementAvailableCopiesAsync(Guid bookId)
    {
        var affectedRows = await _context.Books
            .Where(b => b.Id == bookId && b.AvailableCopies > 0)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.AvailableCopies, b => b.AvailableCopies - 1));

        return affectedRows == 1;
    }

    public async Task<bool> TryIncrementAvailableCopiesAsync(Guid bookId)
    {
        var affectedRows = await _context.Books
            .Where(b => b.Id == bookId && b.AvailableCopies < b.TotalCopies)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(b => b.AvailableCopies, b => b.AvailableCopies + 1));

        return affectedRows == 1;
    }
}
