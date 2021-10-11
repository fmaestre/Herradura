using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;
namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla role_permissions en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("role_permissions")]
    public class role_permissions_comp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private string _role = string.Empty;
        private string _object_id = string.Empty;
        private Nullable<bool> _ins = null;
        private Nullable<bool> _upd = null;
        private Nullable<bool> _del = null;
        private Nullable<bool> _sel = null;
        private string _created_by = string.Empty;
        private Nullable<DateTime> _created_date = null;
        private Nullable<DateTime> _last_mod_date = null;
        private string _last_mod_by = string.Empty;
        #endregion
        #region Contructors
        public role_permissions_comp()
            : base()
        {
        }
        #endregion
        #region Public Interface
        #region properties
        [Field("role", "Role", true, enmDataTypes.stringType, true, true)]
        public string Role
        {
            get { return _role; }
            set
            {
                if (this._role != value)
                {
                    _role = value;
                    this.firePropertyChange("Role");
                }
            }
        }
        [Field("obj_id", "Obj_id", true, enmDataTypes.stringType, true, true)]
        public string Object_id
        {
            get { return _object_id; }
            set
            {
                if (this._object_id != value)
                {
                    _object_id = value;
                    this.firePropertyChange("Obj_id");
                }
            }
        }
        [Field("ins", "Ins", false, enmDataTypes.boolType, true, true)]
        public Nullable<bool> Ins
        {
            get { return _ins; }
            set
            {
                if (this._ins != value)
                {
                    _ins = value;
                    this.firePropertyChange("Ins");
                }
            }
        }
        [Field("upd", "Upd", false, enmDataTypes.boolType, true, true)]
        public Nullable<bool> Upd
        {
            get { return _upd; }
            set
            {
                if (this._upd != value)
                {
                    _upd = value;
                    this.firePropertyChange("Upd");
                }
            }
        }
        [Field("del", "Del", false, enmDataTypes.boolType, true, true)]
        public Nullable<bool> Del
        {
            get { return _del; }
            set
            {
                if (this._del != value)
                {
                    _del = value;
                    this.firePropertyChange("Del");
                }
            }
        }
        [Field("sel", "Sel", false, enmDataTypes.boolType, true, true)]
        public Nullable<bool> Sel
        {
            get { return _sel; }
            set
            {
                if (this._sel != value)
                {
                    _sel = value;
                    this.firePropertyChange("Sel");
                }
            }
        }
        [Field("created_by", "Created_by", false, enmDataTypes.stringType, true, true)]
        public string Created_by
        {
            get { return _created_by; }
            set
            {
                if (this._created_by != value)
                {
                    _created_by = value;
                    this.firePropertyChange("Created_by");
                }
            }
        }
        [Field("created_date", "Created_date", false, enmDataTypes.DateTimeType, true, true)]
        public Nullable<DateTime> Created_date
        {
            get { return _created_date; }
            set
            {
                if (this._created_date != value)
                {
                    _created_date = value;
                    this.firePropertyChange("Created_date");
                }
            }
        }
        [Field("last_mod_date", "Last_mod_date", false, enmDataTypes.DateTimeType, true, true)]
        public Nullable<DateTime> Last_mod_date
        {
            get { return _last_mod_date; }
            set
            {
                if (this._last_mod_date != value)
                {
                    _last_mod_date = value;
                    this.firePropertyChange("Last_mod_date");
                }
            }
        }
        [Field("last_mod_by", "Last_mod_by", false, enmDataTypes.stringType, true, true)]
        public string Last_mod_by
        {
            get { return _last_mod_by; }
            set
            {
                if (this._last_mod_by != value)
                {
                    _last_mod_by = value;
                    this.firePropertyChange("Last_mod_by");
                }
            }
        }

        public string Permission
        {
            get;
            set;
        }
        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {
            _role = string.Empty;
            _object_id = string.Empty;
            _ins = false;
            _upd = false;
            _del = false;
            _sel = false;
            _created_by = string.Empty;
            _created_date = null;
            _last_mod_date = null;
            _last_mod_by = string.Empty;
            base.resetObjects();
        }
        #endregion
        #region Methods
        #endregion //Termina Metodos
        #region ICatalog Members
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

    public static class Permitions
    {
        public const string Ins = "Ins";
        public const string Upd = "Upd";
        public const string Del = "Del";
        public const string Sel = "Sel";

    }
}
