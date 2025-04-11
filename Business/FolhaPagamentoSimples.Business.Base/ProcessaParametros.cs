using System.Text.Json;

namespace FolhaPagamentoSimples.Business.Base
{
    public class ProcessaParametros
    {
        //Faz a leitura das propriedades de Parametros.json para a classe comuns manipular.
        public static Comuns Processar(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Comuns>(json);
        }
    }
}
