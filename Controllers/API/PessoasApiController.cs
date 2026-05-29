using Microsoft.AspNetCore.Mvc;
using CRUD_Dapper.Models;
using CRUD_Dapper.Services;
using Npgsql;
using Dapper;

namespace CRUD_Dapper.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasApiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly PessoaImportService _service;

        public PessoasApiController(IConfiguration config, PessoaImportService service)
        {
            _config = config;
            _service = service;
        }
        [HttpPost("importar-json")]
        public IActionResult ImportarJson()
        {
            var pessoas = _service.ImportarJson();

            using var connection = new NpgsqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            foreach (var pessoa in pessoas)
            {
                connection.Execute(
                    "INSERT INTO pessoas (nome, idade, peso) VALUES (@nome, @idade, @peso)",
                    pessoa
                );
            }

            return Ok("Importação concluída");
        }

        [HttpPost]
        public IActionResult Create([FromBody] Pessoas pessoa)
        {
            using var connection = new NpgsqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            connection.Execute(
                "INSERT INTO pessoas (nome, idade, peso) VALUES (@nome, @idade, @peso)",
                pessoa
            );

            return Ok("Pessoa criada");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            using var connection = new NpgsqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            var pessoas = connection.Query<Pessoas>(
                "SELECT * FROM pessoas"
            );

            return Ok(pessoas);
        }


        [HttpGet("{pessoaid}")]
        public IActionResult GetById(int pessoaid)
        {
            using var connection = new NpgsqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            var pessoa = connection.QueryFirstOrDefault<Pessoas>(
                "SELECT * FROM pessoas WHERE pessoaid = @pessoaid",
                new { pessoaid }
            );

            return Ok(pessoa);
        }

        [HttpPut("{pessoaid}")]
        public IActionResult Update(int pessoaid, [FromBody] Pessoas pessoa)
        {
            using var connection = new NpgsqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            connection.Execute(
                "UPDATE pessoas SET nome = @nome, idade = @idade, peso = @peso WHERE pessoaid = @pessoaid",
                new
                {
                    pessoa.nome,
                    pessoa.idade,
                    pessoa.peso,
                    pessoaid = pessoaid
                }
            );

            return Ok("Pessoa atualizada");
        }

        [HttpDelete("{pessoaid}")]
        public IActionResult Delete(int pessoaid)
        {
            using var connection = new NpgsqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            connection.Execute(
                "DELETE FROM pessoas WHERE pessoaid = @pessoaid",
                new { pessoaid }
            );

            return Ok("Pessoa deletada");
        }

    }
}
