/*============================================================================
   Namespace        : DataAccess
   Class            : SqlDataAccess
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Sql DataAccess related operations
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using Helpers;
    public class SqlDataAccess : DataAccess
    {
        #region Member Variables
        private readonly string _connectionString;
        private SqlTransaction _sqlTransaction = null;
        private SqlConnection _sqlCon = null;
        private CSqlDbParameterCollection _sqlParamCollection = null;
        private SqlDataReader _dataReader = null;
        #endregion

        #region SQL Operational methods
        /// <summary>
        /// Get DataReader 
        /// </summary>
        public override DbDataReader DataReader
        {
            get { return (DbDataReader)_dataReader; }
        }

        /// <summary>
        /// get Db Transaction
        /// </summary>
        public override DbTransaction Transaction
        {
            get { return _sqlTransaction; }
        }

        /// <summary>
        /// SqlConnection
        /// </summary>
        public override DbConnection Connection
        {
            get { return (DbConnection)_sqlCon; }
            set { _sqlCon = (SqlConnection)value; }
        }

        /// <summary>
        /// Get or Set Database Connection String
        /// </summary>
        public override string ConnectionString
        {
            get
            {
                return _sqlCon.ConnectionString;
            }
            set
            {
                _sqlCon.ConnectionString = value;
            }
        }

        /// <summary>
        /// Connection Initialization
        /// </summary>
        public override void Initialize()
        {
            //default set to procedure
            _sqlCon = new SqlConnection(_connectionString);
            _sqlParamCollection = new CSqlDbParameterCollection();
        }

        /// <summary>
        /// Initialize Connection String
        /// </summary>
        public SqlDataAccess(string connectionStrig)
        {
            string connection = "connectionString";
            if (connectionStrig == null)
                throw new ArgumentNullException(connection);

            _connectionString = connectionStrig;
            this.CommandType = CommandType.StoredProcedure;
        }

        /// <summary>
        /// set the procedure type in data access
        /// </summary>
        public SqlDataAccess()
        {
            //default set to procedure
            this.CommandType = CommandType.StoredProcedure;
            _sqlCon = new SqlConnection();
        }

        /// <summary>
        /// Get output and input SQL paramter collection
        /// </summary>
        public override CSqlDbParameterCollection Parameters
        {
            get { return _sqlParamCollection; }
        }

        /// <summary>
        /// Begin Sql Transaction, if there's no transaction in the current context.
        /// </summary>
        public override void BeginTrans()
        {
            if (_sqlTransaction == null)
            {
                OpenConnection();
                _sqlTransaction = _sqlCon.BeginTransaction();
            }
        }

        /// <summary>
        /// Commit Transaction
        /// </summary>
        public override void Commit()
        {
            if (_sqlTransaction != null)
            {
                _sqlTransaction.Commit();
                _sqlTransaction = null;
            }
        }

        /// <summary>
        /// Rollback Transaction
        /// </summary>
        public override void Rollback()
        {
            if (_sqlTransaction != null)
            {
                _sqlTransaction.Rollback();
                _sqlTransaction = null;
            }
        }

        /// <summary>
        /// Fill command result set into dataSet variable
        /// </summary>
        /// <param name="dataSet">DataSet object</param>
        /// <param name="commandName">Stored Procedure Name</param>
        /// <param name="paramters">SQL parameter colelction</param>
        public override void FillData(DataSet dataSet, CSqlDbCommand command)
        {
            SqlCommand cmd = _sqlCon.CreateCommand();

            if (command.CommandTimeout != 0)
                cmd.CommandTimeout = command.CommandTimeout;
            else if (this.CommandTimeout != 0)
                cmd.CommandTimeout = this.CommandTimeout;

            cmd.CommandText = command.CommandText;
            //Command Type
            if (command.CommandType != 0)
                cmd.CommandType = command.CommandType;
            else
                cmd.CommandType = this.CommandType;

            if (command.Parameters != null && command.Parameters.Count > 0)
                cmd.Parameters.AddRange(GetParametersWithNativeType(command.Parameters));


            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            try
            {
                adp.Fill(dataSet);
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error("Error occurred at SqlDataAccess - method FillData"+ ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Open sql connection
        /// </summary>
        private void OpenConnection()
        {
            if (_sqlCon.State != ConnectionState.Open)
            {
                try
                {
                    _sqlCon.Open();
                }
                catch (Exception ex)
                {
                    LoggerMgr.Web.Error("Error occurred at SqlDataAccess - method OpenConnection" + ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Close Sql Connection
        /// </summary>
        private void CloseConnection()
        {
            if (_sqlCon != null && _sqlCon.State == ConnectionState.Open)
            {
                _sqlCon.Close();
            }
        }

        /// <summary>
        /// Execute Scalar
        /// </summary>
        /// <param name="commandName">Stored Procedure Name</param>
        /// <param name="paramters">SQL parameter collection</param>
        /// <returns></returns>
        public override object ExecScalar(CSqlDbCommand command)
        {
            OpenConnection();
            SqlCommand sqlCmd;
            try
            {
                sqlCmd = _sqlCon.CreateCommand();

                sqlCmd.CommandText = command.CommandText;

                //set transaction object
                if (_sqlTransaction != null)
                    sqlCmd.Transaction = _sqlTransaction;

                //set command timeout
                if (command.CommandTimeout != 0)
                    sqlCmd.CommandTimeout = command.CommandTimeout;
                else if (this.CommandTimeout != 0)
                    sqlCmd.CommandTimeout = this.CommandTimeout;

                //set Command Type
                if (command.CommandType != 0)
                    sqlCmd.CommandType = command.CommandType;
                else
                    sqlCmd.CommandType = this.CommandType;

                //set parameters
                if (command.Parameters != null && command.Parameters.Count > 0)
                    sqlCmd.Parameters.AddRange(GetParametersWithNativeType(command.Parameters));


                return sqlCmd.ExecuteScalar();
            }

            catch (Exception ex)
            {
                LoggerMgr.Web.Error("Error occurred at SqlDataAccess - method ExecScalar"+ex.Message);
                throw;
            }
            finally
            {
                if (_sqlTransaction == null)
                    CloseConnection();
            }
        }

        /// <summary>
        /// Execute SQL Stored Procedure
        /// </summary>
        /// <param name="commandName">Stored Procedure Name</param>
        /// <param name="paramters">SQL parameter colelction</param>
        /// <returns></returns>
        public override int ExecCommand(CSqlDbCommand command)
        {
            OpenConnection();

            SqlCommand sqlCmd;
            try
            {
                sqlCmd = _sqlCon.CreateCommand();

                sqlCmd.CommandText = command.CommandText;
                //Command Type
                if (command.CommandType != 0)
                    sqlCmd.CommandType = command.CommandType;
                else
                    sqlCmd.CommandType = this.CommandType;

                if (command.CommandTimeout != 0)
                    sqlCmd.CommandTimeout = command.CommandTimeout;
                else if (this.CommandTimeout != 0)
                    sqlCmd.CommandTimeout = this.CommandTimeout;


                if (_sqlTransaction != null)
                    sqlCmd.Transaction = _sqlTransaction;

                if (sqlCmd.CommandTimeout != 0)
                    sqlCmd.CommandTimeout = CommandTimeout;

                if (command.Parameters != null && command.Parameters.Count > 0)
                    sqlCmd.Parameters.AddRange(GetParametersWithNativeType(command.Parameters));

                int i = sqlCmd.ExecuteNonQuery();

                GetOutputParamterValuesIncludeInput((DbParameterCollection)sqlCmd.Parameters, "@");

                return i;
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error("Error occurred at SqlDataAccess - method ExecCommand "+ex.Message);
                throw;
            }
           
            finally
            {
                if (_sqlTransaction == null)
                    CloseConnection();
            }
        }

        /// <summary>
        /// Close the connection object associated with the data reader.
        /// </summary>
        public override void CloseDataReader()
        {
            if (_dataReader != null)
            {
                if (!_dataReader.IsClosed)
                {
                    _dataReader.Close();
                }
                _dataReader.Dispose();
            }
        }

        /// <summary>
        /// Execute Reader
        /// </summary>
        /// <param name="procName">Procedure Name</param>
        /// <param name="sqlParams">SqlParameterCollection
        /// use the GetInitCollection() method to init the SqlparameterCollection.
        /// Eg: SqlParameterCollection sqlParams = GetInitCollection();
        /// </param>
        /// <returns></returns>
        public override void ExecReader(CSqlDbCommand command)
        {
            OpenConnection();

            SqlCommand cmd = _sqlCon.CreateCommand();
            try
            {
                cmd.CommandText = command.CommandText;

                //Command Type
                if (command.CommandType != 0)
                    cmd.CommandType = command.CommandType;
                else
                    cmd.CommandType = this.CommandType;

                if (command.CommandTimeout != 0)
                    cmd.CommandTimeout = command.CommandTimeout;
                else if (this.CommandTimeout != 0)
                    cmd.CommandTimeout = this.CommandTimeout;

                if (_sqlTransaction != null)
                    cmd.Transaction = _sqlTransaction;

                if (command.Parameters != null && command.Parameters.Count > 0)
                    cmd.Parameters.AddRange(GetParametersWithNativeType(command.Parameters));

                this._dataReader = cmd.ExecuteReader();

                GetOutputParamterValuesIncludeInput((DbParameterCollection)cmd.Parameters, "@");

            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error("Error occurred at SqlDataAccess - method ExecReader" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Set Parameter all properties
        /// </summary>
        /// <param name="dbParameters"></param>
        /// <returns></returns>
        private SqlParameter[] GetParametersWithNativeType(CSqlDbParameterCollection dbParameters)
        {
            SqlParameter[] paramArray = new SqlParameter[dbParameters.Count];
            for (int i = 0; i < dbParameters.Count; i++)
            {
                paramArray[i] = new SqlParameter("@" + dbParameters[i].ParameterName, (dbParameters[i].Value == null) ? DBNull.Value : dbParameters[i].Value);
                paramArray[i].Direction = dbParameters[i].Direction;
                paramArray[i].Size = dbParameters[i].Size;
                paramArray[i].DbType = dbParameters[i].DbType;
            }
            return paramArray;
        }

        /// <summary>
        /// Free the allocated resources
        /// </summary>
        public override void Dispose()
        {
            try
            {
                CloseDataReader();

                if (_sqlTransaction != null)
                {
                    _sqlTransaction.Dispose();
                }

                CloseConnection();
            }

            catch (Exception ex)
            {
                LoggerMgr.Web.Error("Error occurred at SqlDataAccess - method Dispose" + ex.Message);
                throw;
            }
            _sqlCon.Dispose();
        }

        /// <summary>
        /// SQL data access specific method
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public IList<object[]> Query(string commandText, CommandType cmdType)
        {
            CSqlDbCommand cmd = new CSqlDbCommand(commandText, cmdType);

            ExecReader(cmd);

            List<object[]> list = new List<object[]>();
            while (DataReader.Read())
            {

                object[] data = new object[DataReader.FieldCount];

                for (int index = 0; index < DataReader.FieldCount; index++)
                    data[index] = DataReader[index];

                list.Add(data);
            }
            CloseDataReader();
            return list;
        }

        /// <summary>
        /// return list as a result set from command text
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IList<object[]> Query(string commandText)
        {
            return Query(commandText, CommandType.Text);
        }
        #endregion

    }
}
