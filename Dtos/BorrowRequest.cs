namespace LibraryBookBorrowingSystem.Dtos;

public class BorrowRequest
{
    public Guid BookId {get; set;} = Guid.Empty;
    public Guid MemberId {get; set;} = Guid.Empty;
}