using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using CRUD_Dapper.Models;
using System.Security.Cryptography.X509Certificates;


namespace CRUD_Dapper.Controllers
{
    public class PessoasController : Controller
    {

        private readonly string ConnectionString = "User ID=SeuNomeDeUsuario;Password=SuaSenha;Host=localhost;Port=5432;Database=pessoaDB;";
        public IActionResult Index()
        {
            try
            {
                using IDbConnection con =
                    new NpgsqlConnection(ConnectionString);

                string sql = "select * from pessoas";

                var listaPessoas = con.Query<Pessoas>(sql).ToList();

                return View(listaPessoas);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Pessoas pessoas)
        {
            if (ModelState.IsValid)
            {
                IDbConnection con;
                try
                {
                    string insercaoQuerry = "INSERT INTO pessoas(nome, idade, peso) VALUES(@nome, @idade, @peso)";
                    con = new NpgsqlConnection(ConnectionString);
                    con.Open();
                    con.Execute(insercaoQuerry, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }

                catch (Exception ex) {
                    return Content(ex.ToString());
                }
            }

            return View(pessoas);
        }
        [HttpGet]
        public IActionResult Edit(int pessoaid) {

            IDbConnection con;

            try
            {
                String selecaoQuery = "SELECT * FROM pessoas WHERE pessoaid = @pessoaid";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                Pessoas pessoas = con.Query<Pessoas>(selecaoQuery, new { pessoaid = pessoaid }).FirstOrDefault();
                con.Close();

                return View(pessoas);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }

        }

        [HttpPost]
        public IActionResult Edit(int pessoaid, Pessoas pessoas){
            if (pessoaid != pessoas.pessoaid)
                return NotFound();
            if (ModelState.IsValid)
            {
                IDbConnection con;
                try
                {
                    con = new NpgsqlConnection(ConnectionString);
                    String atualizarQuery = "UPDATE pessoas SET nome = @nome, idade = @idade, peso = @peso WHERE pessoaid = @pessoaid";
                    con.Open();
                    con.Execute(atualizarQuery, pessoas);
                    con.Close();
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    return Content(ex.ToString());
                }
            }

            return View(pessoas);
        }

        [HttpPost]
        public IActionResult Delete(int pessoaid)
        {
            IDbConnection con;
            try
            {
                string excluirQuery = "DELETE FROM pessoas WHERE pessoaid = @pessoaid";
                con = new NpgsqlConnection(ConnectionString);
                con.Open();
                con.Execute(excluirQuery, new { pessoaid = pessoaid });
                con.Close();
                return RedirectToAction(nameof(Index));

            }
            catch(Exception ex)
            {
                return Content(ex.ToString());
            }
        } 
        
    }
}
