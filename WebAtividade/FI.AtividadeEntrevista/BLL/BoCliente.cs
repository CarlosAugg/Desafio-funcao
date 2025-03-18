using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        private static bool IsValidCpf(string cpf)
        {
            cpf = new string(cpf.Where(c => char.IsDigit(c)).ToArray());

            if (cpf.Length != 11)
            {
                return false;
            }

            if (new string(cpf[0], 11) == cpf)
            {
                return false;
            }

            var multipliers = new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            var sum = 0;

            for (var i = 0; i < 9; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * multipliers[i];
            }

            var remainder = sum % 11;
            if (remainder < 2)
            {
                remainder = 0;
            }
            else
            {
                remainder = 11 - remainder;
            }

            sum = 0;
            for (var i = 0; i < 10; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * multipliers[i];
            }

            remainder = sum % 11;
            if (remainder < 2)
            {
                remainder = 0;
            }
            else
            {
                remainder = 11 - remainder;
            }

            return cpf[10] == remainder.ToString()[0];
        }
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            if (!IsCpfValid(cliente.CPF))
            {
                throw new Exception("O CPF informado é inválido.");
            }
            try
            {
                DAL.DaoCliente cli = new DAL.DaoCliente();
                return cli.Incluir(cliente);
            }
            catch (Exception ex)
            {
                // Re-lança a exceção para que a camada superior possa tratá-la.
                throw;
            }
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            if (!IsCpfValid(cliente.CPF))
            {
                throw new Exception("O CPF informado é inválido.");
            }
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }
    }
}