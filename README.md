# Herradura
# ORM Data Entity 
# Adding herradura to your project. 
#####################################################################################
using System.Data;
using Herradura.Lib.Portal.DAL;

namespace CNR.DAL
{
    public class ODALSQLProvider : GenericDALSQLProvider
    {
        IDbTransaction _txn;
        public ODALSQLProvider(string conn)
            : base(conn)
        {
        }
        public ODALSQLProvider(IDbTransaction txn, string conn)
            : base(conn)
        {
            _txn = txn;
        }
   
    }
}
#####################################################################################
using System.Data;
using Herradura.Lib.Portal.DAL;

namespace CNR.DAL
{
    public static class PortalData
    {
        public static GenericDALSQLProvider GenericDAL
        {
            get { return new GenericDALSQLProvider("CNR"); }
        }
        public static ODALSQLProvider ODAL
        {
            get { return new ODALSQLProvider("CNR"); }
        }
        public static ODALSQLProvider ODALX(IDbTransaction _txn)
        {
            return new ODALSQLProvider(_txn, "CNR");
        }

    }
}

#####################################################################################
#Calling DAL from BL
public List<InsuranceComp> getInsurances(InsuranceComp comp)
{
    comp.Insurancename = comp.Insurancename.add_like();
    comp.Enabled = true;
    var x = PortalData.GenericDAL.getList(comp);

    return x.OrderBy(z => z.Insurancename).ToList();
}

#####################################################################################
#A Component
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;
namespace CNR.Lib.Components
{
    /// <summary>
    /// Mapea la tabla Insurances en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("Insurances")]
    public partial class InsuranceComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private string _insurancename = string.Empty;
        private bool _needcard = false;
        private string _address = string.Empty;
        private string _state = string.Empty;
        private string _city = string.Empty;
        private string _zipcode = string.Empty;
        private string _phone = string.Empty;
        private string _contact1 = string.Empty;
        private string _contact2 = string.Empty;
        private string _webpage = string.Empty;
        private int _insuranceid = 0;
        private bool _mxaddressrequired = false;
        private bool _enabled = false;
        #endregion
        #region Contructors
        public InsuranceComp() : base()
        {
        }
        #endregion
        #region Public Interface
        #region properties
        [Field("insuranceName", "Insurancename", true, enmDataTypes.stringType, true, true)]
        public string Insurancename
        {
            get { return _insurancename; }
            set
            {
                _insurancename = value;
                this.firePropertyChange("Insurancename");
            }
        }
        [Field("needCard", "Needcard", false, enmDataTypes.boolType, true, true)]
        public bool Needcard
        {
            get { return _needcard; }
            set
            {
                _needcard = value;
                this.firePropertyChange("Needcard");
            }
        }
        [Field("address", "Address", false, enmDataTypes.stringType, true, true)]
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                this.firePropertyChange("Address");
            }
        }
        [Field("state", "State", false, enmDataTypes.stringType, true, true)]
        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                this.firePropertyChange("State");
            }
        }
        [Field("city", "City", false, enmDataTypes.stringType, true, true)]
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                this.firePropertyChange("City");
            }
        }
        [Field("zipcode", "Zipcode", false, enmDataTypes.stringType, true, true)]
        public string Zipcode
        {
            get { return _zipcode; }
            set
            {
                _zipcode = value;
                this.firePropertyChange("Zipcode");
            }
        }
        [Field("phone", "Phone", false, enmDataTypes.stringType, true, true)]
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                this.firePropertyChange("Phone");
            }
        }
        [Field("contact1", "Contact1", false, enmDataTypes.stringType, true, true)]
        public string Contact1
        {
            get { return _contact1; }
            set
            {
                _contact1 = value;
                this.firePropertyChange("Contact1");
            }
        }
        [Field("contact2", "Contact2", false, enmDataTypes.stringType, true, true)]
        public string Contact2
        {
            get { return _contact2; }
            set
            {
                _contact2 = value;
                this.firePropertyChange("Contact2");
            }
        }
        [Field("webpage", "Webpage", false, enmDataTypes.stringType, true, true)]
        public string Webpage
        {
            get { return _webpage; }
            set
            {
                _webpage = value;
                this.firePropertyChange("Webpage");
            }
        }
        [Field("insuranceId", "Insuranceid", false, enmDataTypes.intType, true, true, true)]
        public int Insuranceid
        {
            get { return _insuranceid; }
            set
            {
                _insuranceid = value;
                this.firePropertyChange("Insuranceid");
            }
        }
        [Field("mxAddressRequired", "Mxaddressrequired", false, enmDataTypes.boolType, true, true)]
        public bool Mxaddressrequired
        {
            get { return _mxaddressrequired; }
            set
            {
                _mxaddressrequired = value;
                this.firePropertyChange("Mxaddressrequired");
            }
        }
        [Field("Enabled", "Enabled", false, enmDataTypes.boolType, true, true)]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                this.firePropertyChange("Enabled");
            }
        }
        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {
            _insurancename = string.Empty;
            _needcard = false;
            _address = string.Empty;
            _state = string.Empty;
            _city = string.Empty;
            _zipcode = string.Empty;
            _phone = string.Empty;
            _contact1 = string.Empty;
            _contact2 = string.Empty;
            _webpage = string.Empty;
            _insuranceid = 0;
            _mxaddressrequired = false;
            _enabled = false;
            base.resetObjects();
        }
        #endregion
        #region Methods
        #endregion //Termina Metodos
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
        #endregion //termina public interface
        #region Private Interface
        #endregion //Termina Private Interface
    }
}
