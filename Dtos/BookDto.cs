using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Dtos;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    public BookDto(Book book)
    {
        Id = book.Id;
        Title = book.Title; 
        Author = book.Author;
        ISBN = book.ISBN;
        TotalCopies = book.TotalCopies;
        AvailableCopies = book.AvailableCopies;
    }
}