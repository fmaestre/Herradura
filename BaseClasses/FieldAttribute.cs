using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Herradura.Lib.core
{
    [Serializable()]
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=true)]
    public sealed class FieldAttribute : System.Attribute
    {
        #region Class Instance Variables

            private string _fieldName = string.Empty;
            private string _propertyName = string.Empty;
            private bool _isKey = false;
            private enmDataTypes _dataType = enmDataTypes.stringType;
            private bool _isForINS = false;
            private bool _isForUPD = false;
            private object _fieldValue = null;
            
            private string _fieldName2 = string.Empty; //added Fmaestre 1-aug-09
            private bool _isIdentity = false; //added Fmaestre 13-may-10
            private bool _isOutSelect = false;

        #endregion

        #region Class Constructors

            public FieldAttribute():base()
            { 
            }

            public FieldAttribute(string prmFieldName, string prmPropertyName, bool prmIsKey, enmDataTypes prmDataType) :base()
            {
                _fieldName = prmFieldName;
                _propertyName = prmPropertyName;
                _isKey = prmIsKey;
                _dataType = prmDataType;
            }
            //Added by famestre 1-ago-09
            public FieldAttribute(string prmFieldName, string prmFieldName2, string prmPropertyName, bool prmIsKey, enmDataTypes prmDataType)
                : base()
            {
                _fieldName = prmFieldName;
                _propertyName = prmPropertyName;
                _isKey = prmIsKey;
                _dataType = prmDataType;
                _fieldName2 = prmFieldName2;
            }

            public FieldAttribute(string prmFieldName, string prmPropertyName, bool prmIsKey, enmDataTypes prmDataType, bool prmIsForINS, bool prmIsForUPD)
                : base()
            {
                _fieldName = prmFieldName;
                _propertyName = prmPropertyName;
                _isKey = prmIsKey;
                _dataType = prmDataType;
                _isForINS = prmIsForINS;
                _isForUPD = prmIsForUPD;
            }
            
           //Added by famestre 13-may-10
            public FieldAttribute(string prmFieldName, string prmPropertyName, bool prmIsKey, enmDataTypes prmDataType, bool prmIsForINS, bool prmIsForUPD, bool prmIsIdentity)
                : base()
            {
                _fieldName = prmFieldName;
                _propertyName = prmPropertyName;
                _isKey = prmIsKey;
                _dataType = prmDataType;
                _isForINS = prmIsForINS;
                _isForUPD = prmIsForUPD;
                _isIdentity = prmIsIdentity;
            }

            //Added by famestre 1-ago-09
            public FieldAttribute(string prmFieldName, string prmFieldName2, string prmPropertyName, bool prmIsKey, enmDataTypes prmDataType, bool prmIsForINS, bool prmIsForUPD)
                : base()
            {
                _fieldName = prmFieldName;
                _propertyName = prmPropertyName;
                _isKey = prmIsKey;
                _dataType = prmDataType;
                _isForINS = prmIsForINS;
                _isForUPD = prmIsForUPD;
                _fieldName2 = prmFieldName2;
            }

            //Added by famestre 14-may-2010
            public FieldAttribute(string prmFieldName, string prmFieldName2, string prmPropertyName, bool prmIsKey, enmDataTypes prmDataType, bool prmIsForINS, bool prmIsForUPD, bool prmIsIdentity)
                : base()
            {
                _fieldName = prmFieldName;
                _propertyName = prmPropertyName;
                _isKey = prmIsKey;
                _dataType = prmDataType;
                _isForINS = prmIsForINS;
                _isForUPD = prmIsForUPD;
                _fieldName2 = prmFieldName2;
                _isIdentity = prmIsIdentity;
            }

            //Added by famestre 16-feb-2013
            public FieldAttribute(string prmFieldName, string prmFieldName2, string prmPropertyName, bool prmIsKey, enmDataTypes prmDataType, bool prmIsForINS, bool prmIsForUPD, bool prmIsIdentity, bool prmOutSelect)
                : base()
            {
                _fieldName = prmFieldName;
                _propertyName = prmPropertyName;
                _isKey = prmIsKey;
                _dataType = prmDataType;
                _isForINS = prmIsForINS;
                _isForUPD = prmIsForUPD;
                _fieldName2 = prmFieldName2;
                _isIdentity = prmIsIdentity;
                _isOutSelect = prmOutSelect;
            }


            //Added by famestre 16-feb-2013
            public FieldAttribute(string prmFieldName, string prmPropertyName, bool prmIsKey, enmDataTypes prmDataType, bool prmIsForINS, bool prmIsForUPD, bool prmIsIdentity, bool prmOutSelect)
                : base()
            {
                _fieldName = prmFieldName;
                _propertyName = prmPropertyName;
                _isKey = prmIsKey;
                _dataType = prmDataType;
                _isForINS = prmIsForINS;
                _isForUPD = prmIsForUPD;
                _isIdentity = prmIsIdentity;
                _isOutSelect = prmOutSelect;
            }


        #endregion

        #region Class Public Interface

            #region Properties

            
                public string FieldName
                {
                    get { return _fieldName; }
                    set { _fieldName = value; }
                }
                
                //Added by famestre 1-ago-09
                public string FieldName2
                {
                    get { return _fieldName2; }
                    set { _fieldName2 = value; }
                }

                public string PropertyName
                {
                    get { return _propertyName; }
                    set { _propertyName = value; }
                }

                public bool IsKey
                {
                    get { return _isKey; }
                    set { _isKey = value; }
                }

                public enmDataTypes DataType
                {
                    get { return _dataType; }
                    set { _dataType = value; }
                }

                public object FieldValue
                {
                    get { return _fieldValue; }
                    set { _fieldValue = value; }
                }

                public bool IsForINS
                {
                    get { return _isForINS; }
                    set { _isForINS = value; }
                }

                public bool IsForUPD
                {
                    get { return _isForUPD; }
                    set { _isForUPD = value; }
                }

                public bool IsIdentity
                {
                    get { return _isIdentity; }
                    set { _isIdentity = value; }
                }

                public bool IsOutSelect
                {
                    get { return _isOutSelect; }
                    set { _isOutSelect = value; }
                }

            #endregion

        #endregion
    }
}
