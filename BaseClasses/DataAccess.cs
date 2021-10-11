using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using Herradura.Lib.Components;
using Herradura.Lib.core;


namespace Herradura.Lib.core
{
    public abstract class DataAccess
    {
        private string _connectionString;
        private string _connectionStringFile;
        private bool _enableCaching;
        private int _cacheDuration;

        public DataAccess(string conexion)
        {
            //this.ConnectionStringInFile = Globals.Instance(_sys.Instance().DBConnection).ConnString;
            this.ConnectionString = Globals.Instance(conexion).ConnString;
        }
        


        protected string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        protected bool EnableCaching
        {
            get { return _enableCaching; }
            set { _enableCaching = value; }
        }

        protected int CacheDuration
        {
            get { return _cacheDuration; }
            set { _cacheDuration = value; }
        }

        //#endregion

        //#region Public Methods


        /// <summary>
        /// Creates an Entity Comp using Reflection based on the ICatalog Object.
        /// </summary>
        /// <param name="prmICatalog">EntityComp object</param>
        /// <returns></returns>
        public virtual EntityComp getEntityComp(ICatalog prmICatalog)
        {
            EntityComp entityComp = new EntityComp();
            try
            {
                //0.- Get the Type of the given parametro
                Type type = prmICatalog.GetType();

                //1.- Get TableAttribute and initialize the entityComp.
                entityComp.setTableAttributes(loadEntityInfo(type));

                //2.- Obtain the Object Properties
                PropertyInfo[] props = type.GetProperties();

                //3.- For each property obtain the attributes of type FieldAttribute and create 
                //    an EntityItempComp.

                foreach (PropertyInfo pInfo in props)
                {
                    EntityItemComp eItemComp = getEntityItem(pInfo, prmICatalog);
                    if (eItemComp != null)
                        entityComp.ItemColl.Add(eItemComp);
                }

                entityComp.PropertyChanges = prmICatalog.getPropertyChanges();

                return entityComp;
            }
            catch (Exception err)
            {
                entityComp.ErrorMsg = err.ToString();
                return entityComp;
            }
        }




        public virtual string getFindQryByPks(ICatalog prmICatalog)
        {

            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getFindQryByPks();

        }

        public virtual string getInsSQLNonIDReturn(ICatalog prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getInsSQLNonIDReturn();

        }
        /// <summary>
        /// Obtiene un full insert sin importar sin los valores cambiaron o no.
        /// </summary>
        /// <param name="prmICatalog"></param>
        /// <returns></returns>
        public virtual string getFULLInsSQLNonIDReturn(ICatalog prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getFULLInsSQLNonIDReturn();

        }

        public virtual string getInsSQLIdentityReturn(ICatalog prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getInsSqlWithIDentityReturn();

        }

        /// <summary>
        /// Obtiene un full insert sin importar sin los valores cambiaron o no.
        /// </summary>
        /// <param name="prmICatalog"></param>
        /// <returns></returns>
        public virtual string getFULLInsSQLIdentityReturn(ICatalog prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getFULLInsSqlWithIDentityReturn();


        }

        public virtual string getUpdSQL(ICatalog prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getUpdSQL();

        }
        public string getAddSQL(ICatalog prmICatalog, DataRow dataRow)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getInsSQLNonIDReturn(dataRow);


        }


        public string getUpdSQL(ICatalog prmICatalog, DataRow dataRow)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getUpdSQL(dataRow);


        }

        public virtual string getDelSQL(ICatalog prmICatalog, DataRow dataRow)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getDelSQL(dataRow);

        }

        public virtual string getDelSQL(ICatalog prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getDelSQL();

        }
        public virtual string getDelSQLWithPK(ICatalog prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getDelSQLWithPK();


        }

        public virtual string getSqlSearch(ICatalog prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getSQLSearch();


        }

        public virtual string getSqlSearch<T>(T prmICatalog)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getSQLSearch();


        }

        public virtual string getSqlSearch(ICatalog prmICatalog, bool withnolock)
        {
            EntityComp ent = getEntityComp(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getSQLSearch(withnolock);


        }

        public virtual ICatalog fill_object(ICatalog prmICatalog, IDataReader prmReader)
        {
            try
            {
                ICatalog obj = (ICatalog)Activator.CreateInstance(prmICatalog.GetType());
                fillObject(obj, prmReader);

                return obj;
            }
            catch (Exception errx)
            {
                err.fire(true, "fill_object:- " + errx.Message); return null;
            }
        }

        /// <summary>
        /// Fill the given object "prmICatalog" with the given reader "prmReader"
        /// </summary>
        /// <param name="prmICatalog"></param>
        /// <param name="prmReader"></param>
        //public virtual void fillObject(ICatalog prmICatalog, IDataReader prmReader)
        //{
        //    try
        //    {
        //        bool fldFound = false;

        //        Type type = prmICatalog.GetType();
        //        PropertyInfo[] props = type.GetProperties();

        //        prmICatalog.markAsUnSaved();

        //        for (int i = 0; i < prmReader.FieldCount; i++)
        //        {
        //            fldFound = false;
        //            // Por cada propiedad obtener sus atributos ( Esto despues de se puede refactorizar con LINQ)
        //            foreach (PropertyInfo p in props)
        //            {
        //                if (fldFound == true)
        //                    break;

        //                // Obtener los atributos de tipo Field Attribute y buscar si el field name es el mismo al del reader
        //                // en la posicion actual.  (Esto despues se puede refactorizar con LINQ
        //                object[] cusAtt = p.GetCustomAttributes(typeof(FieldAttribute), false);

        //                // if the current PropertyInfo has attributes of type FieldAttribute then continue with checking if the 
        //                // if the field name is the same as the current field in the reader.
        //                if (cusAtt.Length > 0)
        //                {
        //                    foreach (FieldAttribute fld in cusAtt)
        //                    {
        //                        try
        //                        {
        //                            //if (fld.FieldName == prmReader.GetName(i)) //Commented by fmaestre 1-aug-09
        //                            if (fld.FieldName == prmReader.GetName(i) || fld.FieldName2 == prmReader.GetName(i)) //Added by fmaestre 1-aug-09
        //                            {
        //                                // Actualizar propiedad !
        //                                if (prmReader[i] != System.DBNull.Value)
        //                                {
        //                                    p.SetValue(prmICatalog, prmReader[i], null);
        //                                    fldFound = true;
        //                                }
        //                            }
        //                        }
        //                        catch (Exception errc)
        //                        {
        //                            err.require(true,"Field Name:" + fld.FieldName + errc.Message);
        //                        }
        //                        if (fldFound == true)
        //                            break;
        //                    }
        //                }

        //            }
        //        }
        //        prmICatalog.markAsSaved();
        //    }
        //    catch (Exception errx)
        //    {
        //        err.require(true,"fillObject: -" +  errx.Message);
        //    }
        //}


        /// <summary>
        /// Fill the given object "prmICatalog" with the given reader "prmReader"
        /// </summary>
        /// <param name="prmICatalog"></param>
        /// <param name="prmReader"></param>
        public virtual void fillObject(ICatalog prmICatalog, IDataReader prmReader)
        {
            string fldname = "";
            try
            {
                bool fldFound = false;

                Type type = prmICatalog.GetType();
                PropertyInfo[] props = type.GetProperties();

                prmICatalog.markAsUnSaved();

                for (int i = 0; i < prmReader.FieldCount; i++)
                {
                    fldFound = false;

                    // Por cada propiedad obtener sus atributos ( Esto despues de se puede refactorizar con LINQ)
                    foreach (PropertyInfo p in props)
                    {
                        if (fldFound == true) break; //pendiente

                        // Obtener los atributos de tipo Field Attribute y buscar si el field name es el mismo al del reader
                        // en la posicion actual.  (Esto despues se puede refactorizar con LINQ
                        object[] cusAtt = p.GetCustomAttributes(typeof(FieldAttribute), false);

                        // if the current PropertyInfo has attributes of type FieldAttribute then continue with checking if the 
                        // if the field name is the same as the current field in the reader.
                        if (cusAtt.Length <= 0) continue;

                        foreach (FieldAttribute fld in cusAtt)
                        {
                            if (prmReader.GetName(i) == string.Empty) continue; //filtro para los atributos no asociados a tabla
                            if (fld.IsOutSelect) continue;

                            if (fld.FieldName == prmReader.GetName(i) || fld.FieldName2 == prmReader.GetName(i))//Added by fmaestre 1-aug-09
                            {
                                if (prmReader[i] != System.DBNull.Value)
                                {
                                    fldname = fld.FieldName;
                                    if (prmReader[i].GetType().Name == "Decimal")
                                    {
                                        p.SetValue(prmICatalog, Convert.ToDouble(prmReader[i]), null);
                                    }
                                    else p.SetValue(prmICatalog, prmReader[i], null);
                                }

                                fldFound = true;
                                break;
                            }
                        }


                    }
                }
                prmICatalog.markAsSaved();
            }
            catch (Exception errx)
            {
                err.fire(true, "fillObject: -Field Name:" + fldname + " " + errx.Message);
            }
        }



        //#endregion

        //#region Protected Methods


        //#endregion

        //#endregion

        //#region Private Interface

        private TableAttribute loadEntityInfo(Type prmType)
        {
            TableAttribute tblAttribute = new TableAttribute();
            object[] custAtt = prmType.GetCustomAttributes(typeof(TableAttribute), false);
            err.fire(custAtt == null, "Object does not contain the TableAttribute");

            foreach (TableAttribute tblAtt in custAtt)
            {
                tblAttribute.TableName = tblAtt.TableName;
                tblAttribute.StoredProc = tblAtt.StoredProc;
                break;
            }

            //Evaluate if there are correct values
            err.fire(tblAttribute.TableName == String.Empty, "Invalid Table Name in TableAttributes");

            return tblAttribute;

        }

        private FieldAttribute loadEntityItemInfo(PropertyInfo prmPropertyInfo, object prmReflectedObj)
        {
            FieldAttribute fldAttribute = new FieldAttribute();

            object[] cusAtt = prmPropertyInfo.GetCustomAttributes(typeof(FieldAttribute), false);

            if (cusAtt.Length == 0)
                return null;


            foreach (FieldAttribute fld in cusAtt)
            {
                fldAttribute.FieldName = fld.FieldName;
                fldAttribute.IsForINS = fld.IsForINS;
                fldAttribute.IsForUPD = fld.IsForUPD;
                fldAttribute.IsKey = fld.IsKey;
                fldAttribute.PropertyName = fld.PropertyName;
                fldAttribute.FieldValue = prmPropertyInfo.GetValue(prmReflectedObj, null);
                //prmPropertyInfo.GetValue(prmPropertyInfo.ReflectedType, null);//
                break;
            }

            err.fire(fldAttribute.FieldName == string.Empty, "Invalid Attribute Name FieldAttribute");

            return fldAttribute;
        }

        private EntityItemComp getEntityItem(PropertyInfo prmPropertyInfo, object prmReflectedObj)
        {
            EntityItemComp ec = null;
            object[] custAtt = null;

            custAtt = prmPropertyInfo.GetCustomAttributes(typeof(FieldAttribute), false);
            if (custAtt.Length > 0)
            {
                foreach (FieldAttribute f in custAtt)
                {
                    ec = new EntityItemComp();
                    ec.setFieldAttributes(f);
                    ec.FieldValue = prmPropertyInfo.GetValue(prmReflectedObj, null);
                    break;
                }
            }

            return ec;
        }

        //#endregion


        //#region frank Experiments


        //public static List<PropertyInfo> getProperties(ICatalog prmICatalog)
        //{

        //    Type type = prmICatalog.GetType();
        //    PropertyInfo[] props = type.GetProperties();
        //    List<PropertyInfo> lst = new List<PropertyInfo>();
        //    foreach (var x in props)
        //    {
        //        lst.Add(x);
        //    }
        //    return lst;
        //}



        /// <summary>
        /// Method to populate a list with all the class
        /// in the namespace provided by the user
        /// </summary>
        /// <param name="nameSpace">The namespace the user wants searched</param>
        /// <returns></returns>
        public static List<string> GetAllClasses(string nameSpace)
        {
            //create an Assembly and use its GetExecutingAssembly Method
            //http://msdn2.microsoft.com/en-us/library/system.reflection.assembly.getexecutingassembly.aspx
            Assembly asm = Assembly.GetExecutingAssembly();
            //create a list for the namespaces
            List<string> namespaceList = new List<string>();
            //create a list that will hold all the classes
            //the suplied namespace is executing
            List<string> returnList = new List<string>();
            //loop through all the "Types" in the Assembly using
            //the GetType method:
            //http://msdn2.microsoft.com/en-us/library/system.reflection.assembly.gettypes.aspx
            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == nameSpace && !type.IsAbstract)
                    namespaceList.Add(type.Name);
            }
            //now loop through all the classes returned above and add
            //them to our classesName list
            foreach (String className in namespaceList)
                returnList.Add(className);
            //return the list
            return returnList;
        }

        //#endregion


        public virtual string getSqlSearch<T>(T prmICatalog, bool withnolock)
        {
            EntityComp ent = getEntityComp<T>(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getSQLSearch(withnolock);


        }

        public virtual string getFindQryByPks<T>(T prmICatalog)
        {

            EntityComp ent = getEntityComp<T>(prmICatalog);
            err.fire(!ent.IsValid, ent.ErrorMsg);

            return ent.getFindQryByPks();

        }

        public virtual EntityComp getEntityComp<T>(T prmICatalog)
        {
            EntityComp entityComp = new EntityComp();
            try
            {
                //0.- Get the Type of the given parametro
                Type type = prmICatalog.GetType();

                //1.- Get TableAttribute and initialize the entityComp.
                entityComp.setTableAttributes(loadEntityInfo(type));

                //2.- Obtain the Object Properties
                PropertyInfo[] props = type.GetProperties();

                //3.- For each property obtain the attributes of type FieldAttribute and create 
                //    an EntityItempComp.

                foreach (PropertyInfo pInfo in props)
                {
                    EntityItemComp eItemComp = getEntityItem(pInfo, prmICatalog);
                    if (eItemComp != null)
                        entityComp.ItemColl.Add(eItemComp);
                }

                entityComp.PropertyChanges = ((ICatalog)prmICatalog).getPropertyChanges();

                return entityComp;
            }
            catch (Exception err)
            {
                entityComp.ErrorMsg = err.ToString();
                return entityComp;
            }
        }

        public virtual T fill_object<T>(T prmICatalog, IDataReader prmReader)
        {
                T obj = (T)Activator.CreateInstance(prmICatalog.GetType());
                fillObject(obj, prmReader);
                return obj;
        }
        public virtual void fillObject<T>(T prmICatalog, IDataReader prmReader)
        {
            //string fldname = "";
            string iName = "";
            Type iType;
            Type ft = typeof(FieldAttribute);
            List<int> il = new List<int>();
            int ppos;
            try
            {
                bool fldFound = false;

                Type type = prmICatalog.GetType();
                PropertyInfo[] props = type.GetProperties();
                //((ICatalog)prmICatalog).markAsUnSaved();
                for (int i = 0; i < prmReader.FieldCount; i++)
                {
                    fldFound = false;
                    iName = prmReader.GetName(i);
                    if (iName == string.Empty) continue; //filtro para los atributos no asociados a tabla
                            
                    iType = prmReader[i].GetType();
                    // Por cada propiedad obtener sus atributos ( Esto despues de se puede refactorizar con LINQ)
                    ppos = -1;
                    foreach (PropertyInfo p in props)
                    {                                              
                        ppos++;  
                        if (il.Exists(ix => ix == ppos)) continue;

                        // Obtener los atributos de tipo Field Attribute y buscar si el field name es el mismo al del reader
                        // en la posicion actual.  (Esto despues se puede refactorizar con LINQ
                        object[] cusAtt = p.GetCustomAttributes(ft, false);

                        // if the current PropertyInfo has attributes of type FieldAttribute then continue with checking if the 
                        // if the field name is the same as the current field in the reader.
                        if (cusAtt.Length == 0) continue;

                        foreach (FieldAttribute fld in cusAtt)
                        {
                            if (fld.IsOutSelect) continue;

                            if (fld.FieldName == iName || fld.FieldName2 == iName)//Added by fmaestre 1-aug-09
                            {
                                if (prmReader[i] != System.DBNull.Value)
                                {
                                    //fldname = fld.FieldName;
                                    //if (iType.Name == "Decimal")
                                    //    p.SetValue(prmICatalog, Convert.ToDouble(prmReader[i]), null);
                                    //else p.SetValue(prmICatalog, prmReader[i], null);
                                    var targetType = IsNullableType(p.PropertyType) ? Nullable.GetUnderlyingType(p.PropertyType) : p.PropertyType;
                                    p.SetValue(prmICatalog, Convert.ChangeType(prmReader[i], targetType), null);
                                }
                                il.Add(ppos);
                                fldFound = true;
                                break;
                            }
                      }
                        if (fldFound == true) break;
                    }
                }
                ((ICatalog)prmICatalog).markAsSaved();
            }
            catch (Exception errx)
            {
                err.fire(true, "fillObject: -Field Name:" + iName + " " + errx.Message);
            }
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

    }
}
