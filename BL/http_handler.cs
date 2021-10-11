using System;
using System.Web;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using Herradura.Lib.core;
using System.Web.SessionState;

namespace httphandler
{
    public class http_handler : IHttpHandler, IRequiresSessionState
    {

        public static string name_spaceH = "Herradura.Lib.Components";
        
        public static string http_module_name = "";
        public static string http_module_ns = "";
        public static string server_path = "";
        public static string web_path = "";
        HttpContext _context;
        public static Dictionary<string, Assembly> _assemblies = new Dictionary<string, Assembly>();
        public static Dictionary<string, string> _blmap = new Dictionary<string, string>();
        public static Dictionary<string, string> _namespaces = new Dictionary<string, string>();
        public static Dictionary<string, string> _namespaces_bl = new Dictionary<string, string>();
        public static List<string[,,]> _txnHistory = new List<string[,,]>();

        public static Assembly _herradura = null;
        private string reversehtmlEscape(string str) {
            return   str.Replace("&amp;","&").Replace("&quot;","\"").Replace("&#39;","'").Replace("&lt;","<").Replace( "&gt;",">");
        }
        
        private bool is_a_file_requets()
        {
            string r = _context.Request.Url.AbsolutePath;
            var x = r.Substring(r.Length - 4, 4);
            
            if (x == "ashx") return false;

            var y = r.Substring(r.Length - 5, 5);

            return (
                        x.ToUpper() == ".PDF"  ||
                        x.ToUpper() == ".PNG"  || 
                        x.ToUpper() == ".JPG"  || 
                        x.ToUpper() == ".XLS"  ||                    
                        x.ToUpper() == ".DOC"  ||
                        x.ToUpper() == ".BMP"  ||
                        x.ToUpper() == ".TIF"  ||
                        y.ToUpper() == ".XLSX" ||
                        y.ToUpper() == ".DOCX" 
                   );            
        }
        public void ProcessRequest(HttpContext context)
        {
            object result = null;
            var func = context.Request.QueryString["function"];
            string blogic;
            string params_;
            var user = context.Request.QueryString["user"];
            var caller = context.Request.QueryString["caller"];            
#if DEBUG
            if (user == "z") clock.init(caller + ":" + func);
#endif
            _context = context;

            try
            {

                //si el request es un uri para abrir un archivo dejarlo pasar.
                if (is_a_file_requets())
                {
                    try
                    {
                        var strRequestedFile = context.Server.MapPath(context.Request.FilePath);
                        context.Response.TransmitFile(strRequestedFile);
                        context.Response.End();                        
                    }
                    catch
                    {
                        context.Response.Write("Upps.. Lo sentimos, el archivo no fue encontrado!");
                    }

                    return;
                }
#if DEBUG
                clock.measure("is_a_file_requets");
#endif

                if (context.Request.InputStream.Length > 0 && func != "upload")
                {
                    var stream = new StreamReader(context.Request.InputStream);
                    var sr = stream.ReadToEnd();  // added to view content of input stream
                    sr = HttpUtility.UrlDecode(sr);
                    string[] vec = sr.Split('&');

                    var vec_func = vec[0].Split('=');
                    func = vec_func[1];
                    var vec_blog = vec[1].Split('=');
                    blogic = vec_blog[1];
                    var vec_caller = vec[2].Split('=');
                    caller = vec_caller[1];
                    params_ = reversehtmlEscape(context.Request.Form["params"]); //vec_param[1];
                    user = context.Request.Form["user"];
#if DEBUG
                    clock.measure("context.Request.InputStream.Length");
#endif
                }
                else if (func == "upload")
                {
                    var folder = context.Request.QueryString["folder"];
                    var ext = context.Request.QueryString["ext"];
                    var gui = context.Request.QueryString["gui"];

                    handler.require(string.IsNullOrEmpty(folder), "folder parameter not provided");
                    err.fire(context.Request.Files.Count == 0, "se debe especificar un archivo");
                    var sext = context.Request.Files[0].FileName.ToLower().Substring(context.Request.Files[0].FileName.Length - 3, 3);
                    handler.require(!string.IsNullOrEmpty(ext) && !ext.ToLower().Contains(sext), "only " + ext + " allowed");

                    var cx = upload_files.save_file(context.Request.Files, folder, gui == "1" ? true : false);
                    var xf = JsonConvert.SerializeObject(cx);
                    xf = xf.Replace("\"Caller\":null", string.Format("\"Caller\":\"{0}\"", caller));
                    context.Response.Write(xf);
                    return;
                }
                else
                {
                    blogic = context.Request.QueryString["bl"];
                    params_ = context.Request.QueryString["params"];
                }

                if (func == "loginUser")
                {
                    _sys.Instance().user_got_logged_now += http_handler_user_got_logged_now;
                }
                else if (func == "logoffUser")
                {
                    object objuser = context.Session["user"];
                    if (objuser != null)
                    {
                        Herradura.Lib.core._sys.Instance().logoff_user(objuser.ToString());
                        //context.Session["user"] = null;
                        context.Session.Abandon();
                    }
                    return;
                }
                //para el envio de mail no se valida si el usuario esta logeado
                if (func != "loginUser" && func != "send_mail" && blogic != "RegBL" && func != "logoffUser")
                {
                    _sys.Instance().is_user_logged(user, context.Session["user"]);
                }

#if DEBUG
                clock.measure("evaluate session");
#endif

                handler.require(string.IsNullOrEmpty(func), "function parameter not provided");
                handler.require(string.IsNullOrEmpty(blogic), "bl parameter not provided");
                handler.require(string.IsNullOrEmpty(params_), "params parameter not provided");


                object[] paramaters = null;
                string component = "";
                try { paramaters = set_parameters(blogic, params_, out component); }
                catch (Exception emm)
                {
                    handler.require(true, emm.Message);
                }

#if DEBUG
                clock.measure("set paramaters");
#endif

                object[] ov = new Object[1]; ov[0] = func;
                result = reflex.invoke(http_handler.http_module_ns, http_module_name, "get_cache", ov, user);
                if ((result == null || (paramaters[0] as BaseComponentClass).IsChange) && !func.Contains(".html") && func != "build")
                {                        
                    reflex.invoke(blogic, blogic, "validate_method_permissions", new object[] { func }, user);
                    reflex.invoke(blogic, blogic, "validate_method_permissions", new object[] { caller }, user);
                    result = reflex.invoke(blogic, blogic, func, paramaters, user);
                }
                else
                {
                    if (func == "build")
                    {
                        result = reflex.invoke(http_handler.http_module_ns, http_module_name, "build_tran", paramaters, user);
                    }

                }

                if (func == "insertItemGetIdentity" || func == "updateItem" || func == "deleteItem")
                {
                    ov = new Object[1]; ov[0] = component;
                    object resx = reflex.invoke(blogic, http_module_name, "get_cache", ov, user);
                    if (resx != null && resx.ToString() == "1") reflex.invoke(blogic, http_module_name, "reset_cache", ov, user);
                }

#if DEBUG
                clock.measure("function invoke");
#endif
                string x = JsonConvert.SerializeObject(result);

                if (func == "loginUser")
                {
                    var stuff = JObject.Parse(x);
                    _context.Session["Idsucursal"] = stuff["Idsucursal"].ToString();
                }

#if DEBUG
                clock.measure("json converter");
#endif
                x = x.Replace("\"Caller\":null", string.Format("\"Caller\":\"{0}\"", caller));
#if DEBUG
                clock.measure("replace caller");
                clock.save();

                
#endif
                logTxnHistory(caller, user, string.Format("{0}|{1}|{2}|{3}", DateTime.Now.ToString("MMM-dd-yyyy HH:mm:ss:ff tt"), blogic, func, params_));
                context.Response.Write(x);
            }
            catch (Exception handledEx)
            {
                result = new HerraduraError(handledEx.InnerException == null ? handledEx.Message : handledEx.InnerException.Message + " | " + handledEx.InnerException.Source + " | " + handledEx.InnerException.StackTrace);
                string x = JsonConvert.SerializeObject(result);
                x = x.Replace("\"Caller\":null", string.Format("\"Caller\":\"{0}\"", caller));
                context.Response.Write(x);
            }
            
        }

        public void validateExt(string fileName, string ext)
        {
            handler.require(string.IsNullOrEmpty(fileName), "no se encontró archivo");          
            var sext = fileName.ToLower().Substring(fileName.Length - 3, 3);
            handler.require(!string.IsNullOrEmpty(ext) && !ext.ToLower().Contains(sext), "only " + ext + " allowed");          
        }

        void http_handler_user_got_logged_now(object sender, EventArgs e)
        {
            err.fire(_context == null, "context is null");
            err.fire(_context.Session == null, "session is null");
            
            _context.Session["user"] = sender.ToString();
            _sys.Instance().user_got_logged_now -= http_handler_user_got_logged_now;
            err.fire(sender.ToString() == "zz","user or password not found");
            err.fire(sender.ToString() == "sqlinjection", "service is not available");
        }

        private object[] set_parameters(string blogic,string params_, out string comp)
        {
            //scape literals            
            if (params_.Contains("\\"))
                params_ = params_.Replace("\\","\\\\");

            JObject jo = JObject.Parse(params_);
            comp = "";

            object[] ov = new Object[jo.Count];
            Type o_class = null;

            int i = 0;
            foreach (var c in jo)
            {
                if (c.Key == "string")
                    ov[i] = c.Value.ToString();
                else if (c.Key == "int")
                    ov[i] = int.Parse(c.Value.ToString());
                else
                {
                    comp = c.Key;
                    
                    o_class = reflex.get_app_assembly(blogic).GetType(reflex.get_app_namespace(blogic) + "." + c.Key);
                    if (o_class == null) o_class = reflex.get_app_assembly("Herradura").GetType(reflex.get_app_namespace("Herradura") + "." + c.Key);
                    handler.require(o_class == null, "clas not exists");
                    object classInstance = Activator.CreateInstance(o_class, null);

                    if (c.Value is JArray)
                    {

                        int k = 0;
                        foreach (var yy in c.Value.ToObject<JArray>())
                        {
                            //if (yy.Next == null) break;
                            var oo = (yy as JObject);
                            
                            classInstance = Activator.CreateInstance(o_class, null);
                            
                            foreach (var x in oo)
                            {
                                PropertyInfo pi = o_class.GetProperty(x.Key);
                                handler.require(pi == null, "property not found:" + x.Key);

                                if (pi.PropertyType.IsGenericType &&
                                    pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) //Convert.ChangeType just plain does not support nullable types.
                                {
                                    if (String.IsNullOrEmpty(x.Value.ToString()))
                                        pi.SetValue(classInstance, null, null);
                                    else
                                        pi.SetValue(classInstance, Convert.ChangeType(x.Value.ToString(), pi.PropertyType.GetGenericArguments()[0]), null);
                                }
                                else
                                {
                                    try
                                    {
                                        if (String.IsNullOrEmpty(x.Value.ToString()))
                                        {
                                            pi.SetValue(classInstance, Convert.ChangeType("Null", pi.PropertyType), null);  //para que al exportar a excel el blanco no presente error de insercios, property change issue.                                       
                                        }
                                        else
                                            pi.SetValue(classInstance, Convert.ChangeType(x.Value.ToString(), pi.PropertyType), null);
                                    }
                                    catch
                                    {
                                        handler.require(true, "tipo de dato incorrecto:" + x.Key);
                                    }
                                }
                            }
                            if (k > 0 ) Array.Resize(ref ov, k + 1);
                            ov[k] = classInstance;
                            
                            k++;
                        }
                        
                    }
                    else
                    {
                        int onlyOnce = 0;
                        foreach (var x in c.Value.ToObject<JObject>())
                        {
                            if (++onlyOnce == 1)
                            {                                
                                setSessionValues(o_class, classInstance);
                            }

                            var pi = o_class.GetProperty(x.Key);
                            handler.require(pi == null, "property not found:" + x.Key);

                            if (pi.PropertyType.IsGenericType &&
                                pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) //Convert.ChangeType just plain does not support nullable types.
                            {
                                if (String.IsNullOrEmpty(x.Value.ToString()))
                                    pi.SetValue(classInstance, null, null);
                                else
                                    pi.SetValue(classInstance, Convert.ChangeType(x.Value.ToString(), pi.PropertyType.GetGenericArguments()[0]), null);
                            }
                            else
                            {
                                try
                                {                                    
                                    if (pi.PropertyType.ToString() != "System.String" && x.Value.ToString().Trim() == "")
                                        pi.SetValue(classInstance, null, null);
                                    else
                                        pi.SetValue(classInstance, Convert.ChangeType(x.Value.ToString().Replace("^@^", ""), pi.PropertyType), null);
                                    //pi.SetValue(classInstance, Convert.ChangeType(x.Value.ToString().Replace("^@^", "\""), pi.PropertyType), null);
                                }
                                catch
                                {
                                    handler.require(true, "tipo de dato incorrecto:" + x.Key);
                                }
                            }
                        }
                        
                        ov[i] = classInstance;
                    }

                    

                }

                i++;
            }

            return ov;
        }

        private void setSessionValues(Type o_class, object classInstance)
        {

            var prp = o_class.GetProperty("Org");
            if (prp != null)
                prp.SetValue(classInstance, _context.Session["Idsucursal"], null);

            prp = o_class.GetProperty("Created_by");
            if (prp != null)
                prp.SetValue(classInstance, _context.Session["user"], null);

            prp = o_class.GetProperty("Creation_date");
            if (prp != null)
                prp.SetValue(classInstance, DateTime.Now, null);

            prp = o_class.GetProperty("Lastmod_date");
            if (prp != null)
                prp.SetValue(classInstance, DateTime.Now, null);

            prp = o_class.GetProperty("CurrentUser"); //mandatory property
            prp.SetValue(classInstance, _context.Session["user"], null);
        }
        
        private void logTxnHistory(string txn, string user, string action = "")
        {
            lock(_txnHistory)
            {
                _txnHistory.Add( new string[,,] { { { txn }, { user }, { action } } });
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //****************************************************************************************//
       
    }

    public static class reflex
    {
        //static string name_space = "Herradura.Lib.BL";
        static MethodInfo mi = null;
        static Type o_bl;
        static ParameterInfo[] paramss = null;

        public static object invoke(string assembly_name, string bl, string metodo, object[] parameters, string prmUser)
        {
            try
            {
                err.fire(!http_handler._namespaces_bl.ContainsKey(assembly_name), "application domain not trusted, try refreshing the browser cache");
                o_bl = get_app_assembly(assembly_name).GetType(http_handler._namespaces_bl[assembly_name] + "." + bl);
            }
            catch (Exception ex) { err.fire(true, ex.Message, assembly_name); }
            handler.require(o_bl == null, "bl not exists", bl);
            mi = o_bl.GetMethod(metodo);
            handler.require(mi == null, "function not exists", metodo);

            //object result = null;
            paramss = mi.GetParameters();
            _user currentUser = _sys.Instance()._logged_users.Find(x => x.LoginName == prmUser);
            object classInstance;
            if (currentUser != null)
                classInstance = Activator.CreateInstance(o_bl, currentUser);
            else
                classInstance = Activator.CreateInstance(o_bl, null);

#if DEBUG
            clock.measure("bl instance");
#endif
            if (paramss.Length == 0)
                return mi.Invoke(classInstance, null); //The invoke without
            else
            {
                handler.require(parameters == null || parameters.Length == 0, "parameters are expected");

                object[] parametersArray = new object[paramss.Length];
                

                if (parameters.Length == 1 && metodo != "ExportToXls")
                    return mi.Invoke(classInstance, parameters); //The invoke with
                else
                {
                   var l = new List<Herradura.Lib.core.ICatalog>();
                   for (int i = 0; i < parameters.Length; i++)
                   {
                       l.Add(parameters[i] as Herradura.Lib.core.ICatalog);
                   }
                   parametersArray[0] = l;
                   return mi.Invoke(classInstance, parametersArray); //The invoke with
                }               
            }
        }
        public static Assembly get_app_assembly(string assembly)
        {
            try
            {
                if (assembly == "Herradura") return http_handler._herradura;
                err.fire(http_handler._blmap.Count == 0, "bl assemblies dictionary map is empty");
                err.fire(!http_handler._blmap.ContainsKey(assembly), "application domain not trusted, try refreshing the browser cache");
                return http_handler._assemblies[http_handler._blmap[assembly]];
            }
            catch (Exception ex) {

                err.fire(true, ex.Message, assembly); 
            
            }

            return null;

        }
        public static string get_app_namespace(string assembly)
        {
            try
            {
                if (assembly == "Herradura") return http_handler.name_spaceH;
                err.fire(http_handler._namespaces.Count == 0, "namespaces dictionary map is empty");
                err.fire(!http_handler._namespaces.ContainsKey(assembly), "application domain not trusted, try refreshing the browser cache");
                return http_handler._namespaces[assembly];
            }
            catch (Exception ex) { err.fire(true, ex.Message, assembly); }

            return null;
        }

    }

    public static class clock
    {
        static System.Diagnostics.Stopwatch sw;
        static int i = 0;
        static string log = "";
        static string transacc = "";
        public static void init(string txn)
        {
            log = "";
            i = 0;
            transacc = txn;
             sw = System.Diagnostics.Stopwatch.StartNew();
             sw.Start();
        }

        public static void measure(string action) {
            if (sw == null) return;
            sw.Stop();
            i++;
            log += string.Format("\r\n" + transacc + " Time taken for {0}: {1}ms", i.ToString() + ":" + action, sw.Elapsed.TotalMilliseconds);
            sw.Reset();
            sw.Start();
        }

        public static void save()
        {
            if (sw == null) return;
            var thread = new System.Threading.Thread(() => saveit(log));
            thread.Start(); 
            
        }

        private static void saveit(string c){
            try
            {
                var g = System.IO.File.ReadAllText(http_handler.server_path + @"\arch_temp\" + "clock" + ".txt");
                System.IO.File.WriteAllText(http_handler.server_path + @"\arch_temp\" + "clock" + ".txt", g + c);
            }
            catch { }

        }
    }

}