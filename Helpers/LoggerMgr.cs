/*============================================================================
   Namespace        : Helpers
   Class            : LoggerMgr
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Logging Exceptions for application Level
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace Helpers
{
    using System;
    using System.Collections.Generic;
    using log4net;
    public static class LoggerMgr
    {

        #region LoggerMgr
        private static ILog _logger = LogManager.GetLogger("OMSLogger");

        private static string _ViewDataErrorsMessageKey { get; set; } = "_ERROR_MESSAGE_LOGGER_";

        public static string ViewDataErrorsMessageKey
        {
            get
            {
                return _ViewDataErrorsMessageKey;
            }
            set
            {
                _ViewDataErrorsMessageKey = value;
            }
        }

        static LoggerMgr()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static string LogId
        {
            get
            {   
                return Guid.NewGuid().ToString();
            }
        }

        public static ILog Web
        {
            get
            {
                return _logger;
            }
        }

        public static void UserLog(Exception ex, IDictionary<string, object> viewData)
        {
            string logId = LoggerMgr.LogId;
            _logger.Error(logId, ex);
            viewData[ViewDataErrorsMessageKey] = ex.Message + ". Log ID:" + logId;
        }

        public static ILog App
        {
            get
            {
                return _logger;
            }
        }

        public static ILog Email
        {
            get
            {
                return _logger;
            }
        }

        public static ILog Database
        {
            get
            {
                return _logger;
            }
        }

        public static ILog Log
        {
            get
            {
                return _logger;
            }
        }

        public static string GetErrorMessageRootText(string layer, string controller, string action)
        {
            string result = DataHelper.ErrorOccured.Replace("@Layer", layer);
            result = result.Replace("@Class", controller);
            result = result.Replace("@Action", action);
            return result;

        }
        #endregion

    }
}
