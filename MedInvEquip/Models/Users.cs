using Microsoft.AspNetCore.Identity;

namespace MedInvEquip.Models
{
    public class Users: IdentityUser
    {
        public string FullName { get; set; }


    }
}
