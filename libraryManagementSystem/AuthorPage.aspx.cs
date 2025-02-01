using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace libraryManagementSystem
{
    public partial class AuthorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        // Bind data to GridView
        private void BindGridView(string sortByExpression = "", string sortDirection = "ASC")
        {
            string query = @"
            SELECT 
                AuthorID,
                FirstName,
                LastName,
                Bio
            FROM 
                Author";

            DataTable dt = DatabaseHelper.GetData(query);

            if (!string.IsNullOrEmpty(sortByExpression))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = sortByExpression + " " + sortDirection;
                GridViewAuthors.DataSource = dv;
            }
            else
            {
                GridViewAuthors.DataSource = dt;
            }

            GridViewAuthors.DataBind();
        }
        protected void GridViewAuthors_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridViewAuthors.PageIndex = e.NewPageIndex;
            BindGridView();
        }
        protected void GridViewAuthors_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortDirection = "ASC";

            if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
            {
                if ((string)ViewState["SortExpression"] == sortExpression)
                {
                    sortDirection = ((string)ViewState["SortDirection"] == "ASC") ? "DESC" : "ASC";
                }
            }

            ViewState["SortExpression"] = sortExpression;
            ViewState["SortDirection"] = sortDirection;

            BindGridView(sortExpression, sortDirection);
        }

        protected void GridViewBooks_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridViewAuthors.PageIndex = e.NewPageIndex;
            BindGridView();
        }
       
        protected void btnAddAuthor_Click(object sender, EventArgs e)
        {
            addAuthorForm.Style["display"] = "block";
        }

       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Author (FirstName, LastName, Bio) VALUES (@FirstName, @LastName, @Bio)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FirstName", txtFirstName.Text),
                    new SqlParameter("@LastName", txtLastName.Text),
                    new SqlParameter("@Bio", txtBio.Text)
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

               
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtBio.Text = "";

                
                addAuthorForm.Style["display"] = "none";

                
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            addAuthorForm.Style["display"] = "none";
        }

      
        protected void GridViewAuthors_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewAuthors.EditIndex = e.NewEditIndex;
            BindGridView();
        }

       
        protected void GridViewAuthors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewAuthors.EditIndex = -1;
            BindGridView();
        }

       
        protected void GridViewAuthors_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int authorID = Convert.ToInt32(GridViewAuthors.DataKeys[e.RowIndex].Value);
                GridViewRow row = GridViewAuthors.Rows[e.RowIndex];

                string firstName = (row.Cells[1].Controls[0] as TextBox).Text;
                string lastName = (row.Cells[2].Controls[0] as TextBox).Text;
                string bio = (row.Cells[3].Controls[0] as TextBox).Text;

                string query = "UPDATE Author SET FirstName = @FirstName, LastName = @LastName, Bio = @Bio WHERE AuthorID = @AuthorID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FirstName", firstName),
                    new SqlParameter("@LastName", lastName),
                    new SqlParameter("@Bio", bio),
                    new SqlParameter("@AuthorID", authorID)
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

                // Cancel editing mode and refresh the grid
                GridViewAuthors.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

     
        protected void GridViewAuthors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int authorID = Convert.ToInt32(GridViewAuthors.DataKeys[e.RowIndex].Value);
                string query = "DELETE FROM Author WHERE AuthorID = @AuthorID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@AuthorID", authorID)
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

                
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }
    }

}