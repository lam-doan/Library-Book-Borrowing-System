namespace LibraryBookBorrowingSystem.Models
{
    public class BorrowRecord
    {
        public Guid Id {get; set;}
        public Guid BookId {get; set;} = Guid.Empty;
        public Guid MemberId {get; set;} = Guid.Empty;
        public DateTime BorrowDate {get; set;}
        public DateTime? ReturnDate {get; set;} 
        public BorrowStatus Status {get; set;}
        public Book? Book { get; set; }
        public Member? Member { get; set; }
    }
}