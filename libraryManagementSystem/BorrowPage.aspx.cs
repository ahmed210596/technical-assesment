using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace libraryManagementSystem
{
    public partial class BorrowPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

       
        private void BindGrid(string sortByExpression = "", string sortDirection = "ASC")
        {
            string query = @"
            SELECT 
                BorrowID,
                Book.Title as BookTitle,
                Member.FirstName + ' ' + Member.LastName as MemberName,
                Borrow.BorrowDate,
                Borrow.DueDate,
                Borrow.ReturnDate,
                Borrow.BookID,
                Borrow.MemberID
            FROM 
                Borrow
            INNER JOIN 
                Book ON Borrow.BookID = Book.BookID
            INNER JOIN 
                Member ON Borrow.MemberID = Member.MemberID";

            DataTable dt = DatabaseHelper.GetData(query);

            if (!string.IsNullOrEmpty(sortByExpression))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = sortByExpression + " " + sortDirection;
                GridViewBorrows.DataSource = dv;
            }
            else
            {
                GridViewBorrows.DataSource = dt;
            }

            GridViewBorrows.DataBind();
        }
        protected void GridViewBorrows_Sorting(object sender, GridViewSortEventArgs e)
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

            BindGrid(sortExpression, sortDirection);
        }
        protected void GridViewBooks_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridViewBorrows.PageIndex = e.NewPageIndex;
            BindGrid();
        }
       
        protected void btnAddBorrow_Click(object sender, EventArgs e)
        {
            addBorrowForm.Style["display"] = "block";
        }

        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int bookID = Convert.ToInt32(ddlNewBook.SelectedValue);
                int memberID = Convert.ToInt32(ddlNewMember.SelectedValue);

                DateTime borrowDate;
                DateTime dueDate;
                DateTime? returnDate = null;

                if (!DateTime.TryParse(txtBorrowDate.Text, out borrowDate) ||
                    !DateTime.TryParse(txtDueDate.Text, out dueDate))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter valid dates.');", true);
                    return;
                }

                if (!string.IsNullOrEmpty(txtReturnDate.Text))
                {
                    returnDate = DateTime.Parse(txtReturnDate.Text);
                }

                if (dueDate < borrowDate)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Due Date cannot be earlier than Borrow Date.');", true);
                    return;
                }

                if (returnDate.HasValue && returnDate.Value < borrowDate)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Return Date cannot be earlier than Borrow Date.');", true);
                    return;
                }

                string insertQuery = "INSERT INTO Borrow (BookID, MemberID, BorrowDate, DueDate, ReturnDate) VALUES (@BookID, @MemberID, @BorrowDate, @DueDate, @ReturnDate)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BookID", bookID),
                    new SqlParameter("@MemberID", memberID),
                    new SqlParameter("@BorrowDate", borrowDate),
                    new SqlParameter("@DueDate", dueDate),
                    new SqlParameter("@ReturnDate", returnDate)
                };
                DatabaseHelper.ExecuteQuery(insertQuery, parameters);

                ClearInputs();
                addBorrowForm.Style["display"] = "none";
                BindGrid();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearInputs();
            addBorrowForm.Style["display"] = "none";
        }

       
        protected void GridViewBorrows_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewBorrows.EditIndex = e.NewEditIndex;
            BindGrid();
        }

       
        protected void GridViewBorrows_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewBorrows.EditIndex = -1;
            BindGrid();
        }

        
        protected void GridViewBorrows_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int borrowID = Convert.ToInt32(GridViewBorrows.DataKeys[e.RowIndex].Values["BorrowID"]);
                GridViewRow row = GridViewBorrows.Rows[e.RowIndex];

                int newBookID = int.Parse((row.FindControl("ddlBook") as DropDownList).SelectedValue);
                int newMemberID = int.Parse((row.FindControl("ddlMember") as DropDownList).SelectedValue);
                DateTime newBorrowDate = DateTime.Parse((row.FindControl("txtBorrowDateGV") as TextBox).Text);
                DateTime newDueDate = DateTime.Parse((row.FindControl("txtDueDateGV") as TextBox).Text);
                DateTime? newReturnDate = null;

                if (!string.IsNullOrEmpty((row.FindControl("txtReturnDateGV") as TextBox).Text))
                {
                    newReturnDate = DateTime.Parse((row.FindControl("txtReturnDateGV") as TextBox).Text);
                }

                if (newDueDate < newBorrowDate)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Due Date cannot be earlier than Borrow Date in edit mode.');", true);
                    return;
                }

                if (newReturnDate.HasValue && newReturnDate.Value < newBorrowDate)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Return Date cannot be earlier than Borrow Date in edit mode.');", true);
                    return;
                }

                string updateQuery = "UPDATE Borrow SET BookID = @BookID, MemberID = @MemberID, BorrowDate = @BorrowDate, DueDate = @DueDate, ReturnDate = @ReturnDate WHERE BorrowID = @BorrowID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BookID", newBookID),
                    new SqlParameter("@MemberID", newMemberID),
                    new SqlParameter("@BorrowDate", newBorrowDate),
                    new SqlParameter("@DueDate", newDueDate),
                    new SqlParameter("@ReturnDate", newReturnDate),
                    new SqlParameter("@BorrowID", borrowID)
                };
                DatabaseHelper.ExecuteQuery(updateQuery, parameters);

                GridViewBorrows.EditIndex = -1;
                BindGrid();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        
        protected void GridViewBorrows_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int borrowID = Convert.ToInt32(GridViewBorrows.DataKeys[e.RowIndex].Values["BorrowID"]);

                string deleteQuery = "DELETE FROM Borrow WHERE BorrowID = @BorrowID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@BorrowID", borrowID)
                };
                DatabaseHelper.ExecuteQuery(deleteQuery, parameters);

                BindGrid();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        
        protected void imgCalendar_Click(object sender, ImageClickEventArgs e)
{
    
    ImageButton imgButton = (ImageButton)sender;

   
    string calendarID = GetCalendarID(imgButton.ID);

    
    Calendar calendar = (Calendar)FindControlRecursive(Page, calendarID);

    if (calendar != null)
    {
        
        calendar.Visible = !calendar.Visible;

       
        HideOtherCalendars(calendarID);
    }
}


private string GetCalendarID(string buttonID)
{
    if (buttonID.Contains("Borrow"))
    {
        return "CalendarBorrowDate";
    }
    else if (buttonID.Contains("Due"))
    {
        return "CalendarDueDate";
    }
    else if (buttonID.Contains("Return"))
    {
        return "CalendarReturnDate";
    }
    return string.Empty; 
}


public static Control FindControlRecursive(Control root, string id)
{
    if (root.ID == id)
    {
        return root;
    }

    foreach (Control c in root.Controls)
    {
        Control t = FindControlRecursive(c, id);
        if (t != null)
        {
            return t;
        }
    }

    return null;
}


private void HideOtherCalendars(string currentCalendarID)
{
    var calendarIDs = new[] { "CalendarBorrowDate", "CalendarDueDate", "CalendarReturnDate" };

    foreach (var id in calendarIDs)
    {
        if (id != currentCalendarID)
        {
            Calendar calendar = (Calendar)FindControlRecursive(Page, id);
            if (calendar != null && calendar.Visible)
            {
                calendar.Visible = false;
            }
        }
    }
}

        protected void imgCalendar_Click2(object sender, ImageClickEventArgs e)
        {
            ImageButton imgButton = (ImageButton)sender;
            GridViewRow row = (GridViewRow)imgButton.NamingContainer;
            string calendarID = imgButton.ID.Contains("Start") ? "CalendarBorrowDateGV" : imgButton.ID.Contains("Due") ? "CalendarDueDateGV" : "CalendarReturnDateGV";
            Calendar calendar = (Calendar)row.FindControl(calendarID);
            if (calendar != null)
            {
                calendar.Visible = !calendar.Visible;
            }
        }

        
        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            
            Calendar calendar = (Calendar)sender;

            
            string textBoxID = calendar.ID.Replace("Calendar", "txt");

            TextBox txtDate = (TextBox)FindControlRecursive(Page, textBoxID);

            if (txtDate != null)
            {
               
                txtDate.Text = calendar.SelectedDate.ToString("yyyy-MM-dd");
            }
           

           
            calendar.Visible = false;
        }


        protected void Calendar_SelectionChanged2(object sender, EventArgs e)
        {
            Calendar calendar = (Calendar)sender;
            GridViewRow row = (GridViewRow)calendar.NamingContainer;
            string textBoxID = calendar.ID.Replace("Calendar", "txt");
            TextBox txtDate = (TextBox)row.FindControl(textBoxID);
            if (txtDate != null)
            {
                txtDate.Text = calendar.SelectedDate.ToString("yyyy-MM-dd");
            }
            calendar.Visible = false;
        }

        
        private void ClearInputs()
        {
            ddlNewBook.SelectedIndex = 0;
            ddlNewMember.SelectedIndex = 0;
            txtBorrowDate.Text = "";
            txtDueDate.Text = "";
            txtReturnDate.Text = "";
        }
    }
}