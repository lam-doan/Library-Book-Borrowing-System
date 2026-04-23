using LibraryBookBorrowingSystem.Validation;

namespace LibraryBookBorrowingSystem.Dtos;

public class ReturnRequest
{
    [NotEmptyGuid]
    public Guid MemberId { get; set; } = Guid.Empty;

    [NotEmptyGuid]
    public Guid BookId { get; set; } = Guid.Empty;
}
