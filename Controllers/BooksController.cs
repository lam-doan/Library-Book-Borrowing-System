using LibraryBookBorrowingSystem.Dtos;
using LibraryBookBorrowingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryBookBorrowingSystem.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookRequest request)
    {
        var book = await _bookService.CreateBookAsync(request);
        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookDto>> GetBookById(Guid id)
    {
        var book = await _bookService.GetByIdAsync(id);
        return Ok(book);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookDto>> UpdateBook(Guid id, [FromBody] UpdateBookRequest request)
    {
        var book = await _bookService.UpdateBookAsync(id, request);
        return Ok(book);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        await _bookService.DeleteBookAsync(id);
        return NoContent();
    }
}
