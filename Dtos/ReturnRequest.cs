namespace LibraryBookBorrowingSystem.Dtos;

public class ReturnRequest
{
    public Guid MemberId {get; set;} = Guid.Empty;
    public Guid BookId {get; set;} = Guid.Empty;
}