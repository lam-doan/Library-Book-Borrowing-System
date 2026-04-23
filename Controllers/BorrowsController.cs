using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBookBorrowingSystem.Controllers;

[ApiController]
[Route("api/borrows")]
public class BorrowsController : ControllerBase
{
    private readonly IBorrowService _borrowService;

    public BorrowsController(IBorrowService borrowService)
    {
        _borrowService = borrowService;
    }

    [HttpPost]
    public async Task<ActionResult<BorrowRecordDto>> BorrowBook([FromBody] BorrowRequest request)
    {
        var record = await _borrowService.BorrowBookAsync(request);
        return Ok(record);
    }

    [HttpPost("return")]
    public async Task<ActionResult<BorrowRecordDto>> ReturnBook([FromBody] ReturnRequest request)
    {
        var record = await _borrowService.ReturnBookAsync(request);
        return Ok(record);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BorrowRecordDto>>> GetBorrowRecords()
    {
        var records = await _borrowService.GetAllAsync();
        return Ok(records);
    }
}
