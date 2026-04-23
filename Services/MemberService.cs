using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Exceptions;
using LibraryBookBorrowingSystem.Models;
using LibraryBookBorrowingSystem.Repositories;
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
        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new BadRequestException("FullName is required.");
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new BadRequestException("Email is required.");

        if (!MailAddress.TryCreate(request.Email, out _))
            throw new BadRequestException("Invalid email.");

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
        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new BadRequestException("FullName is required.");
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new BadRequestException("Email is required.");

        if (!MailAddress.TryCreate(request.Email, out _))
            throw new BadRequestException("Invalid email.");

        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new NotFoundException("Member not found.");

        member.FullName = request.FullName;
        member.Email = request.Email;

        await _memberRepository.UpdateAsync(member);
        return new MemberDto(member);
    }

    public async Task DeleteMemberAsync(Guid memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new NotFoundException("Member not found.");

        var activeBorrows = await _borrowRecordRepository.GetByMemberIdAsync(memberId);
        if (activeBorrows.Any(r => r.Status == BorrowStatus.Borrowed))
            throw new ConflictException("Cannot delete a member with active borrowed books.");

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
            ?? throw new NotFoundException("Member not found.");

        return new MemberDto(member);
    }
}
