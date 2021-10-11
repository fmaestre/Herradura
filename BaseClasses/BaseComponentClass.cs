using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Herradura.Lib.core
{
    [Serializable()]
    public abstract class BaseComponentClass : System.EventArgs ,IDisposable, IEditableObject, INotifyPropertyChanged//, IDataErrorInfo
    {
        protected bool _isChange = false;
        protected bool _isSaved = false;
        protected bool _isDeleted = false; //Added By Fmaestre 4/9/2009
        //protected MyCustomException _myExceptions = null;
        protected ArrayList _propChanges = null;
        protected bool _isSelected = false; // Esta variable se utiliza para hacer la seleccion en los grids
        

        public BaseComponentClass()
        {
            initMyObjects();          
        }

        public bool IsChange
        {
            get { return _isChange; }
            set { _isChange = true; }
        }

        public string Warning
        {
            get;
            set;
        }

        public string Success
        {
            get;
            set;
        }

        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = true; }
        }

        public bool hasPendingChangesToSave
        {
            get { return !(_isSaved && !IsChange); }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
            }
        }
        public bool IsDeleted  //Added By Fmaestre 4/9/2009
        {
            get { return _isDeleted; }
        }

        public ArrayList PropertyChanges
        {
            get { return _propChanges; }
        }

        //public MyCustomException MyCustomExceptions
        //{
        //    get { return _myExceptions; }
        //    set { _myExceptions = value; }

        //}

        

        //public bool IsValid
        //{
        //    get
        //    {
        //        return _myExceptions.HasErrors;
        //    }

        //}
        

        public void validate_change()
        {
            err.fire(this.IsSaved == true, "object_mustbe_new_to_search, ");
            err.fire(this.PropertyChanges.Count == 0, "no_criteria_to_search, ");
        }

        public string retMsg(string prmMsg)
        {
            _isChange = true;

            return prmMsg + _isChange.ToString();
        }

        #region IDisposable Members

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEditableObject Members

        public void BeginEdit()
        {
            //throw new NotImplementedException();
        }

        public void CancelEdit()
        {
            //throw new NotImplementedException();
        }

        public void EndEdit()
        {
            //throw new NotImplementedException();
        }




        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private string _Error = "record to insert already exists";

        #endregion

        //#region IDataErrorInfo Members

        //public string Error
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //public string this[string columnName]
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //#endregion


        public virtual void initMyObjects()
        {
            _propChanges = new ArrayList();
        }

        public virtual void resetObjects()
        {
            _propChanges.Clear();
            
            _isChange = false;
            _isSaved = false;
        }


        public virtual void validate_to_save()
        {

        }

        public void firePropertyChange(string prmPropertyName)
        {
            this._isChange = true;
            _propChanges.Add(prmPropertyName);
            if (PropertyChanged != null)
            {                                
                PropertyChanged(this, new PropertyChangedEventArgs(prmPropertyName));
            }
        }

        public virtual void markCompAsSaved()
        {
            _isSaved = true;
            _isChange = false;
            _propChanges.Clear();
                               
        }

        public virtual void MarkCompAsUnSaved()
        {
            _isSaved = false;
        }

        public virtual void markCompAsDeleted()
        {
            _isDeleted = true;
        } //Added By Fmaestre 4/9/2009

        public virtual void MarkCompAsUnDeleted() //Added By Fmaestre 4/9/2009
        {
            _isDeleted = false;
        }

        public virtual void clearPropertyChange(string prmPropertyName)
        {
            _propChanges.Remove(prmPropertyName);
        }

        //public virtual void addError(CustomError prmError)
        //{
        //    this._myExceptions.addError(prmError);
        //}

        public object GetClone()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            ms.Flush();
            ms.Position = 0;            
            return bf.Deserialize(ms);
        }


        public string Caller
        {
            get;
            set;
        }

        public string CurrentUser
        {
            get;
            set;
        }


        public string Error
        {
            get
            {
                return _Error;
            }
            set
            {
                _Error = value;
            }
        }

        public string Password { get; set; }
    }
}
