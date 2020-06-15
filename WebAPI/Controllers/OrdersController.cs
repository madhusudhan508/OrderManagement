/*============================================================================
   Namespace        : Helpers
   Class            : Request
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : API calls/methods
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace WebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using BusinessModule;
    using Helpers;
    using BusinessServices;

    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {

        [AuthorizationFilter(Roles = "Admin,Buyer")]
        [HttpPost]
        [Route("InsertOrders")]
        public IHttpActionResult InsertOrders(OrdersEntity _orderInfo)
        {
            try
            {
                int _res = new OrdersManager().SaveOrders(_orderInfo);
                if (_res > 0)
                    return Ok();
                else
                    return BadRequest("Invalid data entry.");
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.WebApiLayer, DataHelper.ApiControllers.Orders, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }

        }

        [AuthorizationFilter(Roles = "Admin,Buyer")]
        [HttpPut]
        [Route("UpdateOrders")]
        public IHttpActionResult UpdateOrders(OrdersEntity _orderInfo)
        {
            try
            {
                int _res = new OrdersManager().UpdateOrders(_orderInfo);
                if (_res > 0)
                    return Ok();
                else
                    return BadRequest("Invalid data entry.");
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.WebApiLayer, DataHelper.ApiControllers.Orders, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }
        }

        [AuthorizationFilter(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllOrders")]
        public IHttpActionResult GetAllOrders()
        {
            try
            {
                List<OrdersEntity> _orderList = new OrdersManager().GetAllOrders();
                if (_orderList.Count > 0)
                    return Ok(_orderList);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.WebApiLayer, DataHelper.ApiControllers.Orders, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }
        }

        [AuthorizationFilter(Roles = "Admin,Buyer")]
        [HttpGet]
        [Route("GetAllOrdersByOrderId/{OrderId:int}")]
        public IHttpActionResult GetAllOrdersByOrderId(int OrderId)
        {
            try
            {
                List<OrdersEntity> _orderList = new OrdersManager().GetAllOrders(OrderId);
                if (_orderList.Count > 0)
                    return Ok(_orderList);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.WebApiLayer, DataHelper.ApiControllers.Orders, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }
        }

        [AuthorizationFilter(Roles = "Admin,Buyer")]
        [HttpDelete]
        [Route("DeleteOrders/{OrderId:int}")]
        public IHttpActionResult DeleteOrders(int OrderId)
        {
            try
            {
                int _res = new OrdersManager().DeleteOrders(OrderId);
                return Ok();

            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.WebApiLayer, DataHelper.ApiControllers.Orders, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }
        }

    }
}