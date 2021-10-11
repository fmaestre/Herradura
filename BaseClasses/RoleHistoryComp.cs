using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Herradura.Lib.core;
namespace Herradura.Lib.Components
{
    /// <summary>
    /// Mapea la tabla ROLE_HISTORY en la Base de Datos
    /// </summary>
    [Serializable()]
    [Table("ROLE_HISTORY")]
    public partial class RoleHistoryComp : BaseComponentClass, ICatalog
    {
        #region Class Instance Variables
        private int _id = 0;
        private string _role = string.Empty;
        private string _obj_id = string.Empty;
        private Nullable<DateTime> _created_date = null;
        private string _action = string.Empty;
        #endregion
        #region Contructors
        public RoleHistoryComp() : base()
        {
        }
        #endregion
        #region Public Interface
        #region properties
        [Field("id", "Id", true, enmDataTypes.intType, true, true, true)]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.firePropertyChange("Id");
            }
        }
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
        [Field("created_date", "Created_date", false, enmDataTypes.DateTimeType, true, true)]
        public Nullable<DateTime> Created_date
        {
            get { return _created_date; }
            set
            {
                _created_date = value;
                this.firePropertyChange("Created_date");
            }
        }
        [Field("action", "Action", false, enmDataTypes.stringType, true, true)]
        public string Action
        {
            get { return _action; }
            set
            {
                _action = value;
                this.firePropertyChange("Action");
            }
        }
        #endregion //termina properties
        #region reset objects
        public override void resetObjects()
        {
            _id = 0;
            _role = string.Empty;
            _obj_id = string.Empty;
            _created_date = null;
            _action = string.Empty;
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
"Role":"_Role",
"Obj_id":"_Obj_id",
"Created_date":"_Created_date",
"Action":"_Action"


"_Id":"Id",
"_Role":"Role",
"_Obj_id":"Obj_id",
"_Created_date":"Created_date",
"_Action":"Action"


<input id = "_Id" type = "text" size = "20" maxlength = "12" placeholder = "" /> 
<input id = "_Role" type = "text" size = "20" maxlength = "50" placeholder = "" /> 
<input id = "_Obj_id" type = "text" size = "20" maxlength = "30" placeholder = "" /> 
<input id = "_Created_date" type = "date" size = "20" maxlength = "12" placeholder = "" /> 
<input id = "_Action" type = "text" size = "20" maxlength = "-1" placeholder = "" /> 



<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 
<div class="label" style="width:10em;">:</div> 



"c0":"Id",
"c1":"Role",
"c2":"Obj_id",
"c3":"Created_date",
"c4":"Action",



<div id = "panel" >

</div>
<div id = "box">
     <h3> ROLE_HISTORY </h3>
     <table>
         <thead>
             <tr id = "t0r-1" >
                  <th id = "t0c-1" width = "5px" > *</th>
                  <th  id ="t0c0" width = "30px" ></th>
                  <th  id ="t0c1" width = "30px" ></th>
                  <th  id ="t0c2" width = "30px" ></th>
                  <th  id ="t0c3" width = "30px" ></th>
                  <th  id ="t0c4" width = "30px" ></th>
             </tr>
        </thead>
        <!--<script type="text/javascript"> tbody('{"table":"t0","cols":5,"cedit":"","rows":16,"w0":40,"w1":300,"selectable":"false","position":"relative","maxlength0":3,"maxlength1":40,"selectable":"true"}');</script> -->
     </table>
<!--<script type="include">include{"file":"ipagerd.html"}</script> -->
</div>
<!--<script type="events" >
{
    "oninit": {
        "fire": "onenter"
    },
    "onenter": {
        "blog": "GenericBL",
        "func": "getRoleHistoryComp",
        "param": {
            "RoleHistoryComp": {
                "Role": "%"
           }
        },
        "fetchs":{
            "t0":{
                 "c0":"Id",
                 "c1":"Role",
                 "c2":"Obj_id",
                 "c3":"Created_date",
                 "c4":"Action"
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
             "func": "insertRoleHistoryComp",
             "param": {
                  "RoleHistoryComp": {
                          "Id":"t0c0",
                          "Role":"t0c1",
                          "Obj_id":"t0c2",
                          "Created_date":"t0c3",
                          "Action":"t0c4"
                    }
               }
        }
    },
    "ondelete":{
             "t0":{
             "blog": "GenericBL",
             "func": "deleteRoleHistoryComp",
             "param": {
                 "RoleHistoryComp": {
                          "Id":"t0c0",
                          "Role":"t0c1",
                          "Obj_id":"t0c2",
                          "Created_date":"t0c3",
                          "Action":"t0c4"
              }
                 }
          }
    },
    "onupdate":{
        "t0":{
            "blog": "GenericBL",
            "func": "updateRoleHistoryComp",
            "param": {
                 "RoleHistoryComp": {
                          "Id":"t0c0",
                          "Role":"t0c1",
                          "Obj_id":"t0c2",
                          "Created_date":"t0c3",
                          "Action":"t0c4"
                     }
                 }
            }
        }
}
</script> -->


public List<RoleHistoryComp> getRoleHistoryComp(RoleHistoryComp comp)
{
        var x = PortalData.GenericDAL.getList(comp);
        return x.ToList(); 
}

public RoleHistoryComp insertRoleHistoryComp(RoleHistoryComp comp)
{
        var x = PortalData.GenericDAL.insertFULLItemGetIdentity(comp) as RoleHistoryComp;
        return x; 
}

public void updateRoleHistoryComp(RoleHistoryComp comp) 
{ 
        PortalData.GenericDAL.updateItem(comp);
}  
 
 public void deleteRoleHistoryComp(RoleHistoryComp comp)
{
        PortalData.GenericDAL.DeleteItemWithPK(comp);
}


*/