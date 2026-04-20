using LibraryBookBorrowingSystem.Models;
using LibraryBookBorrowingSystem.Dtos;

namespace LibraryBookBorrowingSystem.Services;

public interface IBorrowService
{
    Task<BorrowRecordDto> BorrowBookAsync(BorrowRequest request);
    Task<BorrowRecordDto> ReturnBookAsync(ReturnRequest request);
    Task<IEnumerable<BorrowRecordDto>> GetAllAsync();
    Task<IEnumerable<BorrowRecordDto>> GetRecordByMemberIdAsync(Guid memberId);
}
