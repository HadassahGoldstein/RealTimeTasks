using RealTimeTasks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeTasks.Web.Models
{
    public class SignupViewModel : User
    {
        public string Password { get; set; }
    }
}
