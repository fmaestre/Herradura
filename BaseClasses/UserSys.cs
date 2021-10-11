using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Herradura.Lib.Components;
//using Herradura.Lib.BL;

namespace Herradura.Lib.core
{
    public class _user
    {
        #region Class Instance Variables

        private double _employeeNo;
        private string _userLogin = string.Empty;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private bool _isLogged = false;
        private string _defaultSite = string.Empty;
        private Nullable<DateTime> _loggedTime = null;
        private List<string> _roles = null;
        private List<string> _siteColl = null;
        private string _defaultSiteDesc;
        //private List<EmployeeRelComp> _relatedEmployees = null;




        #endregion

        #region Constructors

        public _user()
        {
            _roles = new List<string>();
            _siteColl = new List<string>();
        }

        #endregion

        #region Public Interface

        #region Methods

        public void LogOff()
        {
            _employeeNo = 0;
            _userLogin = string.Empty;
            _userName = string.Empty;
            _isLogged = false;
            _defaultSite = string.Empty;
            _loggedTime = null;
            _roles.Clear();
            _siteColl.Clear();
        }
        public bool doesUserHasRoles()
        {
            if (_sys.Instance().user.Roles.Count > 0)
                return true;

            return false;
        }
        public bool doesUserHasRole(string strRole)
        {
            bool retVal = false;

            if (doesUserHasRoles() == true)
            {
                foreach (string role in _sys.Instance().user.Roles)
                {
                    if (role.ToUpper() == strRole.ToUpper())
                    {
                        retVal = true;
                        break;
                    }
                }

            }
            if (retVal == false)
            {
                err.fire(true, "user has no role " + strRole);
            }
            return retVal;

        }
        //public bool doesUserHasRole(string strRole, bool riseError)
        //{
        //    bool retVal = false;

        //    if (doesUserHasRoles() == true)
        //    {
        //        foreach (string role in _sys.Instance().user.Roles)
        //        {
        //            if (role.ToUpper() == strRole.ToUpper() || role.ToUpper() == Roles_.Admin)
        //            {
        //                retVal = true;
        //                break;
        //            }
        //        }

        //    }
        //    if (!retVal && riseError)
        //    {
        //        err.require(true, "user does not have role:" + strRole);
        //    }
        //    return retVal;
        //}
        //public void SetRelatedEmployees()
        //{
        //    if (_userLogin == string.Empty) err.require(true,"_m0003");
        //    if (EmployeeNo == 0.0) err.require(true,"_m0003");

        //    CatalogBL cb = new CatalogBL();
        //    _relatedEmployees = cb.getEmployeeRelComps(new EmployeeRelComp { Employee_id = EmployeeNo });
        //}
        #endregion


        #region Properties

        public double EmployeeNo
        {
            get { return _employeeNo; }
            set { _employeeNo = value; }
        }

        //public List<EmployeeRelComp> RelatedEmployess
        //{
        //    get { return _relatedEmployees; }

        //}

        public string LoginName
        {
            get { return _userLogin; }
            set { _userLogin = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public bool IsLogged
        {
            get { return _isLogged; }
            set { _isLogged = value; }
        }

        public string DefaultSite
        {
            get { return _defaultSite; }
            set { _defaultSite = value; }
        }

        public string DefaultSiteDesc
        {
            get { return _defaultSiteDesc; }
            set { _defaultSiteDesc = value; }
        }

        public Nullable<DateTime> LoginTime
        {
            get { return _loggedTime; }
            set { _loggedTime = value; }
        }

        public List<string> Roles
        {
            get { return _roles; }
            set { _roles = value; }
        }

        public List<string> AllowedSites
        {
            get { return _siteColl; }
            set { _siteColl = value; }
        }

        #endregion


        #endregion

        #region Private Interface
        #endregion

        public bool doesUserHasRole(string strRole, bool riseError)
        {
            bool retVal = false;

            if (doesUserHasRoles() == true)
            {
                foreach (string role in _sys.Instance().user.Roles)
                {
                    if (role.ToUpper() == strRole.ToUpper() || role.ToUpper() == "DxAdmin")
                    {
                        retVal = true;
                        break;
                    }
                }

            }
            if (!retVal && riseError)
            {
                err.fire(true, "user does not have role:" + strRole);
            }
            return retVal;
        }

    }
}
