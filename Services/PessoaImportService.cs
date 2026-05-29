using System.Text.Json;
using CRUD_Dapper.Models;

namespace CRUD_Dapper.Services
{
    public class PessoaImportService
    {
        public List<Pessoas> ImportarJson()
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "pessoas.json"
            );

            if (!File.Exists(path))
            {
                return new List<Pessoas>();
            }

            var json = File.ReadAllText(path);

            var pessoas = JsonSerializer.Deserialize<List<Pessoas>>(json);

            return pessoas ?? new List<Pessoas>();
        }
    }
}