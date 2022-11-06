using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAC.Shared.Mensagens;
using PAC.Vendas.Data;
using PAC.Vendas.Models.Api;
using PAC.Vendas.Models.Domain;
using System.Collections.Concurrent;

namespace PAC.Vendas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendedoresController : ControllerBase
    {
        private readonly VendasContext _contexto;
        private readonly ConcurrentQueue<IntegracaoMensagem> _filaProcessos;

        public VendedoresController(
            VendasContext contexto,
            ConcurrentQueue<IntegracaoMensagem> filaProcessos)
        {
            _contexto = contexto;
            _filaProcessos = filaProcessos;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Vendedor>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Vendedor>>> ObterTodos()
            => Ok(await _contexto.Vendedores.ToListAsync());

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(List<Vendedor>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Vendedor>>> Obter([FromRoute] Guid id)
        {
            var vendedor = await _contexto.Vendedores.FindAsync(id);

            return vendedor is not null ? Ok(vendedor) : NotFound();
        }

        [HttpPatch("definir-funcao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DefinirFuncao([FromBody] DefinicaoFuncaoRequest request)
        {
            // Validação da request se desejar

            var vendedor = await _contexto.Vendedores.FindAsync(request.Id);

            if (vendedor is null) return NotFound();

            vendedor.DefinirCargo(request.Funcao);

            _contexto.Vendedores.Update(vendedor);
            await _contexto.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("alterar-info-pessoais")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarNome([FromBody] AlteracaoInformacoesPessoaisRequest request)
        {
            // Validação da request se desejar

            var vendedor = await _contexto.Vendedores.FindAsync(request.Id);

            if (vendedor is null) return NotFound();

            vendedor.AlterarInfoPessoais(request.Nome, request.Email, request.Motivo);

            _contexto.Vendedores.Update(vendedor);
            await _contexto.SaveChangesAsync();

            var mensagem = new VendedorInfoPessoaisAlteradaMensagem(vendedor.Id, vendedor.Nome, vendedor.Email);
            _filaProcessos.Enqueue(mensagem);

            return Ok();
        }
    }
}
