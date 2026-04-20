namespace LibraryBookBorrowingSystem.Dtos;

public class UpdateBookRequest
{
        public required string Title {get; set;}
        public required string Author {get; set;}
        public required string ISBN {get; set;}
        public int TotalCopies {get; set;}
}