using System.Collections.Generic;
using System.Web;
using System;
using System.Linq;

public static class upload_files 
{

    public static string CleanFileName(string fileName)
    {
        return System.IO.Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
    }

    public static List<Herradura.Lib.Components.SysFileComp> save_file(HttpFileCollection fc, string folder, bool generateGui = true)
    {
        string a = "", fn = "";
        string fileOk = "";
        for (int i = 0; i < fc.Count; i++)
        {
            HttpPostedFile hpf = fc[i];
            if (hpf.ContentLength > 0)
            {
                var root = System.IO.Path.GetPathRoot(hpf.FileName);
                var sFnane = System.IO.Path.GetFileName(hpf.FileName);
                var ext = System.IO.Path.GetExtension(hpf.FileName);
                var nameOnly = sFnane.Substring(0, sFnane.LastIndexOf(ext)); 

                fileOk = root + CleanFileName(nameOnly).Replace("#", "").Replace(" ","").Replace(".","") + ext;
                fn = (generateGui ? Guid.NewGuid().ToString() : "") + System.IO.Path.GetFileName(fileOk);
                //a = HttpContext.Current.Server.MapPath("");
                a = System.Web.Hosting.HostingEnvironment.MapPath("~");
                hpf.SaveAs(a + "\\" + folder.Replace("\\", "\\\\") + "\\" + fn);
            }
        } 

#if DEBUG
                    var x = new List<Herradura.Lib.Components.SysFileComp>();
                    //x.Add(new Herradura.Lib.Components.SysFileComp { fullpath = a + "\\" + folder.Replace("\\", "\\\\") + "\\" + fn });
                    x.Add(new Herradura.Lib.Components.SysFileComp { fullpath = fullPath(folder) + fn });
                    return x;        
#else
        var x = new List<Herradura.Lib.Components.SysFileComp>();
        //x.Add(new Herradura.Lib.Components.SysFileComp { fullpath = Herradura.Lib.core._sys.Instance().Application_Path1 + fn });
        x.Add(new Herradura.Lib.Components.SysFileComp { fullpath = fullPath(folder) + fn });
        return x;
#endif

    }

    public static string fullPath(string folder)
    {
        
            string a = System.Web.Hosting.HostingEnvironment.MapPath("~");
            
#if DEBUG
            return  a + "\\" + folder.Replace("\\", "\\\\") + "\\";                                 
#else
            return  Herradura.Lib.core._sys.Instance().Application_Path1;
#endif
        
    }

}