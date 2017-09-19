using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.ServicoDireto.Model.InfraStructure
{
    public class Permission
    {
        public Permission()
        {
            this.ControllerInfo = new InfoType();
        }
        public InfoType ControllerInfo { get; set; }
        public InfoType[] ActionInfo { get; set; }
    }

    public class InfoType
    {
        public string Name { get; set; } 
        public RoleType[] Roles { get; set; } 
        public ControlPermissionType[] Controls { get; set; }
    }

    public class RoleType
    {
        public RoleType()
        {
            this.PermissionType = Enums.PermissionType.Permissive;
        }
        public string Name { get; set; }
        public Enums.PermissionType PermissionType { get; set; }
    }

    public class ControlPermissionType
    {
        public string Name { get; set; }
        public Enums.PermissionType PermissionType { get; set; }
        public Enums.ControlType ControlType { get; set; }
    }
}
