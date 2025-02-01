using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace libraryManagementSystem
{
    public partial class RegistrationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // You can add any initialization code here
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Validate inputs
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                lblErrorMessage.Text = "All fields are required.";
                lblErrorMessage.Visible = true;
                return;
            }

            if (password != confirmPassword)
            {
                lblErrorMessage.Text = "Passwords do not match.";
                lblErrorMessage.Visible = true;
                return;
            }

            // Insert user into the database
            try
            {
                string query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                SqlParameter[] parameters = {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Password", password) // In a real app, hash the password
                };

                int rowsAffected = DatabaseHelper.ExecuteQuery(query, parameters);

                if (rowsAffected > 0)
                {
                    lblErrorMessage.Text = "Registration successful!";
                    lblErrorMessage.ForeColor = System.Drawing.Color.Green;
                    lblErrorMessage.Visible = true;

                    // Clear the form
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtPassword.Text = "";
                    txtConfirmPassword.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error: " + ex.Message;
                lblErrorMessage.Visible = true;
            }
        }

    }
}