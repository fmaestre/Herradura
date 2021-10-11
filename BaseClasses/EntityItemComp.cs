using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Herradura.Lib.core
{
    public class EntityItemComp
    {
        #region Class Instance Variables

            private string _fieldName = string.Empty;
            private object _fieldValue = null;
            private enmDataTypes _fieldDataType = enmDataTypes.stringType;
            private string _propertyName = string.Empty;
            private bool _isPK = false;
            private bool _isForINS = false;
            private bool _isForUPD = false;
            private string _errorMsg = string.Empty;
            private bool _isIdentity = false;
            private bool _isOutSelect = false;

        #endregion

        #region Class Variables
        #endregion

        #region Constructors

            public EntityItemComp()
                : base()
            {
            }

        #endregion

        #region Class Public Interface

        #region Properties

            public string FieldName
            {
                get { return _fieldName; }
                set { _fieldName = value; }
            }

            public object FieldValue
            {
                get { return _fieldValue; }
                set { _fieldValue = value; }
            }

            public string PropertyName
            {
                get { return _propertyName; }
                set { _propertyName = value; }
            }

            public enmDataTypes FieldDataType
            {
                get { return _fieldDataType; }
                set { _fieldDataType = value; }
            }

            public bool IsForINS
            {
                get { return _isForINS; }
                set { _isForINS = value; }
            }

            public bool IsIdentity
            {
                get { return _isIdentity; }
                set { _isIdentity = value; }
            }

            public bool IsForUPD
            {
                get { return _isForUPD; }
                set { _isForUPD = value; }
            }

            public bool IsPK
            {
                get { return _isPK; }
                set { _isPK = value; }
            }

            public string ErrorMsg
            {
                get { return _errorMsg; }
                set { _errorMsg = value; }
            }

            public bool IsOutSelect
            {
                get { return _isOutSelect; }
                set { _isOutSelect = value; }
            }

        #endregion

            #region Methods

                public void setFieldAttributes(FieldAttribute prmFldAttribute)
                {
                    this._fieldName = prmFldAttribute.FieldName;
                    this._fieldValue = prmFldAttribute.FieldValue;
                    this._fieldDataType = prmFldAttribute.DataType;
                    this._isPK = prmFldAttribute.IsKey;
                    this._isForINS = prmFldAttribute.IsForINS;
                    this._isForUPD = prmFldAttribute.IsForUPD;
                    this._isIdentity = prmFldAttribute.IsIdentity;
                    this._isOutSelect = prmFldAttribute.IsOutSelect;
                    this._propertyName = prmFldAttribute.PropertyName;                   

                }

            
            #endregion
        #endregion

        #region Class Private Interface
        #endregion

    }
}
