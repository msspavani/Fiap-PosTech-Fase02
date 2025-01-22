using Asp.Versioning;
using FIAP.TC.FASE01.APIContatos.Application.Commands;
using FIAP.TC.FASE01.APIContatos.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TC.FASE01.APIContatos.WebAPI.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")] 
public class ContatosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IContatoRepository _contatoRepository;

    public ContatosController(IMediator mediator, IContatoRepository contatoRepository)
    {
        _mediator = mediator;
        _contatoRepository = contatoRepository;
    }

    
    [HttpPost]
    public async Task<IActionResult> CriarContato([FromBody] CriarContatoCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var contatoId = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObterContatoPorId), new { id = contatoId }, contatoId);
    }
    
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> AtualizarContato(Guid id, [FromBody] AtualizarContatoCommand command)
    {
        if (id != command.ContatoId)
            return BadRequest("O ID do contato na URL não corresponde ao ID no corpo da solicitação.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoverContato(Guid id)
    {
        try
        {
            await _mediator.Send(new RemoverContatoCommand(id));
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterContatoPorId(Guid id)
    {
        var contato = await _contatoRepository.ObterPorIdAsync(id);
        if (contato == null)
            return NotFound("Contato não encontrado.");

        return Ok(contato);
    }

    
    [HttpGet]
    public async Task<IActionResult> ObterContatosPorDdd([FromQuery] string ddd)
    {
        var contatos = await _contatoRepository.ObterPorDddAsync(ddd);
        return Ok(contatos);
    }
}

