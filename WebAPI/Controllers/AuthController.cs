/*============================================================================
   Namespace        : Helpers
   Class            : AuthController
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : validate user
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/
namespace WebAPI
{
    using BusinessModule;
    using System;
    using System.Web.Http;
    using System.Configuration;
    using BusinessServices;

    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("validateUsers")]
        public IHttpActionResult validateUsers(UserInfoEntity ui)
        {
            try
            {
                UsersManager blUser = new UsersManager();
                UserInfoEntity userInfo = blUser.ValidateUser(ui.UserName, ui.Password);

                if (userInfo != null)
                {
                    string Issure = ConfigurationManager.AppSettings["Issure"].ToString();
                    string token = TokenGenerator.GenerateToken(userInfo, Issure, 90);
                    return Ok(token);
                }
                else
                    return BadRequest("User name or password is not valid");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
