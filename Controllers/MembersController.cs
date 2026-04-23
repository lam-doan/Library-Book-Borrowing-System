using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBookBorrowingSystem.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly IBorrowService _borrowService;

    public MembersController(IMemberService memberService, IBorrowService borrowService)
    {
        _memberService = memberService;
        _borrowService = borrowService;
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberRequest request)
    {
        var member = await _memberService.CreateMemberAsync(request);
        return CreatedAtAction(nameof(GetMemberById), new { id = member.Id }, member);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
    {
        var members = await _memberService.GetAllAsync();
        return Ok(members);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemberDto>> GetMemberById(Guid id)
    {
        var member = await _memberService.GetByIdAsync(id);
        return Ok(member);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<MemberDto>> UpdateMember(Guid id, [FromBody] UpdateMemberRequest request)
    {
        var member = await _memberService.UpdateMemberAsync(id, request);
        return Ok(member);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMember(Guid id)
    {
        await _memberService.DeleteMemberAsync(id);
        return NoContent();
    }

    [HttpGet("{id:guid}/borrows")]
    public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetMemberBorrowHistory(Guid id)
    {
        var records = await _borrowService.GetRecordByMemberIdAsync(id);
        return Ok(records);
    }
}
