using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Herradura.Lib.Portal.DAL
{
    public static class PortalCore
    {
          public static GenericDALSQLProvider GenericDAL
          {
             get { return new GenericDALSQLProvider("conexion"); }            
          }

          public static GenericDALSQLProvider DynamicDAL(string conn)
          {
                return new GenericDALSQLProvider(conn); 
          }
    }
}