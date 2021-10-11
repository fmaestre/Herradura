//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing.Imaging;
//using System.Drawing.Printing;
//using System.IO;
//using System.Windows.Forms;
//using Microsoft.Reporting.WinForms;

//namespace Herradura.Lib.core
//{
//    public class Printer
//    {
//        private int m_currentPageIndex;
//        private IList<Stream> m_streams;

//        // Routine to provide to the report renderer, in order to
//        //    save an image for each page of the report.
//        private Stream CreateStream(string name,
//          string fileNameExtension, Encoding encoding,
//          string mimeType, bool willSeek)
//        {
//            Stream stream = new FileStream(@"..\..\" + name +
//               "." + fileNameExtension, FileMode.Create);
//            m_streams.Add(stream);
//            return stream;
//        }
//        // Export the given report as an EMF (Enhanced Metafile) file.
//        public void Export(LocalReport report)
//        {
//            string deviceInfo = "";
//            if (Landscape)
//            {
//                deviceInfo =
//              "<DeviceInfo>" +
//              "  <OutputFormat>EMF</OutputFormat>" +
//              "  <PageWidth>11in</PageWidth>" +
//              "  <PageHeight>8.5in</PageHeight>" +
//              "  <MarginTop>0in</MarginTop>" +
//              "  <MarginLeft>0in</MarginLeft>" +
//              "  <MarginRight>0in</MarginRight>" +
//              "  <MarginBottom>0in</MarginBottom>" +
//              "</DeviceInfo>";
//            }
//            else
//            {
//                deviceInfo =
//              "<DeviceInfo>" +
//              "  <OutputFormat>EMF</OutputFormat>" +
//              "  <PageWidth>8.5in</PageWidth>" +
//              "  <PageHeight>11in</PageHeight>" +
//              "  <MarginTop>0in</MarginTop>" +
//              "  <MarginLeft>0in</MarginLeft>" +
//              "  <MarginRight>0in</MarginRight>" +
//              "  <MarginBottom>0in</MarginBottom>" +
//              "</DeviceInfo>";

//            }

//            Warning[] warnings;
//            m_streams = new List<Stream>();
//            report.Render("Image", deviceInfo, CreateStream,
//               out warnings);
//            foreach (Stream stream in m_streams)
//                stream.Position = 0;
//        }
//        // Handler for PrintPageEvents
//        private void PrintPage(object sender, PrintPageEventArgs ev)
//        {
//            Metafile pageImage = new
//               Metafile(m_streams[m_currentPageIndex]);
//            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
//            m_currentPageIndex++;
//            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
//        }

//        public bool Landscape
//        {
//            get;
//            set;
//        }

//        public void Print()
//        {
//            m_currentPageIndex = 0;
//            //const 
//            string printerName =
//                //"Microsoft Office Document Image Writer";
//                //"Microsoft XPS Document Writer";
//               DefaultPrinterName();
//            if (m_streams == null || m_streams.Count == 0)
//                return;
//            PrintDocument printDoc = new PrintDocument();
//            printDoc.PrinterSettings.PrinterName = printerName;
//            printDoc.DefaultPageSettings.Landscape = Landscape; 
//            if (!printDoc.PrinterSettings.IsValid)
//            {
//                string msg = String.Format(
//                   "Can't find printer \"{0}\".", printerName);
//                MessageBox.Show(msg, "Print Error");
//                return;
//            }
//            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
//            printDoc.Print();
//        }

//        public static string DefaultPrinterName()
//        {
//            string functionReturnValue = null;
//            System.Drawing.Printing.PrinterSettings oPS = new System.Drawing.Printing.PrinterSettings();
//            try
//            {
//                functionReturnValue = oPS.PrinterName;
//            }
//            catch (System.Exception)
//            {
//                functionReturnValue = "";
//            }
//            finally
//            {
//                oPS = null;
//            }
//            return functionReturnValue;
//        }
//        // Create a local report for Report.rdlc, load the data,
//        //    export the report to an .emf file, and print it.
//        //private void Run()
//        //{
//        //    LocalReport report = new LocalReport();
//        //    report.ReportPath = @"..\..\Report.rdlc";
//        //    report.DataSources.Add(
//        //       new ReportDataSource("Sales", LoadSalesData()));
//        //    Export(report);
//        //    m_currentPageIndex = 0;
//        //    Print();
//        //}

//        public void Dispose()
//        {
//            if (m_streams != null)
//            {
//                foreach (Stream stream in m_streams)
//                    stream.Close();
//                m_streams = null;
//            }
//        }


//    }
//}
