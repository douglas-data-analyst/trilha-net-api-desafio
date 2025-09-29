using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            // R:   Implementado a busca por ID usando Find() do Entity Framework
            var tarefas = _context.Tarefas.Find(id);

            // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound
            // R:   Validação implementada - retorna NotFound se tarefa não for encontrada
            if (tarefas == null)
                return NotFound();

            // R:   Retorna OK com a tarefa encontrada
            return Ok(tarefas);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            // TODO: Buscar todas as tarefas no banco utilizando o EF
            // R:   Implementado - busca todas tarefas convertendo para lista
            var tarefas = _context.Tarefas.ToList();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            // TODO: Buscar as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            // R:   Implementado usando Where() e Contains() para busca parcial case-insensitive
            var tarefas = _context.Tarefas
                .Where(x => x.Titulo.ToLower().Contains(titulo.ToLower()))
                .ToList();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            // R:   Busca tarefas filtrando pela data (ignora horário)
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            // TODO: Buscar as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // R:   Implementado seguindo exemplo do ObterPorData - filtra pelo status
            var tarefa = _context.Tarefas.Where(x => x.Status == status);
            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            // R:   Valida se a data foi informada corretamente
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            // R:   Implementado - adiciona tarefa ao contexto e salva no banco
            _context.Tarefas.Add(tarefa);
            _context.SaveChanges();

            // R:   Retorna Created com location header apontando para o recurso criado
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            // R:   Busca tarefa existente no banco
            var tarefaBanco = _context.Tarefas.Find(id);

            // R:   Valida se tarefa existe
            if (tarefaBanco == null)
                return NotFound();

            // R:   Valida data da tarefa
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // R:   Implementado - atualiza todos os campos da tarefa existente
            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;

            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            // R:   Implementado - marca entidade como modificada e salva mudanças
            _context.Tarefas.Update(tarefaBanco);
            _context.SaveChanges();

            // R:   Retorna OK com a tarefa atualizada
            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // R:   Busca tarefa a ser deletada
            var tarefaBanco = _context.Tarefas.Find(id);

            // R:   Valida se tarefa existe
            if (tarefaBanco == null)
                return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            // R:   Implementado - remove tarefa do contexto e salva mudanças
            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            // R:    Retorna NoContent (204) para operação bem sucedida sem conteúdo
            return NoContent();
        }
    }
}