using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace Herradura.Lib.core
{
    public static class handler
    {

     public static string getError(Exception ex)
        {
            return ex.Message;
        }
     public static void require(bool predicate, string error)
     {
         if (predicate) throw new Exception(error);
     }
     public static void require(bool predicate, string error, string extra)
     {
         if (predicate) throw new Exception(error + ":" + extra);
     }
     public static object require_parse(TypeCode tc, object value, string error)
     {
         object o = null;
         switch (tc)
         {
             case TypeCode.Int32:
                 int x;
                 require(!int.TryParse(value.ToString(), out x), error);
                 o = x;
                 break;
             case TypeCode.Double:
                 double y;
                 require(!Double.TryParse(value.ToString(), out y), error);
                 o = y;
                 break;
             case TypeCode.DateTime:
                 DateTime z;
                 require(!DateTime.TryParse(value.ToString(), out z), error);
                 o = z;
                 break;

             default:
                 break;
         }
         return o;
     }
        

      #region General Methods
        public static bool isNumeric(string myNumber)
            {
                bool IsNum = false;
                for (int index = 0; index < myNumber.Length; index++)
                {
                    IsNum = true;
                    if (!Char.IsNumber(myNumber[index]))
                    {
                        IsNum = false;
                        break;
                    }
                }
                return IsNum;
            }
        public static object getDefaultIfNull(string obj, TypeCode typeCode)
            {
                //If object is dbnull then return the default for that type.
                //Otherwise just return the orginal value.
                object obj2 = obj;
                if (obj.Length == 0)//(obj == "")
                {
                    switch (typeCode)
                    {
                        case TypeCode.Int32: obj2 = 0; break;
                        case TypeCode.Double: obj2 = 0; break;
                        case TypeCode.String: obj2 = string.Empty; break;
                        case TypeCode.Boolean: obj2 = false; break;
                        case TypeCode.DateTime: obj2 = new DateTime(); break;
                        case TypeCode.Int64: obj2 = 0; break;
                        default: break;
                    }
                }
                return obj2;
            }
      #endregion

      #region DAL Methods
        /// <summary>
            /// Checks if an object coming back from the database is dbnull.  If it is this returns the default
            /// value for that type of object.
            /// </summary>
            /// <param name="obj">Object to check for null.</param>
            /// <param name="typeCode">Type of object, used to determine what the default value is.</param>
            /// <returns>Either the object passed in or the default value.</returns>
        public static object getDefaultIfDBNull(object obj, TypeCode typeCode)
            {
                //If object is dbnull then return the default for that type.
                //Otherwise just return the orginal value.
                if (obj == DBNull.Value)
                {
                    switch (typeCode)
                    {
                        case TypeCode.Int32: obj = 0; break;
                        case TypeCode.Double: obj = 0; break;
                        case TypeCode.String: obj = ""; break;
                        case TypeCode.Boolean: obj = false; break;
                        case TypeCode.DateTime: obj = new DateTime(); break;
                        case TypeCode.Int64: obj = 0; break;
                        default: break;
                    }
                }
                return obj;
            }
        /// <summary>
            /// Evaluate the parameters list and assign DBNull as apropiated
            /// </summary>
            /// <param name="parameters"></param>
            /// <param name="evaluateNumeric"></param>
        public static void evaluateParameters(IDataParameterCollection parameters, bool evaluateNumeric)
            { 

                foreach (IDataParameter Parameter in parameters)
                {

                    if (Parameter.Value == null || Convert.ToString(Parameter.Value).Length == 0) //== "")
                    {
                        Parameter.Value = DBNull.Value;
                    }else if( Parameter.Value.GetType().Name == "DateTime" )
                    {
                        if (Convert.ToDateTime(Parameter.Value) == DateTime.MinValue)
                            Parameter.Value = DBNull.Value;
                    }
                    else
                    {
                        if (evaluateNumeric && (string.Equals(Convert.ToString(Parameter.Value), "0") ||
                                                string.Equals(Convert.ToString(Parameter.Value), "0.0")))
                        {
                            Parameter.Value = DBNull.Value;
                        }
                    }
                }
            }

        public static  string form_query(string qry)
            {
                return form_query(new StringBuilder(qry));
            }
        public static string form_query(StringBuilder qry)
            {
                var q = new StringBuilder("begin try\n");
                q.Append(qry);
                q.Append("\nend try\n");
                q.Append("begin catch\n");
                q.Append("declare @e varchar(256) = ERROR_MESSAGE()\n");
                q.Append("RAISERROR (@e,16,1)\n");
                q.Append("end catch");
                return q.ToString();
            }

        public static Object setDBNull(String str)
            {
                return (str == null || str.Equals(String.Empty)) ? (Object)DBNull.Value : str;
            }
        
        public static bool isvalidDate(DateTime d)
            {
                return d != DateTime.MinValue;
            }
      #endregion



        
    
    }
}
