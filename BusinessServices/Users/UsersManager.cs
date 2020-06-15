/*============================================================================
   Namespace        : BusinessServices
   Class            : UsersManager
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : 
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace BusinessServices
{
    using System;
    using System.Reflection;
    using BusinessModule;
    using DataAccess;
    using Helpers;

    public class UsersManager
    {
        private readonly UserContext _context;
        public UsersManager()
        {
            _context = new UserContext();
        }

        /// <summary>
        /// Validate Username and Password
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public UserInfoEntity ValidateUser(string UserName, string Password)
        {
            try
            {
                return _context.ValidateUser(UserName, Password);
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.BusinessServiceLayer, DataHelper.BusinessServices.UserManager, MethodBase.GetCurrentMethod().Name), ex);
                throw;
            }
            
        }
    }
}
