namespace LibraryBookBorrowingSystem.Dtos;

public class CreateMemberRequest
{
        public string FullName {get; set;} = string.Empty;
        public required string Email {get; set;} = string.Empty;
        public DateTime MembershipDate {get; set;}
}