using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Repositories;

public interface IBorrowRecordRepository
{
    Task<BorrowRecord> CreateAsync(BorrowRecord record);
    Task<IEnumerable<BorrowRecord>> GetAllAsync();
    Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(Guid memberId);
    Task<BorrowRecord?> GetActiveBorrowAsync(Guid bookId, Guid memberId);
    Task<BorrowRecord?> GetActiveBorrowByBookIdAsync(Guid bookId);
    Task<BorrowRecord> UpdateAsync(BorrowRecord record);
    Task<bool> ExistsAsync(Guid recordId);

}