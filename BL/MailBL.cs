//using ISS.Engine;

using System.Text;

namespace Herradura.Lib.BL
{
    /// <summary>
    /// Summary description for uploadFiles_BL
    /// </summary>
    public static class strings
    {
        public static string mrl_replace(this string text, params string[] list)
        {
            var s = new StringBuilder();
            s.Append(text);

            for (int i = 0; i < list.Length; i++)
            {
                s.Replace("{" + i.ToString() + "}", list[i].ToString());
            }

            return s.ToString();
        }

    }

}