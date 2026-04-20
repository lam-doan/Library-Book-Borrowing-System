using LibraryBookBorrowingSystem.Models;
using LibraryBookBorrowingSystem.Dtos;

namespace LibraryBookBorrowingSystem.Services;

public interface IMemberService
{
    Task<MemberDto> CreateMemberAsync(CreateMemberRequest request);
    Task<MemberDto> UpdateMemberAsync(Guid memberId, UpdateMemberRequest request);
    Task DeleteMemberAsync(Guid memberId);
    Task<IEnumerable<MemberDto>> GetAllAsync();
    Task<MemberDto> GetByIdAsync(Guid memberId);
}