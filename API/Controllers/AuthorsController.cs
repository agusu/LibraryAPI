using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Application.DTOs;
using Application.Authors.Commands;
using Application.Authors.Queries;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDto>>> GetAll()
        {
            return await _mediator.Send(new GetAuthorsQuery());
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> Create(CreateAuthorDto author)
        {
            var result = await _mediator.Send(new CreateAuthorCommand(author));
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetAuthorByIdQuery(id));
            if (result == null) return NotFound();
            return result;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDto>> Update(int id, UpdateAuthorDto author)
        {
            var result = await _mediator.Send(new UpdateAuthorCommand(id, author));
            if (result == null) return NotFound();
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteAuthorCommand(id));
            if (!result) return NotFound();
            return NoContent();
        }
    }
}