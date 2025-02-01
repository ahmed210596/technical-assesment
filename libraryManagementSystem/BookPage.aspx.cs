using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;

namespace libraryManagementSystem
{
    public partial class BookPage : System.Web.UI.Page
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
            string searchTerm = ViewState["SearchTerm"] as string;
            string query = @"
    SELECT 
        Book.BookID,
        Book.Title,
        Book.ISBN,
        Genre.Name as Genre,
        Publisher.Name as Publisher,
        Genre.GenreID,
        Publisher.PublisherID,
        Book.TotalCopies,
        Book.AvailableCopies
    FROM 
        Book
    INNER JOIN 
        Genre ON Book.GenreID = Genre.GenreID
    INNER JOIN 
        Publisher ON Book.PublisherID = Publisher.PublisherID";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query += " WHERE Book.Title LIKE @SearchTerm OR " +
                         "Book.ISBN LIKE @SearchTerm OR " +
                         "Genre.Name LIKE @SearchTerm OR " +
                         "Publisher.Name LIKE @SearchTerm";

                parameters.Add(new SqlParameter("@SearchTerm", "%" + searchTerm + "%"));
            }

            // Pass the parameters list as an array to GetData
            DataTable dt = DatabaseHelper.GetData(query, parameters.ToArray());

            if (!string.IsNullOrEmpty(sortByExpression))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = sortByExpression + " " + sortDirection;
                GridViewBooks.DataSource = dv;
            }
            else
            {
                GridViewBooks.DataSource = dt;
            }

            GridViewBooks.DataBind();
        }


        protected void GridViewBooks_Sorting(object sender, GridViewSortEventArgs e)
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
            GridViewBooks.PageIndex = e.NewPageIndex;
            BindGridView();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["SearchTerm"] = txtSearch.Text.Trim();
            BindGridView();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ViewState["SearchTerm"] = null;
            BindGridView();
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            
            ViewState["SearchTerm"] = txtSearch.Text.Trim();
            BindGridView();
        }
        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            addBookForm.Style["display"] = "block";
        }

        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Book (Title, ISBN, GenreID, PublisherID, TotalCopies, AvailableCopies) " +
                               "VALUES (@Title, @ISBN, @GenreID, @PublisherID, @TotalCopies, @AvailableCopies)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Title", txtTitle.Text),
                    new SqlParameter("@ISBN", txtISBN.Text),
                    new SqlParameter("@GenreID", int.Parse(ddlNewGenre.SelectedValue)),
                    new SqlParameter("@PublisherID", int.Parse(ddlNewPublisher.SelectedValue)),
                    new SqlParameter("@TotalCopies", int.Parse(txtTotalCopies.Text)),
                    new SqlParameter("@AvailableCopies", int.Parse(txtAvailableCopies.Text))
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

               
                txtTitle.Text = "";
                txtISBN.Text = "";
                txtTotalCopies.Text = "";
                txtAvailableCopies.Text = "";

                
                addBookForm.Style["display"] = "none";

                
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            addBookForm.Style["display"] = "none";
        }

        
        protected void GridViewBooks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewBooks.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        
        protected void GridViewBooks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewBooks.EditIndex = -1;
            BindGridView();
        }

        
        protected void GridViewBooks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int bookID = Convert.ToInt32(GridViewBooks.DataKeys[e.RowIndex].Value);
                GridViewRow row = GridViewBooks.Rows[e.RowIndex];

                
                string title = (row.Cells[1].Controls[0] as TextBox).Text; 
                string isbn = (row.Cells[2].Controls[0] as TextBox).Text;  
                int genreID = Convert.ToInt32((row.FindControl("ddlGenre") as DropDownList).SelectedValue);
                int publisherID = Convert.ToInt32((row.FindControl("ddlPublisher") as DropDownList).SelectedValue);
                int totalCopies = int.Parse((row.Cells[5].Controls[0] as TextBox).Text); 
                int availableCopies = int.Parse((row.Cells[6].Controls[0] as TextBox).Text); 

                string query = "UPDATE Book SET Title = @Title, ISBN = @ISBN, GenreID = @GenreID, PublisherID = @PublisherID, " +
                               "TotalCopies = @TotalCopies, AvailableCopies = @AvailableCopies WHERE BookID = @BookID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Title", title),
                    new SqlParameter("@ISBN", isbn),
                    new SqlParameter("@GenreID", genreID),
                    new SqlParameter("@PublisherID", publisherID),
                    new SqlParameter("@TotalCopies", totalCopies),
                    new SqlParameter("@AvailableCopies", availableCopies),
                    new SqlParameter("@BookID", bookID)
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

                
                GridViewBooks.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

       
        protected void GridViewBooks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int bookID = Convert.ToInt32(GridViewBooks.DataKeys[e.RowIndex].Value);
                string query = "DELETE FROM Book WHERE BookID = @BookID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BookID", bookID)
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