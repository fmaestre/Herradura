using System;
using System.Collections.Generic;
using System.Linq;
using Herradura.Lib.Components;
using Herradura.Lib.BL;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web.Hosting;

namespace Herradura.Lib.core
{
    public static class Utils
    {

#if DEBUG
        public static string WebServerPath = HostingEnvironment.MapPath("~");
#else
        public static string WebServerPath = HostingEnvironment.MapPath("~");
#endif

        static string _NLINE_ = "\n";
        public static void send_mail(
                                string From,
                                string To,
                                string Subject,
                                string Message,
                                string mailServer,
                                int mailPort,
                                string FromPassword,
                                bool ishtml
                             )
        {
            System.Net.Mail.MailMessage Email;
            Email = new System.Net.Mail.MailMessage(From, To, Subject, Message);
            System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient("smtp." + mailServer);
            Email.IsBodyHtml = ishtml;
            smtpMail.EnableSsl = false;
            smtpMail.UseDefaultCredentials = false;
            smtpMail.Host = "mail." + mailServer;  //changed 5/31/2013 requeted by cesar.
            smtpMail.Port = mailPort;
            smtpMail.Credentials = new System.Net.NetworkCredential(From, FromPassword);

            smtpMail.Send(Email);
        }
        public static void copy_file(string _source, string destiny)
        {
            File.Delete(destiny);
            File.Copy(_source, destiny);
        }

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

        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
        public static string get_extension(string exs)
        {
            return exs.Substring(exs.Length - 4, 4);
        }

        public static string formatDate(DateTime dateToFormat)
        {
            return dateToFormat.ToString("MM/dd/yyyy HH:mm:ss", _sys.Instance().cultureInfo);
        }

        public static DateTime formatDate(string dateToFormat)
        {
            return  Convert.ToDateTime(dateToFormat, _sys.Instance().cultureInfo);
        }

        public static string add_like(this string str)
        {
            if (str == "") return "%";
            return "%" + str + "%";
        }
        public static string getPart(this string str, int from, int to)
        {
            if (to > str.Length)
                to = str.Length - from;

            if (from > str.Length)
                return "";

            return str.Substring(from, to);
        }

        public static bool isNumber(this string str)
        {
            return Utils.isNumeric(str);            
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            System.Text.StringBuilder hex = new System.Text.StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static byte[] ReadFully(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static void SendMail(string smtp_adress, string mailuser, string mailpass, string recipient, string subject, string body, int port, bool IsBodyHtml, string[] attachmentFilenames, string[] attsnames, string bcc= "")
        {
            SmtpClient smtpClient = new SmtpClient();
            
            NetworkCredential basicCredential = new NetworkCredential(mailuser, mailpass);
            using (MailMessage message = new MailMessage())
            {
                MailAddress fromAddress = new MailAddress(mailuser);

                // setup up the host, increase the timeout to 5 minutes
                smtpClient.Host = "smtp." + smtp_adress;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                smtpClient.Timeout = (60 * 5 * 1000);
                smtpClient.Port = port;

                message.From = fromAddress;
                message.Subject = subject;
                message.IsBodyHtml = IsBodyHtml;
                message.Body = body;
                message.To.Add(recipient);
                if (bcc != "") message.Bcc.Add(bcc);

                if (attachmentFilenames != null)
                {
                    int i = 0;
                    foreach (string at in attachmentFilenames)
                    {
                        Attachment h = new Attachment(at);
                        h.Name = attsnames[i];
                        message.Attachments.Add(h);
                        i++;
                    }
                }

                smtpClient.Send(message);                
            }
        }


        public static bool delete_file(string fileName)
        {
            if ((System.IO.File.Exists(fileName)))
            {
                System.IO.File.Delete(fileName);
                return true;
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Componenet">Nombre del Componente a Crear</param>
        /// <param name="prmTablaName">Tabla a la que mapeara este componenete</param>
        /// <param name="customUNameSpaces">Lista de NameSpace que importara el Componente</param>
        /// <param name="NameSpace">Name Space del Componente</param>
        /// <returns></returns>
        public static string generateComp(string Componenet, string prmTablaName, List<string> customUNameSpaces, string NameSpace, string BL = "")
        {           
            var cBl = new GenericBL();
            List<Inf_SchemaComp> lInfo = cBl.getInfoSchemaComps(new Inf_SchemaComp { TableName = prmTablaName }).ToList();

            string t = string.Empty;
            t = t + "using System; \n" +
                    "using System.Collections.Generic; \n" +
                    "using System.Collections; \n" +
                    "using System.Linq; \n" +
                    "using System.Text; \n"; //+
                                             //"using Xtrategics.DentiCenterLib.BaseClasses; \n";
            foreach (string x in customUNameSpaces)
            {
                t += "using " + x + "; \n";
            }


            //t = t + "namespace Xtrategics.DentiCenterLib.Components \n" +
            t += "namespace " + NameSpace + " \n" +
            "{\n" +
            "/// <summary>\n" +
            "/// Mapea la tabla " + prmTablaName + " en la Base de Datos\n" +
            "/// </summary>\n" +
            "[Serializable()]\n" +
            "[Table(\"" + prmTablaName + "\")]\n" +
            "public partial class " + Componenet + " : BaseComponentClass, ICatalog\n" +
            "{\n" +
            "#region Class Instance Variables\n";

            foreach (Inf_SchemaComp i in lInfo)
            {
                string type = string.Empty;
                string Default = string.Empty;

                if (i.DataType == "nvarchar") { type = "string"; Default = "string.Empty"; }
                else if (i.DataType == "varchar") { type = "string"; Default = "string.Empty"; }
                else if (i.DataType == "bit") { type = "bool"; Default = "false"; }
                else if (i.DataType == "datetime") { type = "Nullable<DateTime>"; Default = "null"; }
                else if (i.DataType == "float") { type = "double"; Default = "0.0"; }
                else if (i.DataType == "money") { type = "double"; Default = "0.0"; }
                else if (i.DataType == "smallmoney") { type = "double"; Default = "0.0"; }
                else if (i.DataType == "int") { type = "int"; Default = "0"; }
                else if (i.DataType == "tinyint") { type = "int"; Default = "0"; }
                else if (i.DataType == "smallint") { type = "int"; Default = "0"; }
                else if (i.DataType == "bigint") { type = "double"; Default = "0.0"; }
                else if (i.DataType == "decimal") { type = "double"; Default = "0.0"; }
                else if (i.DataType == "numeric") { type = "double"; Default = "0.0"; }
                else err.fire(true, "Data Type => " + i.DataType + " not supported. " + i.ColumnName + " Please use(nvarchar,bit,datetime,float,int,bigint)");

                t = t + "   private " + type + " _" + i.ColumnName.ToLower() + " = " + Default + ";\n";

            }
            t = t + "#endregion\n";


            t = t + "#region Contructors\n";
            t = t + "   public " + Componenet + "():base()\n";
            t = t + "   {\n";
            t = t + "   }\n";
            t = t + "#endregion\n";

            t = t + "#region Public Interface\n";
            t = t + "#region properties\n";

            foreach (Inf_SchemaComp j in lInfo)
            {
                string type = string.Empty;
                string type2 = string.Empty;
                string TRUEIdentity = j.Is_identity == 1 ? ",true" : "";

                //if (j.DataType == "nvarchar") { type = "enmDataTypes.stringType"; }
                //else if (j.DataType == "bit") { type = "enmDataTypes.boolType";  }
                //else if (j.DataType == "datetime") { type = "enmDataTypes.DateTimeType";  }
                //else if (j.DataType == "float") { type = "enmDataTypes.doubleType"; }
                //else if (j.DataType == "int") { type = "enmDataTypes.intType"; }

                if (j.DataType == "nvarchar") { type = "string"; type2 = "enmDataTypes.stringType"; }
                else if (j.DataType == "varchar") { type = "string"; type2 = "enmDataTypes.stringType"; }
                else if (j.DataType == "bit") { type = "bool"; type2 = "enmDataTypes.boolType"; }
                else if (j.DataType == "datetime") { type = "Nullable<DateTime>"; type2 = "enmDataTypes.DateTimeType"; }
                else if (j.DataType == "float") { type = "double"; type2 = "enmDataTypes.doubleType"; }
                else if (j.DataType == "money") { type = "double"; type2 = "enmDataTypes.doubleType"; }
                else if (j.DataType == "smallmoney") { type = "double"; type2 = "enmDataTypes.doubleType"; }
                else if (j.DataType == "int") { type = "int"; type2 = "enmDataTypes.intType"; }
                else if (j.DataType == "smallint") { type = "int"; type2 = "enmDataTypes.intType"; }
                else if (j.DataType == "tinyint") { type = "int"; type2 = "enmDataTypes.intType"; }
                else if (j.DataType == "bigint") { type = "double"; type2 = "enmDataTypes.doubleType"; }
                else if (j.DataType == "decimal") { type = "double"; type2 = "enmDataTypes.doubleType"; }
                else if (j.DataType == "numeric") { type = "double"; type2 = "enmDataTypes.doubleType"; }

                if (j.Is_pk == 0)
                    t = t + "   [Field(" + "\"" + j.ColumnName + "\", \"" + CapitalizeString(j.ColumnName) + "\", false, " + type2 + ",true,true" + TRUEIdentity + ")]\n";
                else
                    t = t + "   [Field(" + "\"" + j.ColumnName + "\", \"" + CapitalizeString(j.ColumnName) + "\", true, " + type2 + ",true,true" + TRUEIdentity + ")]\n";

                t = t + "   public " + type + " " + CapitalizeString(j.ColumnName) + "\n";
                t = t + "   {\n";
                t = t + "       get { return _" + j.ColumnName.ToLower() + "; }\n";
                t = t + "       set\n";
                t = t + "       {\n";
                if (j.Is_fk == 1)
                {
                    t = t + "           if (this._" + j.ColumnName.ToLower() + "!= value)\n";
                    t = t + "           {\n";
                }
                t = t + "             _" + j.ColumnName.ToLower() + "= value;\n";
                t = t + "             this.firePropertyChange(\"" + CapitalizeString(j.ColumnName) + "\");\n";
                if (j.Is_fk == 1)
                {
                    t = t + "           }\n";
                }
                t = t + "       }\n";
                t = t + "   }\n";

            }




            t = t + "#endregion //termina properties\n";



            t = t + "#region reset objects\n";
            t = t + "   public override void  resetObjects()\n";
            t = t + "   {\n";
            foreach (Inf_SchemaComp k in lInfo)
            {
                string type = string.Empty;
                string Default = string.Empty;

                if (k.DataType == "nvarchar") { type = "string"; Default = "string.Empty"; }
                else if (k.DataType == "varchar") { type = "string"; Default = "string.Empty"; }
                else if (k.DataType == "bit") { type = "bool"; Default = "false"; }
                else if (k.DataType == "datetime") { type = "Nullable<DateTime>"; Default = "null"; }
                else if (k.DataType == "float") { type = "double"; Default = "0.0"; }
                else if (k.DataType == "money") { type = "double"; Default = "0.0"; }
                else if (k.DataType == "smallmoney") { type = "double"; Default = "0.0"; }
                else if (k.DataType == "int") { type = "int"; Default = "0"; }
                else if (k.DataType == "tinyint") { type = "int"; Default = "0"; }
                else if (k.DataType == "smallint") { type = "int"; Default = "0"; }
                else if (k.DataType == "bigint") { type = "double"; Default = "0.0"; }
                else if (k.DataType == "decimal") { type = "double"; Default = "0.0"; }
                else if (k.DataType == "numeric") { type = "double"; Default = "0.0"; }

                t = t + "       _" + k.ColumnName.ToLower() + " = " + Default + ";\n";

            }

            t = t + "       base.resetObjects();\n";
            t = t + "   }\n";
            t = t + "#endregion\n";

            t = t + "#region Methods\n";
            t = t + "#endregion //Termina Metodos\n";

            t = t + "#region ICatalog Members\n";

            t = t + "        ArrayList ICatalog.getPropertyChanges()\n";
            t = t + "        {\n";
            t = t + "            return base._propChanges;\n";
            t = t + "        }\n";

            t = t + "        void ICatalog.markAsSaved()\n";
            t = t + "        {\n";
            t = t + "            base.markCompAsSaved();\n";
            t = t + "        }\n";

            t = t + "        public void markAsUnSaved()\n";
            t = t + "        {\n";
            t = t + "            base.MarkCompAsUnSaved();\n";
            t = t + "        }\n";
            /*
                        t = t + "        public void customMethod_1()\n";
                        t = t + "        {\n";
                        t = t + "            err.require(true," + "\"Method not implemented\"" + ");\n";
                        t = t + "        }\n";

                        t = t + "        public void customMethod_2()\n";
                        t = t + "        {\n";
                        t = t + "            err.require(true," + "\"Method not implemented\"" + ");\n";
                        t = t + "        }\n";

                        t = t + "        public void customMethod_3()\n";
                        t = t + "        {\n";
                        t = t + "            err.require(true," + "\"Method not implemented\"" + ");\n";
                        t = t + "        }\n";
                        */

            t = t + "        #endregion\n";

            t = t + "        #endregion //termina public interface\n";
            t = t + "        #region Private Interface\n";
            t = t + "        #endregion //Termina Private Interface\n";
            t = t + "        }\n";
            t = t + "        }\n";


            //prepare json & html map -- commented section
            string json = "";
            string inverted = "";
            string htmlinputs = "";
            string htmltables = "";
            string htmllabels = "";
            int iter = 0;
            foreach (Inf_SchemaComp z in lInfo)
            {
                string type = "text";
                if (z.DataType == "bit")
                    type = "checkbox";
                else if (z.DataType == "datetime")
                    type = "date";

                var c = CapitalizeString(z.ColumnName);
                json += string.Format("\"{0}\":\"_{1}\",\n", c, c);
                inverted += string.Format("\"_{0}\":\"{1}\",\n", c, c);
                htmltables += string.Format("\"c{0}\":\"{1}\",\n", iter++.ToString(), c);
                htmlinputs += string.Format("<input id = \"_{0}\" type = \"{3}\" size = \"20\" maxlength = \"{1}\" placeholder = \"{2}\" /> \n", c, z.MaxLength == 0 ? 12 : z.MaxLength, "", type);
                htmllabels += string.Format("<div class=\"label\" style=\"width:10em;\">{0}:</div> \n", z.ColumnDescription);
            }

            json = json.Substring(0, json.Length - 2);
            inverted = inverted.Substring(0, inverted.Length - 2);

            t += "\n/*\n" + json + "\n\n\n" + inverted + "\n\n\n" + htmlinputs + "\n\n\n" + htmllabels + "\n\n\n" + htmltables + "\n\n\n";
            //end  json map & html 


            t += getTransactionTypeCatalog(lInfo, BL, Componenet) + "\n\n\n" + "*/";

            return t;
        }

        public static string CapitalizeString(string matchString)
        {
            string strTemp = matchString.ToString();
            strTemp = char.ToUpper(strTemp[0]) + strTemp.Substring(1, strTemp.Length - 1).ToLower();
            return strTemp;
        }

//        public static string resetPassword(string Username, string prmnewPassword)
//        {
//            MembershipUser u;
//            string outP;
//            string newPassword;

//            u = Membership.GetUser(Username, false);

//            if (u == null)
//                Membership.CreateUser(Username, prmnewPassword);
//                //throw new Exception ("Username " + HttpUtility.HtmlEncode(Username) + " not found. Please check the value and re-enter.");
//            try
//            {
//                newPassword = "";
//                u.UnlockUser(); //PRIMERO
//                newPassword = u.ResetPassword(null); //(SEGUNDO SE HACE ESTO)
//                u.ChangePassword(newPassword, prmnewPassword); //FINALMENTE
                
//                //newPassword = u.GetPassword();                
//            }
//            catch (MembershipPasswordException)
//            {
//                outP = "Invalid password answer. Please re-enter and try again.";
//                return outP;
//            }
//            catch (Exception e)
//            {
//                outP = e.Message;
//                return outP;
//            }

            
//            return "Password reset. Your new password is: " + HttpUtility.HtmlEncode(prmnewPassword);
            
//        }

//        public static string resetPassword(string user, string old, string _new , string _new2)
//        {
//            var e = (EmployeeComp) Portal.DAL.GenericDAL.Instance.getItemNonPK(new EmployeeComp { UserName = user });

//            if (old.ToUpper() != e.Password) err.require(true,"Old Password is not correct"); 
//            if (old == _new) err.require(true,"please use another new password");
//            if (_new != _new2 ) err.require(true,"New and repeated do not match");
             

//             EmployeeComp x = null;
//             try
//             {
//                 x = (EmployeeComp)Portal.DAL.GenericDAL.Instance.getItemNonPK(new EmployeeComp { Password = _new.ToUpper() });
//             }
//             catch
//             {
//             }

//             if (x != null) err.require(true,"entry password is no valid");

//             e.Password = _new;
//             Portal.DAL.GenericDAL.Instance.updateItem(e);
//            return resetPassword(user, _new);
//        }

//        public static void createUser(string user, string inRole)
//        {

//            // Check User exists 
//            if (Membership.GetUser(user) == null)
//                Membership.CreateUser(user, user + "1", "admin@domain.com");

//            // Check Role exists or create 
//            if (!Roles.RoleExists(inRole))
//                Roles.CreateRole(inRole);

//            // Check Users in Roles 
//            if (!Roles.IsUserInRole(user, inRole))
//                Roles.AddUserToRole(user, inRole); 
//        }

//        public static void createRole(string rolename)
//        {
//            if (!Roles.RoleExists(rolename))
//                Roles.CreateRole(rolename);
//        }

        public static object GetClone(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            ms.Flush();
            ms.Position = 0;
            return bf.Deserialize(ms);
        }

//        public static bool isNumeric(string myNumber)
//        {
//            bool IsNum = false;
//            for (int index = 0; index < myNumber.Length; index++)
//            {
//                IsNum = true;
//                if (!Char.IsNumber(myNumber[index]))
//                {
//                    IsNum = false;
//                    break;
//                }
//            }
//            return IsNum;
//        }


        public static bool HelperConvertNumberToText(int num, out string buf)
        {
            string[] strones = {
            "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight",
            "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen",
            "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen",
          };

            string[] strtens = {
              "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty",
              "Seventy", "Eighty", "Ninety", "Hundred"
          };

            string result = "";
            buf = "";
            int single, tens, hundreds;

            if (num > 1000)
                return false;

            hundreds = num / 100;
            num = num - hundreds * 100;
            if (num < 20)
            {
                tens = 0; // special case
                single = num;
            }
            else
            {
                tens = num / 10;
                num = num - tens * 10;
                single = num;
            }

            result = "";

            if (hundreds > 0)
            {
                result += strones[hundreds - 1];
                result += " Hundred ";
            }
            if (tens > 0)
            {
                result += strtens[tens - 1];
                result += " ";
            }
            if (single > 0)
            {
                result += strones[single - 1];
                result += " ";
            }

            buf = result;
            return true;
        }

        public static bool ConvertNumberToText(int num, out string result)
        {
            string tempString = "";
            int thousands;
            int temp;
            result = "";
            if (num < 0 || num > 100000)
            {
                System.Console.WriteLine(num + " \tNot Supported");
                return false;
            }

            if (num == 0)
            {
                System.Console.WriteLine(num + " \tZero");
                return false;
            }

            if (num < 1000)
            {
                HelperConvertNumberToText(num, out tempString);
                result += tempString;
            }
            else
            {
                thousands = num / 1000;
                temp = num - thousands * 1000;
                HelperConvertNumberToText(thousands, out tempString);
                result += tempString;
                result += "Thousand ";
                HelperConvertNumberToText(temp, out tempString);
                result += tempString;
            }
            return true;
        }

        private static string getTransactionTypeCatalog(List<Inf_SchemaComp> lInfo, string _BL_, string _COMPONENT)
        {
            return
                getEmptyPanel()
                + getBox(lInfo)
                + getEvents(lInfo, _BL_, _COMPONENT)
                + getMethods(_COMPONENT);
        }

        private static string getEmptyPanel()
        {
            return "<div id = \"panel\" >" + _NLINE_ + _NLINE_ +
                    "</div>" + _NLINE_; ;
        }

        private static string getBox(List<Inf_SchemaComp> lInfo)
        {
            string box =
               "<div id = \"box\">" + _NLINE_ +
               "     <h3> " + lInfo[0].TableName + " </h3>" + _NLINE_ +
               "     <table>" + _NLINE_ +
               "         <thead>" + _NLINE_ +
               "             <tr id = \"t0r-1\" >" + _NLINE_ +
               "                  <th id = \"t0c-1\" width = \"5px\" > *</th>" + _NLINE_;
            int i = 0;
            foreach (var z in lInfo)
            {
                box += "                  " +
                  string.Format("<th  id =\"t0c{0}\" width = \"30px\" >{1}</th>{2}", i++.ToString(), z.ColumnDescription, _NLINE_);

            }
            box +=
                        "             </tr>" + _NLINE_ +
                        "        </thead>" + _NLINE_ +
            string.Format("        <!--<script type=\"text/javascript\"> tbody('{{\"table\":\"t0\",\"cols\":{0},\"cedit\":\"\",\"rows\":16,\"w0\":40,\"w1\":300,\"selectable\":\"false\",\"position\":\"relative\",\"maxlength0\":3,\"maxlength1\":40,\"selectable\":\"true\"}}');</script> -->", (i--).ToString()) + _NLINE_ +
                        "     </table>" + _NLINE_ +
                        "<!--<script type=\"include\">include{\"file\":\"ipagerd.html\"}</script> -->" + _NLINE_ +
                        "</div>" + _NLINE_;

            return box;
        }

        private static string getFirstStringElement(List<Inf_SchemaComp> lInfo)
        {
            foreach (var d in lInfo)
            {
                if (d.DataType == "nvarchar")
                    return CapitalizeString(d.ColumnName);
            }

            return "";
        }
        private static string getEvents(List<Inf_SchemaComp> lInfo, string _BL_, string _COMPONENT_)
        {
            string firstStringField = getFirstStringElement(lInfo);
            string events =
             "<!--<script type=\"events\" >" + _NLINE_ +
             "{" + _NLINE_ +
             "    \"oninit\": {" + _NLINE_ +
             "        \"fire\": \"onenter\"" + _NLINE_ +
             "    }," + _NLINE_ +
             "    \"onenter\": {" + _NLINE_ +
             "        \"blog\": \"" + _BL_ + "\"," + _NLINE_ +
             "        \"func\": \"get" + _COMPONENT_ + "\"," + _NLINE_ +
             "        \"param\": {" + _NLINE_ +
             "            \"" + _COMPONENT_ + "\": {" + _NLINE_ +
             "                \"" + firstStringField + "\": \"%\"" + _NLINE_ +
             "           }" + _NLINE_ +
             "        }," + _NLINE_ +
             "        \"fetchs\":{" + _NLINE_ +
             "            \"t0\":{" + _NLINE_;
            int iter = 0;
            string comma = "";
            foreach (var x in lInfo)
            {
                events += comma +
                string.Format("                 \"c{0}\":\"{1}\"", iter++.ToString(), CapitalizeString(x.ColumnName));
                comma = "," + _NLINE_;
            }
            events += _NLINE_;
            events += "                }" + _NLINE_ +
             "            }" + _NLINE_ +
             "    }," + _NLINE_ +
             "    \"actions\":{" + _NLINE_ +
             "             \"t0c1\":{" + _NLINE_ +
             "                       \"dispatchs\":\"t0c0,t0c1\"" + _NLINE_ +
             "                      }," + _NLINE_ +
             "             \"t0c0\":{" + _NLINE_ +
             "                       \"dispatchs\":\"t0c0,t0c1\"" + _NLINE_ +
             "                      }" + _NLINE_ +
             "   }," + _NLINE_ +
             "    \"oninsert\":{" + _NLINE_ +
             "      \"t0\":{" + _NLINE_ +
             "             \"blog\": \"" + _BL_ + "\"," + _NLINE_ +
             "             \"func\": \"insert" + _COMPONENT_ + "\"," + _NLINE_ +
             "             \"param\": {" + _NLINE_ +
             "                  \"" + _COMPONENT_ + "\": {" + _NLINE_;

            iter = 0;
            comma = "";
            foreach (var x in lInfo)
            {

                events += comma +
                string.Format("                          \"{0}\":\"t0c{1}\"", CapitalizeString(x.ColumnName), iter++.ToString());
                comma = "," + _NLINE_;
            }
            events += _NLINE_;
            events += "                    }" + _NLINE_ +
            "               }" + _NLINE_ +
            "        }" + _NLINE_ +
            "    }," + _NLINE_ +
            "    \"ondelete\":{" + _NLINE_ +
            "             \"t0\":{" + _NLINE_ +
            "             \"blog\": \"" + _BL_ + "\"," + _NLINE_ +
            "             \"func\": \"delete" + _COMPONENT_ + "\"," + _NLINE_ +
            "             \"param\": {" + _NLINE_ +
             "                 \"" + _COMPONENT_ + "\": {" + _NLINE_;


            iter = 0;
            comma = "";
            foreach (var x in lInfo)
            {

                events += comma +
                string.Format("                          \"{0}\":\"t0c{1}\"", CapitalizeString(x.ColumnName), iter++.ToString());
                comma = "," + _NLINE_;
            }

            events += _NLINE_;
            events += "              }" + _NLINE_ +
            "                 }" + _NLINE_ +
            "          }" + _NLINE_ +
            "    }," + _NLINE_ +
            "    \"onupdate\":{" + _NLINE_ +
            "        \"t0\":{" + _NLINE_ +
            "            \"blog\": \"" + _BL_ + "\"," + _NLINE_ +
            "            \"func\": \"update" + _COMPONENT_ + "\"," + _NLINE_ +
            "            \"param\": {" + _NLINE_ +
            "                 \"" + _COMPONENT_ + "\": {" + _NLINE_;
            iter = 0;
            comma = "";
            foreach (var x in lInfo)
            {

                events += comma +
                string.Format("                          \"{0}\":\"t0c{1}\"", CapitalizeString(x.ColumnName), iter++.ToString());
                comma = "," + _NLINE_;
            }
            events += _NLINE_;
            events +=
           "                     }" + _NLINE_ +
           "                 }" + _NLINE_ +
           "            }" + _NLINE_ +
           "        }" + _NLINE_ +
          "}" + _NLINE_ +
"</script> -->" + _NLINE_ + _NLINE_ + _NLINE_;


            return events;

        }

        private static string getMethods(string _COMPONENT_, string _DAL_ = "GenericDAL")
        {
            string m =
                        "public List<" + _COMPONENT_ + "> get" + _COMPONENT_ + "(" + _COMPONENT_ + " comp)" + _NLINE_ +
                        "{" + _NLINE_ +
                        "        var x = PortalData." + _DAL_ + ".getList(comp);" + _NLINE_ +
                        "        return x.ToList(); " + _NLINE_ +
                        "}" + _NLINE_ + _NLINE_ +

                        "public " + _COMPONENT_ + " insert" + _COMPONENT_ + "(" + _COMPONENT_ + " comp)" + _NLINE_ +
                        "{" + _NLINE_ +
                        "        var x = PortalData." + _DAL_ + ".insertFULLItemGetIdentity(comp) as " + _COMPONENT_ + ";" + _NLINE_ +
                        "        return x; " + _NLINE_ +
                        "}" + _NLINE_ + _NLINE_ +

                        $"public void update{_COMPONENT_}({_COMPONENT_} comp) {_NLINE_}" +
                        $"{{ {_NLINE_}" +
                        $"        PortalData.{_DAL_}.updateItem(comp);{_NLINE_}" +
                        $"}}  {_NLINE_} {_NLINE_} " +

                        "public void delete" + _COMPONENT_ + "(" + _COMPONENT_ + " comp)" + _NLINE_ +
                        "{" + _NLINE_ +
                        "        PortalData." + _DAL_ + ".DeleteItemWithPK(comp);" + _NLINE_ +
                        "}";
            return m;
        }


    }

    public sealed class err
    {
        public static void fire(bool p, string e) { if (p) throw new Exception(e); }
        
        public static void fire(bool p, string e, string extra) { if (p) throw new Exception(e + " : " + extra); }
        public static void fire(bool p, string e, int extra) { if (p) throw new Exception(e + " : " + extra.ToString()); }
        public static void fire(bool p, string e, double extra) { if (p) throw new Exception(e + " : " + extra.ToString()); }

        public static void fire(bool p, string e, ref char[] ex, int exl) { if (p) throw new Exception(e); }        
        public static void fire(bool p, ref string e) { if (p) throw new Exception(e); }
        public static void fire(bool p, ref string e, ref string extra) { if (p) throw new Exception(e); }
        public static void fire(bool p, string e, ref string extra) { if (p) throw new Exception(e); }
        public static void fire(bool p, ref string e, ref char[] ex, int exl) { if (p) throw new Exception(e); }
        public static void fire(bool p, ref string e, int extra) { if (p) throw new Exception(e); }
    }
}
