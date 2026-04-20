using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Dtos;

public class BorrowRecordReturn
{
    public Guid Id {get; set;}
    public required Guid BookId {get; set;}
    public required Guid MemberId {get; set;}
    public DateTime BorrowDate {get; set;}
    public DateTime? ReturnDate {get; set;} 
    public BorrowStatus Status {get; set;}
}