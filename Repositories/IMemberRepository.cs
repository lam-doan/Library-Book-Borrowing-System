using LibraryBookBorrowingSystem.Models;

namespace LibraryBookBorrowingSystem.Repositories;

public interface IMemberRepository
{
    Task<Member> CreateAsync(Member member);
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(Guid memberId);
    Task<Member> UpdateAsync(Member member);
    Task DeleteAsync(Member member);
    Task<bool> ExistsAsync(Guid memberId);
}