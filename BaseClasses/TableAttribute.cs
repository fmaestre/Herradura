using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Herradura.Lib.core
{
    /// <summary>
    /// Attribute to denote a table's name
    /// </summary>
    /// 
    [Serializable()]
    public sealed class TableAttribute : System.Attribute
    {
        // Class private variables
        private string _tableName = string.Empty;
        private string _storedProc = string.Empty;

        // Attribute Constructor
        public TableAttribute() :base()
        { 
        }
        public TableAttribute(string prmTableName):base()
        {
            _tableName = prmTableName;
        }

        public TableAttribute(string prmTableName, string prmStoredProc)
            : base()
        {
            _tableName = prmTableName;
            _storedProc = prmStoredProc;
        }

        // Public Properties
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public string StoredProc
        {
            get { return _storedProc; }
            set { _storedProc = value; }
        }


    }



}
