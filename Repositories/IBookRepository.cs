using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Repositories;

public interface IBookRepository
{
    Task<Book> CreateAsync(Book book);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(Guid bookId);
    Task<Book> UpdateAsync(Book book);
    Task DeleteAsync(Book book);
    Task<bool> ExistsAsync(Guid bookId);
    Task<bool> TryDecrementAvailableCopiesAsync(Guid bookId);
    Task<bool> TryIncrementAvailableCopiesAsync(Guid bookId);
}
