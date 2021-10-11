using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;
namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla tipo_archivos en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("none")]
    public class FileComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private string _filename = string.Empty;
        private int _idusuario = 0;
        #endregion
        #region Contructors
        public FileComp()
            : base()
        {
        }
        #endregion
        #region Public Interface
        #region properties
        
        public string Filename
        {
            get { return _filename; }
            set
            {
                if (this._filename != value)
                {
                    _filename = value;
                    this.firePropertyChange("Filename");
                }
            }
        }
        
        public int Idusuario
        {
            get { return _idusuario; }
            set
            {
                if (this._idusuario != value)
                {
                    _idusuario = value;
                    this.firePropertyChange("Idusuario");
                }
            }
        }

        public string Path
        {
            get;
            set;
        }

        public string ValidExts
        {
            get;
            set;
        }

        public string Id01
        {
            get;
            set;
        }

        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {
            _filename = string.Empty;
            _idusuario = 0;
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
}
