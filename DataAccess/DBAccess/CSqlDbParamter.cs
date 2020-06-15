/*============================================================================
   Namespace        : DataAccess
   Class            : CSqlDbParamter
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
    using System.Data.Common;
    using System.Data;

    public class CSqlDbParamter : DbParameter, IDbDataParameter, ICloneable
    {

        #region Constructor
        public CSqlDbParamter(string paramterName)
        {
            this.ParameterName = paramterName;
        }

        public CSqlDbParamter(string paramterName, object value)
        {
            this.ParameterName = paramterName;
            this.Value = value;
        }
        #endregion

        #region Overridden Properties
        public override System.Data.DbType DbType
        {
            get;
            set;
        }

        public override System.Data.ParameterDirection Direction
        {
            get;

            set;
        }

        public override bool IsNullable
        {
            get;
            set;
        }

        public override string ParameterName
        {
            get;
            set;
        }

        public override int Size
        {
            get;
            set;
        }

        public override string SourceColumn
        {
            get;
            set;
        }

        public override bool SourceColumnNullMapping
        {
            get;
            set;
        }

        public override System.Data.DataRowVersion SourceVersion
        {
            get;
            set;
        }

        public override object Value
        {
            get;
            set;
        }

        #endregion
        
        #region Methods
        
        #region Unused Override methods
        public override void ResetDbType()
        {
            throw new NotImplementedException();
        }
        #endregion
        
        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
        
        #endregion


    }
}
