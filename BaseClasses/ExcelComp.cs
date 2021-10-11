using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;
namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla tipo_archivos en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("none")]
    public class ExcelComp : BaseComponentClass, ICatalog
    {
       
        private string _col1 = string.Empty;        private string _col2 = string.Empty;
        private string _col3 = string.Empty;        private string _col4 = string.Empty;
        private string _col5 = string.Empty;        private string _col6 = string.Empty;
        private string _col7 = string.Empty;        private string _col8 = string.Empty;
        private string _col9 = string.Empty;        private string _col10 = string.Empty;
        private string _col11 = string.Empty; private string _col12 = string.Empty;
        private string _col13 = string.Empty; private string _col14 = string.Empty;
        private string _col15 = string.Empty; private string _col16 = string.Empty;

        private string _col17 = string.Empty; private string _col18 = string.Empty;
        private string _col19 = string.Empty; private string _col20 = string.Empty;
        private string _col21 = string.Empty;

        private string _colk = string.Empty;        private string _coll = string.Empty;
        private string _colm = string.Empty;        private string _coln = string.Empty;
        private string _colo = string.Empty;        private string _colp = string.Empty;
        private string _colq = string.Empty;        private string _colr = string.Empty;
        private string _cols = string.Empty;        private string _colt = string.Empty;
        private string _colu = string.Empty;        private string _colv = string.Empty;
        private string _colw = string.Empty;        private string _colx = string.Empty;
        private string _coly = string.Empty;        private string _colz = string.Empty;
       
       
        #region Contructors
        public ExcelComp()
            : base()
        {
        }
        #endregion
        #region properties
        
        public string Col1
        {
            get { return _col1; }
            set
            {
                if (this._col1 != value)
                {
                    _col1 = value;
                    this.firePropertyChange("Col1");
                }
            }
        }
        public string Col2
        {
            get { return _col2; }
            set
            {
                if (this._col2 != value)
                {
                    _col2 = value;
                    this.firePropertyChange("Col2");
                }
            }
        }
        public string Col3
        {
            get { return _col3; }
            set
            {
                if (this._col3 != value)
                {
                    _col3 = value;
                    this.firePropertyChange("Col3");
                }
            }
        }
        public string Col4
        {
            get { return _col4; }
            set
            {
                if (this._col4 != value)
                {
                    _col4 = value;
                    this.firePropertyChange("Col4");
                }
            }
        }
        public string Col5
        {
            get { return _col5; }
            set
            {
                if (this._col5 != value)
                {
                    _col5 = value;
                    this.firePropertyChange("Col5");
                }
            }
        }
        public string Col6
        {
            get { return _col6; }
            set
            {
                if (this._col6 != value)
                {
                    _col6 = value;
                    this.firePropertyChange("Col6");
                }
            }
        }
        public string Col7
        {
            get { return _col7; }
            set
            {
                if (this._col7 != value)
                {
                    _col7 = value;
                    this.firePropertyChange("Col7");
                }
            }
        }
        public string Col8
        {
            get { return _col8; }
            set
            {
                if (this._col8 != value)
                {
                    _col8 = value;
                    this.firePropertyChange("Col8");
                }
            }
        }
        public string Col9
        {
            get { return _col9; }
            set
            {
                if (this._col9 != value)
                {
                    _col9 = value;
                    this.firePropertyChange("Col9");
                }
            }
        }
        public string Col10
        {
            get { return _col10; }
            set
            {
                if (this._col10 != value)
                {
                    _col10 = value;
                    this.firePropertyChange("Col10");
                }
            }
        }


        public string Col11
        {
            get { return _col11; }
            set
            {
                if (this._col11 != value)
                {
                    _col11 = value;
                    this.firePropertyChange("Col11");
                }
            }
        }

        public string Col12
        {
            get { return _col12; }
            set
            {
                if (this._col12 != value)
                {
                    _col12 = value;
                    this.firePropertyChange("Col12");
                }
            }
        }


        public string Col13
        {
            get { return _col13; }
            set
            {
                if (this._col13 != value)
                {
                    _col13 = value;
                    this.firePropertyChange("Col13");
                }
            }
        }



        public string Col14
        {
            get { return _col14; }
            set
            {
                if (this._col14 != value)
                {
                    _col14 = value;
                    this.firePropertyChange("Col14");
                }
            }
        }


        public string Col15
        {
            get { return _col15; }
            set
            {
                if (this._col15 != value)
                {
                    _col15 = value;
                    this.firePropertyChange("Col15");
                }
            }
        }

        public string Col16
        {
            get { return _col16; }
            set
            {
                if (this._col16 != value)
                {
                    _col16 = value;
                    this.firePropertyChange("Col16");
                }
            }
        }

        public string Col17
        {
            get { return _col17; }
            set
            {
                if (this._col17 != value)
                {
                    _col17 = value;
                    this.firePropertyChange("Col17");
                }
            }
        }

        public string Col18
        {
            get { return _col18; }
            set
            {
                if (this._col18 != value)
                {
                    _col18 = value;
                    this.firePropertyChange("Col18");
                }
            }
        }


        public string Col19
        {
            get { return _col19; }
            set
            {
                if (this._col19 != value)
                {
                    _col19 = value;
                    this.firePropertyChange("Col19");
                }
            }
        }

        public string Col20
        {
            get { return _col20; }
            set
            {
                if (this._col20 != value)
                {
                    _col20 = value;
                    this.firePropertyChange("Col20");
                }
            }
        }

        public string Col21
        {
            get { return _col21; }
            set
            {
                if (this._col21 != value)
                {
                    _col21 = value;
                    this.firePropertyChange("Col21");
                }
            }
        }

        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {
            base.resetObjects();
        }
        #endregion
        #region ICatalog Members
        ArrayList ICatalog.getPropertyChanges()
        {
            return base._propChanges;
        }
        void ICatalog.markAsSaved()
        {
            base.markCompAsSaved();
        }
        public void markAsUnSaved()
        {
            base.MarkCompAsUnSaved();
        }
        #endregion
   
    }
}
