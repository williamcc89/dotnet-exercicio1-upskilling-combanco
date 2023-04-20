using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercicio.Models;
using Npgsql;

namespace Exercicio.Servicos
{
    public class PostgresPersistenciaCliente
    {
        private static readonly string connString = "Server=localhost;User Id=postgres;Password=postgres;Database=upskilling1;";

        public static void Incluir(Cliente cliente)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO clientes (nome, cpf) VALUES (@nome, @cpf);", conn);
            cmd.Parameters.AddWithValue("nome", cliente.Nome);
            cmd.Parameters.AddWithValue("cpf", cliente.CPF);
            cmd.ExecuteNonQuery();
        }

        public static void Atualizar(Cliente cliente)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE clientes SET nome = @nome, telefone = @telefone WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("nome", cliente.Nome);
            cmd.Parameters.AddWithValue("telefone", cliente.CPF);
            cmd.Parameters.AddWithValue("id", cliente.Id);
            cmd.ExecuteNonQuery();
        }

        public static List<Cliente> Listar()
        {
            List<Cliente> clientes = new List<Cliente>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM clientes;", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                clientes.Add(new Cliente(reader.GetInt32(0))
                {
                    Nome = reader.GetString(1),
                    CPF = reader.GetString(2)
                });
            }
            return clientes;
        }

        public static void Apagar(int id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM clientes WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
