/*============================================================================
   Namespace        : Helpers
   Class            : DataHelper
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Maintain all constraints
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/


namespace Helpers
{
    public class DataHelper
    {
        public const string ErrorOccured = "Error occurred at Layer @Layer, Class Or Controller @Class and Action @Action.";

        public static class Layer
        {
            public const string WebApiLayer = "WebAPI";
            public const string BusinessServiceLayer = "BusinessServices";
            public const string DataAccessLayer = "DataAccess";
            public const string DataHelperLayer = "Helpers";
        }
        public static class BusinessServices
        {
            public const string OrdersManager = "OrdersManager";
            public const string UserManager = "UsersManager";
        }

        public static class DataAccess
        {
            public const string OrdersContext = "OrdersContext";
            public const string UserContext = "UserContext";
        }

        public static class DBCommands
        {
            //User
            public const string VALIDATE_USERINFO = "Validate_UserInfo";

            //Order
            public const string USP_SAVE_ORDERINFO = "[USP_SAVE_ORDERINFO]";
            public const string USP_GET_ORDERLIST = "[USP_GET_ORDERLIST]";
            public const string USP_GET_PRODUCTLIST = "[USP_GET_PRODUCTLIST]";
            public const string USP_Save_PRODUCTINFO = "[USP_SAVE_PRODUCTINFO]";
            public const string USP_DELETE_ORDER = "[USP_DELETE_ORDERDETAILS]";
        }

        public static class ApiControllers
        {
            public const string Orders = "Orders";
            public const string Users = "Users";
        }

        public static class Emails
        {
            public const string Subject = "Order Confirmation - Your Order has been successfully placed!";
            public const string Body = "Hi Customer,<br/><br/> Thank you for your order! ";
        }
    }
}
