using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Herradura.Lib.core;
using Herradura.Lib.Components;
using System.Collections;

namespace Herradura.Lib.Portal.DAL
{
    public class GenericDALSQLProvider : DataAccess
    {
        public GenericDALSQLProvider(string conn)
            : base(conn)
        {

        }

        #region Generic CRUD Implementation



        private object getPropertyValue(ICatalog prmICatalog, string p)
        {
            Type type = prmICatalog.GetType();
            System.Reflection.PropertyInfo pi = type.GetProperty(p);
            return pi.GetValue(prmICatalog, null);
            
        }
        
        public ICatalog insertItem(ICatalog prmICatalog)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Insert), "denied to insert on", prmICatalog.GetType().Name);
            err.fire(existItemPK(prmICatalog), getPropertyValue(prmICatalog, "Error").ToString());
            var insSql = base.getInsSQLNonIDReturn(prmICatalog);
            var response = 0;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(insSql) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                response = sqlCmd.ExecuteNonQuery();
            }

            err.fire(response == 0, "registro_no_agregado");

            return getItem(prmICatalog);

        }
        public ICatalog insertFULLItem(ICatalog prmICatalog)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Insert), "denied to insert on", prmICatalog.GetType().Name);
            err.fire(existItemPK(prmICatalog), getPropertyValue(prmICatalog, "Error").ToString());
            var insSql = base.getFULLInsSQLNonIDReturn(prmICatalog);
            var response = 0;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(insSql) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                response = sqlCmd.ExecuteNonQuery();
            }


            err.fire(response == 0, "registro_no_agregado");

            return getItem(prmICatalog);


        }

        public enum enumSqlAction
        {
            Select = 0,
            Insert = 1,
            Update = 2,
            Delete = 3
        }

        public bool isSecure(ICatalog prmICatalog, enumSqlAction Action)
        {
            string cn = prmICatalog.GetType().Name;
            object objUser = getPropertyValue(prmICatalog, "CurrentUser");
            if (objUser == null) return true; //BL created the object
            string curentUser = objUser.ToString();

            vwRolePermissionsComp x = new vwRolePermissionsComp { Obj_id = cn, Role = curentUser };
            var qry = base.getSqlSearch(x);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(qry);
                sqlCmd.CommandType = CommandType.Text;
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                var reader = sqlCmd.ExecuteReader();

                if (!reader.HasRows) return false;

                reader.Read();
                base.fillObject(x, reader);

                
                switch (Action)
                {
                    case enumSqlAction.Select:
                        return x.Sel;
                    case enumSqlAction.Insert:
                        return x.Ins;
                    case enumSqlAction.Update:
                        return x.Upd;
                    case enumSqlAction.Delete:
                        return x.Del;
                    default:
                        break;
                }
            }
            return false;
        }

        public ICatalog insertItemGetIdentity(ICatalog prmICatalog)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Insert), "denied to insert on", prmICatalog.GetType().Name);
            //err.fire(existItemPK(prmICatalog), getPropertyValue(prmICatalog, "Error").ToString());                
            var qry = base.getInsSQLIdentityReturn(prmICatalog);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                var sqlCmd = new SqlCommand(qry, sqlConn) { CommandType = CommandType.Text };

                var reader = sqlCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    base.fillObject(prmICatalog, reader);
                }
            }
            return prmICatalog;
        }
        public ICatalog insertFULLItemGetIdentity(ICatalog prmICatalog)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Insert), "denied to insert on", prmICatalog.GetType().Name);
            err.fire(existItemPK(prmICatalog), getPropertyValue(prmICatalog, "Error").ToString());
            var qry = base.getFULLInsSQLIdentityReturn(prmICatalog);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                sqlConn.Open();
                var sqlCmd = new SqlCommand(qry, sqlConn) { CommandType = CommandType.Text };

                var reader = sqlCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    base.fillObject(prmICatalog, reader);
                }
            }
            return prmICatalog;
        }

        public bool DeleteItem(ICatalog prmICatalog)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Delete), "denied to delete on", prmICatalog.GetType().Name);
            var delSql = base.getDelSQL(prmICatalog);
            var response = 0;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(delSql) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                try
                {
                    response = sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    int i = ex.Message.IndexOf('"');
                    int j = ex.Message.IndexOf('"', i + 1);

                    err.fire(ex.Message.Contains("REFERENCE"), "No se puede borrar registro. Ya esta en uso." + ex.Message.Substring(i, j - i + 1));
                    err.fire(true, ex.Message);
                }
            }

            err.fire(response == 0, "registro_no_eliminado");

            return true;

        }

        public bool DeleteItemWithPK(ICatalog prmICatalog)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Delete), "denied to delete on", prmICatalog.GetType().Name);
            var delSql = base.getDelSQLWithPK(prmICatalog);
            var response = 0;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(delSql) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                response = sqlCmd.ExecuteNonQuery();
            }

            err.fire(response == 0, "registro_no_eliminado");
            return true;
        }

        public ICatalog updateItem(ICatalog prmICatalog)
        {            
            err.fire(!isSecure(prmICatalog, enumSqlAction.Update), "denied to update on", prmICatalog.GetType().Name);
            var updSql = base.getUpdSQL(prmICatalog);
            var response = 0;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(updSql) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                response = sqlCmd.ExecuteNonQuery();
            }

            err.fire(response == 0, "registro_no_actualizado");


            return getItem(prmICatalog);

        }


        public bool updateItem(ICatalog prmICatalog, DataRow dataRow)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Update), "denied to update on", prmICatalog.GetType().Name);
            var updSql = base.getUpdSQL((ICatalog)Utils.GetClone(prmICatalog), dataRow);
            var response = 0;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(updSql) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                response = sqlCmd.ExecuteNonQuery();
            }

            err.fire(response == 0, "registro_no_actualizado");

            return true;

        }
        public bool addItem(ICatalog prmICatalog, DataRow dataRow)
        {
            var updSql = base.getAddSQL((ICatalog)Utils.GetClone(prmICatalog), dataRow);
            var response = 0;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(updSql) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                response = sqlCmd.ExecuteNonQuery();
            }

            err.fire(response == 0, "registro_no_agregado");

            return true;
        }

        public bool DeleteItem(ICatalog prmICatalog, DataRow dataRow)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Delete), "denied to delete on", prmICatalog.GetType().Name);
            var delSql = base.getDelSQL((ICatalog)Utils.GetClone(prmICatalog), dataRow);
            var response = 0;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(delSql) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                response = sqlCmd.ExecuteNonQuery();
            }
            err.fire(response == 0, "registro_no_eliminado");
            return true;
        }
        /// <summary>
        /// Busca el ITEM por su PrimaryKey
        /// </summary>
        /// <param name="prmICatalog"></param>
        /// <returns></returns>
        public ICatalog getItem(ICatalog prmICatalog)
        {
            //obtengo las llaves primarias
            var qry = base.getFindQryByPks(prmICatalog);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(qry) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                var reader = sqlCmd.ExecuteReader();

                err.fire(!reader.HasRows, "dato(s)_no_encontrado(s)");

                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;

            }

        }
        public bool existItemPK(ICatalog prmICatalog)
        {
            //obtengo las llaves primarias
            var qry = base.getFindQryByPks(prmICatalog);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(qry) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                var reader = sqlCmd.ExecuteReader();

                return reader.HasRows;

            }

        }

        public bool existItem(ICatalog prmICatalog)
        {
            //obtengo las llaves primarias
            var qry = base.getSqlSearch(prmICatalog);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(qry) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                var reader = sqlCmd.ExecuteReader();

                return reader.HasRows;

            }

        }

        /// <summary>
        /// Get Item Non Primary Key
        /// </summary>
        /// <param name="prmICatalog"></param>
        /// <returns></returns>
        public ICatalog getItemNonPK(ICatalog prmICatalog)
        {
            //obtengo las llaves primarias
            var qry = base.getSqlSearch(prmICatalog);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(qry) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                var reader = sqlCmd.ExecuteReader();
                err.fire(!reader.HasRows, "dato(s)_no_encontrado(s)");

                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;
            }

        }

        public T getItemNonPK<T>(T prmICatalog)
        {
            //obtengo las llaves primarias
            var qry = base.getSqlSearch(prmICatalog);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(qry) { CommandType = CommandType.Text };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                var reader = sqlCmd.ExecuteReader();
                err.fire(!reader.HasRows, "dato(s)_no_encontrado(s)");

                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;
            }

        }

        public ICatalog insertItem(ICatalog prmICatalog, IDbTransaction prmTrans)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Insert), "denied to insert on", prmICatalog.GetType().Name);
            err.fire(existItem(prmICatalog, prmTrans), getPropertyValue(prmICatalog, "Error").ToString());
            SqlCommand sqlCmd = null;
            try
            {
                var query = base.getInsSQLNonIDReturn(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, query);
                var response = sqlCmd.ExecuteNonQuery();

                err.fire(response == 0, "registro_no_agregado");
                return getItem(prmICatalog, prmTrans);

            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }

        public bool existItemPK(ICatalog prmICatalog, IDbTransaction prmTrans)
        {
            //obtengo las llaves primarias
            SqlCommand sqlCmd = null;
            try
            {
                var qry = base.getFindQryByPks(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, qry); //new SqlCommand(qry) { CommandType = CommandType.Text };
                var reader = sqlCmd.ExecuteReader();
                return reader.HasRows;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }

        }


        public bool existItem(ICatalog prmICatalog, IDbTransaction prmTrans)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader reader = null;
            try
            {
                //obtengo las llaves primarias
                var qry = base.getFindQryByPks(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, qry);
                reader = sqlCmd.ExecuteReader();
                return reader.HasRows;

            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }


        /// <summary>
        /// Inserta todos los campos del objeto cambiantes o no.
        /// </summary>
        /// <param name="prmICatalog"></param>
        /// <param name="prmTrans"></param>
        /// <returns></returns>
        public ICatalog insertFULLItem(ICatalog prmICatalog, IDbTransaction prmTrans)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Insert), "denied to insert on", prmICatalog.GetType().Name);
            err.fire(existItem(prmICatalog, prmTrans), getPropertyValue(prmICatalog, "Error").ToString());
            SqlCommand sqlCmd = null;
            try
            {
                var query = base.getFULLInsSQLNonIDReturn(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, query);
                var response = sqlCmd.ExecuteNonQuery();
                err.fire(response == 0, "registro_no_agregado");

                return getItem(prmICatalog, prmTrans);

            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }
        public ICatalog insertItemGetIdentity(ICatalog prmICatalog, IDbTransaction prmTrans)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Insert), "denied to insert on", prmICatalog.GetType().Name);
            //err.fire(existItem(prmICatalog, prmTrans), getPropertyValue(prmICatalog, "Error").ToString());                
            SqlCommand sqlCmd = null;
            SqlDataReader reader = null;
            try
            {
                var query = base.getInsSQLIdentityReturn(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, query);
                reader = sqlCmd.ExecuteReader();

                err.fire(!reader.HasRows, "registro_no_agregado");

                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;

            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }
        /// <summary>
        /// Inserta todos los campos del objeto cambiantes o no.
        /// </summary>
        /// <param name="prmICatalog"></param>
        /// <param name="prmTrans"></param>
        /// <returns></returns>
        public ICatalog insertFULLItemGetIdentity(ICatalog prmICatalog, IDbTransaction prmTrans)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Insert), "denied to insert on", prmICatalog.GetType().Name);
            //err.fire(existItem(prmICatalog, prmTrans), getPropertyValue(prmICatalog, "Error").ToString());                
            SqlCommand sqlCmd = null;
            SqlDataReader reader = null;
            try
            {
                var query = base.getFULLInsSQLIdentityReturn(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, query);
                reader = sqlCmd.ExecuteReader();

                err.fire(!reader.HasRows, "registro_no_agregado");

                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;

            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }
        public ICatalog updateItem(ICatalog prmICatalog, IDbTransaction prmTrans)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Update), "denied to update on", prmICatalog.GetType().Name);            
            SqlCommand sqlCmd = null;
            try
            {
                var query = base.getUpdSQL(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, query);
                var response = sqlCmd.ExecuteNonQuery();
                err.fire(response == 0, "registro_no_actualizado");

                return getItem(prmICatalog, prmTrans);

            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }

        public bool DeleteItem(ICatalog prmICatalog, IDbTransaction prmTrans)
        {
            err.fire(!isSecure(prmICatalog, enumSqlAction.Delete), "denied to delete on", prmICatalog.GetType().Name);
            SqlCommand sqlCmd = null;
            try
            {
                var query = base.getDelSQLWithPK(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, query);
                var response = sqlCmd.ExecuteNonQuery();
                err.fire(response == 0, "registro_no_eliminado");

                return true;

            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }

        public T getItem<T>(T prmICatalog, IDbTransaction prmTrans)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader reader = null;
            try
            {
                var query = base.getFindQryByPks(prmICatalog);
                sqlCmd = getCmdWithTrans(prmTrans, query);
                reader = sqlCmd.ExecuteReader();

                err.fire(!reader.HasRows, "dato(s)_no_encontrado(s)");


                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;

            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }

        public ICatalog getItem(ICatalog prmICatalog, IDbConnection prmConn)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader reader = null;
            try
            {
                var query = base.getFindQryByPks(prmICatalog);
                sqlCmd = getCmdWithConn(prmConn, query);
                reader = sqlCmd.ExecuteReader();
                err.fire(!reader.HasRows, "dato(s)_no_encontrado(s)");

                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }
        public ICatalog getItemNonPK(ICatalog prmICatalog, IDbConnection prmConn)
        {
            SqlCommand sqlCmd = null;
            SqlDataReader reader = null;
            try
            {
                string query = base.getSqlSearch(prmICatalog);
                sqlCmd = getCmdWithConn(prmConn, query);
                reader = sqlCmd.ExecuteReader();
                err.fire(!reader.HasRows, "dato(s)_no_encontrado(s)");

                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;

            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (sqlCmd != null)
                    sqlCmd.Dispose();
            }
        }



        #endregion

        #region Generic Data Objects

        public IDbConnection getConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        public IDbTransaction getTransaction()
        {
            var sqlConn = new SqlConnection(this.ConnectionString);
            sqlConn.Open();

            var sqlTnx = sqlConn.BeginTransaction();
            return sqlTnx;
        }

        public IDbConnection getAnOpenConnection()
        {
            var sqlConn = new SqlConnection(this.ConnectionString);
            sqlConn.Open();
            return sqlConn;
        }



        #endregion

        #region Instance Methods

        public DateTime getTimeofServer()
        {
            var strSql = "Select GetDate()";
            SqlDataReader reader = null;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(strSql);
                sqlCmd.CommandType = CommandType.Text;
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                reader = sqlCmd.ExecuteReader();

                err.fire(!reader.HasRows, "Time from server could not be obtained");

                reader.Read();
                return (DateTime)reader[0];
            }
        }
        #endregion

        private void addParameters(SqlCommand sqlCmd, Hashtable parameters)
        {
            if (parameters == null) return;

            foreach (DictionaryEntry di in parameters)
            {
                if (di.Value is string)
                    sqlCmd.Parameters.AddWithValue(di.Key as string, di.Value as string);
                else if (di.Value is int)
                    sqlCmd.Parameters.AddWithValue(di.Key as string, di.Value as int?);
                else if (di.Value is double)
                    sqlCmd.Parameters.AddWithValue(di.Key as string, di.Value as double?);
                else if (di.Value is DateTime)
                    sqlCmd.Parameters.AddWithValue(di.Key as string, di.Value as DateTime?);
            }
        }

        public SqlDataReader Excuete_Procedure_get_reader(string procName, Hashtable parameters)
        {
            SqlDataReader reader = null;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(procName) { CommandType = CommandType.StoredProcedure };
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                addParameters(sqlCmd, parameters);

                reader = sqlCmd.ExecuteReader();

                err.fire(!reader.HasRows, "no results found");

                reader.Read();

                return reader;
            }
        }

        public DataTable Excuete_Procedure_get_dataTable(string procName, Hashtable parameters)
        {
            SqlDataReader reader = null;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(procName);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                addParameters(sqlCmd, parameters);

                reader = sqlCmd.ExecuteReader();

                err.fire(!reader.HasRows, "no results found");
                DataTable dt = null;
                if (reader.HasRows)
                {
                    dt = new DataTable();
                    dt.Load(reader);
                }

                return dt;
            }
        }

        public SqlDataReader Excuete_procedure_void(string procName, Hashtable parameters)
        {
            SqlDataReader reader = null;
            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(procName);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                addParameters(sqlCmd, parameters);

                err.fire(sqlCmd.ExecuteNonQuery() == 0, "no records affected");

                reader.Read();
                return reader;
            }
        }

        #region Private Interface

        #region Methods

        private SqlCommand getCmdWithTrans(IDbTransaction prmTrans, string prmQuery)
        {
            if (isTransOk(prmTrans) == false)
                err.fire(true, "Invalid state of the Connection or Transaction");

            if (prmQuery == string.Empty)
                err.fire(true, "Missing Command Text");

            var sqlCmd = new SqlCommand(prmQuery);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = (SqlConnection)prmTrans.Connection;
            sqlCmd.Transaction = (SqlTransaction)prmTrans;

            return sqlCmd;

        }

        private SqlCommand getCmdWithConn(IDbConnection prmConn, string prmQuery)
        {
            if (isConnOk(prmConn) == false)
                err.fire(true, "Invalid state of the Connection");

            if (prmQuery == string.Empty)
                err.fire(true, "Missing Command Text");

            var sqlCmd = new SqlCommand(prmQuery);
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.Connection = (SqlConnection)prmConn;

            return sqlCmd;
        }

        #endregion

        #endregion


        public bool isConnOk(IDbConnection prmConn)
        {
            if (prmConn == null)
                return false;

            if (prmConn.State == ConnectionState.Closed || prmConn.State == ConnectionState.Broken)
                return false;

            return true;
        }

        public bool isConnAndTransOk(IDbConnection prmConn, IDbTransaction prmTrans)
        {
            if (isConnOk(prmConn) == false)
                return false;

            if (prmTrans == null)
                return false;

            if (prmTrans.Connection.State == ConnectionState.Closed || prmTrans.Connection.State == ConnectionState.Broken)
                return false;

            return true;
        }

        public bool isTransOk(IDbTransaction prmTrans)
        {
            if (prmTrans == null)
                return false;

            if (prmTrans.Connection.State == ConnectionState.Closed || prmTrans.Connection.State == ConnectionState.Broken)
                return false;

            return true;
        }

        public void Destroy(IDbConnection dbConn, IDbTransaction dbTnx)
        {
            if (dbTnx != null) { dbTnx.Dispose(); dbTnx = null; }
            if (dbConn != null) { dbConn.Close(); dbConn.Dispose(); dbConn = null; }
        }

        public virtual IEnumerable<T> getList<T>(T t)
        {
            var qrySel = base.getSqlSearch<T>(t, true);
            var l = new List<T>();

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(qrySel);
                sqlCmd.CommandType = CommandType.Text;
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                var reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    T cComp = base.fill_object<T>(t, reader);
                    l.Add(cComp);
                }
            }
            return l;
        }

        public virtual List<T> getList<T>(T t, IDbTransaction dbTnx)
        {
            var qrySel = base.getSqlSearch<T>(t, true);
            var l = new List<T>();
            SqlCommand sqlCmd;
            sqlCmd = getCmdWithTrans(dbTnx, qrySel);
            var reader = sqlCmd.ExecuteReader();

            while (reader.Read())
            {
                T cComp = base.fill_object<T>(t, reader);
                l.Add(cComp);
            }

            return l;

        }


        public T getItem<T>(T prmICatalog)
        {
            //obtengo las llaves primarias
            var qry = base.getFindQryByPks(prmICatalog);

            using (var sqlConn = new SqlConnection(this.ConnectionString))
            {
                var sqlCmd = new SqlCommand(qry);
                sqlCmd.CommandType = CommandType.Text;
                sqlConn.Open();
                sqlCmd.Connection = sqlConn;

                var reader = sqlCmd.ExecuteReader();

                err.fire(!reader.HasRows, "dato(s)_no_encontrado(s)");

                reader.Read();
                base.fillObject(prmICatalog, reader);
                return prmICatalog;

            }

        }


    }
}
