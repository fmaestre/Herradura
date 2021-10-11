using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;
namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla vwUsers en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("vwUsers")]
    public partial class vwUsersComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private int _id = 0;
        private string _idsucursal = string.Empty;
        private string _usuario = string.Empty;
        private string _nombre = string.Empty;
        private string _tipo = string.Empty;
        private string _menu = string.Empty;
        private bool _expire = false;
        private Nullable<DateTime> _until = null;
        #endregion
        #region Contructors
        public vwUsersComp() : base()
        {
        }
        #endregion
        #region Public Interface
        #region properties
        [Field("id", "Id", false, enmDataTypes.intType, true, true)]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.firePropertyChange("Id");
            }
        }
        [Field("idsucursal", "Idsucursal", false, enmDataTypes.stringType, true, true)]
        public string Idsucursal
        {
            get { return _idsucursal; }
            set
            {
                _idsucursal = value;
                this.firePropertyChange("Idsucursal");
            }
        }
        [Field("usuario", "Usuario", false, enmDataTypes.stringType, true, true)]
        public string Usuario
        {
            get { return _usuario; }
            set
            {
                _usuario = value;
                this.firePropertyChange("Usuario");
            }
        }
        [Field("nombre", "Nombre", false, enmDataTypes.stringType, true, true)]
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                this.firePropertyChange("Nombre");
            }
        }
        [Field("tipo", "Tipo", false, enmDataTypes.stringType, true, true)]
        public string Tipo
        {
            get { return _tipo; }
            set
            {
                _tipo = value;
                this.firePropertyChange("Tipo");
            }
        }
        [Field("menu", "Menu", false, enmDataTypes.stringType, true, true)]
        public string Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                this.firePropertyChange("Menu");
            }
        }
        [Field("expire", "Expire", false, enmDataTypes.boolType, true, true)]
        public bool Expire
        {
            get { return _expire; }
            set
            {
                _expire = value;
                this.firePropertyChange("Expire");
            }
        }
        [Field("until", "Until", false, enmDataTypes.DateTimeType, true, true)]
        public Nullable<DateTime> Until
        {
            get { return _until; }
            set
            {
                _until = value;
                this.firePropertyChange("Until");
            }
        }
        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {
            _id = 0;
            _idsucursal = string.Empty;
            _usuario = string.Empty;
            _nombre = string.Empty;
            _tipo = string.Empty;
            _menu = string.Empty;
            _expire = false;
            _until = null;
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

/*
"Id":"_Id",
"Idsucursal":"_Idsucursal",
"Usuario":"_Usuario",
"Nombre":"_Nombre",
"Tipo":"_Tipo",
"Menu":"_Menu",
"Expire":"_Expire",
"Until":"_Until"


"_Id":"Id",
"_Idsucursal":"Idsucursal",
"_Usuario":"Usuario",
"_Nombre":"Nombre",
"_Tipo":"Tipo",
"_Menu":"Menu",
"_Expire":"Expire",
"_Until":"Until"


<input id = "_Id" type = "text" size = "20" maxlength = "12" placeholder = "" /> 
<input id = "_Idsucursal" type = "text" size = "20" maxlength = "3" placeholder = "" /> 
<input id = "_Usuario" type = "text" size = "20" maxlength = "50" placeholder = "" /> 
<input id = "_Nombre" type = "text" size = "20" maxlength = "80" placeholder = "" /> 
<input id = "_Tipo" type = "text" size = "20" maxlength = "3" placeholder = "" /> 
<input id = "_Menu" type = "text" size = "20" maxlength = "4" placeholder = "" /> 
<input id = "_Expire" type = "checkbox" size = "20" maxlength = "12" placeholder = "" /> 
<input id = "_Until" type = "date" size = "20" maxlength = "12" placeholder = "" /> 



<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 



"c0":"Id",
"c1":"Idsucursal",
"c2":"Usuario",
"c3":"Nombre",
"c4":"Tipo",
"c5":"Menu",
"c6":"Expire",
"c7":"Until",



<div id = "panel" >

</div>
<div id = "box">
     <h3> vwUsers </h3>
     <table>
         <thead>
             <tr id = "t0r-1" >
                  <th id = "t0c-1" width = "5px" > *</th>
                  <th  id ="t0c0" width = "30px" ></th>
                  <th  id ="t0c1" width = "30px" ></th>
                  <th  id ="t0c2" width = "30px" ></th>
                  <th  id ="t0c3" width = "30px" ></th>
                  <th  id ="t0c4" width = "30px" ></th>
                  <th  id ="t0c5" width = "30px" ></th>
                  <th  id ="t0c6" width = "30px" ></th>
                  <th  id ="t0c7" width = "30px" ></th>
             </tr>
        </thead>
        <!--<script type="text/javascript"> tbody('{"table":"t0","cols":8,"cedit":"","rows":16,"w0":40,"w1":300,"selectable":"false","position":"relative","maxlength0":3,"maxlength1":40,"selectable":"true"}');</script> -->
     </table>
<!--<script type="include">include{"file":"ipagerd.html"}</script> -->
</div>
<!--<script type="events" >
{
    "onenter": {
        "blog": "GenericBL",
        "func": "getvwUsersComp",
        "param": {
            "vwUsersComp": {
                "Idsucursal": "%"
           }
        },
        "fetchs":{
            "t0":{
                 "c0":"Id",
                 "c1":"Idsucursal",
                 "c2":"Usuario",
                 "c3":"Nombre",
                 "c4":"Tipo",
                 "c5":"Menu",
                 "c6":"Expire",
                 "c7":"Until"
                }
            }
    },
    "actions":{
             "t0c1":{
                       "dispatchs":"t0c0,t0c1"
                      },
             "t0c0":{
                       "dispatchs":"t0c0,t0c1"
                      }
   },
    "oninsert":{
      "t0":{
             "blog": "GenericBL",
             "func": "insertvwUsersComp",
             "param": {
                  "vwUsersComp": {
                          "Id":"t0c0",
                          "Idsucursal":"t0c1",
                          "Usuario":"t0c2",
                          "Nombre":"t0c3",
                          "Tipo":"t0c4",
                          "Menu":"t0c5",
                          "Expire":"t0c6",
                          "Until":"t0c7"
                    }
               }
        }
    },
    "ondelete":{
             "t0":{
             "blog": "GenericBL",
             "func": "deletevwUsersComp",
             "param": {
                 "vwUsersComp": {
                          "Id":"t0c0",
                          "Idsucursal":"t0c1",
                          "Usuario":"t0c2",
                          "Nombre":"t0c3",
                          "Tipo":"t0c4",
                          "Menu":"t0c5",
                          "Expire":"t0c6",
                          "Until":"t0c7"
              }
                 }
          }
    },
    "onupdate":{
        "t0":{
            "blog": "GenericBL",
            "func": "updatevwUsersComp",
            "param": {
                 "vwUsersComp": {
                          "Id":"t0c0",
                          "Idsucursal":"t0c1",
                          "Usuario":"t0c2",
                          "Nombre":"t0c3",
                          "Tipo":"t0c4",
                          "Menu":"t0c5",
                          "Expire":"t0c6",
                          "Until":"t0c7"
                     }
                 }
            }
        }
}
</script> -->


public List<vwUsersComp> getvwUsersComp(vwUsersComp comp)
{
        var x = PortalData.GenericDAL.getList(comp);
        return x.ToList(); 
}

public List<vwUsersComp> insertvwUsersComp(vwUsersComp comp)
{
        var x = PortalData.GenericDAL.insertFULLItemGetIdentity(comp) as vwUsersComp;
        return x.ToList(); 
}

public List<vwUsersComp> updatevwUsersComp(vwUsersComp comp) 
{ 
        var x = PortalData.GenericDAL.updateItem(comp) as vwUsersComp;
        return x.ToList(); 
}  
 
 public List<vwUsersComp> deletevwUsersComp(vwUsersComp comp)
{
        var x = PortalData.GenericDAL.DeleteItemWithPK(comp) as vwUsersComp;
        return x.ToList(); 
}


*/
