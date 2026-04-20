using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Dtos;

public class BorrowRecordDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; } = Guid.Empty;
    public Guid MemberId { get; set; } = Guid.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public BorrowStatus Status { get; set; }

    public BorrowRecordDto(BorrowRecord record)
    {
        Id = record.Id;
        BookId = record.BookId;
        MemberId = record.MemberId;
        BorrowDate = record.BorrowDate;
        ReturnDate = record.ReturnDate;
        Status = record.Status;
    }
}