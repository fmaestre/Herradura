//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using Herradura.Lib.Components;
//using Herradura.Lib.BL;
//using Herradura.Lib.Portal;

//namespace Herradura.Lib.core
//{
//    public class Timer
//    {
//        private static System.Timers.Timer Tlog;
//        private static System.Timers.Timer TlogD;
        
//        public static void run()
//        {
//            Tlog = new System.Timers.Timer(15000);
//            Tlog.Elapsed += new System.Timers.ElapsedEventHandler(Tlog_Elapsed);
//            Tlog.Disposed += new EventHandler(Tlog_Disposed);
//            Tlog.Enabled = true;
            
//            if (_sys.Instance().user.doesUserHasRole(Roles_.Admin,false))
//            {
//                TlogD = new System.Timers.Timer(5000);
//                TlogD.Elapsed += new System.Timers.ElapsedEventHandler(TlogD_Elapsed);
//                TlogD.Enabled = true;
//            }

//        }

//        static void Tlog_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
//        {
//            //Buscar y subir log.
//            foreach (var x in _sys.Instance().log.FindAll(z=>z.flagup == false))
//            {
//                if (_sys.Instance().DBConnection != string.Empty)
//                {
//                    x.flagup = true;
//                    Portal.DAL.GenericDAL.Instance.insertItem(x);                    
//                }
//            }
//        }

//        static void Tlog_Disposed(object sender, EventArgs e)
//        {



//        }


//        static void TlogD_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
//        {
//            if (_sys.Instance().DBConnection != string.Empty)
//            {
//                TlogD.Enabled = false;
//                try { Portal.DAL.GenericDAL.Instance.DeleteItem(new SysLogComp { DateS = "< getdate()-15", Type = 0 }); }catch { }
//                try { Portal.DAL.GenericDAL.Instance.DeleteItem(new SysLogComp { DateS = "< getdate()-1", Type = 1 }); }catch { }
//                //try { Portal.DAL.GenericDAL.Instance.DeleteItem(new SysLogComp { DateS = "< getdate()-15", Type = 0 }); }catch { }                
//            }
//        }
//    }
//}
