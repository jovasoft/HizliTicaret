using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.ViewComponents
{
    public class UserNav : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
