<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuthorPage.aspx.cs" Inherits="libraryManagementSystem.AuthorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        
        <div class="page-header">
            <h1>Author Management</h1>
            <asp:Button ID="btnAddAuthor" runat="server" Text="Add New Author" OnClick="btnAddAuthor_Click" CssClass="btn btn-primary" />
        </div>

      
        <div class="table-responsive">
            <asp:GridView ID="GridViewAuthors" runat="server" AutoGenerateColumns="False" DataKeyNames="AuthorID"
                OnRowEditing="GridViewAuthors_RowEditing" OnRowCancelingEdit="GridViewAuthors_RowCancelingEdit"
                OnRowUpdating="GridViewAuthors_RowUpdating" OnRowDeleting="GridViewAuthors_RowDeleting"
                AllowPaging="True" PageSize="4" OnPageIndexChanging="GridViewAuthors_PageIndexChanging"
                AllowSorting="True" OnSorting="GridViewAuthors_Sorting" CssClass="table table-striped table-hover">
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" PageButtonCount="5" />
                <Columns>
                    <asp:BoundField DataField="AuthorID" HeaderText="Author ID" ReadOnly="true" SortExpression="AuthorID" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                    <asp:BoundField DataField="Bio" HeaderText="Bio" SortExpression="Bio" />
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ControlStyle-CssClass="btn btn-sm btn-action" />
                </Columns>
                <PagerStyle CssClass="pagination" />
            </asp:GridView>
        </div>

       
        <div id="addAuthorForm" class="modal" runat="server">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Add New Author</h2>
                <div class="form-group">
                    <label for="txtFirstName">First Name:</label>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtLastName">Last Name:</label>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtBio">Bio:</label>
                    <asp:TextBox ID="txtBio" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
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
            document.getElementById('<%= addAuthorForm.ClientID %>').style.display = 'block';
        }

        
        function closeModal() {
            document.getElementById('<%= addAuthorForm.ClientID %>').style.display = 'none';
        }

        // Attach the openModal function to the button click event
        document.getElementById('<%= btnAddAuthor.ClientID %>').onclick = function (e) {
            e.preventDefault(); // Prevent postback
            openModal(); // Open the modal
        };

        // Close the modal if the user clicks outside of it
        window.onclick = function (event) {
            var modal = document.getElementById('<%= addAuthorForm.ClientID %>');
            if (event.target == modal) {
                closeModal();
            }
        };
    </script>
</asp:Content>