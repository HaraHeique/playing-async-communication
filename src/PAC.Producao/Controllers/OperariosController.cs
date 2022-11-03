using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAC.Producao.ApiModels;
using PAC.Producao.Data;
using PAC.Producao.Models;
using PAC.Shared.Mensagens;
using System.Collections.Concurrent;

namespace PAC.Producao.Controllers
{
    [Route("[controller]")]
    public class OperariosController : ControllerBase
    {
        private readonly ProducaoContext _contexto;
        private readonly ConcurrentQueue<IntegracaoMensagem> _filaProcessos;

        public OperariosController(
            ProducaoContext contexto, 
            ConcurrentQueue<IntegracaoMensagem> filaProcessos)
        {
            _contexto = contexto;
            _filaProcessos = filaProcessos;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Operario>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Operario>>> ObterTodos()    
            => Ok(await _contexto.Operarios.ToListAsync());

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(List<Operario>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Operario>>> Obter([FromRoute] Guid id)
        {
            var operario = await _contexto.Operarios.FindAsync(id);

            return operario is not null ? Ok(operario) : NotFound();
        }
        
        [HttpPatch("definir-cargo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DefinirCargo([FromBody] DefinicaoCargoRequest request)
        {
            // Validação da request se desejar

            var operario = await _contexto.Operarios.FindAsync(request.Id);

            if (operario is null) return NotFound();

            operario.DefinirCargo(request.Funcao, request.Periodo);

            _contexto.Operarios.Update(operario);
            await _contexto.SaveChangesAsync();

            return Ok();
        }
        
        [HttpPatch("alterar-nome")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarNome([FromBody] AlteracaoNomeRequest request)
        {
            // Validação da request se desejar

            var operario = await _contexto.Operarios.FindAsync(request.Id);

            if (operario is null) return NotFound();

            operario.AlterarNome(request.Nome, request.Apelido, request.Motivo);

            _contexto.Operarios.Update(operario);
            await _contexto.SaveChangesAsync();

            var mensagem = new OperarioNomeAlteradoMensagem(operario.Id, operario.Nome, operario.Apelido);
            _filaProcessos.Enqueue(mensagem);

            return Ok();
        }
    }
}
