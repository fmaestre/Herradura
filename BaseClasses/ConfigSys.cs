using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Herradura.Lib.BL;
using Herradura.Lib.Components;
using System.Web.Security;

namespace Herradura.Lib.core
{
    public delegate void user_got_logged(object sender, EventArgs e);

    /// <summary>
    /// Class that store the global variables to be used for the application
    /// Implements the Singleton Pattern
    /// </summary>
    public class _sys
    {
        #region Instance Variables

        private static _sys _configSys = null;
        private string _localDBConn = string.Empty;
        private string _dbConn = string.Empty;
        private string _companyID = string.Empty;
        private _user _user = null;
        private List<SysLogComp> _log;
        public  List<_user> _logged_users;

        public event user_got_logged user_got_logged_now;
        public string Application_Path0 = string.Empty;
        public string Application_Path1 = string.Empty;

        #endregion

        #region Class Constructors

        /// <summary>
        /// The constructor is declared private only the class Config Sys has can instantiate this class
        /// </summary>
        private _sys()
        {
            _user = new _user();
            _companyID = "MRL";
            _log = new List<SysLogComp>();
            _logged_users = new List<_user>();
        }

        #endregion

        #region Public Interface

        #region Singleton Entry Point

        public static _sys Instance()
        {
            if (_configSys == null)
            {
                _configSys = new _sys();
            }
            return _configSys;
        }

        int subs = 0;
        public void Subscribe(user_got_logged method)
        {
            if (subs == 1) return;
            user_got_logged_now += method;
            subs++;
        }

        #endregion

        #region Properties

        public _user user
        {
            get { return _user; }
            set { _user = value; }
        }

        public IEnumerable<SysParameterComp> SysParamsColl
        {
            get;
            set;
        }

        public string DBConnection
        {
            get { return _dbConn; }
            set { _dbConn = value; }
        }


        public string CompanyID
        {
            get { return _companyID; }
        }

        public CultureInfo cultureInfo
        {

            get { return new CultureInfo("en-US"); }
        }

        #endregion

        #region Methods
        private void loadParams()
        {
            if (_dbConn != string.Empty && (SysParamsColl == null || !SysParamsColl.Any()))
            {
                GenericBL catBL = new GenericBL();
                SysParamsColl = catBL.getSysParamaters(new SysParameterComp { ParamName = "%" });
            }

        }
        public string GetParamValue(string paramName)
        {
            this.loadParams();

            if (SysParamsColl == null || !SysParamsColl.Any())
                err.fire(true, "Parameter:" + paramName + " was no found due param collection is empty");

            SysParameterComp sc = SysParamsColl.First(delegate(SysParameterComp s)
            {
                return (s.ParamName == paramName);
            });

            if (sc == null)
                err.fire(true, "Parameter:" + paramName + " was no found within collection");
            if (sc.ParamValue1 == string.Empty)
                err.fire(true, "Parameter:" + paramName + " is empty");

            return sc.ParamValue1;
        }
        public IEnumerable<SysParameterComp> GetParamValues(string paramName)
        {
            this.loadParams();

            if (SysParamsColl == null || !SysParamsColl.Any())
                err.fire(true, "Parameter:" + paramName + " was no found due param collection is empty");

            IEnumerable<SysParameterComp> sc = SysParamsColl.Where(delegate(SysParameterComp s)
            {
                return (s.ParamName == paramName);
            });

            if (sc == null)
                err.fire(true, "Parameter:" + paramName + " was no found within collection");

            return sc;
        }


        public List<SysLogComp> log
        {
            get { return _log; }
        }

        public void logit(string id, int type, string action)
        {
            var c = action.Substring(0, action.Length >= 100 ? 100 : action.Length);
            _log.Add(new SysLogComp(id, c, type));
        }
        public void logit(string id, int type, string action, object passedValue)
        {
            string a = action;
            if (passedValue != null)
            {
                try
                {
                    action = passedValue.ToString() + ":" + a;
                }
                catch
                {
                    action = "ToString() error" + ":" + a;
                }
            }
            var c = action.Substring(0, action.Length >= 100 ? 100 : action.Length);
            _log.Add(new SysLogComp(id, c, type));
        }


        #endregion

        #endregion

        #region Private Inteface
        #endregion
/*
        public void setConfigSys(EmployeeComp _empComp)
        {
            _sys.Instance().user.EmployeeNo = _empComp.EmployeeID;
            _sys.Instance().user.LoginName = _empComp.UserName;
            _sys.Instance().user.Password = _empComp.Password;
            _sys.Instance().user.UserName = _empComp.completeEmployeeName;
            _sys.Instance().user.DefaultSite = _empComp.DefaultClinic;
            _sys.Instance().user.LoginTime = DateTime.Now;
        }

        public static bool empHasRole(EmployeeComp emp, string role)
        {
            Array array = Roles.GetRolesForUser(emp.UserName).ToArray<string>();
            foreach (object obj in array)
            {
                if (obj.ToString() == role) return true;

            }
            return false;
        }

        */
        public bool is_user_logged(string user, object suser)
        {
            
            //err.fire(_sys.Instance()._logged_users.Count == 0, "user_is_not_logged_or_found");
            //err.fire(!_sys.Instance()._logged_users.Exists(x => x.LoginName == user), "user_is_not_logged");
            //var z = _sys.Instance()._logged_users.Find(x => x.LoginName == user);
            //err.fire(z.LoginTime < DateTime.Now.AddMinutes(-30), "session_limit_reached: z:" + z.LoginTime.ToString());

            err.fire(suser == null && user != "guest", "Session_limit_reached, need to log in again" /*+ z.LoginTime.ToString()*/);
            //err.fire(suser != null && user != suser.ToString() && user != "guest", "wrong_user");
            var z = _sys.Instance()._logged_users.Find(x => x.LoginName == user);
            err.fire(z == null && user != "guest", "Your session has been ended. Please log back again." /*+ z.LoginTime.ToString()*/);

            //z.LoginTime = DateTime.Now;

            return true;
        }

        public void login_user(string user)
        {
            var z = _sys.Instance()._logged_users.Find(x => x.LoginName == user);
            if (z==null)
                _sys.Instance()._logged_users.Add(new _user { LoginName = user, IsLogged = true, LoginTime = DateTime.Now });
            else
                z.LoginTime = DateTime.Now;
        }

        public void logoff_user(string user)
        {
            var z = _sys.Instance()._logged_users.FindAll(x => x.LoginName == user);
            foreach (var x in z)
            {
                x.LoginTime = DateTime.MinValue;
                x.IsLogged = false;
            }
        }

        public void login_user(string user, string company, string company_name)
        {
            var z = _sys.Instance()._logged_users.Find(x => x.LoginName == user);
            if (z == null)
                _sys.Instance()._logged_users.Add(new _user { LoginName = user, IsLogged = true, LoginTime = DateTime.Now, DefaultSite = company, DefaultSiteDesc = company_name });
            else
                z.LoginTime = DateTime.Now;

            this.user_got_logged_now(user, EventArgs.Empty);
        }

        //public static bool empHasRole(EmployeeComp emp, string role)
        //{
        //    Array array = Roles.GetRolesForUser(emp.UserName).ToArray<string>();
        //    foreach (object obj in array)
        //    {
        //        if (obj.ToString() == role) return true;

        //    }
        //    return false;
        //}

    }
}
