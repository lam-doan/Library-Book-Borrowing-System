using LibraryBookBorrowingSystem.Data;
using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Exceptions;
using LibraryBookBorrowingSystem.Models;
using LibraryBookBorrowingSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryBookBorrowingSystem.Services;

public class BorrowService : IBorrowService
{
    private readonly ApplicationDbContext _context;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IMemoryCache _cache;
    private const string BooksCacheKey = "all_books_cache";
    public BorrowService(
        ApplicationDbContext context,
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        IBorrowRecordRepository borrowRecordRepository,
        IMemoryCache cache)
    {
        _context = context;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _borrowRecordRepository = borrowRecordRepository;
        _cache = cache;
    }

    // borrow a book
    public async Task<BorrowRecordDto> BorrowBookAsync(BorrowRequest request)
    {
        var bookExists = await _bookRepository.ExistsAsync(request.BookId);
        if (!bookExists)
            throw new NotFoundException("Book not found.");

        if (await _memberRepository.GetByIdAsync(request.MemberId) is null)
            throw new NotFoundException("Member not found.");

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var activeBorrow = await _borrowRecordRepository.GetActiveBorrowAsync(request.BookId, request.MemberId);
            if (activeBorrow != null)
                throw new ConflictException("Member already borrowed this book.");

            var copyReserved = await _bookRepository.TryDecrementAvailableCopiesAsync(request.BookId);
            if (!copyReserved)
            {
                if (!await _bookRepository.ExistsAsync(request.BookId))
                    throw new NotFoundException("Book not found.");

                throw new ConflictException("No copies available.");
            }

            _cache.Remove("all_books_cache");

            var record = new BorrowRecord
            {
                Id = Guid.NewGuid(),
                BookId = request.BookId,
                MemberId = request.MemberId,
                BorrowDate = DateTime.UtcNow,
                Status = BorrowStatus.Borrowed
            };

            await _borrowRecordRepository.CreateAsync(record);
            await transaction.CommitAsync();
            return new BorrowRecordDto(record);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }   

    // return a book
    public async Task<BorrowRecordDto> ReturnBookAsync(ReturnRequest request)
    {
        if (!await _bookRepository.ExistsAsync(request.BookId))
            throw new NotFoundException("Book not found.");

        if (await _memberRepository.GetByIdAsync(request.MemberId) is null)
            throw new NotFoundException("Member not found.");

        var record = await _borrowRecordRepository.GetActiveBorrowAsync(request.BookId, request.MemberId)
            ?? throw new ConflictException("This member has not borrowed this book.");

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            record.ReturnDate = DateTime.UtcNow;
            record.Status = BorrowStatus.Returned;
            await _borrowRecordRepository.UpdateAsync(record);

            var copyReturned = await _bookRepository.TryIncrementAvailableCopiesAsync(request.BookId);
            if (!copyReturned)
            {
                if (!await _bookRepository.ExistsAsync(request.BookId))
                    throw new NotFoundException("Book not found.");

                throw new ConflictException("Cannot return a book with all copies already available.");
            }

            _cache.Remove("all_books_cache");

            await transaction.CommitAsync();
            return new BorrowRecordDto(record);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }

    // get all borrow records
    public async Task<IEnumerable<BorrowRecordDto>> GetAllAsync()
    {
        var records = await _borrowRecordRepository.GetAllAsync();
        return records.Select(r => new BorrowRecordDto(r));
    }

    // get member's borrow history
    public async Task<IEnumerable<BorrowRecordDto>> GetRecordByMemberIdAsync(Guid memberId)
    {
        var records = await _borrowRecordRepository.GetByMemberIdAsync(memberId);
        return records.Select(r => new BorrowRecordDto(r));
    }
}
