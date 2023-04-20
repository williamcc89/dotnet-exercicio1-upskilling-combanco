using Exercicio.Servicos;
using Exercicio.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exercicio
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var clientes = new List<Cliente>();
            var estacionados = new List<Movimentacao>();
            double preco = 0;
            double receitaTotal = 0;
            Console.WriteLine("==============================================================");
            Console.WriteLine("                  ESTACIONAMENTO GRUPO 3                      ");
            Console.WriteLine("==============================================================");

            Console.Write("Informe o valor inicial do estacionamento por minuto R$ ");
            preco = Double.Parse(Console.ReadLine());

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==============================================================");
                Console.WriteLine("           ESTACIONAMENTO GRUPO 3 - MENU DE OPÇÕES            ");
                Console.WriteLine("==============================================================");
                Console.WriteLine($"1 - Alterar Preço Atual (R$ {preco})");
                Console.WriteLine("2 - Cadastrar Cliente");
                Console.WriteLine("3 - Cadastrar Veículos de Clientes");
                Console.WriteLine("4 - Listar Clientes");
                Console.WriteLine("5 - Listar Veículos Estacionados");
                Console.WriteLine("6 - Cadastrar Entrada");
                Console.WriteLine("7 - Cadastrar Saída");
                Console.WriteLine("8 - Relatório de receita total");
                Console.WriteLine("9 - Sair");

                Console.Write("Digite uma das opções continuar: ");
                var opcao = Console.ReadLine();
                var sair = false;

                switch (opcao)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("=========Alteração de preço=======");
                        Console.Write("Digite o valor por minuto R$ ");
                        preco = Double.Parse(Console.ReadLine());
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("=========Cadastro de cliente=======");
                        Console.Write("Informe o nome do cliente: ");
                        string nome = Console.ReadLine();
                        Console.Write("Informe o CPF do cliente: ");
                        string cpf = Console.ReadLine();
                        Cliente cliente = new Cliente(nome, cpf);
                        PostgresPersistenciaCliente.Incluir(cliente);
                        Console.WriteLine("Cliente cadastrado com sucesso!");
                        Thread.Sleep(1000);
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("=========Cadastro de veículos=======");
                        Console.Write("Informe o CPF do cliente: ");
                        string cpfCliente = Console.ReadLine();
                        int encontrado = -1;
                        dynamic clienteEncontrado = null;
                        clientes = PostgresPersistenciaCliente.Listar();
                        clienteEncontrado = clientes.Find(c => c.CPF == cpfCliente);
                        if (clienteEncontrado == null)
                        {
                            Console.WriteLine("Erro! Cliente não encontrado ...");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.Write("Informe a marca do veículo: ");
                            string marca = Console.ReadLine();
                            Console.Write("Informe o modelo do veículo: ");
                            string modelo = Console.ReadLine();
                            Console.Write("Informe a placa do veículo: ");
                            string placa = Console.ReadLine();
                            Veiculo veiculo = new Veiculo(clienteEncontrado.Id, marca, modelo, placa);
                            PostgresPersistenciaVeiculo.Incluir(veiculo);
                            Console.WriteLine("Veículo cadastrado com sucesso!");
                            Thread.Sleep(1000);
                        }
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("=========Lista de clientes=======");
                        clientes = PostgresPersistenciaCliente.Listar();
                        foreach (var item in clientes)
                        {
                            Console.WriteLine($"Id: {item.Id}");
                            Console.WriteLine($"Nome: {item.Nome}");
                            Console.WriteLine($"CPF: {item.CPF}");
                            Console.WriteLine("Veículos do cliente:");
                            List<Veiculo> veiculos = PostgresPersistenciaVeiculo.Listar().Where(v => v.ClienteId == item.Id).ToList();
                            foreach (var veiculo in veiculos)
                            {
                                Console.WriteLine($"Id: {veiculo.Id} - Marca: {veiculo.Marca} - Modelo: {veiculo.Modelo} - Placa: {veiculo.Placa}");
                            }
                            Console.WriteLine("---------------------------------");
                        }
                        Console.WriteLine("Pressione qualquer tecla para voltar ao menu ...");
                        Console.ReadKey();
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("=========Lista de veículos estacionados=======");
                        estacionados = PostgresPersistenciaMovimentacao.Listar().Where(m => m.Saida == null).ToList();
                        if (estacionados.Count == 0)
                        {
                            Console.WriteLine("O estacionamento está vazio!");
                        }
                        foreach (var item in estacionados)
                        {
                            Veiculo veiculo = PostgresPersistenciaVeiculo.Listar().Find(v => v.Id == item.VeiculoId);
                            Console.WriteLine($"Marca: {veiculo.Marca} - Modelo: {veiculo.Modelo} - Placa: {veiculo.Placa} - Entrada: {item.Entrada}");
                            Console.WriteLine("---------------------------------");
                        }
                        Console.WriteLine("Pressione qualquer tecla para voltar ao menu ...");
                        Console.ReadKey();
                        break;
                    case "6":
                        Console.Clear();
                        Console.WriteLine("=========Entrada de Veículo=======");
                        Console.Write("Informe a placa do veículo: ");
                        string placaVeiculo = Console.ReadLine();
                        Veiculo veiculoEntrada = PostgresPersistenciaVeiculo.Listar().Find(v => v.Placa == placaVeiculo);
                        if (veiculoEntrada == null)
                        {
                            Console.WriteLine("Erro! Veículo não encontrado, faça o cadastro do cliente/veículo ...");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            bool estaEstacionado = false;
                            estacionados = PostgresPersistenciaMovimentacao.Listar().Where(m => m.Saida == null && m.VeiculoId == veiculoEntrada.Id).ToList();
                            if(estacionados.Count == 1) 
                            { 
                                Console.WriteLine("Erro! Veículo já está estacionado ...");
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                Console.Write("Informe a data e hora (dd/MM/yyyy HH:mm) de entrada do veículo ou ENTER para usar data/hora atual: ");
                                string dataInformada = Console.ReadLine();
                                DateTime dataHoraEntrada = DateTime.Now;
                                bool erro = false;
                                if (dataInformada != "")
                                {
                                    if (dataInformada.Length != 16)
                                    {
                                        erro = true;
                                        Console.WriteLine("Data/Hora informada é inválida!");
                                        Thread.Sleep(1000);
                                    }
                                    else
                                    {
                                        dataHoraEntrada = DateTime.ParseExact(dataInformada, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                    }
                                }
                                if (!erro)
                                {
                                    Movimentacao movimentacao = new Movimentacao(veiculoEntrada.Id, dataHoraEntrada, null, null);
                                    PostgresPersistenciaMovimentacao.Incluir(movimentacao);
                                    Console.WriteLine("Veículo estacionado com sucesso!");
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                        break;
                    case "7":
                        Console.Clear();
                        Console.WriteLine("=========Saída de Veículo=======");
                        Console.Write("Informe a placa do veículo: ");
                        string placaVeiculoSaida = Console.ReadLine();
                        DateTime dataHoraEntradaVeiculo = DateTime.Now;
                        Veiculo veiculoEncontrado = null;
                        Movimentacao veiculoEstacionado = null;

                        veiculoEncontrado = PostgresPersistenciaVeiculo.Listar().Find(v => v.Placa == placaVeiculoSaida);
                        if(veiculoEncontrado != null)
                        {
                            veiculoEstacionado = PostgresPersistenciaMovimentacao.Listar().Find(m => m.VeiculoId == veiculoEncontrado.Id && m.Saida == null);
                        }
                        if (veiculoEncontrado == null || veiculoEstacionado == null)
                        {
                            Console.WriteLine("Erro! Veículo não está estacionado ...");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            dataHoraEntradaVeiculo = veiculoEstacionado.Entrada;
                            Console.WriteLine($"Entrada do veículo: {dataHoraEntradaVeiculo}");
                            Console.Write("Informe a data e hora (dd/MM/yyyy HH:mm) de saída do veículo ou ENTER para usar data/hora atual: ");
                            string dataInformada = Console.ReadLine();
                            DateTime dataHoraSaida = DateTime.Now;
                            bool erro = false;
                            if (dataInformada != "")
                            {
                                if (dataInformada.Length != 16)
                                {
                                    erro = true;
                                    Console.WriteLine("Data/Hora informada é inválida!");
                                    Thread.Sleep(1000);
                                }
                                else
                                {
                                    dataHoraSaida = DateTime.ParseExact(dataInformada, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                }
                            }
                            if (!erro)
                            {
                                dataHoraEntradaVeiculo = veiculoEstacionado.Entrada;
                                TimeSpan diferenca = dataHoraSaida.Subtract(dataHoraEntradaVeiculo);
                                TimeSpan diferencaSemSegundos = new TimeSpan(diferenca.Hours, diferenca.Minutes, 0);
                                double minutos = diferencaSemSegundos.TotalMinutes;
                                if (minutos == 0)
                                {
                                    minutos = 1; //Tempo mínimo
                                }
                                double totalTicket = minutos * preco;
                                string totalTicketString = string.Format("{0:N2}", totalTicket);
                                Console.WriteLine($"O valor total do período é de R$: {totalTicketString}.");

                                receitaTotal += totalTicket;

                                veiculoEstacionado.Saida = dataHoraSaida;
                                veiculoEstacionado.Valor = totalTicket;
                                PostgresPersistenciaMovimentacao.Atualizar(veiculoEstacionado);

                                Console.WriteLine("Pressione qualquer tecla para voltar ao menu ...");
                                Console.ReadKey();
                            }
                        }
                        break;
                    case "8":
                        Console.Clear();
                        Console.WriteLine("=========Relatório de Receita=======");
                        string totalReceitaString = string.Format("{0:N2}", receitaTotal);
                        Console.WriteLine($"O valor total acumulado no estacionamento foi de R$: {totalReceitaString}.");
                        Console.WriteLine("Pressione qualquer tecla para voltar ao menu ...");
                        Console.ReadKey();
                        break;
                    case "9":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Erro! Opção inválida ...");
                        Thread.Sleep(1000);
                        break;
                }

                if (sair)
                {
                    break;
                }
            }
        }
    }
}
