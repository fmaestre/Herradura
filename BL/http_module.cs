using System;
using System.Web;
using System.Collections.Generic;
using Herradura.Lib.BL;
using Herradura.Lib.Components;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Herradura.Lib.core;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace httpmodule
{
    public class http_module : IHttpModule
    {
        public static HttpApplication App;
        string _connection ="";
        string _files0 = "";
        string _files1 = "";
        string[] _assemblies;
        string[] _bls;
        string[] _ns;
        string[] _nsbl;
        string _httpmodule;
        string _httpmodule_ns;
        public static string server_path = "";
        public string _wpath = "";
        static string _root = "";
        private const string transacs_path = "\\transactions\\";

        public void initThreads()
        {
            // Creating and initializing threads 
            Thread a = new Thread(logHistory);
            a.Start();
        }

        public static async void logHistory()
        {            
            for (;;)
            {
                try
                {
                    await Task.Delay(60000);                    
                    lock (httphandler.http_handler._txnHistory)
                    {                        
                        for (int i = httphandler.http_handler._txnHistory.Count - 1; i >= 0; i--)
                        {
                            var txn =       httphandler.http_handler._txnHistory[i][0, 0, 0];
                            var usr =       httphandler.http_handler._txnHistory[i][0, 1, 0];
                            var action =    httphandler.http_handler._txnHistory[i][0, 2, 0];
                            if (usr == null || txn == null || txn == "null") continue;

                            log(new RoleHistoryComp { Role = usr, Obj_id = txn, Action = action});
                            httphandler.http_handler._txnHistory.RemoveAt(i);
                        }
                    }
                }
                catch(Exception e) { log(e.Message); }
            }
        }

        public static void log(string message)
        {
            GenericBL gbl = new GenericBL();
            gbl.insert_history(new RoleHistoryComp { Role = "", Obj_id = "error", Action = message });
        }
        public static void log(RoleHistoryComp comp)
        {
            GenericBL gbl = new GenericBL();
            gbl.insert_history(comp);
        }

        public http_module(/*string connection, string httpmodule, string[] assemblies, string[] bls, string[] ns, string[] nsbl*/)
        {            
            if (_connection != "") return;
            load_parameters();
            initThreads();
        }
        public virtual void load_parameters()
        {
            if (httphandler.http_handler._blmap.Count > 0) return;
            server_path = HttpContext.Current.Server.MapPath("~");

            var c = new System.Net.WebClient();
            string data;            
            try
            {
                data = c.DownloadString(server_path + @"\herradura.txt");                
            }
            catch
            {                
                return;
            }

            var o = JObject.Parse(data);
            _connection = o["dbc"].ToString();
            _files0 = o["files0"].ToString();
            _files1 = o["files1"].ToString();
            _httpmodule = "_httpmodule";
            _httpmodule_ns = o["httpm"].ToString();
            _wpath = o["wpath"].ToString();
            _assemblies  = o["apps"].ToObject<List<string>>().ToArray();
            _bls = o["bls"].ToObject<List<string>>().ToArray();
            _ns = o["ns"].ToObject<List<string>>().ToArray();
            _nsbl = o["nsbl"].ToObject<List<string>>().ToArray();
            _root = o["root"].ToString().Replace("\\",@"\");

        }
        public void Dispose()
        {
           
        }
        public virtual void Init(HttpApplication app)
        {
            if (_connection == "") return;

            _sys.Instance().DBConnection = _connection;
            _sys.Instance().Application_Path0 = _files0;
            _sys.Instance().Application_Path1 = _files1;
            App = app;
            set_caches();
            set_cache_roles();
            set_transactions();
            var ii = 0;
            foreach (var _a in _assemblies)
            {
                set_app_assembly(_a);
                if (!httphandler.http_handler._blmap.ContainsKey(_bls[ii]))
                {
                    httphandler.http_handler._blmap.Add(_bls[ii], _a);
                    httphandler.http_handler._namespaces.Add(_bls[ii], _ns[ii]);
                    httphandler.http_handler._namespaces_bl.Add(_bls[ii], _nsbl[ii]);
                }
                ii++;
            }

            set_core_assembly();

            httphandler.http_handler.web_path = _wpath;
            
        }
        public virtual void set_caches()
        {            
        }
        /// <summary>
        /// cargar todas las transacciones en memoria
        /// </summary>
        public virtual void set_transactions()
        {   
            string[] fileEntries = System.IO.Directory.GetFiles(server_path + _root + transacs_path);
            foreach (string fileName in fileEntries)
            {
                build(fileName);                
            }            
        }
        public static string DoTheBuild(string fileName)
        {            
            string data = get_file_by_path(fileName);
            try
            {
                data = resolve_includes(data);
            }
            catch (Exception ex)
            {
                err.fire(true, ex.Message + "{" + fileName  + "}");
            }

            return data;
        }
        private static string build(string fileName)
        {
            var i = fileName.LastIndexOf("\\");
            var id = fileName.Substring(i + 1, fileName.Length - (i + 1));
            App.Application[id] = DoTheBuild(fileName);
            return id;
        }
        public object build_tran(role_permissions_comp r)
        {
            string fileName = httphandler.http_handler.server_path + _root + transacs_path + r.Object_id + ".html";
            return App.Application[build(fileName)];             
        }
        public static List<SysFileComp> get_tran(SysFileComp f)
        {
            var fileName = httphandler.http_handler.server_path + _root + transacs_path + f.Name + ".html";
            var x = new List<SysFileComp>();
            f.Content = App.Application[build(fileName)].ToString();
            x.Add(f);

            return x;
        }
        public static void update_tran(SysFileComp f)
        {
            var fileName = httphandler.http_handler.server_path + _root + transacs_path + f.Name + ".html";
            err.fire(!File.Exists(fileName), "transaction not exists");
            File.WriteAllText(fileName, f.Content, Encoding.Unicode);
            App.Application[build(fileName)] = f.Content;
        }
        public static void insert_tran(SysFileComp f)
        {
            var fileName = httphandler.http_handler.server_path + _root + transacs_path + f.Name + ".html";
            err.fire(File.Exists(fileName), "transaction already exists");
            File.WriteAllText(fileName, f.Content, Encoding.Unicode);
            App.Application[build(fileName)] = f.Content;
        }
        private static string get_file_by_path(string file ){
            string data = "";
            try
            {
                var c = new System.Net.WebClient();              
                data = c.DownloadString(file);
            }
            catch
            {
                err.fire(true, "file not found");
            }
            return data;
        }
        private static string get_file_by_name(string file)
        {
            var c = new System.Net.WebClient();
#if DEBUG
            string data = c.DownloadString(@"C:\Users\fmaestre\Google Drive\Projects\CNR\7\transactions\" + file);            
#else
            string data = c.DownloadString(server_path + _root + transacs_path + file);
#endif
            return data;
        }
        private static string resolve_includes(string data)
        {
            int ScriptTagPos = 0, first =0, last = 0;
            string[] lines = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            int j = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                ScriptTagPos = lines[i].IndexOf("<!--");
                if (ScriptTagPos != -1 && lines[i].IndexOf("include") != -1)
                {                    
                    first = lines[i].IndexOf("{");
                    last = lines[i].LastIndexOf("}");                    
                    var sprm = lines[i].Substring(first, last - first + 1); 
                    
                    var o = JObject.Parse(sprm);
                    var a = o["file"].ToString();

                    string fdata =  get_file_by_name(a);

                    var res = o["replace"];
                    if (res != null) //recorrer los keys del replace
                    {
                        o = JObject.Parse(res.ToString());
                        foreach (var od in o)
                            fdata = fdata.Replace(od.Key.ToString(), od.Value.ToString());
                    }

                    //lines[i] = fdata;
                    lines[i] = resolve_includes(fdata);
                }

            }


            return string.Join("\r\n", lines); 
        }
        public virtual  void set_cache_roles()
        {            
            GenericBL.set_role_permissions_Comps(new role_permissions_comp { Role = "%" });
            GenericBL.set_role_history_Comps(new RoleHistoryComp { Role = "%" });
        }
        public static void add_cache_roles(string role)
        {
            GenericBL.add_role_permissions_Comps(new role_permissions_comp { Role = role });
        }
        //public virtual  void reset_cache(string component)
        //{

        //}
        public virtual object get_cache(string obj)
        {
            return App.Application[obj];
        }
        public virtual void set_app_assembly(string assembly)
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var p = System.IO.Path.GetDirectoryName(path);
            var Kassembly = Assembly.LoadFrom( p +  "\\" + assembly +".dll");
            //httphandler.http_handler._assembly = Kassembly;
            if (!httphandler.http_handler._assemblies.ContainsKey(assembly))
            httphandler.http_handler._assemblies.Add(assembly, Kassembly);
            httphandler.http_handler.http_module_name = _httpmodule;
            httphandler.http_handler.http_module_ns = _httpmodule_ns;
            httphandler.http_handler.server_path = server_path;
            //foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    if (currentassembly.GetName().Name == assembly)
            //    {
            //        httphandler.http_handler._assembly = currentassembly;
            //        httphandler.http_handler.http_module_name = _httpmodule;
            //        break;
            //    }
            //}            
        }
        public virtual void set_core_assembly()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var p = System.IO.Path.GetDirectoryName(path);
            var Kassembly = Assembly.LoadFrom(p + "\\Herradura.dll");
            httphandler.http_handler._herradura = Kassembly;
            

            //foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    if (currentassembly.GetName().Name == "Herradura")
            //    {
            //        httphandler.http_handler._herradura = currentassembly;
            //        break;
            //    }
            //}
        }

        public static List<string[,]> assemblyMethods(string _assembly)
        {
            var p = @"C:\Users\fmaestre\Google Drive\Projects\CNR\CNR_APP\bin\Debug";
            var Assembly_ = Assembly.LoadFrom(p + "\\" + _assembly + ".dll");

            //Assembly Assembly_ = httphandler.http_handler._assemblies[_assembly];
            var lst = new List<string[,]> { };
            foreach (var t in Assembly_.DefinedTypes)
            {
                if (t.Name == "CNRBL")
                {
                   var  meths = t.GetMethods();
                    foreach (var m in meths)
                    {
                        if (m.ReflectedType.Name.Contains("CNR"))
                        {
                            lst.Add( new string[,] { { m.DeclaringType.Name },{ m.Name } });
                        }
                    }
                }
            }

            return lst;
        }
    }

}