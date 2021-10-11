using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using Herradura.Lib.Components;
using Herradura.Lib.core;
using Herradura.Lib.Portal.DAL;

namespace Herradura.Lib.BL
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    public class GenericBL : httphandler.http_handler
    {
        IDbTransaction _txn = null;
        _user _currentUser;

        public static List<role_permissions_comp> roles_;
        public static List<RoleHistoryComp> roles_hist;

        public GenericBL() : base()
        {
        }
        public GenericBL(_user user) : base()
        {
            _currentUser = user;
        }


        public GenericBL(IDbTransaction txn)
            : base()
        {
            _txn = txn;
        }
        public void updateItem(ICatalog prmICatalog)
        {
            PortalCore.GenericDAL.updateItem(prmICatalog);
        }
        public void insertItemGetIdentity(ICatalog prmICatalog)
        {
            PortalCore.GenericDAL.insertItemGetIdentity(prmICatalog);
        }
        public void insertItem(ICatalog prmICatalog)
        {
            PortalCore.GenericDAL.insertItem(prmICatalog);
        }

        public _user CurrentUser
        {
            get { return _currentUser; }
        }
        public void deleteItem(ICatalog prmICatalog)
        {
            PortalCore.GenericDAL.DeleteItem(prmICatalog);
        }
        public void validate_permissions(role_permissions_comp r)
        {
            validate_permissions(new string[] { r.Role }, r.Object_id, Permitions.Sel);
            //List<role_permissions_comp> x = new List<role_permissions_comp>();
            //x.Add(r);
            //return x;
        }
        /// <summary>
        /// valida si el usuario tiene permiso en la forma,funcion(metodo), y componente.
        /// </summary>
        /// <param name="object">Function or screen</param>
        public void validate_method_permissions(string @object)
        {
            switch (@object)
            {
                case "validate_permissions":
                    return;
                case "getRolePermissionCached":
                    return;
                case "login":
                    return;
                case "loginUser":
                    return;
                case "insert_history":
                    return;
                case "resetUser":
                    return;
                case "iforgot":
                    return;
                default:
                    break;
            }
            validate_permissions(new string[] { CurrentUser.LoginName }, @object.Replace(".html",""), Permitions.Sel);
        }

        private void validate_permissions(IEnumerable<string> roles, string _object)
        {

        }

        //check if user has permision
        private void validate_permissions(IEnumerable<string> roles, string _object, string permission, string override_msg = "")
        {
            if (_object == "null") return;

            foreach (var r in roles)
            {
                bool x = false;
                
                //x = (role_permissions_comp)PortalData.GenericDAL.getItem(new role_permissions_comp { Role = r, Object_id = _object });
                x = GenericBL.roles_.Any(y => y.Role == r && y.Object_id == _object && y.Sel == true);

                err.fire(x == false, "user_is_not_allowed_on:", _object + " " + override_msg);

                //if (permission == Permitions.Sel) { if (x.Sel) return; }
                //if (permission == Permitions.Upd) { if (x.Upd) return; }
                //if (permission == Permitions.Del) { if (x.Del) return; }
                //if (permission == Permitions.Ins) { if (x.Ins) return; }

            }

            //err.fire(true, "user_has_no_priviliges:", _object + " : " + permission + " " + override_msg);

        }

        public static void set_role_permissions_Comps(role_permissions_comp comp)
        {
            roles_ = PortalCore.GenericDAL.getList(comp).ToList();
        }
        public static void set_role_history_Comps(RoleHistoryComp comp)
        {
            roles_hist = PortalCore.GenericDAL.getList(comp).ToList();
        }

        public static void add_role_permissions_Comps(role_permissions_comp comp)
        {
            var rs = PortalCore.GenericDAL.getList(comp);
            foreach (var x in rs)
                roles_.Add(x);

        }
        public static void delete_role_permissions_Comps(role_permissions_comp comp)
        {
            roles_.RemoveAll(x => x.Role == comp.Role);
        }

        public static void delete_role_permissions_Comp(role_permissions_comp comp)
        {
            roles_.RemoveAll(x => x.Role == comp.Role && x.Object_id == comp.Object_id);
        }
        public static void clear_role(role_permissions_comp comp)
        {
            roles_.RemoveAll(x => x.Role == comp.Role);
        }
        public void insert_history(RoleHistoryComp comp)
        {
           // if (PortalCore.GenericDAL.existItemPK(comp))
           //     PortalCore.GenericDAL.DeleteItemWithPK(comp);

            PortalCore.GenericDAL.insertItemGetIdentity(comp);
            
        }

        public List<RoleHistoryComp> getHistory(RoleHistoryComp comp)
        {
            comp.Role = comp.Role.add_like();
            comp.Obj_id = comp.Obj_id.add_like();
            comp.Action = comp.Action.add_like();

            var x = PortalCore.GenericDAL.getList(comp);
            if (x != null)
            {
                foreach (var z in x)
                {
                    z.Obj_id = z.Obj_id.Replace(".html", "");
                    z.Action = z.Action.Replace("|"," *** ")
                                       .Replace("Comp","")
                                       .Replace(".html", "")
                                       .Replace("_comp", "")
                                       ;
                }
            }

            return x.OrderByDescending(c => c.Created_date).ToList();
        }


        public IEnumerable<role_permissions_comp> getRolePermission(role_permissions_comp comp)
        {
            err.fire(comp.Role == "", "type a valid role");
            err.fire(comp.Object_id == "", "type a valid transaction");
            err.fire(!PortalCore.GenericDAL.existItemPK(comp), "role does not exist");
            var x = PortalCore.GenericDAL.getList(comp);
            return x;
        }

        public IEnumerable<role_permissions_comp> getRolePermissionCached(role_permissions_comp comp)
        {
            IEnumerable <role_permissions_comp> r = roles_.FindAll(x => x.Role == comp.Role && x.Sel == true && x.Object_id.Length <=5);
            return r;
        }

        public IEnumerable<role_permissions_comp> getRolePermissionCachedByRole(role_permissions_comp comp)
        {
            IEnumerable<role_permissions_comp> r = roles_.FindAll(x => x.Role == comp.Role);
            return r;
        }
        public void insertRolePermission(role_permissions_comp comp)
        {
            err.fire(comp.Role == "", "type a valid role");
            err.fire(comp.Object_id == "", "type a valid transaction");

            var x = PortalCore.GenericDAL.insertItem(comp.GetClone() as role_permissions_comp);
            GenericBL.add_role_permissions_Comps(comp);

        }
        public void saveRolePermission(role_permissions_comp comp)
        {
            err.fire(comp.Role == "", "type a valid role");
            err.fire(comp.Object_id == "", "type a valid transaction");
            err.fire(!PortalCore.GenericDAL.existItemPK(comp), "role does not exist");

            var c = comp.GetClone();
            var x = PortalCore.GenericDAL.updateItem(comp);
            
            GenericBL.delete_role_permissions_Comp(comp);
            GenericBL.add_role_permissions_Comps(c as role_permissions_comp);
        }

        public void refreshRolePermission(role_permissions_comp comp)
        {
            err.fire(comp.Role == "", "type a valid role");

            if (comp.Role != "*")
            {

                err.fire(!PortalCore.GenericDAL.existItem(comp), "role does not exist");

                GenericBL.clear_role(comp);
                GenericBL.add_role_permissions_Comps(comp);
            }
            else
            {
                var rolesper = Portal.DAL.PortalCore.GenericDAL.getList(new role_permissions_comp { Role = "%" });
                var roles = rolesper.GroupBy(x => x.Role).Select(z => z.FirstOrDefault());

                foreach (var r in roles)
                {
                    GenericBL.clear_role(r);
                    GenericBL.add_role_permissions_Comps(r);
                }
            }
        }



        public void deleteRolePermission(role_permissions_comp comp)
        {
            err.fire(comp.Role == "", "type a valid role");
            err.fire(comp.Object_id == "", "type a valid transaction");
            err.fire(!PortalCore.GenericDAL.existItemPK(comp), "role does not exist");

            var x = PortalCore.GenericDAL.DeleteItemWithPK(comp.GetClone() as role_permissions_comp);
            GenericBL.delete_role_permissions_Comp(comp);

        }

        public IEnumerable<SysParameterComp> getSysParamaters(SysParameterComp prmSysParamComp)
        {
            err.fire(prmSysParamComp == null, "Sys Param object cannot be null");
            return PortalCore.GenericDAL.getList(prmSysParamComp);
        }
        public IEnumerable<Inf_SchemaComp> getInfoSchemaComps(Inf_SchemaComp prmInfoSch)
        {
            return PortalCore.DynamicDAL(_sys.Instance().DBConnection).getList(prmInfoSch);
        }
        public List<MailComp> send_mail(MailComp Comp, string host = "relay-hosting.secureserver.net")
        {
            //err.fire(true, "Holla");
            var m = new System.Net.Mail.MailMessage();
            m.To.Add(Comp.To);
            m.From = new System.Net.Mail.MailAddress(Comp.From);
            m.Subject = Comp.Subject;

            m.IsBodyHtml = true;
            m.Body = Comp.Body;

            var client = new System.Net.Mail.SmtpClient { Host = host };
            try { client.Send(m); }
            catch (Exception errd) { Comp.Body = errd.Message; }

            var x = new List<MailComp> { Comp };
            return x.ToList();
        }
        public List<MailComp> send_mail(MailComp Comp, string[] attachs, string host = "smtpout.secureserver.net")
        {

            var m = new System.Net.Mail.MailMessage();
            m.To.Add(Comp.To);
            m.From = new System.Net.Mail.MailAddress(Comp.From);
            m.Subject = Comp.Subject;

            m.IsBodyHtml = true;
            m.Body = Comp.Body;

            if (attachs != null)
            {
                foreach (string at in attachs)
                {
                    m.Attachments.Add(new System.Net.Mail.Attachment(at));
                }
            }

            var client = new System.Net.Mail.SmtpClient { Host = host };
            try { client.Send(m); }
            catch (Exception err) { Comp.Body = err.Message; }

            var x = new List<MailComp>();
            x.Add(Comp);
            return x.ToList();
        }
        public List<FileComp> ExportToXls(List<ICatalog> clns)
        {
            err.fire(clns == null, "lista a importar es nula");
            err.fire(clns.Count == 0, "lista a importar sin elementos");
            string c = clns[0].GetType().FullName;
            OleDbConnection oleConn = null;
            string destiny = "";
            try
            {
                try { destiny = create_excel_file(httphandler.http_handler.server_path, "\\arch_temp", "\\arch_temp", "generic"); }
                catch
                {
                    try { destiny = create_excel_file(httphandler.http_handler.server_path, "arch_temp", "arch_temp", "generic"); }
                    catch (Exception _err) { err.fire(true, "no se pudo crear el archivo. " + _err.Message); }
                }
                oleConn = open_file(destiny);
                int i = 0;
                foreach (var ls in clns)
                {
                    i++;

                    //var m = Convert.ChangeType(ls, Type.GetType(c));
                    PropertyInfo pi = _herradura.GetType(c).GetProperty("PropertyChanges");
                    var cc = (pi.GetValue(ls, null) as ArrayList);
                    //if (i == 1)
                    //    insert_header(oleConn, cc);
                    var values = new ArrayList();
                    if (i == 1)
                    {
                        foreach (var ccc in cc)
                        {
                            string prop = ccc.ToString();
                            //PropertyInfo pj = Type.GetType(c).GetProperty(prop);
                            PropertyInfo pj = _herradura.GetType(c).GetProperty(prop);
                            var m = pj.GetValue(ls, null); //property value
                            var str = (m as string);
                            if (str.ToUpper() == "DATE") str += "_"; //date is a reserved word
                            values.Add(str.Replace(' ', '_').Replace('.', '_'));
                        }

                        insert_header(oleConn, values);
                        continue;
                    }


                    values = new ArrayList();
                    foreach (var ccc in cc)
                    {
                        string prop = ccc.ToString();
                        //PropertyInfo pj = Type.GetType(c).GetProperty(prop);
                        PropertyInfo pj = _herradura.GetType(c).GetProperty(prop);
                        var m = pj.GetValue(ls, null); //property value

                        values.Add(m.ToString().Replace("'", "`"));
                    }
                    insert_body(oleConn, values, "Sheet1");

                }
            }
            catch
            {
                throw;
            }
            finally
            {

                if (oleConn != null)
                {
                    oleConn.Close();
                    try
                    {
                        delete_active_sheet(destiny);
                    }
                    catch { }
                    //catch (Exception e) { err.fire(true, e.Message); }

                }

            }

            List<FileComp> fl = null;

            try
            {
                fl = new List<FileComp>
                    {
                        new FileComp {Filename = destiny.Substring(destiny.IndexOf("\\arch_temp"))}
                    };
            }
            catch
            {
                err.fire(true, "err adding file");
            }
            return fl;
        }

        public void insert_header(OleDbConnection oleConn, ArrayList array)
        {
            var cmd = new OleDbCommand();
            cmd.Connection = oleConn;
            var query = new StringBuilder();
            query.Append("CREATE TABLE Sheet1 (");

            foreach (var cl in array)
            {
               var clm = System.Text.RegularExpressions.Regex.Replace(cl.ToString(), @"[^0-9a-zA-Z]+", "");
                query.Append(clm);
                query.Append(' ');
                query.Append("nvarchar(255)");
                query.Append(',');
            }
            query.Length--; // to eliminate the last comma
            query.Append(')');

            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();
        }

        public void insert_body(OleDbConnection oleConn, ArrayList array, string sheet)
        {
            var cmd = new OleDbCommand();
            cmd.Connection = oleConn;
            var query = new StringBuilder();
            query.Length = 0;
            query.Append("INSERT INTO [");

            query.Append(sheet);
            query.Append("] values(");

            foreach (var cl in array)
            {
                query.Append('\'');
                query.Append(cl.ToString());
                query.Append('\'');
                query.Append(',');
            }
            query.Length--; // to eliminate the last comma
            query.Append(')');

            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();


        }

        public OleDbConnection open_file(string destiny)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + destiny +
            ";Extended Properties=\"Excel 8.0;HDR=Yes;\"";
            var oleConn = new OleDbConnection(strConn);
            oleConn.Open();
            return oleConn;
        }
        public void delete_active_sheet(string destiny)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(destiny);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Move(After: xlWorkBook.Sheets[xlWorkBook.Sheets.Count]);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            ((Microsoft.Office.Interop.Excel._Worksheet)xlWorkSheet).Activate();
            //xlWorkSheet.Delete();
            xlWorkBook.Save();
            xlWorkBook.Close(true);
            xlApp.Quit();
        }
        public string create_excel_file(string homepath, string foldersource, string folderdestiny, string template)
        {
            var filename = string.Format("{0}_{1}_{2}.xls", "", Guid.NewGuid().ToString(), template);
            var source = string.Format("{0}{1}\\{2}.xls", homepath, foldersource, template);
            var destiny = string.Format("{0}{1}\\{2}", homepath, folderdestiny, filename);
            System.IO.File.Copy(source, destiny);

            var fileInfo = new System.IO.FileInfo(destiny);
            if (fileInfo.IsReadOnly == true)
                fileInfo.IsReadOnly = false;

            return destiny;
        }


        public List<SysFileComp> get_tran(SysFileComp f)
        {
            return httpmodule.http_module.get_tran(f);
        }


        public void update_tran(SysFileComp f)
        {
            httpmodule.http_module.update_tran(f);
        }

        public void insert_tran(SysFileComp f)
        {
            httpmodule.http_module.insert_tran(f);
        }

        public List<UserComp> ChangeOrg(UserComp comp)
        {
            comp.Usuario = _currentUser.LoginName;
            comp.ChangeOrg();
            comp.Success = "Usuario actualizado exitosamente.";

            var z =_sys.Instance()._logged_users.Find(x => x.LoginName == _currentUser.LoginName);            
            z.LogOff();
            
            return new List<UserComp> { comp };
            
        }

        public List<UserComp> onlineUsers(UserComp comp)
        {
            var z = _sys.Instance()._logged_users;
            List<UserComp> users = new List<UserComp>();

            foreach (var user in z)
            {
                users.Add(new UserComp { Nombre = user.LoginName , LoginTime = user.LoginTime });
            }

            return users;
        }

        public List<vwUsersComp> getvwUsersComp(vwUsersComp comp)
        {
            var x = PortalCore.GenericDAL.getList(comp);
            return x.ToList();
        }

        public List<vwUsersComp> insertvwUsersComp(vwUsersComp comp)
        {
            var x = PortalCore.GenericDAL.insertFULLItemGetIdentity(comp) as vwUsersComp;
           return  new List<vwUsersComp> { x };
        }

        public List<vwUsersComp> updatevwUsersComp(vwUsersComp comp)
        {
            var x = PortalCore.GenericDAL.updateItem(comp) as vwUsersComp;
            return new List<vwUsersComp> { x };
        }

        public void deletevwUsersComp(vwUsersComp comp)
        {
            PortalCore.GenericDAL.DeleteItemWithPK(comp);            
        }


        
        public void TestEmail(MailComp comp)
        {
            string host = "ionos.mx";//"1and1.mx";
            string account = "verificaciones@cobranzadental.com";//"russia2018@todofacturas.com";
            string password = "CnrDental@2020";//"Zrussi@2018";
            int port = 587;//int port = 25;
            string zzquery = @"TEST";

            //string emails = "cobranzadental @gmail.com,francio.maestre@gmail.com,albertopr73@gmail.com";
            string emails = "francio.maestre@gmail.com";
            Utils.SendMail(host, account, password,
                           emails, "TEST",
                          zzquery
                          //comp.Body
                           ,
                           port, true, null, null);
        }

    }
}
