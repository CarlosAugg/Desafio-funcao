using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FI.AtividadeEntrevista.DAL
{
    internal class AcessoDados
    {
        private string stringDeConexao
        {
            get
            {
                ConnectionStringSettings conn = ConfigurationManager.ConnectionStrings["BancoDeDados"];
                if (conn != null)
                    return conn.ConnectionString;
                else
                    return string.Empty;
            }
        }

        internal bool VerificarCpfExistente(string cpf)
        {
            string NomeProcedure = "VerificarCpfExistente";
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
             new SqlParameter("@CPF", cpf)
            };

            DataSet ds = Consultar(NomeProcedure, parametros);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal void Executar(string NomeProcedure, List<SqlParameter> parametros, string cpf = null)
        {
            if (NomeProcedure.ToLower() == "FI_SP_IncClienteV2".ToLower() && !string.IsNullOrEmpty(cpf))
            {
                if (VerificarCpfExistente(cpf))
                {
                    throw new Exception($"CPF {cpf} já existe!");
                }
            }

            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(stringDeConexao);
            comando.Connection = conexao;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = NomeProcedure;
            foreach (var item in parametros)
                comando.Parameters.Add(item);

            conexao.Open();
            try
            {
                comando.ExecuteNonQuery();
            }
            finally
            {
                conexao.Close();
            }
        }

        internal DataSet Consultar(string NomeProcedure, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(stringDeConexao);

            comando.Connection = conexao;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = NomeProcedure;
            foreach (var item in parametros)
                comando.Parameters.Add(item);

            SqlDataAdapter adapter = new SqlDataAdapter(comando);
            DataSet ds = new DataSet();
            conexao.Open();

            try
            {
                adapter.Fill(ds);
            }
            catch (SqlException ex) when (ex.Number == 2627)  // Error number for unique constraint violation
            {
                // Rethrow a new exception with a more specific message.
                if (conexao != null && conexao.State == ConnectionState.Open)
                    conexao.Close();
                throw new Exception("Erro: Já existe um cliente cadastrado com esse CPF", ex);
            }
            finally
            {
                if (conexao != null && conexao.State == ConnectionState.Open)
                    conexao.Close();
            }

            return ds;
        }
    }
}