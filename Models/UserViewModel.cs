using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YTUsageViewer.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTime? LockoutEndDate { get; set; }

        public List<UserRoleViewModel> Roles { get; set; }
    }

    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }

}