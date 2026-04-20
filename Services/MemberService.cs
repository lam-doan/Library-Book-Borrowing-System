using System.Data.Common;
using System.Runtime.Serialization;
using LibraryBookBorrowingSystem.Data;
using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Repositories;
using LibraryBookBorrowingSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace LibraryBookBorrowingSystem.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    public MemberService(
        IMemberRepository memberRepository,
        IBorrowRecordRepository borrowRecordRepository)
    {
        _memberRepository = memberRepository;
        _borrowRecordRepository = borrowRecordRepository;
    }

    public async Task<MemberDto> CreateMemberAsync(CreateMemberRequest request)
    {
        // validate empty string
        if (string.IsNullOrEmpty(request.FullName))
            throw new Exception("Fullname is required.");
        if (string.IsNullOrEmpty(request.Email))
            throw new Exception("Email is required.");
        
        // validate valid email
        if (!MailAddress.TryCreate(request.Email, out _))
            throw new Exception("Invalid email.");

        var member = new Member
        {   
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            MembershipDate = DateTime.UtcNow
        };

        await _memberRepository.CreateAsync(member);
        return new MemberDto(member);
    }

    public async Task<MemberDto> UpdateMemberAsync(Guid memberId, UpdateMemberRequest request)
    {
        // validate empty string
        if (string.IsNullOrEmpty(request.FullName))
            throw new Exception("Fullname is required.");
        if (string.IsNullOrEmpty(request.Email))
            throw new Exception("Email is required.");  

        // validate valid email
        if (!MailAddress.TryCreate(request.Email, out _))
            throw new Exception("Invalid email.");

        // fetch existing member
        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new Exception("Member not found.");
        
        // update info
        member.FullName = request.FullName;
        member.Email = request.Email;

        // save
        await _memberRepository.UpdateAsync(member);
        return new MemberDto(member);
    }

    public async Task DeleteMemberAsync(Guid memberId)
    {
        // fetch existing member
        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new Exception("Member not found.");
        
        // validate if member is currently borrowing any books
        var activeBorrows = await _borrowRecordRepository.GetByMemberIdAsync(memberId);
        if (activeBorrows.Any(r => r.Status == BorrowStatus.Borrowed))
            throw new Exception("Cannot delete a member with active borrowed books.");

        await _memberRepository.DeleteAsync(member);
    }

    public async Task<IEnumerable<MemberDto>> GetAllAsync()
    {
        var members = await _memberRepository.GetAllAsync();
        return members.Select(r => new MemberDto(r));
    }

    public async Task<MemberDto> GetByIdAsync(Guid memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new Exception("Member not found.");

        return new MemberDto(member);
    }
}