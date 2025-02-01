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
    public partial class BookAuthorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        // Bind data to GridView
        private void BindGridView()
        {
            string query = @"
                SELECT 
                    BookAuthor.BookID,
                    BookAuthor.AuthorID,
                    Book.Title AS BookTitle,
                    Author.FirstName + ' ' + Author.LastName AS AuthorName
                FROM 
                    BookAuthor
                INNER JOIN 
                    Book ON BookAuthor.BookID = Book.BookID
                INNER JOIN 
                    Author ON BookAuthor.AuthorID = Author.AuthorID";

            DataTable dt = DatabaseHelper.GetData(query);
            GridViewBookAuthors.DataSource = dt;
            GridViewBookAuthors.DataBind();
        }
        protected void GridViewBooks_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridViewBookAuthors.PageIndex = e.NewPageIndex;
            BindGridView();
        }
        
        protected void btnAddBookAuthor_Click(object sender, EventArgs e)
        {
            addBookAuthorForm.Style["display"] = "block";
        }

        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO BookAuthor (BookID, AuthorID) VALUES (@BookID, @AuthorID)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BookID", int.Parse(ddlNewBook.SelectedValue)),
                    new SqlParameter("@AuthorID", int.Parse(ddlNewAuthor.SelectedValue))
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

                
                addBookAuthorForm.Style["display"] = "none";

                
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            addBookAuthorForm.Style["display"] = "none";
        }

       
        protected void GridViewBookAuthors_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewBookAuthors.EditIndex = e.NewEditIndex;
            BindGridView();
        }

       
        protected void GridViewBookAuthors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewBookAuthors.EditIndex = -1;
            BindGridView();
        }

        
        protected void GridViewBookAuthors_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int bookID = Convert.ToInt32(GridViewBookAuthors.DataKeys[e.RowIndex].Values["BookID"]);
                int authorID = Convert.ToInt32(GridViewBookAuthors.DataKeys[e.RowIndex].Values["AuthorID"]);
                GridViewRow row = GridViewBookAuthors.Rows[e.RowIndex];

                int newBookID = Convert.ToInt32((row.FindControl("ddlBook") as DropDownList).SelectedValue);
                int newAuthorID = Convert.ToInt32((row.FindControl("ddlAuthor") as DropDownList).SelectedValue);

                string query = "UPDATE BookAuthor SET BookID = @NewBookID, AuthorID = @NewAuthorID " +
                               "WHERE BookID = @BookID AND AuthorID = @AuthorID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@NewBookID", newBookID),
                    new SqlParameter("@NewAuthorID", newAuthorID),
                    new SqlParameter("@BookID", bookID),
                    new SqlParameter("@AuthorID", authorID)
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

                
                GridViewBookAuthors.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        
        protected void GridViewBookAuthors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int bookID = Convert.ToInt32(GridViewBookAuthors.DataKeys[e.RowIndex].Values["BookID"]);
                int authorID = Convert.ToInt32(GridViewBookAuthors.DataKeys[e.RowIndex].Values["AuthorID"]);

                string query = "DELETE FROM BookAuthor WHERE BookID = @BookID AND AuthorID = @AuthorID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BookID", bookID),
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