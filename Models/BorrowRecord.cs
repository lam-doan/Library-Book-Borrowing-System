namespace LibraryBookBorrowingSystem.Models
{
    public class BorrowRecord
    {
        public Guid Id {get; set;}
        public int BookId {get; set;}
        public int MemberId {get; set;}
        public DateTime BorrowDate {get; set;}
        public DateTime? ReturnDate {get; set;} 
        public BorrowStatus Status {get; set;}
        public Book? Book { get; set; }
        public Member? Member { get; set; }
    }
}