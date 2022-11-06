using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PAC.RH.Data;
using PAC.RH.Factories;
using PAC.RH.Models;
using PAC.Shared.Mensagens;
using System.Collections.Concurrent;

namespace PAC.RH.Controllers
{
    [Route("[controller]")]
    public class FuncionariosController : ControllerBase
    {
        private readonly RhContext _context;
        private readonly ConcurrentQueue<IntegracaoMensagem> _filaProcessos;

        public FuncionariosController(
            RhContext context,
            ConcurrentQueue<IntegracaoMensagem> filaProcessos)
        {
            _context = context;
            _filaProcessos = filaProcessos;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Funcionario>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Funcionario>>> ObterTodos()
        {
            return Ok(await _context.Funcionarios.ToListAsync());
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Funcionario), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Funcionario?>> ObterPorId([FromRoute] Guid id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            return funcionario is not null ? Ok(funcionario) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Adicionar([FromBody] Funcionario funcionario)
        {
            // Caso queira alguma validação aqui no modelo anêmico de dados

            await _context.Funcionarios.AddAsync(funcionario);
            await _context.SaveChangesAsync();

            var mensagem = FuncionarioMensagemFactory.Criar(funcionario);
            _filaProcessos.Enqueue(mensagem);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar([FromBody] Funcionario funcionario)
        {
            // Caso queira alguma validação aqui no modelo anêmico de dados

            funcionario.Desligado = false; // Sei que tem uma falha aqui, mas não é o foco

            if (!_context.Funcionarios.Any(f => f.Id == funcionario.Id))
                return NotFound();

            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();

            var mensagem = new FuncionarioAtualizadoMensagem(funcionario.Id, funcionario.NomeCompleto.Nome, funcionario.Setor);
            _filaProcessos.Enqueue(mensagem);

            return NoContent();
        }

        [HttpPatch("desligar/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Desligar([FromRoute] Guid id)
        {
            // Caso queira alguma validação aqui no modelo anêmico de dados

            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario is null) return NotFound();

            funcionario.Desligado = true;
            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();

            var mensagem = new FuncionarioDesligadoMensagem(funcionario.Id, funcionario.Setor);
            _filaProcessos.Enqueue(mensagem);

            return NoContent();
        }
    }
}
