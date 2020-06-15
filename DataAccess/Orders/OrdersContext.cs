/*============================================================================
   Namespace        : DataAccess
   Class            : OrdersContext
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Orders context class perform insert, update , delete & get operations 
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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BusinessModule;
    using Helpers;
    using System.Reflection;
    using System.Data;

    public class OrdersContext
    {
        /// <summary>
        /// Get Orderlist
        /// </summary>
        /// <returns></returns>
        public List<OrdersEntity> GetAllOrders(int? _orderId)
        {
            List<OrdersEntity> _ordersEntity = new List<OrdersEntity>();
            try
            {
                using (var dao = DbActivity.Open())
                {
                    CSqlDbCommand cmd = new CSqlDbCommand(DataHelper.DBCommands.USP_GET_ORDERLIST);
                    cmd.Parameters.AddWithValue("OrderId", _orderId);
                    dao.ExecReader(cmd);
                    while (dao.DataReader.Read())
                    {
                        _ordersEntity.Add(new OrdersEntity()
                        {
                            OrderId = dao.DataReader["OrderId"].ToInt(),
                            BuyerId = dao.DataReader["BuyerId"].ToInt(),
                            OrderStatus = dao.DataReader["Status_Code"].ToStr(),
                            BuyerInfoDetail = new BuyerEntity()
                            {
                                BuyerId = dao.DataReader["BuyerId"].ToInt(),
                                LastName = dao.DataReader["LastName"].ToStr(),
                                FirstName = dao.DataReader["FirstName"].ToStr(),
                                PhoneNumber = dao.DataReader["Mobile"].ToStr(),
                                Email = dao.DataReader["Email"].ToStr(),
                                Address = dao.DataReader["Address"].ToStr()
                            },
                            ProductInfoDetail = GetProductDetails(dao.DataReader["OrderId"].ToInt())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
               LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.DataAccessLayer, DataHelper.DataAccess.OrdersContext, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }
            return _ordersEntity;
        }

        /// <summary>
        /// Get Product list based on order id
        /// </summary>
        /// <param name="_orderId"></param>
        /// <returns></returns>
        public List<ProductEntity> GetProductDetails(int _orderId)
        {
            List<ProductEntity> _productEntity = new List<ProductEntity>();
            try
            {
                using (var dao = DbActivity.Open())
                {
                    CSqlDbCommand cmd = new CSqlDbCommand(DataHelper.DBCommands.USP_GET_PRODUCTLIST);
                    cmd.Parameters.AddWithValue("OrderId", _orderId);
                    dao.ExecReader(cmd);
                    while (dao.DataReader.Read())
                    {
                        _productEntity.Add(new ProductEntity()
                        {
                            OrderId = dao.DataReader["OrderId"].ToInt(),
                            SKUID = dao.DataReader["SKUID"].ToInt(),
                            Name = dao.DataReader["Name"].ToStr(),
                            Weight = dao.DataReader["Weight"].ToDecimal(),
                            Height = dao.DataReader["Height"].ToDecimal(),
                            Quantity = dao.DataReader["Quantity"].ToInt(),
                            Amount = dao.DataReader["Amount"].ToDecimal(),
                            Image = dao.DataReader["Image"].ToStr(),
                            Barcode = dao.DataReader["Barcode"] == DBNull.Value ? new byte() :Convert.ToByte(dao.DataReader["Barcode"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.DataAccessLayer, DataHelper.DataAccess.OrdersContext, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }
            return _productEntity;
        }

        /// <summary>
        /// Save order information
        /// </summary>
        /// <param name="_ordersInfo"></param>
        public int SaveOrderlist(OrdersEntity _ordersInfo)
        {
            int ResultsId = 0;
            using (var dao = DbActivity.Open())
            {
                try
                {
                    dao.BeginTrans();
                    CSqlDbCommand cmd = new CSqlDbCommand(DataHelper.DBCommands.USP_SAVE_ORDERINFO);
                    cmd.Parameters.AddWithValue("OrderId", _ordersInfo.OrderId);
                    cmd.Parameters.AddWithValue("BuyerId", _ordersInfo.BuyerId);
                    cmd.Parameters.AddWithValue("OrderStatus", _ordersInfo.OrderStatus);
                    cmd.Parameters.AddWithValue("LastName", _ordersInfo.BuyerInfoDetail.LastName);
                    cmd.Parameters.AddWithValue("FirstName", _ordersInfo.BuyerInfoDetail.FirstName);
                    cmd.Parameters.AddWithValue("Mobile", _ordersInfo.BuyerInfoDetail.PhoneNumber);
                    cmd.Parameters.AddWithValue("Email", _ordersInfo.BuyerInfoDetail.Email);
                    cmd.Parameters.AddWithValue("Address", _ordersInfo.BuyerInfoDetail.Address);
                    cmd.Parameters.Add("BuyerResultId", _ordersInfo.BuyerInfoDetail.BuyerId, ParameterDirection.InputOutput, DbType.Int32);
                    dao.ExecCommand(cmd);
                    ResultsId = dao.Parameters["BuyerResultId"].Value.ToInt();
                    SaveProductsInfo(_ordersInfo.ProductInfoDetail, ResultsId, dao);
                    dao.Commit();
                }
                catch (Exception ex)
                {
                    dao.Rollback();
                    LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.DataAccessLayer, DataHelper.DataAccess.OrdersContext, MethodInfo.GetCurrentMethod().Name), ex);
                    throw;
                }
            }
            return ResultsId;
        }

        /// <summary>
        /// Save Products Information
        /// </summary>
        /// <param name="_buyerEntity"></param>
        /// <returns></returns>
        public void SaveProductsInfo(List<ProductEntity> _product, int _orderId, IDataAccess dao)
        {
            try
            {
                CSqlDbCommand cmd = new CSqlDbCommand(DataHelper.DBCommands.USP_Save_PRODUCTINFO);
                foreach (ProductEntity _pr in _product)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("OrderId", _orderId);
                    cmd.Parameters.AddWithValue("SKUID", _pr.SKUID);
                    cmd.Parameters.AddWithValue("Name", _pr.Name);
                    cmd.Parameters.AddWithValue("Weight", _pr.Weight);
                    cmd.Parameters.AddWithValue("Height", _pr.Height);
                    cmd.Parameters.AddWithValue("Quantity", _pr.Quantity);
                    cmd.Parameters.AddWithValue("Amount", _pr.Weight);
                    //cmd.Parameters.AddWithValue("Barcode", _pr.Barcode);
                    cmd.Parameters.AddWithValue("Image", _pr.Image);
                    dao.ExecCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.DataAccessLayer, DataHelper.DataAccess.OrdersContext, MethodInfo.GetCurrentMethod().Name), ex);
                throw;
            }

        }

        /// <summary>
        /// Delete Order id 
        /// </summary>
        /// <param name="_orderId"></param>
        /// <returns></returns>
        public int DeleteOrderId(int _orderId)
        {
            int result = 0;
            using (var dao = DbActivity.Open())
            {
                try
                {
                    CSqlDbCommand cmd = new CSqlDbCommand(DataHelper.DBCommands.USP_DELETE_ORDER);
                    cmd.Parameters.AddWithValue("OrderId", _orderId);
                    result = dao.ExecCommand(cmd);
                    dao.Commit();
                }
                catch (Exception ex)
                {
                    dao.Rollback();
                    LoggerMgr.Web.Error(LoggerMgr.GetErrorMessageRootText(DataHelper.Layer.DataAccessLayer, DataHelper.DataAccess.OrdersContext, MethodInfo.GetCurrentMethod().Name), ex);
                    throw;
                }
                return result;
            }
        }

    }
}
