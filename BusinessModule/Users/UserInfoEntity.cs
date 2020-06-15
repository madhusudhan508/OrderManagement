/*============================================================================
   Namespace        : BusinessModule
   Class            : UserInfoEntity
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Users Properties
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace BusinessModule
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserInfoEntity
    {
        public UserInfoEntity()
        {
            this._UserId = 0;
        }
        [Key]
        #region private propoty

        private int _UserId;
        private string _UserName;
        private string _Password;
        private string _Role;
        private DateTime _CreatedOn;
        private DateTime? _UpdatedOn;
        private DateTime? _ExpiredDate;

        #endregion

        #region public property

        public int UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                this._UserId = value;
            }
        }

        public string UserName
        {
            get
            {
                return this._UserName;
            }
            set
            {
                this._UserName = value;
            }
        }

        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                this._Password = value;
            }
        }

        public string Role
        {
            get
            {
                return this._Role;
            }
            set
            {
                this._Role = value;
            }
        }

        public DateTime CreatedOn
        {
            get
            {
                return this._CreatedOn;
            }
            set
            {
                this._CreatedOn = value;
            }
        }

        public DateTime? UpdatedOn
        {
            get
            {
                return this._UpdatedOn;
            }
            set
            {
                this._UpdatedOn = value;
            }
        }

        public DateTime? ExpiredDate
        {
            get
            {
                return this._ExpiredDate;
            }
            set
            {
                this._ExpiredDate = value;
            }
        }

        #endregion
    }
}
