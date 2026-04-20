namespace LibraryBookBorrowingSyste.Dtos;

public class BorrowRequest
{
    public Guid BookId {get; set;}
    public Guid MemberId {get; set;}
}