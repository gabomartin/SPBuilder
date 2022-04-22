using System;
using System.Data;
using System.Data.SqlClient;

/// <summary>
///  Clase para llamar SPs por ADO.NET, simplificando la carga de parametros por medio de constructor fluido
/// </summary>
/// <author> Gabriel Martin Pognante </author>


namespace SPBuilder
{
    public class SPBuilder
    {
        private SqlConnection _conn;
        private SqlCommand _cmd;
        private SqlDataReader _reader;
        private DataTable _result = new DataTable();

        public SPBuilder(string procedureName, string connectionString)
        {
            _conn = new SqlConnection(connectionString);
            _cmd = new SqlCommand(procedureName, _conn);
            _cmd.CommandType = CommandType.StoredProcedure;
        }
        public SPBuilder( string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        #region Parameters
        public SPBuilder AddVarChar(string nombreParametro, string valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.VarChar).Value = valorParametro;
            return this;
        }
        public SPBuilder AddText(string nombreParametro, string valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.Text).Value = valorParametro;
            return this;
        }
        public SPBuilder AddFloat(string nombreParametro, double valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.Float).Value = valorParametro;
            return this;
        }
        public SPBuilder AddInt(string nombreParametro, int valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.Int).Value = valorParametro;
            return this;
        }
        public SPBuilder AddSmallInt(string nombreParametro, short valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.SmallInt).Value = valorParametro;
            return this;
        }
        public SPBuilder AddBigInt(string nombreParametro, double valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.BigInt).Value = valorParametro;
            return this;
        }
        public SPBuilder AddNumeric(string nombreParametro, decimal valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.Decimal).Value = valorParametro;
            return this;
        }
        public SPBuilder AddChar(string nombreParametro, string valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.Char).Value = valorParametro;
            return this;
        }
        public SPBuilder AddBit(string nombreParametro, bool valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.Bit).Value = valorParametro;
            return this;
        }
        public SPBuilder AddMoney(string nombreParametro, decimal valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.Money).Value = valorParametro;
            return this;
        }
        public SPBuilder AddSmallMoney(string nombreParametro, decimal valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.SmallMoney).Value = valorParametro;
            return this;
        }
        public SPBuilder AddDateTime(string nombreParametro, DateTime valorParametro)
        {
            _cmd.Parameters.Add($"@{nombreParametro}", SqlDbType.DateTime).Value = valorParametro;
            return this;
        }

        #endregion

        public DataTable ExecuteReader()
        {
            try
            {
                _conn.Open();
                _reader = _cmd.ExecuteReader();
                _result.Load(_reader);
                return _result;
            }
            finally
            {
                if (_conn.State != ConnectionState.Closed)
                {
                    _conn.Close();
                }
                if (_reader != null)
                {
                    _reader.Close();
                }
            }

        }

        #region Methods
        public int Execute()
        {
            try
            {
                _conn.Open();
                return _cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_conn.State != ConnectionState.Closed)
                {
                    _conn.Close();
                }
            }

        }

        public SPBuilder SetSP(string procedureName)
        {
            if(_reader != null)
            {
                _reader.Close();
            }
            if (_conn.State != ConnectionState.Closed)
            {
                _conn.Close(); //NO APTO PARA ASINCRONISMO
            }

            _result = new DataTable();
            _cmd = new SqlCommand(procedureName, _conn);
            _cmd.CommandType = CommandType.StoredProcedure;
            return this;
        }
        #endregion

    }
}
