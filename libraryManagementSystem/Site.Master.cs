using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace libraryManagementSystem
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is authenticated
            if (Request.IsAuthenticated)
            {
                // Optionally, you can display the username or other user-specific information
            }
        }

        protected void lbLogout_Click(object sender, EventArgs e)
        {
            // Sign out the user
            FormsAuthentication.SignOut();

            // Redirect to the login page or home page
            Response.Redirect("~/Login.aspx");
        }
    }
}