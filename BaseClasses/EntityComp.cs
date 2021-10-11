using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Herradura.Lib.core;

namespace Herradura.Lib.core
{
    public class EntityComp
    {
        #region Class Intance Variables
            private string _tableName = string.Empty;
            private string _storedProc = string.Empty;
            private string _errorMsg = string.Empty;
            private bool _isSaved = false;
            private bool _isChanged = false;
            private List<EntityItemComp> itemsColl = null;
            private ArrayList propChanges = null;
        #endregion

        #region Class Variables
        #endregion

        #region Class Constructors

            public EntityComp()
            {
                intializeMyObjects();
            }

            public EntityComp(TableAttribute prmTblAttribute)
            {
                this._tableName = prmTblAttribute.TableName;
                this._storedProc = prmTblAttribute.StoredProc;
                this.intializeMyObjects();
            }

        #endregion

        #region Class Public Interface

            #region Properties

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

                public string ErrorMsg
                {
                    get { return _errorMsg; }
                    set { _errorMsg = value; }
                }

                public bool IsSaved
                {
                    get { return _isSaved; }
                    set { _isSaved = value; }
                }

                public bool IsChanged
                {
                    get { return _isChanged; }
                    set { _isChanged = value; }
                }

                public List<EntityItemComp> ItemColl
                {
                    get { return itemsColl; }
                    set { itemsColl = value; }
                }

                public bool IsValid
                {
                    get
                    {
                        if (this._errorMsg != string.Empty)
                            return false;
                        else
                            return true;
                    }
                }

                public ArrayList PropertyChanges
                {
                    get { return propChanges;  }
                    set { propChanges = value; }
                }
            
            #endregion

            #region Methods

                public void setTableAttributes(TableAttribute prmTblAttribute)
                {
                    this._tableName = prmTblAttribute.TableName;
                    this._storedProc = prmTblAttribute.StoredProc;
                }

                public string getFindQryByPks()
                {
                    if (this.IsValid != true)
                        err.fire(true,this._errorMsg);
                    //Begin Added by Fmaestre 1-Aug-09
                    //Todos los campos se agregaran al query y se remueve el *, esto permitira flexibilidad al crear los queries
                    string selectFields = string.Empty;
                    foreach (EntityItemComp ei in this.itemsColl)
                    {
                        if (selectFields != string.Empty) selectFields += ",";
                            selectFields += ei.FieldName;
                    }
                    //END Added by Fmaestre 1-Aug-09

                    string qrySel = "select " + selectFields + " from " + this._tableName + " xtraTable"; //Added by Fmaestre 1-Aug-09
                    string qryWhere = string.Empty;

                    foreach (EntityItemComp ei in this.itemsColl)
                    {
                        if (ei.IsPK == true)
                        {
                            if (qryWhere == string.Empty)
                                qryWhere = " where " + ei.FieldName; // + " = " + formatValue(ei);
                            else
                                qryWhere = qryWhere + " and " + ei.FieldName; // +" = " + formatValue(ei);

                            qryWhere = qryWhere + defineOperator(ei) + formatValue(ei);
                        }
                    }
                    return  qrySel + qryWhere;                                        
                }
                public string getInsSQLNonIDReturn(System.Data.DataRow dataRow)
                {
                    err.fire(this.IsValid != true,this._errorMsg);

                    string qryIns = "insert into " + this._tableName;
                    string qryFields = string.Empty;
                    string qryValues = string.Empty;
                    int i = -1;
                    foreach (var x in dataRow.ItemArray)
                    {
                        i++;
                        var s = dataRow.Table.Columns[i].ColumnName;
                        var z = this.ItemColl.Find(zz => zz.FieldName == s);
                        if (z == null) continue;
                        z.FieldValue = x;

                        if (!z.IsForINS) continue;
                        if (z.IsIdentity) continue;

                        if (qryFields == string.Empty)
                        {
                            qryFields = "(" + z.FieldName;
                            qryValues = " values (" + formatValue(z);
                        }
                        else
                        {
                            qryFields = qryFields + ", " + z.FieldName;
                            qryValues = qryValues + ", " + formatValue(z);
                        }
                    }
                    qryFields += ")";
                    qryValues += ")";

                    return qryIns + qryFields + qryValues;
                }
                public string getInsSQLNonIDReturn()
                {
                    err.fire(this.IsValid != true,this._errorMsg);

                    string qryIns = "insert into " + this._tableName;
                    string qryFields = string.Empty;
                    string qryValues = string.Empty;

                    foreach (EntityItemComp ei in this.itemsColl)
                    {
                        if (!ei.IsForINS || !isValueChg(ei) || ei.IsIdentity) continue;

                            if (qryFields == string.Empty)
                            {
                                qryFields = "(" + ei.FieldName;
                                qryValues = " values (" + formatValue(ei,true);
                            }
                            else 
                            {
                                qryFields = qryFields + ", " + ei.FieldName;
                                qryValues = qryValues + ", " + formatValue(ei, true);
                            }
                    }
                    //qryFields = qryFields + ", created_by)";
                    //qryValues = qryValues + ", '" + _sys.Instance().user.LoginName.ToUpper() + "')";
                    qryFields += ")";
                    qryValues += ")";


                    return qryIns + qryFields + qryValues;
                }
                /// <summary>
                /// Get the Insert stament without check if the property changes or not.
                /// </summary>
                /// <returns></returns>
                public string getFULLInsSQLNonIDReturn()
                {
                    err.fire(this.IsValid != true,this._errorMsg);

                    string qryIns = "insert into " + this._tableName;
                    string qryFields = string.Empty;
                    string qryValues = string.Empty;

                    foreach (EntityItemComp ei in this.itemsColl)
                    {
                        if (!ei.IsForINS) continue;
                        if (ei.IsIdentity) continue;

                        if (qryFields == string.Empty)
                        {
                            qryFields = "(" + ei.FieldName;
                            qryValues = " values (" + formatValue(ei, true);
                        }
                        else
                        {
                            qryFields = qryFields + ", " + ei.FieldName;
                            qryValues = qryValues + ", " + formatValue(ei, true);
                        }
                    }
                    qryFields += ")";
                    qryValues += ")";

                    return qryIns + qryFields + qryValues;
                }

                public string getUpdSQL(System.Data.DataRow dataRow)
                {

                    string qryUPD = "update " + this._tableName;
                    string qryUPD2 = string.Empty;
                    string qryPKs = string.Empty;

                    int i = -1;
                    foreach (var x in dataRow.ItemArray)
                    {
                        i++;
                        var s = dataRow.Table.Columns[i].ColumnName;
                        var z = this.ItemColl.Find(zz => zz.FieldName == s);
                        if (z == null) continue;
                        z.FieldValue = x;
                        if (z.IsPK)
                        {
                            if (qryPKs == string.Empty)
                                qryPKs = " where " + z.FieldName + " = " + formatValue(z);
                            else
                                qryPKs = qryPKs + " and " + z.FieldName + " = " + formatValue(z);
                        }
                        else if (z.IsForUPD)
                        {
                            if (qryUPD2 == string.Empty)                                                            
                                qryUPD2 += " Set " + z.FieldName + " = " + formatValue(z);                            
                            else
                                qryUPD2 = qryUPD2 + ", " + z.FieldName + " = " + formatValue(z);
                        }
                    }

                    return qryUPD + qryUPD2 + qryPKs;
                }

                public string getUpdSQL()
                {
                    err.fire(this.IsValid != true,this._errorMsg);

                    string qryUPD = "update " + this._tableName;
                    string qryUPD2 = string.Empty;
                    string qryWhere = string.Empty;

                    foreach (EntityItemComp ei in this.itemsColl)
                    {
                        if (ei.IsPK == true)
                        {
                            if (qryWhere == string.Empty)
                                qryWhere = " where " + ei.FieldName;// +" = " + formatValue(ei);
                            else
                                qryWhere = qryWhere + " and " + ei.FieldName;// +" = " + formatValue(ei);

                            qryWhere = qryWhere + defineOperator(ei) + formatValue(ei);

                        }
                        else if (ei.IsForUPD && isValueChg(ei))
                        {
                            if (qryUPD2 == string.Empty)
                                //qryUPD2 = " set last_mod_by = '" + _sys.Instance().user.LoginName.ToUpper() + "', " + ei.FieldName + " = " + formatValue(ei);
                                qryUPD2 = " set " + ei.FieldName + " = " + formatValue(ei, true);
                            else
                                qryUPD2 = qryUPD2 + ", " + ei.FieldName + " = " + formatValue(ei, true);
                        }
                    }
                    
                        err.fire(qryWhere == string.Empty,"Missing PKs");
                        err.fire(qryUPD2 == string.Empty,"No Data To Update");
                    

                    return qryUPD + qryUPD2 + qryWhere;
                }

                public string getDelSQL(System.Data.DataRow dataRow)
                {
                   err.fire(this.IsValid != true,this._errorMsg);

                    string qryDel = "delete from " + this._tableName;
                    string qryDel2 = string.Empty;

                    int i = -1;
                    foreach (var x in dataRow.ItemArray)
                    {
                        i++;
                        var s = dataRow.Table.Columns[i].ColumnName;
                        var z = this.ItemColl.Find(zz => zz.FieldName == s);
                        if (z == null) continue;
                        z.FieldValue = x;
                        if (z.IsPK)
                        {
                            if (qryDel2 == string.Empty)
                                qryDel2 = " where " + z.FieldName + " = " + formatValue(z);
                            else
                                qryDel2 +=  " and " + z.FieldName + " = " + formatValue(z);
                        }
                    }
                  
                    err.fire(qryDel2 == string.Empty,"No PKs found in the Delete Operation");
  

                    return qryDel + qryDel2;
                }

        public string getDelSQL()
        {

            err.fire(this.IsValid != true, this._errorMsg);

            string qryDel = "delete from " + this._tableName;
            string qryWhere = string.Empty;

            foreach (EntityItemComp ei in this.itemsColl)
            {
                //if (ei.IsPK )
                if (isValueChg(ei) == true)
                {
                    if (ei.FieldName == "created_by") continue;
                    if (ei.FieldName == "creation_date") continue;

                    if (qryWhere == string.Empty)
                        qryWhere = " where " + ei.FieldName; // +" = " + formatValue(ei);
                    else
                        qryWhere = qryWhere + " and " + ei.FieldName; // +" = " + formatValue(ei);

                    qryWhere = qryWhere + defineOperator(ei) + formatValue(ei);
                }
            }
            err.fire(qryWhere == string.Empty, "No conditions found in the Delete Operation");

            return qryDel + qryWhere;
        }

                public string getDelSQLWithPK()
                {
                    err.fire(this.IsValid != true,this._errorMsg);

                    string qryDel = "delete from " + this._tableName;
                    string qryWhere = string.Empty;


                    foreach (EntityItemComp ei in this.itemsColl)
                    {
                        if (ei.IsPK)
                        {
                            if (qryWhere == string.Empty)
                                qryWhere = " where " + ei.FieldName; // + " = " + formatValue(ei);
                            else
                                qryWhere = qryWhere + " and " + ei.FieldName; // +" = " + formatValue(ei);

                            qryWhere = qryWhere + defineOperator(ei) + formatValue(ei);
                        }
                    }

                    
                    err.fire(qryWhere == string.Empty,"No PKs found in the Delete Operation");
                    

                    return qryDel + qryWhere;
                }

                public string getInsSqlWithIDentityReturn()
                {
                    err.fire(this.IsValid != true,this._errorMsg);
                    string qryIns = getInsSQLNonIDReturn() + ";\n";
                    string qrySel = "select * from " + this._tableName + " where " + getPrimaryKey() + " = SCOPE_IDENTITY();\n";
                    return "begin \n" + qryIns + qrySel + "end;";                                           
                }
                public string getFULLInsSqlWithIDentityReturn()
                {
                    err.fire(this.IsValid != true,this._errorMsg);

                    string qryIns = getFULLInsSQLNonIDReturn() + ";\n";
                    string qrySel = "select * from " + this._tableName + " where " + getPrimaryKey() + " = SCOPE_IDENTITY();\n";
                    return "begin \n" + qryIns + qrySel + "end;";
                }

                public string getPrimaryKey()
                {
                    string retVal = string.Empty;
                    foreach (EntityItemComp ei in this.itemsColl)
                    {
                        if (ei.IsPK)  retVal = ei.FieldName;
                    }

                    return retVal;
                }

                public string defineOperator(EntityItemComp ei)
                {
                    string evalue = ei.FieldValue.ToString();
                    //if (evalue.Contains("'")) evalue = evalue.Replace("'","`");

                    if (evalue.Trim() == "") return " = ";
                    else if //(evalue.Trim().Substring(0, 1) == "'"                        
                          ( evalue.Trim().Substring(0, 1) == ";"
                            || evalue.Contains("--")
                            || evalue.Contains("*/")
                            || evalue.Contains("/*")
                          )
                    {
                        err.fire(true, "service not available"); return null;
                    }
                    else if (evalue.Contains("%") == true)
                    {
                        return " like ";
                    }
                    //Begin Added by Fmaestre 10-Apr-09
                    else if (
                     evalue.Trim().ToUpper().PadRight(7, 'x').Substring(0, 7) == "BETWEEN" ||
                     evalue.Trim().Substring(0, 1) == ">" ||
                     evalue.Trim().Substring(0, 1) == "<" ||
                     evalue.Trim().ToUpper().PadRight(6, 'x').Substring(0, 6) == "NOT IN" ||
                     evalue.Trim().ToUpper().PadRight(3, 'x').Substring(0, 3) == "IN(" || ////Modified by fmaestre 1-Aug-09, se agrego ( al IN.
                     evalue.Trim().ToUpper().PadRight(4, 'x').Substring(0, 4) == "IN (" //Added by fmaestre 1-Aug-09
                    )
                    {
                        return " "; 
                    }
                    //End Added by Fmaestre 10-Apr-09
                    else
                    {
                        return " = ";                        
                    }  
                }

        public string getSQLSearch()
        {
            err.fire(this.IsValid != true, this._errorMsg);

            //Begin Added by Fmaestre 1-Aug-09
            //Todos los campos se agregaran al query y se remueve el *, esto permitira flexibilidad al crear los queries
            string selectFields = string.Empty;
            foreach (EntityItemComp ei in this.itemsColl)
            {
                if (ei.IsOutSelect) continue; //la columna no se quiere en el select.

                if (selectFields != string.Empty) selectFields += ",";
                selectFields += ei.FieldName;
            }
            //END Added by Fmaestre 1-Aug-09

            string qrySel = "select " + selectFields + " from " + this._tableName + " [xtraTable] "; //Added by Fmaestre 1-Aug-09
            string qryWhere = string.Empty;
            string orderby = ""; //" order by last_mod_date"; Comentado para no impactar queries.

            foreach (EntityItemComp ei in this.itemsColl)
            {
                if (isValueChg(ei))
                {
                    if (ei.FieldName == "created_by") continue;
                    if (ei.FieldName == "creation_date") continue;

                    //process the hack 
                    if (ei.FieldValue != null
                                && ei.FieldDataType == enmDataTypes.stringType
                                && ei.FieldValue.ToString() != string.Empty
                                && ei.FieldValue.ToString().Substring(0, 1) == "@"
                                )
                    {
                        if (qryWhere == string.Empty) qryWhere += " where ";
                        else qryWhere += " and ";
                        qryWhere += ei.FieldValue.ToString().Substring(1);
                    }
                    else
                    {

                        if (qryWhere == string.Empty)
                            qryWhere = " where " + ei.FieldName;
                        else
                            qryWhere = qryWhere + " and " + ei.FieldName;

                        if (ei.FieldValue == null) continue; //new 21 nov

                        qryWhere = qryWhere + defineOperator(ei) + formatValue(ei);
                    }
                }

            }
            return qrySel + qryWhere + orderby;
        }

                public string getSQLSearch(bool withNoLock)
                {
                    string x = getSQLSearch();
                    if (withNoLock)
                       x = x.Replace("[xtraTable] ", "[xtraTable] with(nolock) ");

                    return x;
                }
            
            #endregion

        #endregion

        #region Class Private Interface

            private void intializeMyObjects()
            {
                itemsColl = new List<EntityItemComp>();
                propChanges = new ArrayList();
            }

            private string formatValue(EntityItemComp prmEIComp, bool isUpd  = false)
            {
                try
                {
                    string retVal = string.Empty;
                    switch (prmEIComp.FieldDataType)
                    {
                        case enmDataTypes.stringType:
                            if (prmEIComp.FieldValue != null)
                            {
                                if (defineOperator(prmEIComp) != " " || isUpd)
                                    retVal = "N'" + prmEIComp.FieldValue.ToString().Replace("'","`") + "'"; //for nvarchar and unicode
                                else
                                    retVal = prmEIComp.FieldValue.ToString();
                            }
                            else
                                retVal = "NULL";
                            break;
                        case enmDataTypes.intType:
                            if (prmEIComp.FieldValue != null)
                                retVal = prmEIComp.FieldValue.ToString();
                            else
                                retVal = "NULL";
                            break;

                        case enmDataTypes.floatType:
                            if (prmEIComp.FieldValue != null)
                                retVal = prmEIComp.FieldValue.ToString();
                            else
                                retVal = "NULL";
                            break;

                        case enmDataTypes.doubleType:
                            if (prmEIComp.FieldValue != null)
                                retVal = prmEIComp.FieldValue.ToString();
                            else
                                retVal = "NULL";
                            break;

                        case enmDataTypes.boolType:
                            if (prmEIComp.FieldValue == DBNull.Value)
                            {
                                retVal = "0";
                                break;
                            }

                            bool fieldValue = bool.Parse(prmEIComp.FieldValue.ToString());
                            if (fieldValue) retVal = "1";
                            else retVal = "0";

                            break;

                        case enmDataTypes.DateTimeType:
                            // Falta formetar el string a Fecha ! ==> Ulises
                            //retVal =  prmEIComp.FieldValue.ToString() ;
                            /*Fmaestre 6 de Abril 09, Anadi las comillas, funciona.*/
                            if (prmEIComp.FieldValue == DBNull.Value)
                            {
                                retVal = "NULL";
                                break;
                            }

                            if (prmEIComp.FieldValue != null)
                                try
                                {
                                    retVal = "'" + Utils.formatDate(DateTime.Parse(prmEIComp.FieldValue.ToString())) + "'";
                                }
                                catch
                                {
                                    retVal = prmEIComp.FieldValue.ToString();
                                }

                            else
                                retVal = "NULL";
                            break;

                        default:
                            err.fire(true, "EntityComp.formatValue. Error : Invalid Data Type Conversion : " + prmEIComp.FieldName.ToString());
                            return "";

                    }
                    return retVal;
                }
                catch (Exception errx)
                {
                    err.fire(true,"EntityComp.formatValue. \n Error : Invalid Data Type Conversion : " + prmEIComp.FieldName.ToString() + " \n " + errx.Message);
                    return "";
                }
            }

            private bool isValueChg(EntityItemComp prmEIComp)
            {

                string propertyName = prmEIComp.PropertyName;
                bool retVal = false;
                foreach (object obj in propChanges)
                {
                    if (propertyName.ToUpper() == obj.ToString().ToUpper())
                    {
                        retVal = true;
                        break;
                    }
                }

                return retVal;
            }

        #endregion
    }
}
