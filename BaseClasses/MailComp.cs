using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;
namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla FIZ_anos en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("none")]
    public class MailComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private int _id = 0;
        #endregion
        #region Contructors
        public MailComp()
            : base()
        {
            base.Error = "mail_ya_existe";
        }
        #endregion

        public string From
        {
            get;
            set;
        }
        public string To
        {
            get;
            set;
        }
        public string Subject
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }

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
         //termina public interface
        #region Private Interface
        #endregion //Termina Private Interface

    }
}
