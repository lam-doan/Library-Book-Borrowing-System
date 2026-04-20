using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Dtos;

public class MemberDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime MembershipDate {get; set;}

    public MemberDto(Member member)
    {
        Id = member.Id;
        FullName = member.FullName;
        Email = member.Email;
        MembershipDate = member.MembershipDate;
    }
}