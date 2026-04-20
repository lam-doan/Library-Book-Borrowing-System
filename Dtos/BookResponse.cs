namespace LibraryBookBorrowingSystem.Dtos;

public class BookResponse
{
        public Guid Id {get; set;}
        public required string Title {get; set;}
        public required string Author {get; set;}
        public required string ISBN {get; set;}
        public int TotalCopies {get; set;}
        public int AvailableCopies {get; set;}
}