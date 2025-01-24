using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Application.DTOs;
using Application.Books.Commands;
using Application.Books.Queries;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookDto>>> GetAll()
    {
        return await _mediator.Send(new GetBooksQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetBookByIdQuery(id));
        if (result == null) return NotFound();
        return result;
    }

    [HttpGet("author/{authorId}")]
    public async Task<ActionResult<List<BookDto>>> GetByAuthor(int authorId)
    {
        return await _mediator.Send(new GetBooksByAuthorQuery(authorId));
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> Create(CreateBookDto book)
    {
        var result = await _mediator.Send(new CreateBookCommand(book));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookDto>> Update(int id, UpdateBookDto book)
    {
        var result = await _mediator.Send(new UpdateBookCommand(id, book));
        if (result == null) return NotFound();
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteBookCommand(id));
        if (!result) return NotFound();
        return NoContent();
    }
}