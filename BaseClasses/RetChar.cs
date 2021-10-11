using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXwf.User_Control
{
    /// <summary>
    /// Clase prinicpal que regresa la tecla presionada en el Keyboar de TouchScreen
    /// Programada by: DABIDDO
    /// </summary>
    public class RetChar: System.EventArgs
    {
        //Clase intermedia para el user control
        private string _retVal = string.Empty;
        private bool _isEnter = false;
        private bool _isClear = false;
        private bool _isBckSpace = false;
        private bool _isSpace = false;

        public RetChar()
            : base()
        {

        }

        public RetChar(string prmVal)
            : base()
        {
            _retVal = prmVal;
        }

        
        public string RetVal
        {
            get { return _retVal; }
            set { _retVal = value; }
        }

        public bool RetEnter
        {
            get { return _isEnter; }
            set { _isEnter = value; }
        }

        public bool RetClear
        {
            get { return _isClear; }
            set { _isClear = value; }
        }

        public bool RetBckSpace
        {
            get { return _isBckSpace; }
            set { _isBckSpace = value; }
        }
        public bool RetSpace
        {
            get
            {
                return _isSpace;
            }
            set
            {
                _isSpace = value;
            }
        }
    }
}
