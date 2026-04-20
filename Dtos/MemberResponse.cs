namespace LibraryBookBorrowingSystem.Dtos;

public class MemberResponse
{
        public Guid Id { get; set; }
        public required string FullName {get; set;}
        public required string Email {get; set;}
        public DateTime MembershipDate {get; set;}
}