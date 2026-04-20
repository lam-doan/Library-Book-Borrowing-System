using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Repositories;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord> CreateAsync(BorrowRecord record);
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task<BorrowRecord?> GetByIdAsync(Guid recordId);
    Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(Guid memberId);
    Task<BorrowRecord> UpdateAsync(BorrowRecord record);
    Task<bool> ExistsAsync(Guid recordId);

}