using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;

namespace Herradura.Lib.core
{
    public class Globals
    {
        #region Class Variables

         //public static readonly string ConnString = ConfigurationManager.ConnectionStrings["SQL2008"].ConnectionString;
         //public static readonly string LocalConnString = ConfigurationManager.ConnectionStrings["LocalSQL"].ConnectionString;
        //public static readonly string ConnString = ConfigurationManager.ConnectionStrings["SQL2008"].ConnectionString;
        public static string conn;
        #endregion

        #region Class Constructors

        public static Globals Instance(string connName)
        {
            return new Globals(connName);
        }

        private Globals(string connName)
        {
            conn = connName;
        }

        public string ConnString 
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[conn].ConnectionString;
            }
        }
        #endregion
        
    }
}

