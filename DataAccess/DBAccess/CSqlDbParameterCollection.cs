/*============================================================================
   Namespace        : DataAccess
   Class            : CSqlDbParameterCollection
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
    using System.Collections;

    public sealed class CSqlDbParameterCollection : DbParameterCollection
    {

        #region Member Variables
        readonly ArrayList _list = new ArrayList();
        #endregion

        #region Properties
        public new CSqlDbParamter this[string parameterName]
        {
            get
            {
                foreach (CSqlDbParamter p in _list)
                    if (p.ParameterName.Equals(parameterName))
                        return p;
                throw new ArgumentOutOfRangeException("The specified name does not exist: " + parameterName);
            }
            set
            {
                if (!Contains(parameterName))
                    throw new ArgumentOutOfRangeException("The specified name does not exist: " + parameterName);

                this[IndexOf(parameterName)] = value;
            }

        }


        public new CSqlDbParamter this[int index]
        {
            get
            {
                return (CSqlDbParamter)_list[index];
            }
            set
            {
                _list[index] = value;
            }
        }
        #endregion

        #region SQL operational methods


        #region Add parameters to Sql Commmand object in different ways

        public void Add(CSqlDbParamter parameter)
        {
            _list.Add(parameter);
        }

        public void Add(string paramterName, object value, System.Data.ParameterDirection direction, System.Data.DbType type)
        {
            _list.Add(new CSqlDbParamter(paramterName, value)
            {
                Direction = direction,
                DbType = type
            });
        }

        public void Add(string paramterName, System.Data.ParameterDirection direction, System.Data.DbType type)
        {
            _list.Add(new CSqlDbParamter(paramterName)
            {
                Direction = direction,
                DbType = type
            });
        }

        public void Add(string paramterName, object value)
        {
            _list.Add(new CSqlDbParamter(paramterName, value));
        }

        public void AddWithValue(string paramterName, object value)
        {
            _list.Add(new CSqlDbParamter(paramterName, value));
        }
        #endregion

        #region Override methods
        public override void Clear()
        {
            _list.Clear();
        }

        public override bool Contains(string value)
        {
            bool flag = false;
            foreach (CSqlDbParamter p in _list)
            {
                if (p.ParameterName.Equals(value))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public override int Count
        {
            get { return _list.Count; }
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            return this[parameterName];
        }

        protected override DbParameter GetParameter(int index)
        {
            return this[index];
        }

        public override int IndexOf(string parameterName)
        {
            int index = 0;
            foreach (CSqlDbParamter p in _list)
            {
                if (p.ParameterName.Equals(parameterName))
                    break;
                index++;
            }
            return index;
        }

        public override void Insert(int index, object value)
        {
            _list[index] = value;
        }
        #endregion

        #region Unused Override methods

        public CSqlDbParameterCollection() { }

        public override int Add(object value)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(Array values)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public override void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
        }

        protected override void SetParameter(int index, DbParameter value)
        {
        }

        public override object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        #endregion



    }
}
