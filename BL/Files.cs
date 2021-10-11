using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.OleDb;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop;

namespace Herradura.BL
{
    public class Files
    {

        public Files() { }
        public Files(string conn)
        {

        }

        public string getFileName(string path)
        {
            string[] arrFiles = path.Split('\\');
            return arrFiles[arrFiles.Length - 1]; //el nombre dle archivo
        }


        //WORK WITH FILES
        public static string path
        {
            get;
            set;
        }
        public static HttpServerUtility Server
        {
            get;
            set;
        }
        public static FileUpload _FileUpload
        {
            get;
            set;
        }

        //To uplad a file into a specified path
        public static string upLoad()
        {
            try
            {
                string savePath = "";
                try
                {
                    savePath = Server.MapPath(path);
                }
                catch
                {
                    savePath = path; //@"c:\";  
                }

                string pathToCheck = savePath + _FileUpload.FileName;             // Create the path and file name to check for duplicates.
                string tempfileName = _FileUpload.FileName;             // Create a temporary file name to use for checking duplicates.
                if (System.IO.File.Exists(pathToCheck)) // Check to see if a file already exists with the // same name as the file to upload.        
                {
                    int counter = 2;
                    while (System.IO.File.Exists(pathToCheck))
                    {
                        tempfileName = counter.ToString() + _FileUpload.FileName; // if a file with this name already exists,// prefix the filename with a number.
                        pathToCheck = savePath + tempfileName;
                        counter++;
                    }
                }

                savePath += tempfileName; // Append the name of the file to upload to the path.
                _FileUpload.SaveAs(savePath); // Call the SaveAs method to save the uploaded // file to the specified directory.

                return savePath;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool Delete(string filePath)
        {

#if DEBUG
            string savedPath = filePath;            
#else
            string strPhysicalFolder = System.Web.Hosting.HostingEnvironment.MapPath("~"); //Server.MapPath(filePath);                        
            string savedPath = filePath.Replace(@"../", "").Replace(@"/", @"\"); // strPhysicalFolder;
            //Herradura.Lib.core.err.fire(true, strPhysicalFolder + @"\" + savedPath);
            savedPath = strPhysicalFolder + @"\" + savedPath;
#endif
            if (File.Exists(savedPath))
            {
                File.Delete(savedPath);
                return true;
            }
            else
            {
                return false;
            }
        }

        
        // This method recieve a path, read the file and return an array list of string with the line information
        public static List<Array> getArrayListFromExcelFile(string fileName, string Range_From, string Range_to)
        {
            List<Array> ArrayList = new List<Array>();
            Array myvalues;
            int i = 1;
            bool bContinue = true;

            try
            {
                Microsoft.Office.Interop.Excel.Application ExcelObj = null;
                ExcelObj = new Microsoft.Office.Interop.Excel.Application(); // See if the Excel Application Object was successfully constructed

                Microsoft.Office.Interop.Excel.Workbook theWorkbook = ExcelObj.Workbooks.Open(fileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, null, null);

                // get the collection of sheets in the workbook
                Microsoft.Office.Interop.Excel.Sheets sheets = theWorkbook.Worksheets;// get the first and only worksheet from the collection of worksheets
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);// loop through 10 rows of the spreadsheet and place each row in the list view

                do
                {
                    Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range(Range_From + i.ToString(), Range_to + i.ToString());
                    myvalues = (Array)range.Cells.Value2;

                    //to catch if no more cells have values
                    try
                    {
                        if (myvalues.GetValue(1, 2).ToString() != null)
                            ArrayList.Add(myvalues);
                    }
                    catch (NullReferenceException err)
                    {
                        bContinue = false;
                    }

                    i++;

                } while (bContinue);

            }
            catch (Exception er)
            {
                throw er;

            }
            return ArrayList;

        }
        // This method recieve a path, read the file and return an array list of string with the line information
        public static List<Array> getArrayListFromFlatFile(string rfileName)
        {
            List<Array> ArrayList = new List<Array>();

            try
            {
                TextReader tr = new StreamReader(rfileName);
                string currentLine;

                while ((currentLine = tr.ReadLine()) != null)
                {
                    string[] myvalues = new string[1];
                    myvalues[0] = currentLine;
                    ArrayList.Add(myvalues);
                }

                tr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in getArrayListFromFlatFile " + ex.Message.ToString());
            }

            return ArrayList;
        }


        public static OleDbDataReader getReaderFromFile(string Path, string SheetName, ref OleDbConnection con)
        {
            string connectionString = "Provider=Microsoft.Jet.OleDb.4.0; data source=" + Path.Replace("/", "\\") + "; Extended Properties=Excel 8.0;";
            string selectString = "SELECT * FROM [" + SheetName + "$]";
            con = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(selectString, con);
            con.Open();
            try
            {
                OleDbDataReader theData = cmd.ExecuteReader();
                return theData;
            }
            catch (Exception ex)
            {
                //UF.Lib.Portal.DAL.PortalData.GenericDAL.insertItemGetIdentity(new UF.Lib.Components.TarjetaComp { Notarjeta = "ErrorJET" + DateTime.Now.ToLongTimeString() });
                throw new Exception(ex.Message + " : " + Path.Replace("/", "\\") + ":" + connectionString);
            }
            finally
            {
                //con.Dispose();
            }
        }



    }

}








