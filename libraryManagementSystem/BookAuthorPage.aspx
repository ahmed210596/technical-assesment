<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookAuthorPage.aspx.cs" Inherits="libraryManagementSystem.BookAuthorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- Page Header -->
        <div class="page-header">
            <h1>Book-Author Management</h1>
            <asp:Button ID="btnAddBookAuthor" runat="server" Text="Add New Book-Author" OnClick="btnAddBookAuthor_Click" CssClass="btn btn-primary" />
        </div>

        
        <div class="table-responsive">
            <asp:GridView ID="GridViewBookAuthors" runat="server" AutoGenerateColumns="False" DataKeyNames="BookID,AuthorID"
                OnRowEditing="GridViewBookAuthors_RowEditing" OnRowCancelingEdit="GridViewBookAuthors_RowCancelingEdit"
                OnRowUpdating="GridViewBookAuthors_RowUpdating" OnRowDeleting="GridViewBookAuthors_RowDeleting"
                AllowPaging="True" PageSize="4" OnPageIndexChanging="GridViewBooks_PageIndexChanging"
                CssClass="table table-striped table-hover">
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" PageButtonCount="5" />
                <Columns>
                    <asp:BoundField DataField="BookID" HeaderText="Book ID" ReadOnly="true" />
                    <asp:BoundField DataField="AuthorID" HeaderText="Author ID" ReadOnly="true" />
                    <asp:TemplateField HeaderText="Book Title">
                        <ItemTemplate>
                            <%# Eval("BookTitle") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlBook" runat="server"
                                DataSourceID="sqlDataSourceBooks"
                                DataTextField="Title"
                                DataValueField="BookID"
                                SelectedValue='<%# Bind("BookID") %>'
                                CssClass="form-control">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Author Name">
                        <ItemTemplate>
                            <%# Eval("AuthorName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlAuthor" runat="server"
                                DataSourceID="sqlDataSourceAuthors"
                                DataTextField="FullName"
                                DataValueField="AuthorID"
                                SelectedValue='<%# Bind("AuthorID") %>'
                                CssClass="form-control">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ControlStyle-CssClass="btn btn-sm btn-action" />
                </Columns>
                <PagerStyle CssClass="pagination" />
            </asp:GridView>
        </div>

       
        <div id="addBookAuthorForm" class="modal" runat="server">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Add New Book-Author Relationship</h2>
                <div class="form-group">
                    <label for="ddlNewBook">Book:</label>
                    <asp:DropDownList ID="ddlNewBook" runat="server"
                        DataSourceID="sqlDataSourceBooks"
                        DataTextField="Title"
                        DataValueField="BookID"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlNewAuthor">Author:</label>
                    <asp:DropDownList ID="ddlNewAuthor" runat="server"
                        DataSourceID="sqlDataSourceAuthors"
                        DataTextField="FullName"
                        DataValueField="AuthorID"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-actions">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-success" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-secondary" />
                </div>
            </div>
        </div>
    </div>

   
    <asp:SqlDataSource ID="sqlDataSourceBooks" runat="server"
        ConnectionString="<%$ ConnectionStrings:LibraryDBConnection %>"
        SelectCommand="SELECT BookID, Title FROM Book">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDataSourceAuthors" runat="server"
        ConnectionString="<%$ ConnectionStrings:LibraryDBConnection %>"
        SelectCommand="SELECT AuthorID, FirstName + ' ' + LastName AS FullName FROM Author">
    </asp:SqlDataSource>
    <style>/* Modal Styling */
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
            document.getElementById('<%= addBookAuthorForm.ClientID %>').style.display = 'block';
        }

       
        function closeModal() {
            document.getElementById('<%= addBookAuthorForm.ClientID %>').style.display = 'none';
        }

        
        document.getElementById('<%= btnAddBookAuthor.ClientID %>').onclick = function (e) {
            e.preventDefault(); 
            openModal(); 
        };

        // Close the modal if the user clicks outside of it
        window.onclick = function (event) {
            var modal = document.getElementById('<%= addBookAuthorForm.ClientID %>');
            if (event.target == modal) {
                closeModal();
            }
        };
    </script>
</asp:Content>