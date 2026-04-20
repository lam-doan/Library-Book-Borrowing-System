namespace LibraryBookBorrowingSystem.Models
{
    public class Member
    {
        public Guid Id {get; set;}
        public string FullName {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public DateTime MembershipDate {get; set;}
        public ICollection<BorrowRecord> BorrowRecords {get; set;} = new List<BorrowRecord>();

    }
}