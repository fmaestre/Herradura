using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;

namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla de columnas de las tablas en la base de datos.
    /// Esta clase se usa para obtner las columnas y tipos de una tabla 
    /// para poder generar sus componenetes
    /// </summary>
    [Serializable()]
    [Table("INFORMATION_SCHEMA_COLUMNS")]
    public class Inf_SchemaComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private string _tableName = string.Empty;
        private string _columName = string.Empty;
        private string _dataType = string.Empty;
        private int _is_fk = 0;
        private int _is_pk = 0;
        private int _MaxLength = 0;
        private int _is_identity;
        private string _columnDescription = "";
        #endregion //terminan variables

        #region Contructors
        public Inf_SchemaComp()
            : base()
        {
        }
        #endregion

        #region Public Interface
        #region properties
        /// Field es la clase de FieldAttribute
        ///El primer parametro es el nombre de la Tabla
        ///El segundo parametro es el nombre de la propiedad/Property
        ///El tercer parametro indica si este campo es llave primaria o no
        [Field("TABLE_NAME", "TableName", true, enmDataTypes.stringType, true, false)]
        public string TableName
        {
            get { return _tableName; }
            set
            {
                if (this._tableName != value)
                {
                    _tableName = value;
                    this.firePropertyChange("TableName");
                }
            }
        }


        [Field("COLUMN_NAME", "ColumnName", true, enmDataTypes.stringType, true, false)]
        public string ColumnName
        {
            get { return _columName; }
            set
            {
                if (this._columName != value)
                {
                    _columName = value;
                    this.firePropertyChange("ColumnName");
                }
            }
        }
        [Field("DATA_TYPE", "DataType", false, enmDataTypes.stringType, true, false)]
        public string DataType
        {
            get { return _dataType; }
            set
            {
                if (this._dataType != value)
                {
                    _dataType = value;
                    this.firePropertyChange("DataType");
                }
            }
        }

        [Field("is_fk", "Is_fk", false, enmDataTypes.intType, true, false)]
        public int Is_fk
        {
            get { return _is_fk; }
            set
            {
                if (this._is_fk != value)
                {
                    _is_fk = value;
                    this.firePropertyChange("Is_fk");
                }
            }
        }

        [Field("is_pk", "Is_pk", false, enmDataTypes.intType, true, false)]
        public int Is_pk
        {
            get { return _is_pk; }
            set
            {
                if (this._is_pk != value)
                {
                    _is_pk = value;
                    this.firePropertyChange("Is_pk");
                }
            }
        }

        [Field("CHARACTER_MAXIMUM_LENGTH", "MaxLength", false, enmDataTypes.intType, true, false)]
        public int MaxLength
        {
            get { return _MaxLength; }
            set
            {
                if (this._MaxLength != value)
                {
                    _MaxLength = value;
                    this.firePropertyChange("MaxLength");
                }
            }
        }


        [Field("is_identity", "Is_identity", false, enmDataTypes.intType, true, false)]
        public int Is_identity
        {
            get { return _is_identity; }
            set
            {
                if (this._is_identity != value)
                {
                    _is_identity = value;
                    this.firePropertyChange("Is_identity");
                }
            }
        }


        [Field("columnDescription", "ColumnDescription", false, enmDataTypes.stringType, true, false)]
        public string ColumnDescription
        {
            get { return _columnDescription; }
            set
            {
                if (this._columnDescription != value)
                {
                    _columnDescription = value;
                    this.firePropertyChange("ColumnDescription");
                }
            }
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

        #region ICatalog2 Members

        ArrayList ICatalog.getPropertyChanges()
        {
            return base._propChanges;
        }



        //MyCustomException ICatalog2.MyExceptions()
        //{
        //    return base._myExceptions;
        //}

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


/*

ALTER view INFORMATION_SCHEMA_COLUMNS
as
select IE.* ,
			ISNULL((
					SELECT  1      
					FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
					INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 
						ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
						AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
						AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME 
					INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU2 
						ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG  
						AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA 
						AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME 
						AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION 
					WHERE KCU1.TABLE_NAME = IE.TABLE_NAME AND KCU1.COLUMN_NAME = IE.COLUMN_NAME
			),0) IS_FK,
			ISNULL((
					SELECT 1
					FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS C
					JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K
					ON C.TABLE_NAME = K.TABLE_NAME
					AND C.CONSTRAINT_CATALOG = K.CONSTRAINT_CATALOG
					AND C.CONSTRAINT_SCHEMA = K.CONSTRAINT_SCHEMA
					AND C.CONSTRAINT_NAME = K.CONSTRAINT_NAME
					WHERE C.CONSTRAINT_TYPE = 'PRIMARY KEY' AND K.TABLE_NAME = IE.TABLE_NAME  AND K.COLUMN_NAME = IE.COLUMN_NAME
			),0) IS_PK
from INFORMATION_SCHEMA.COLUMNS IE 
 
  
*/
