using System;
using System.Collections.Generic; using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;

namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea los Lugares de trabajo
    /// </summary>
    [Serializable()]
    [Table("SYSTEM_PARAMETERS")] 
    public class SysParameterComp : BaseComponentClass, ICatalog
    {

        #region Contructors

        public SysParameterComp(): base()
        {
        }
        #endregion
        #region Public Interface
        #region properties
        ///<summary>Field es la clase de FieldAttribute
        /// El primer parametro es el nombre del campo en la tabla 1
        /// El segundo parametro es el nombre de la propiedad/Property
        /// El tercer parametro indica si este campo es llave primaria o no
        /// </summary> 
        [Field("COMPANY_ID", "CompanyID", false, enmDataTypes.stringType, false, false)]
        public string CompanyID
        {
            get;
            set;
        }

        [Field("CLINIC_ID", "ClinicID", false, enmDataTypes.stringType, false, false)]
        public string ClinicID
        {
            get;
            set;
        }
        [Field("PARAMETER_NAME", "ParamName", false, enmDataTypes.stringType, false, false)]
        public string ParamName
        {
            get;
            set;
        }
        [Field("PARAMETER_VALUE1", "ParamValue1", false, enmDataTypes.stringType, false, false)]
        public string ParamValue1
        {
            get;
            set;
        }
        [Field("PARAMETER_VALUE2", "ParamValue2", false, enmDataTypes.stringType, false, false)]
        public string ParamValue2
        {
            get;
            set;
        }

        [Field("DESCRIPTION", "Description", false, enmDataTypes.stringType, false, false)]
        public string Description
        {
            get;
            set;
        }

        #endregion //termina properties
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
