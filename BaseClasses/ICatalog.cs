using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Herradura.Lib.core
{
    public interface ICatalog
    {
     

        ArrayList getPropertyChanges();

        void markAsSaved();

        void markAsUnSaved();
       
    }
}
