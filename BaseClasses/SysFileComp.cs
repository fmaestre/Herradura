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
    public class SysFileComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private string _folder = string.Empty;
        private int _idusuario = 0;
        #endregion
        #region Contructors
        public SysFileComp()
            : base()
        {
        }
        #endregion
        #region Public Interface
        #region properties

        public string folder
        {
            get;
            set;
        }

        public string fullpath
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {                        
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
