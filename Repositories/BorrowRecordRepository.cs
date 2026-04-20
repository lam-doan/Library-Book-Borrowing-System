using LibraryBookBorrowingSystem.Models;
using LibraryBookBorrowingSystem.Data;
using LibraryBookBorrowingSystem.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookBorrowingSystem.Repositories;

public class BorrowRecordRepository : IBorrowRecordRepository
{
    private readonly ApplicationDbContext _context;
    public BorrowRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BorrowRecord> CreateAsync(BorrowRecord record)
    {
        await _context.BorrowRecords.AddAsync(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
    {
        return await _context.BorrowRecords
            .AsNoTracking()
            .Include(r => r.Book)
            .Include(r => r.Member)
            .ToListAsync();
    }

    public async Task<BorrowRecord?> GetByIdAsync(Guid recordId)
    {
        return await _context.BorrowRecords
            .AsNoTracking()
            .Include(r => r.Book)
            .Include(r => r.Member)
            .FirstOrDefaultAsync(r => r.Id == recordId);
    }

    public async Task<IEnumerable<BorrowRecord>> GetByMemberIdAsync(Guid memberId)
    {
        return await _context.BorrowRecords
            .AsNoTracking()
            .Where(r => r.MemberId == memberId)
            .Include(r => r.Book)
            .ToListAsync();
    }

    public async Task<BorrowRecord> UpdateAsync(BorrowRecord record)
    {
        _context.BorrowRecords.Update(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<bool> ExistsAsync(Guid recordId)
    {
        return await _context.BorrowRecords.AnyAsync(r => r.Id == recordId);
    }
}

