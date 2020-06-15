/*============================================================================
   Namespace        : DataAccess
   Class            : DbActivity
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
    using System.Configuration;

    public static class DbActivity
    {
        /// <summary>
        /// Delaring Connection string
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["OMSConnection"].ConnectionString;
        }

        /// <summary>
        /// Open the Data Acess Connection
        /// </summary>
        public static IDataAccess Open()
        {
            string connectionString = ConnectionString();
            IDataAccess dao = new SqlDataAccess(connectionString);
            int TimeOUt = 120;
            dao.CommandTimeout = TimeOUt;
            dao.Initialize();
            return dao;
        }

        /// <summary>
        /// Dispose the connection
        /// </summary>
        /// <param name="dao"></param>
        public static void Close(IDataAccess dao)
        {
            dao.Dispose();
        }
        /// <summary>
        /// Extension method for IDataAccess
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="cmd"></param>
        /// <param name="action"></param>
        public static void ExecReader(IDataAccess dao, CSqlDbCommand cmd, Action<System.Data.IDataReader> action)
        {
            dao.ExecReader(cmd);

            var reader = dao.DataReader;
            while (reader.Read())
            {
                action(reader);
            }
            dao.CloseDataReader();
        }
    }
}
