using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace EntityGrabber
{
    /// <summary>
    /// Abstract class to be inherited for a model entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="P"></typeparam>
    [DataContract]
    public abstract class ModelRepository<T, P> : IModelRepository<T, P>
    {
        private readonly SqlConnection dbConnection;
        /// <summary>
        /// Constructor to be intitialize with a connection string
        /// </summary>
        /// <param name="connectionString"></param>
        protected ModelRepository(string connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }
        /// <summary>
        /// Connection to the Database.
        /// </summary>
        protected SqlConnection Connection => dbConnection;
        /// <summary>
        /// Used to get the data from the database into tabular form.
        /// </summary>
        /// <param name="query">Query command to fetch the data</param>
        /// <param name="type">Type of the command</param>
        /// <param name="parameters">Parameters if required</param>
        /// <returns>Iterative Dataset</returns>

        protected IEnumerable<IDataRecord> GetIteratableData(string query, SQLCommandTypes type, params SqlParameter[] parameters)
        {
            if (type == SQLCommandTypes.StoredProcedure)
            {
                SqlCommand command = new SqlCommand(query, dbConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(parameters);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader;
                    }
                }
                dbConnection.Close();
            }
            else
            {
                SqlCommand command = new SqlCommand(query, dbConnection);
                dbConnection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader;
                    }
                }
                dbConnection.Close();
            }
        }
        /// <summary>
        /// Used to get the data from the database as a single value.
        /// </summary>
        /// <param name="query">Query command to fetch the data</param>
        /// <param name="type">Type of the command</param>
        /// <param name="parameters">Parameters if required</param>
        /// <returns>Single Object Value</returns>
        protected object GetValue(string query, SQLCommandTypes type, params SqlParameter[] parameters)
        {
            object value;
            if (type == SQLCommandTypes.StoredProcedure)
            {
                SqlCommand command = new SqlCommand(query, dbConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(parameters);
                dbConnection.Open();
                try
                {
                    value = command.ExecuteScalar();
                }
                catch (SqlException)
                {
                    throw;
                }
                dbConnection.Close();
            }
            else
            {
                SqlCommand command = new SqlCommand(query, dbConnection);
                dbConnection.Open();
                try
                {
                    value = command.ExecuteScalar();
                }
                catch (SqlException)
                {
                    throw;
                }
                dbConnection.Close();
            }
            return value;
        }
        /// <summary>
        /// Used to execute query.
        /// </summary>
        /// <param name="query">Query command to fetch the data</param>
        /// <param name="type">Type of the command</param>
        /// <param name="parameters">Parameters if required</param>
        protected void ExecuteQuery(string query, SQLCommandTypes type, params SqlParameter[] parameters)
        {
            if (type == SQLCommandTypes.StoredProcedure)
            {
                SqlCommand command = new SqlCommand(query, dbConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddRange(parameters);
                dbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw;
                }
                dbConnection.Close();
            }
            else
            {
                SqlCommand command = new SqlCommand(query, dbConnection);
                dbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw;
                }
                dbConnection.Close();
            }
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public abstract P Create(IDataModel<P> model);
        public abstract bool Delete(P id);
        public abstract T Read(P id);
        public abstract List<T> ReadAll();
        public abstract bool Update(IDataModel<P> model);
    }
    [DataContract]
    public enum SQLCommandTypes
    {
        Query,
        StoredProcedure
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
