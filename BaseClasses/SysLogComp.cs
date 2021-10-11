using System; 
using System.Collections.Generic; 
using System.Collections; 
using System.Linq; 
using System.Text;
using Herradura.Lib.core;
namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla SYS_LOG en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("SYS_LOG")]
    public class SysLogComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private string _log_id = string.Empty;
        private int _type = -1;
        private string _action = string.Empty;
        private string _machine_id = string.Empty;
        private string _user_id = string.Empty;
        private Nullable<DateTime> _date = null;
        private string _dateS = string.Empty;
        #endregion
        #region Contructors
        public SysLogComp()
            : base()
        {
        }

        public SysLogComp(string id, string action, int type)
            : base()
        {
            Action = action;
            Log_id = id;
            Type = type;
            flagup = false;
            Date = Portal.DAL.PortalCore.GenericDAL.getTimeofServer();
            Machine_id = System.Net.Dns.GetHostName();
        }

        #endregion
        #region Public Interface
        #region properties
        [Field("log_id", "Log_id", true, enmDataTypes.stringType, true, true)]
        public string Log_id
        {
            get { return _log_id; }
            set
            {
                if (this._log_id != value)
                {
                    _log_id = value;
                    this.firePropertyChange("Log_id");
                }
            }
        }
        [Field("type", "Type", false, enmDataTypes.intType, true, true)]
        public int Type
        {
            get { return _type; }
            set
            {
                if (this._type != value)
                {
                    _type = value;
                    this.firePropertyChange("Type");
                }
            }
        }
        [Field("action", "Action", false, enmDataTypes.stringType, true, true)]
        public string Action
        {
            get { return _action; }
            set
            {
                if (this._action != value)
                {
                    _action = value;
                    this.firePropertyChange("Action");
                }
            }
        }
        [Field("machine_id", "Machine_id", false, enmDataTypes.stringType, true, true)]
        public string Machine_id
        {
            get { return _machine_id; }
            set
            {
                if (this._machine_id != value)
                {
                    _machine_id = value;
                    this.firePropertyChange("Machine_id");
                }
            }
        }
        [Field("CREATED_BY", "CreatedBy", false, enmDataTypes.stringType, true, true)]
        public string CreatedBy
        {
            get { return _user_id; }
            set
            {
                if (this._user_id != value)
                {
                    _user_id = value;
                    this.firePropertyChange("CreatedBy");
                }
            }
        }
        [Field("date", "Date", false, enmDataTypes.DateTimeType, true, true)]
        public Nullable<DateTime> Date
        {
            get { return _date; }
            set
            {
                if (this._date != value)
                {
                    _date = value;
                    this.firePropertyChange("Date");
                }
            }
        }

        [Field("date", "DateS", false, enmDataTypes.DateTimeType, false, false)]
        public string DateS
        {
            get { return _dateS; }
            set
            {
                if (this._dateS != value)
                {
                    _dateS = value;
                    this.firePropertyChange("DateS");
                }
            }
        }

        public bool flagup { get; set; }

        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {
            _log_id = string.Empty;
            _type = 0;
            _action = string.Empty;
            _machine_id = string.Empty;
            _user_id = string.Empty;
            _date = null;
            base.resetObjects();
        }
        #endregion
        #region Methods
        #endregion //Termina Metodos
        #region ICatalog2 Members
        ArrayList ICatalog.getPropertyChanges()
        {
            return base._propChanges;
        }
        void ICatalog.markAsSaved()
        {
            base.markCompAsSaved();
        }
        public void markAsUnSaved()
        {
            base.MarkCompAsUnSaved();
        }

        #endregion
        #endregion //termina public interface
        #region Private Interface
        #endregion //Termina Private Interface
    }
}
