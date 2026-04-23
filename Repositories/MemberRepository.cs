using LibraryBookBorrowingSystem.Data;
using LibraryBookBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookBorrowingSystem.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext _context;
    public MemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Member> CreateAsync(Member member)
    {
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _context.Members
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(Guid memberId)
    {
        return await _context.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == memberId);
    }

    public async Task<Member> UpdateAsync(Member member)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync();
        return member;
    }

    public async Task DeleteAsync(Member member)
    {
        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid memberId)
    {
        return await _context.Members.AnyAsync(r => r.Id == memberId); 
    }
}
