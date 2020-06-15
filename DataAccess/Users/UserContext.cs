/*============================================================================
   Namespace        : DataAccess
   Class            : UserContext
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : User contextt class perform insert, update , delete & get operations 
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
    using System.Reflection;
    using BusinessModule;
    using Helpers;

    public class UserContext
    {
       /// <summary>
       /// Validate User login details
       /// </summary>
       /// <param name="UserName"></param>
       /// <param name="Password"></param>
       /// <returns></returns>
        public UserInfoEntity ValidateUser(string UserName, string Password)
        {
            UserInfoEntity _userInfo = new UserInfoEntity();
            try
            {
                using (var dao = DbActivity.Open())
                {
                    CSqlDbCommand cmd = new CSqlDbCommand(DataHelper.DBCommands.VALIDATE_USERINFO);
                    cmd.Parameters.AddWithValue("UserName", UserName);
                    cmd.Parameters.AddWithValue("Password", Password);
                    dao.ExecReader(cmd);
                    while (dao.DataReader.Read())
                    {
                        _userInfo = new UserInfoEntity();
                        _userInfo.UserId = dao.DataReader["UserId"].ToInt();
                        _userInfo.UserName = dao.DataReader["UserName"].ToStr();
                        _userInfo.Password = dao.DataReader["Password"].ToStr();
                        _userInfo.Role = dao.DataReader["Role"].ToStr();
                        _userInfo.UpdatedOn = dao.DataReader["UpdatedOn"].ToDateTime();
                        _userInfo.ExpiredDate = dao.DataReader["ExpiredDate"].ToDateTime();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.DataAccessLayer, DataHelper.DataAccess.OrdersContext, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }
            return _userInfo;
        }


    }
}