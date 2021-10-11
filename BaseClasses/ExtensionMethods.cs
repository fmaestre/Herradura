using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Herradura.Lib.core
{
    [Serializable()]
    public static class ExtensionMethods
    {
      
        /// <summary>
        /// Extension para clonar listas de componentes..
        /// el componente debe implementar el la interface de IClonable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToClone"></param>
        /// <returns></returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
      {
         return listToClone.Select(item => (T)item.Clone()).ToList();
      }
    }
}
