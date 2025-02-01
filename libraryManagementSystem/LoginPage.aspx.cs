using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace libraryManagementSystem
{
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // You can add any initialization code here
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

          
            if (ValidateUser(username, password))
            {
                
                FormsAuthentication.SetAuthCookie(username, false);

                
                Response.Redirect("BookPage.aspx");
            }
            else
            {
                lblErrorMessage.Text = "Invalid username or password.";
                lblErrorMessage.Visible = true;
            }
        }

        private bool ValidateUser(string username, string password)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password ;";
            SqlParameter[] parameters = {
                new SqlParameter("@Email", username),
                new SqlParameter("@Password", password) 
            };

            try
            {
                int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }

        }
    }
}