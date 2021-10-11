using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Herradura.Lib.BL;

/// <summary>
/// Summary description for mail
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class mail : System.Web.Services.WebService {

    public mail () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string sendmail(string from, string to, string subject, string body) {
        GenericBL fbl = new GenericBL();
        Herradura.Lib.Components.MailComp m = new Herradura.Lib.Components.MailComp();

        m.From = from;
        m.To = to;
        m.Subject = subject;
        m.Body = body;
        
        fbl.send_mail(m);        
        return "Envio exitoso!!";
    }
    
}
