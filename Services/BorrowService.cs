using System.Data.Common;
using System.Runtime.Serialization;
using LibraryBookBorrowingSystem.Data;
using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Models;
using LibraryBookBorrowingSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookBorrowingSystem.Services;

public class BorrowService : IBorrowService
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    public BorrowService(
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        IBorrowRecordRepository borrowRecordRepository)
    {
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _borrowRecordRepository = borrowRecordRepository;
    }

    // borrow a book
    public async Task<BorrowRecordDto> BorrowBookAsync(BorrowRequest request)
    {
        // validate book
        var book = await _bookRepository.GetByIdAsync(request.BookId)
                ?? throw new Exception("Book not found.");
        if (book.AvailableCopies <= 0)
            throw new Exception("No copies available.");

        // validate member
        var member = await _memberRepository.GetByIdAsync(request.MemberId)
            ?? throw new Exception("Member not found.");

        // validate active borrow record
        var activeBorrow = await _borrowRecordRepository.GetActiveBorrowAsync(request.BookId, request.MemberId);
        if (activeBorrow != null)
            throw new Exception("Member already borrowed this book.");

        // create borrow record
        var record = new BorrowRecord
        {
            Id = Guid.NewGuid(),
            BookId = request.BookId,
            MemberId = request.MemberId,
            BorrowDate = DateTime.UtcNow,
            Status = BorrowStatus.Borrowed
        };

        await _borrowRecordRepository.CreateAsync(record);

        // update book copies
        --book.AvailableCopies;
        await _bookRepository.UpdateAsync(book);

        // Service gets a BorrowRecord entity from repo and convert to BorrowRecordDto
        // Controller returns this Dto to the client --> does not leak sensitive info
        return new BorrowRecordDto(record);
    }   

    // return a book
    public async Task<BorrowRecordDto> ReturnBookAsync(ReturnRequest request)
    {
        // validate book
        var book = await _bookRepository.GetByIdAsync(request.BookId)
                ?? throw new Exception("Book not found.");
        if (book.AvailableCopies <= 0)
            throw new Exception("No copies available.");

        // validate member
        var member = await _memberRepository.GetByIdAsync(request.MemberId)
            ?? throw new Exception("Member not found.");

        // validate active borrow record
        var record = await _borrowRecordRepository.GetActiveBorrowAsync(request.BookId, request.MemberId)
            ?? throw new Exception("This member has not borrowed this book.");

        // update record
        record.ReturnDate = DateTime.UtcNow;
        record.Status = BorrowStatus.Returned;
        await _borrowRecordRepository.UpdateAsync(record);

         // update book copies
        ++book.AvailableCopies;
        await _bookRepository.UpdateAsync(book);

        return new BorrowRecordDto(record);
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