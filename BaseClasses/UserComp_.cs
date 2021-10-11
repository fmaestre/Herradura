using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Herradura.Lib.core;
using Herradura.Lib.Portal.DAL;
namespace Herradura.Lib.Components
{
   
    public partial class UserComp : BaseComponentClass, ICatalog
    {
        public DateTime? LoginTime { get; set; }

        public List<UserComp> getUserComp(UserComp comp)
        {
            var x = PortalCore.GenericDAL.getList(comp);
            return x.ToList();
        }

        public void insertUserComp(UserComp comp)
        {
            PortalCore.GenericDAL.insertFULLItemGetIdentity(comp) ;
        }

        public void ChangeOrg()
        {
            string org = this.Idsucursal;
            string login = this.Usuario;

            var user = (UserComp)PortalCore.GenericDAL.getItemNonPK(new UserComp { Usuario = login });

            user.Idsucursal = org;
            user.Id = user.Id;

            updateUserComp(user);
        }

        private void updateUserComp(UserComp comp)
        {
             PortalCore.GenericDAL.updateItem(comp);            
        }

        public void deleteUserComp(UserComp comp)
        {
            PortalCore.GenericDAL.DeleteItemWithPK(comp);            
        }
    }
}

/*
"Id":"_Id",
"Idsucursal":"_Idsucursal",
"Tipo":"_Tipo",
"Nombre":"_Nombre",
"Usuario":"_Usuario",
"Password":"_Password",
"Menu":"_Menu",
"Expire":"_Expire",
"Until":"_Until"


"_Id":"Id",
"_Idsucursal":"Idsucursal",
"_Tipo":"Tipo",
"_Nombre":"Nombre",
"_Usuario":"Usuario",
"_Password":"Password",
"_Menu":"Menu",
"_Expire":"Expire",
"_Until":"Until"


<input id = "_Id" type = "text" size = "20" maxlength = "12" placeholder = "" /> 
<input id = "_Idsucursal" type = "text" size = "20" maxlength = "3" placeholder = "" /> 
<input id = "_Tipo" type = "text" size = "20" maxlength = "3" placeholder = "" /> 
<input id = "_Nombre" type = "text" size = "20" maxlength = "80" placeholder = "" /> 
<input id = "_Usuario" type = "text" size = "20" maxlength = "50" placeholder = "" /> 
<input id = "_Password" type = "text" size = "20" maxlength = "50" placeholder = "" /> 
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
<div class="label" style="width:10em;">:</div> 



"c0":"Id",
"c1":"Idsucursal",
"c2":"Tipo",
"c3":"Nombre",
"c4":"Usuario",
"c5":"Password",
"c6":"Menu",
"c7":"Expire",
"c8":"Until",



<div id = "panel" >

</div>
<div id = "box">
     <h3> FIZ_EMPLEADOS </h3>
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
                  <th  id = "t0c6" width = "30px" >  </th>
                  <th  id = "t0c7" width = "30px" >  </th>
                  <th  id = "t0c8" width = "30px" >  </th>
             </tr>
        </thead>
        < !--< script type = "text /javascript" > tbody('{"table":"t0","cols":9,"cedit":"","rows":16,"w0":40,"w1":300,"selectable":"false","position":"relative","maxlength0":3,"maxlength1":40,"selectable":"true"}');</script> -->
     </table>
<!--<script type = "include" > include{ "file":"ipagerd.html"}</script > -->
</div>
< !--< script type = "events" >
{
    "onenter": {
        "blog": "Generic",
        "func": "getUserComp",
        "param": {
            "UserComp": {
                "Idsucursal": "%"
           }
        },
        "fetchs":{
            "t0":{
                 "c0":"Id",
                 "c1":"Idsucursal",
                 "c2":"Tipo",
                 "c3":"Nombre",
                 "c4":"Usuario",
                 "c5":"Password",
                 "c6":"Menu",
                 "c7":"Expire",
                 "c8":"Until"
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
             "blog": "Generic",
             "func": "insertUserComp",
             "param": {
                  "UserComp": {
                          "Id":"t0c0",
                          "Idsucursal":"t0c1",
                          "Tipo":"t0c2",
                          "Nombre":"t0c3",
                          "Usuario":"t0c4",
                          "Password":"t0c5",
                          "Menu":"t0c6",
                          "Expire":"t0c7",
                          "Until":"t0c8"
                    }
               }
        }
    },
    "ondelete":{
             "t0":{
             "blog": "Generic",
             "func": "deleteUserComp",
             "param": {
                 "UserComp": {
                          "Id":"t0c0",
                          "Idsucursal":"t0c1",
                          "Tipo":"t0c2",
                          "Nombre":"t0c3",
                          "Usuario":"t0c4",
                          "Password":"t0c5",
                          "Menu":"t0c6",
                          "Expire":"t0c7",
                          "Until":"t0c8"
              }
                 }
          }
    },
    "onupdate":{
        "t0":{
            "blog": "Generic",
            "func": "updateUserComp",
            "param": {
                 "UserComp": {
                          "Id":"t0c0",
                          "Idsucursal":"t0c1",
                          "Tipo":"t0c2",
                          "Nombre":"t0c3",
                          "Usuario":"t0c4",
                          "Password":"t0c5",
                          "Menu":"t0c6",
                          "Expire":"t0c7",
                          "Until":"t0c8"
                     }
                 }
            }
        }
}
</ script > -->


public List<UserComp> getUserComp(UserComp comp)
{
        var x = PortalData.GenericDAL.getList(comp);
        return x.ToList(); 
}

public List<UserComp> insertUserComp(UserComp comp)
{
        var x = PortalData.GenericDAL.insertFULLItemGetIdentity(comp) as UserComp;
        return x.ToList(); 
}

public List<UserComp> updateUserComp(UserComp comp)
{
        var x = PortalData.GenericDAL.updateItem(comp) as UserComp;
        return x.ToList(); 
}

public List<UserComp> deleteUserComp(UserComp comp)
{
        var x = PortalData.GenericDAL.DeleteItemWithPK(comp) as UserComp;
        return x.ToList(); 
}


*/
