using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMVVM.Model
{
    class Database
    {
        //vooraf: instellingen ophalen uit config
        private static ConnectionStringSettings ConnectionString
        {

            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"];
            }
        }
        //stap 1 connectie opvragen
        private static DbConnection GetConnection()
        {
            DbConnection con = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateConnection();
            con.ConnectionString = ConnectionString.ConnectionString;

            con.Open();

            return con;
        }
        //stap2 connentie vrijgeven
        public static void ReleaseConnection(DbConnection con)
        {
            if (con != null)
            {
                con.Close();
                con = null;
            }
        }
        //stap 3 command opstellen: sql string en parameters
        //opm keyword params laat toe deze methode op te roepen met slecht 1 parameter namelijk de SQL string
        private static DbCommand BuildCommand(String sql, params DbParameter[] parameters)
        {
            DbCommand command = GetConnection().CreateCommand();

            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;

        }
        //stap 3bis: hulp methode om parameters te maken
        public static DbParameter AddParameter(String naam, object value)
        {
            //parameter zijn provider afhankelijk, dus we moeten terug naar de factory
            DbParameter par = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateParameter();
            par.ParameterName = naam;
            par.Value = value;
            return par;
        }
        //stap 4 : data ophalen (select)
        public static DbDataReader GetData(string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;
            try
            {
                command = BuildCommand(sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (reader != null) reader.Close();
                if (command != null) ReleaseConnection(command.Connection);

                throw ex;
            }
        }
        //stap 4 b: database gaan wijzinen ( insert/delete/update)
        public static int ModifyData(String sql, params DbParameter[] parameters)
        {
            DbCommand command = null;


            try
            {
                command = BuildCommand(sql, parameters);
                int aantalRijenGewijzigd = command.ExecuteNonQuery();

                return aantalRijenGewijzigd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (command != null) ReleaseConnection(command.Connection);

                throw ex;
            }

        }
        //extra : werken met transacties.
        //stap 3 extra: command ifv transactie.
        public static DbTransaction BeginStransaction()
        {
            DbConnection con = null;
            try
            {
                con = GetConnection();
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (con != null) ReleaseConnection(con);
                throw ex;
            }

        }
        private static DbCommand BuildCommand(DbTransaction trans, String sql, params DbParameter[] parameters)
        {
            DbCommand command = trans.Connection.CreateCommand();

            command.CommandType = System.Data.CommandType.Text;

            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }


            return command;

        }
        //stap 4a extre: data ophalen in transactie.
        public static DbDataReader GetData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;


            try
            {
                command = BuildCommand(trans, sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                ReleaseConnection(command.Connection);

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (reader != null) reader.Close();
                if (command != null) ReleaseConnection(command.Connection);

                throw ex;
            }
        }
        //stap 4b extre: data wijzigen in transactie.
        public static int ModifyData(DbTransaction trans, String sql, params DbParameter[] parameters)
        {
            DbCommand command = null;


            try
            {
                command = BuildCommand(trans, sql, parameters);
                int aantalRijenGewijzigd = command.ExecuteNonQuery();

                ReleaseConnection(command.Connection);

                return aantalRijenGewijzigd;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (command != null) ReleaseConnection(command.Connection);

                throw ex;
            }


        }
    }
}
