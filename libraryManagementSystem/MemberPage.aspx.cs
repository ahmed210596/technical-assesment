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
    public partial class MemberPage : System.Web.UI.Page
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
                MemberID,
                FirstName,
                LastName,
                Email,
                Phone,
                Address,
                MembershipStartDate,
                MembershipEndDate
            FROM 
                Member";

            DataTable dt = DatabaseHelper.GetData(query);

            if (!string.IsNullOrEmpty(sortByExpression))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = sortByExpression + " " + sortDirection;
                GridViewMembers.DataSource = dv;
            }
            else
            {
                GridViewMembers.DataSource = dt;
            }

            GridViewMembers.DataBind();
        }

        protected void GridViewMembers_Sorting(object sender, GridViewSortEventArgs e)
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
            GridViewMembers.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        
        protected void btnAddMember_Click(object sender, EventArgs e)
        {
            addMemberForm.Style["display"] = "block";
        }

        
        protected void imgCalendar_Click(object sender, ImageClickEventArgs e)
        {
            
            ImageButton imgButton = (ImageButton)sender;

            string calendarID = imgButton.ID.Contains("Start") ? "CalendarStartDate" : "CalendarEndDate";

        
            Calendar calendar = (Calendar)FindControlRecursive(Page, calendarID);

            if (calendar != null)
            {
                
                calendar.Visible = !calendar.Visible;

                
                Calendar otherCalendar = (Calendar)FindControlRecursive(Page, (calendarID == "CalendarStartDate") ? "CalendarEndDate" : "CalendarStartDate");
                if (otherCalendar != null && otherCalendar.Visible)
                {
                    otherCalendar.Visible = false;
                }
            }
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

        protected void imgCalendar_Click2(object sender, ImageClickEventArgs e)
        {
            
            ImageButton imgButton = (ImageButton)sender;
            
            GridViewRow row = (GridViewRow)imgButton.NamingContainer;
            
            string calendarID = imgButton.ID.Contains("Start") ? "CalendarStartDateEdit" : "CalendarEndDateEdit";
           
            Calendar calendar = (Calendar)row.FindControl(calendarID);
            
            if (calendar != null)
            {
                calendar.Visible = !calendar.Visible;
            }
        }

       
        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            
            Calendar calendar = (Calendar)sender;

            
            string textBoxID = calendar.ID.Contains("Start") ? "txtMembershipStartDate" : "txtMembershipEndDate";

            
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
           
            string textBoxID = calendar.ID.Contains("Start") ? "txtMembershipStartDateEdit" : "txtMembershipEndDateEdit";
            
            TextBox txtDate = (TextBox)row.FindControl(textBoxID);
            
            if (txtDate != null)
            {
                txtDate.Text = calendar.SelectedDate.ToString("yyyy-MM-dd");
            }
           
            calendar.Visible = false;
        }

       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate;
                DateTime endDate;

                if (!DateTime.TryParse(txtMembershipStartDate.Text, out startDate) ||
                    !DateTime.TryParse(txtMembershipEndDate.Text, out endDate))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter valid dates.');", true);
                    return;
                }

                if (endDate < startDate)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Membership End Date cannot be earlier than Start Date.');", true);
                    return;
                }

                string query = "INSERT INTO Member (FirstName, LastName, Email, Phone, Address, MembershipStartDate, MembershipEndDate) " +
                               "VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @MembershipStartDate, @MembershipEndDate)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FirstName", txtFirstName.Text),
                    new SqlParameter("@LastName", txtLastName.Text),
                    new SqlParameter("@Email", txtEmail.Text),
                    new SqlParameter("@Phone", txtPhone.Text),
                    new SqlParameter("@Address", txtAddress.Text),
                    new SqlParameter("@MembershipStartDate", startDate),
                    new SqlParameter("@MembershipEndDate", endDate)
                };
                DatabaseHelper.ExecuteQuery(query, parameters);

                ClearInputs();
                addMemberForm.Style["display"] = "none";
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearInputs();
            addMemberForm.Style["display"] = "none";
        }

        
        protected void GridViewMembers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewMembers.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridViewMembers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewMembers.EditIndex = -1;
            BindGridView();
        }

        
        protected void GridViewMembers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int memberID = Convert.ToInt32(GridViewMembers.DataKeys[e.RowIndex].Value);
                GridViewRow row = GridViewMembers.Rows[e.RowIndex];

                string firstName = (row.Cells[1].Controls[0] as TextBox).Text;
                string lastName = (row.Cells[2].Controls[0] as TextBox).Text;
                string email = (row.Cells[3].Controls[0] as TextBox).Text;
                string phone = (row.Cells[4].Controls[0] as TextBox).Text;
                string address = (row.Cells[5].Controls[0] as TextBox).Text;
                string membershipStartDate = (row.FindControl("txtMembershipStartDateEdit") as TextBox).Text;
                string membershipEndDate = (row.FindControl("txtMembershipEndDateEdit") as TextBox).Text;

                DateTime startDate;
                DateTime endDate;

                if (!DateTime.TryParse(membershipStartDate, out startDate) ||
                    !DateTime.TryParse(membershipEndDate, out endDate))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter valid dates in edit mode.');", true);
                    return;
                }

                if (endDate < startDate)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Membership End Date cannot be earlier than Start Date in edit mode.');", true);
                    return;
                }

                string query = "UPDATE Member SET FirstName = @FirstName, LastName = @LastName, Email = @Email, " +
                               "Phone = @Phone, Address = @Address, MembershipStartDate = @MembershipStartDate, " +
                               "MembershipEndDate = @MembershipEndDate WHERE MemberID = @MemberID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FirstName", firstName),
                    new SqlParameter("@LastName", lastName),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Phone", phone),
                    new SqlParameter("@Address", address),
                    new SqlParameter("@MembershipStartDate", startDate),
                    new SqlParameter("@MembershipEndDate", endDate),
                    new SqlParameter("@MemberID", memberID)
                };
                DatabaseHelper.ExecuteQuery(query, parameters);

                GridViewMembers.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        
        protected void GridViewMembers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int memberID = Convert.ToInt32(GridViewMembers.DataKeys[e.RowIndex].Value);
                string query = "DELETE FROM Member WHERE MemberID = @MemberID";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@MemberID", memberID)
                };
                DatabaseHelper.ExecuteQuery(query, parameters);
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

      
        private void ClearInputs()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtMembershipStartDate.Text = "";
            txtMembershipEndDate.Text = "";
        }
    }
}