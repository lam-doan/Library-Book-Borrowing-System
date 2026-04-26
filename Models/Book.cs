using System.ComponentModel.DataAnnotations;
namespace LibraryBookBorrowingSystem.Models
{
    public class Book
    {
        public Guid Id {get; set;}
        public string Title {get; set;} = string.Empty;
        public string Author {get; set;} = string.Empty;
        public string ISBN {get; set;} = string.Empty;
        public int TotalCopies {get; set;}
        public int AvailableCopies {get; set;}

        // ADD THESE 2 LINES for concurrency
        [ConcurrencyCheck]
        public Guid RowVersion {get; set;} = Guid.NewGuid();

        public ICollection<BorrowRecord> BorrowRecords {get; set;} = new List<BorrowRecord>();
    }
}