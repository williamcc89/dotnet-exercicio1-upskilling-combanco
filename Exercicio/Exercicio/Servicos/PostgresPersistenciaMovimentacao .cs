using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercicio.Models;
using Npgsql;

namespace Exercicio.Servicos
{
    public class PostgresPersistenciaMovimentacao
    {
        private static readonly string connString = "Server=localhost;User Id=postgres;Password=postgres;Database=upskilling1;";

        public static void Incluir(Movimentacao movimentacao)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO movimentacoes (entrada, saida, valor, id_veiculo) VALUES (@entrada, @saida, @valor, @id_veiculo);", conn);
            cmd.Parameters.AddWithValue("entrada", movimentacao.Entrada);
            cmd.Parameters.AddWithValue("saida", movimentacao.Saida ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("valor", movimentacao.Valor ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("id_veiculo", movimentacao.VeiculoId);
            cmd.ExecuteNonQuery();
        }

        public static void Atualizar(Movimentacao movimentacao)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE movimentacoes SET entrada = @entrada, saida = @saida, valor = @valor, id_veiculo = @id_veiculo WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("entrada", movimentacao.Entrada);
            cmd.Parameters.AddWithValue("saida", movimentacao.Saida);
            cmd.Parameters.AddWithValue("valor", movimentacao.Valor);
            cmd.Parameters.AddWithValue("id_veiculo", movimentacao.VeiculoId);
            cmd.Parameters.AddWithValue("id", movimentacao.Id);
            cmd.ExecuteNonQuery();
        }

        public static List<Movimentacao> Listar()
        {
            List<Movimentacao> movimentacoes = new List<Movimentacao>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT id, id_veiculo, entrada, saida, valor  FROM movimentacoes;", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Movimentacao movimentacao = new Movimentacao(reader.GetInt32(0))
                {
                    Entrada = reader.GetDateTime(2),
                    VeiculoId = reader.GetInt32(1)
                };
                if(!reader.IsDBNull(3))
                {
                    movimentacao.Saida = reader.GetDateTime(3);
                }
                if (!reader.IsDBNull(4))
                {
                    movimentacao.Valor = reader.GetDouble(4);
                }
                movimentacoes.Add(movimentacao);
            }
            return movimentacoes;
        }

        public static void Apagar(int id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM movimentacoes WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
