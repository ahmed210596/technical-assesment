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
    public partial class PublisherPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        
        private void BindGridView(string sortByExpression = "", string sortDirection = "ASC")
        {
            string query = @"
            SELECT 
                PublisherID,
                Name,
                Address,
                Phone
            FROM 
                Publisher";

            DataTable dt = DatabaseHelper.GetData(query);

            if (!string.IsNullOrEmpty(sortByExpression))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = sortByExpression + " " + sortDirection;
                GridViewPublishers.DataSource = dv;
            }
            else
            {
                GridViewPublishers.DataSource = dt;
            }

            GridViewPublishers.DataBind();
        }
        protected void GridViewPublishers_Sorting(object sender, GridViewSortEventArgs e)
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
            GridViewPublishers.PageIndex = e.NewPageIndex;
            BindGridView();
        }
        
       
        protected void btnAddPublisher_Click(object sender, EventArgs e)
        {
            addPublisherForm.Style["display"] = "block";
        }

        // Save new publisher
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Publisher (Name, Address, Phone) VALUES (@Name, @Address, @Phone)";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", txtName.Text),
                    new SqlParameter("@Address", txtAddress.Text),
                    new SqlParameter("@Phone", txtPhone.Text)
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

                txtName.Text = "";
                txtAddress.Text = "";
                txtPhone.Text = "";

                addPublisherForm.Style["display"] = "none";

                
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            addPublisherForm.Style["display"] = "none";
        }

        protected void GridViewPublishers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewPublishers.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void GridViewPublishers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewPublishers.EditIndex = -1;
            BindGridView();
        }

        protected void GridViewPublishers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int publisherID = Convert.ToInt32(GridViewPublishers.DataKeys[e.RowIndex].Value);
                GridViewRow row = GridViewPublishers.Rows[e.RowIndex];

                string name = (row.Cells[1].Controls[0] as TextBox).Text;      
                string address = (row.Cells[2].Controls[0] as TextBox).Text;    
                string phone = (row.Cells[3].Controls[0] as TextBox).Text;      

                string query = "UPDATE Publisher SET Name = @Name, Address = @Address, Phone = @Phone WHERE PublisherID = @PublisherID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", name),
                    new SqlParameter("@Address", address),
                    new SqlParameter("@Phone", phone),
                    new SqlParameter("@PublisherID", publisherID)
                };

                DatabaseHelper.ExecuteQuery(query, parameters);

                // Cancel editing mode and refresh the grid
                GridViewPublishers.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
            }
        }

        
        protected void GridViewPublishers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int publisherID = Convert.ToInt32(GridViewPublishers.DataKeys[e.RowIndex].Value);
                string query = "DELETE FROM Publisher WHERE PublisherID = @PublisherID";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@PublisherID", publisherID)
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