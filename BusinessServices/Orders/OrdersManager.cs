/*============================================================================
   Namespace        : BusinessServices
   Class            : OrdersManager
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;
    using DataAccess;
    using BusinessModule;
    using Helpers;


    public class OrdersManager
    {
        private readonly OrdersContext context;
        public OrdersManager()
        {
            context = new OrdersContext();
        }

        /// <summary>
        /// Save Order deatils
        /// </summary>
        /// <param name="_orderInfo"></param>
        /// <returns></returns>
        public int SaveOrders(OrdersEntity _orderInfo)
        {
            try
            {
                int _response =  context.SaveOrderlist(_orderInfo);
                if (_response > 0)
                {
                    EmailService _email = new EmailService();
                    _email.SendEmail(_orderInfo.BuyerInfoDetail.Email.ToStr());
                }
                return _response;
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.BusinessServiceLayer, DataHelper.BusinessServices.OrdersManager, MethodBase.GetCurrentMethod().Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Get Order details
        /// </summary>
        /// <returns></returns>
        public List<OrdersEntity> GetAllOrders(int? _orderId = null)
        {
            try
            {
                return context.GetAllOrders(_orderId);
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.BusinessServiceLayer, DataHelper.BusinessServices.OrdersManager, MethodBase.GetCurrentMethod().Name), ex);
                throw;
            }
        }
        

        /// <summary>
        /// Update Order details
        /// </summary>
        /// <param name="_ordersEntity"></param>
        /// <returns></returns>
        public int UpdateOrders(OrdersEntity _ordersEntity)
        {
            try
            {
                return context.SaveOrderlist(_ordersEntity);
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.BusinessServiceLayer, DataHelper.BusinessServices.OrdersManager, MethodBase.GetCurrentMethod().Name), ex);
                throw;
            }
        }

        /// <summary>
        /// Delete order based on id
        /// </summary>
        /// <param name="_orderId"></param>
        /// <returns></returns>
        public int DeleteOrders(int _orderId)
        {
            try
            {
                return context.DeleteOrderId(_orderId);
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.BusinessServiceLayer, DataHelper.BusinessServices.OrdersManager, MethodBase.GetCurrentMethod().Name), ex);
                throw;
            }
            
        }

    }
}
