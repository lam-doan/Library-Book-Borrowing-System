namespace LibraryBookBorrowingSystem.Models
{
    public class Book
    {
        public Guid Id {get; set;}
        public required string Title {get; set;}
        public required string Author {get; set;}
        public required string ISBN {get; set;}
        public int TotalCopies {get; set;}
        public int AvailableCopies {get; set;}
        public ICollection<BorrowRecord> BorrowRecords {get; set;} = new List<BorrowRecord>();
    }
}