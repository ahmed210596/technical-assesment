<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PublisherPage.aspx.cs" Inherits="libraryManagementSystem.PublisherPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        
        <div class="page-header">
            <h1>Publisher Management</h1>
            <asp:Button ID="btnAddPublisher" runat="server" Text="Add New Publisher" OnClick="btnAddPublisher_Click" CssClass="btn btn-primary" />
        </div>

        <div class="table-responsive">
            <asp:GridView ID="GridViewPublishers" runat="server" AutoGenerateColumns="False" DataKeyNames="PublisherID"
                OnRowEditing="GridViewPublishers_RowEditing" OnRowCancelingEdit="GridViewPublishers_RowCancelingEdit"
                OnRowUpdating="GridViewPublishers_RowUpdating" OnRowDeleting="GridViewPublishers_RowDeleting"
                AllowPaging="True" PageSize="4" OnPageIndexChanging="GridViewBooks_PageIndexChanging"
                AllowSorting="True" OnSorting="GridViewPublishers_Sorting" CssClass="table table-striped table-hover">
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" PageButtonCount="5" />
                <Columns>
                    <asp:BoundField DataField="PublisherID" HeaderText="Publisher ID" ReadOnly="true" SortExpression="PublisherID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ControlStyle-CssClass="btn btn-sm btn-action" />
                </Columns>
                <PagerStyle CssClass="pagination" />
            </asp:GridView>
        </div>

        
        <div id="addPublisherForm" class="modal" runat="server">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Add New Publisher</h2>
                <div class="form-group">
                    <label for="txtName">Name:</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtAddress">Address:</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtPhone">Phone:</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                </div>
                <div class="form-actions">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-success" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-secondary" />
                </div>
            </div>
        </div>
    </div>
    <style>
.modal {
    display: none; 
    position: fixed;
    z-index: 1000;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    overflow: auto;
    background-color: rgba(0, 0, 0, 0.5); 
}

.modal-content {
    background-color: #fff;
    margin: 10% auto;
    padding: 20px;
    border: 1px solid #888;
    width: 50%;
    max-width: 600px;
    border-radius: 8px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

.close {
    color: #aaa;
    float: right;
    font-size: 28px;
    font-weight: bold;
    cursor: pointer;
}

.close:hover {
    color: #000;
}

/* Form Styling */
.form-group {
    margin-bottom: 15px;
}

.form-control {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
}

.form-actions {
    text-align: right;
    margin-top: 20px;
}</style>
    
    <script type="text/javascript">
       
        function openModal() {
            document.getElementById('<%= addPublisherForm.ClientID %>').style.display = 'block';
        }

       
        function closeModal() {
            document.getElementById('<%= addPublisherForm.ClientID %>').style.display = 'none';
        }

       
        document.getElementById('<%= btnAddPublisher.ClientID %>').onclick = function (e) {
            e.preventDefault(); 
            openModal(); 
        };

        
        window.onclick = function (event) {
            var modal = document.getElementById('<%= addPublisherForm.ClientID %>');
            if (event.target == modal) {
                closeModal();
            }
        };
    </script>
</asp:Content>