using System.Text.Json;
using StackExchange.Redis;
using WorkerAcoes.Models;

namespace WorkerAcoes.Data;

public class AcoesRepository
{
    private readonly IConfiguration _configuration;
    private readonly ConnectionMultiplexer _redisConnection;

    public AcoesRepository(IConfiguration configuration,
        ConnectionMultiplexer redisConnection)
    {
        _configuration = configuration;
        _redisConnection = redisConnection;
    }

    public void Save(Acao acao)
    {
        var dbRedis = _redisConnection.GetDatabase();

        dbRedis.HashSet($"ACAO-{acao.Codigo}", new HashEntry[]
        {
            new HashEntry("Codigo", acao.Codigo),
            new HashEntry("DataReferencia", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}"),
            new HashEntry("Valor", JsonSerializer.Serialize<double>(acao.Valor!.Value)),
            new HashEntry("CodCorretora", acao.CodCorretora),
            // FIXME: Simulação de falha
            new HashEntry("NomeCorretora", acao.CodCorretora)
            //new HashEntry("NomeCorretora", acao.NomeCorretora) // Correto
        });
    }
}