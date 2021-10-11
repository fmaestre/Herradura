

namespace Herradura.Lib.core
{
    public class HerraduraError 
    {
     
        public HerraduraError()
        {
        }


        public HerraduraError(string msg)
        {
            ErrorMessage = msg;
        }
        public string ErrorMessage { get; set; }

    }
}
