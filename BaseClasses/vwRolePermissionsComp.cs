using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;
namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla vwRolePermissions en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("vwRolePermissions")]
    public partial class vwRolePermissionsComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private string _role = string.Empty;
        private string _obj_id = string.Empty;
        private bool _ins = false;
        private bool _upd = false;
        private bool _del = false;
        private bool _sel = false;
        #endregion
        #region Contructors
        public vwRolePermissionsComp() : base()
        {
        }
        #endregion
        #region Public Interface
        #region properties
        [Field("role", "Role", false, enmDataTypes.stringType, true, true)]
        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                this.firePropertyChange("Role");
            }
        }
        [Field("obj_id", "Obj_id", false, enmDataTypes.stringType, true, true)]
        public string Obj_id
        {
            get { return _obj_id; }
            set
            {
                _obj_id = value;
                this.firePropertyChange("Obj_id");
            }
        }
        [Field("ins", "Ins", false, enmDataTypes.boolType, true, true)]
        public bool Ins
        {
            get { return _ins; }
            set
            {
                _ins = value;
                this.firePropertyChange("Ins");
            }
        }
        [Field("upd", "Upd", false, enmDataTypes.boolType, true, true)]
        public bool Upd
        {
            get { return _upd; }
            set
            {
                _upd = value;
                this.firePropertyChange("Upd");
            }
        }
        [Field("del", "Del", false, enmDataTypes.boolType, true, true)]
        public bool Del
        {
            get { return _del; }
            set
            {
                _del = value;
                this.firePropertyChange("Del");
            }
        }
        [Field("sel", "Sel", false, enmDataTypes.boolType, true, true)]
        public bool Sel
        {
            get { return _sel; }
            set
            {
                _sel = value;
                this.firePropertyChange("Sel");
            }
        }
        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {
            _role = string.Empty;
            _obj_id = string.Empty;
            _ins = false;
            _upd = false;
            _del = false;
            _sel = false;
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
"Role":"_Role",
"Obj_id":"_Obj_id",
"Ins":"_Ins",
"Upd":"_Upd",
"Del":"_Del",
"Sel":"_Sel"


"_Role":"Role",
"_Obj_id":"Obj_id",
"_Ins":"Ins",
"_Upd":"Upd",
"_Del":"Del",
"_Sel":"Sel"


<input id = "_Role" type = "text" size = "20" maxlength = "30" placeholder = "" /> 
<input id = "_Obj_id" type = "text" size = "20" maxlength = "30" placeholder = "" /> 
<input id = "_Ins" type = "checkbox" size = "20" maxlength = "12" placeholder = "" /> 
<input id = "_Upd" type = "checkbox" size = "20" maxlength = "12" placeholder = "" /> 
<input id = "_Del" type = "checkbox" size = "20" maxlength = "12" placeholder = "" /> 
<input id = "_Sel" type = "checkbox" size = "20" maxlength = "12" placeholder = "" /> 



<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 



"c0":"Role",
"c1":"Obj_id",
"c2":"Ins",
"c3":"Upd",
"c4":"Del",
"c5":"Sel",



<div id = "panel" >

</div>
<div id = "box">
     <h3> vwRolePermissions </h3>
     <table>
         <thead>
             <tr id = "t0r-1" >
                  <th id = "t0c-1" width = "5px" > *</th>
                  <th  id = "t0c0" width = "30px" >  </th>
                  <th  id = "t0c1" width = "30px" >  </th>
                  <th  id = "t0c2" width = "30px" >  </th>
                  <th  id = "t0c3" width = "30px" >  </th>
                  <th  id = "t0c4" width = "30px" >  </th>
                  <th  id = "t0c5" width = "30px" >  </th>
             </tr>
        </thead>
        < !--< script type = "text /javascript" > tbody('{"table":"t0","cols":6,"cedit":"","rows":16,"w0":40,"w1":300,"selectable":"false","position":"relative","maxlength0":3,"maxlength1":40,"selectable":"true"}');</script> -->
     </table>
<!--<script type = "include" > include{ "file":"ipagerd.html"}</script > -->
</div>
< !--< script type = "events" >
{
    "onenter": {
        "blog": "CNRBL",
        "func": "getvwRolePermissionsComp",
        "param": {
            "vwRolePermissionsComp": {
                "Role": "%"
           }
        },
        "fetchs":{
            "t0":{
                 "c0":"Role",
                 "c1":"Obj_id",
                 "c2":"Ins",
                 "c3":"Upd",
                 "c4":"Del",
                 "c5":"Sel"
                }
            }
    },
    "actions":{
             "t0c1":{
                       "dispatchs":"t0c0,t0c1"
                      },
             "t0c0":{
                       "dispatchs":"t0c0,t0c1"
                      },
   },
    "oninsert":{
      "t0":{
             "blog": "CNRBL",
             "func": "insertvwRolePermissionsComp",
             "param": {
                  "vwRolePermissionsComp": {
                          "Role":"t0c0",
                          "Obj_id":"t0c1",
                          "Ins":"t0c2",
                          "Upd":"t0c3",
                          "Del":"t0c4",
                          "Sel":"t0c5"
                    }
               }
        }
    },
    "ondelete":{
             "t0":{
             "blog": "CNRBL",
             "func": "deletevwRolePermissionsComp",
             "param": {
                 "vwRolePermissionsComp": {
                          "Role":"t0c0",
                          "Obj_id":"t0c1",
                          "Ins":"t0c2",
                          "Upd":"t0c3",
                          "Del":"t0c4",
                          "Sel":"t0c5"
              }
                 }
          }
    },
    "onupdate":{
        "t0":{
            "blog": "CNRBL",
            "func": "updatevwRolePermissionsComp",
            "param": {
                 "vwRolePermissionsComp": {
                          "Role":"t0c0",
                          "Obj_id":"t0c1",
                          "Ins":"t0c2",
                          "Upd":"t0c3",
                          "Del":"t0c4",
                          "Sel":"t0c5"
                     }
                 }
            }
        }
}
</ script > -->


public List<vwRolePermissionsComp> getvwRolePermissionsComp(vwRolePermissionsComp comp)
{
        var x = PortalData.GenericDAL.getList(comp);
        return x.ToList(); 
}

public List<vwRolePermissionsComp> insertvwRolePermissionsComp(vwRolePermissionsComp comp)
{
        var x = PortalData.GenericDAL.insertFULLItemGetIdentity(comp) as vwRolePermissionsComp;
        return x.ToList(); 
}

public List<vwRolePermissionsComp> updatevwRolePermissionsComp(vwRolePermissionsComp comp)
{
        var x = PortalData.GenericDAL.updateItem(comp) as vwRolePermissionsComp;
        return x.ToList(); 
}

public List<vwRolePermissionsComp> deletevwRolePermissionsComp(vwRolePermissionsComp comp)
{
        var x = PortalData.GenericDAL.DeleteItemWithPK(comp) as vwRolePermissionsComp;
        return x.ToList(); 
}


*/
