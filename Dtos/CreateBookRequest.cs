namespace LibraryBookBorrowingSystem.Dtos;

public class CreateBookRequest
{
        public required string Title {get; set;}
        public required string Author {get; set;}
        public required string ISBN {get; set;}
        public int TotalCopies {get; set;}
}