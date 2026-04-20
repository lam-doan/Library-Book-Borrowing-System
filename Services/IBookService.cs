using System.Numerics;
using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Services;

public interface IBookService
{
    Task<BookDto> CreateBookAsync(CreateBookRequest request);
    Task<BookDto> UpdateBookAsync(Guid bookId, UpdateBookRequest request);

    Task DeleteBookAsync(Guid bookId);
    Task<IEnumerable<BookDto>> GetAllAsync();
    Task<BookDto> GetByIdAsync(Guid bookId);
}