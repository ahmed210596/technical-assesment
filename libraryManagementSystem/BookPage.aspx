<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookPage.aspx.cs" Inherits="libraryManagementSystem.BookPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
       
        <div class="page-header">
            <h1 class="display-4 text-center">Book Management</h1>
            <asp:Button ID="btnAddBook" runat="server" Text="Add New Book" OnClick="btnAddBook_Click" CssClass="btn btn-primary btn-lg" />
        </div>

       <div class="row mb-4">
            <div class="col-md-8">
                <asp:TextBox ID="txtSearch" runat="server" 
                    CssClass="form-control form-control-lg" 
                    placeholder="Search by Title, ISBN, Genre, or Publisher..."
                    AutoPostBack="true"
                    OnTextChanged="txtSearch_TextChanged" />
            </div>
            <div class="col-md-4 d-flex align-items-center">
                <asp:Button ID="btnSearch" runat="server" Text="Search" 
                    OnClick="btnSearch_Click" CssClass="btn btn-primary btn-lg mr-2" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" 
                    OnClick="btnClear_Click" CssClass="btn btn-secondary btn-lg" />
            </div>
        </div>
        
        <div class="table-responsive">
            <asp:GridView ID="GridViewBooks" runat="server" AutoGenerateColumns="False" DataKeyNames="BookID"
                OnRowEditing="GridViewBooks_RowEditing" OnRowCancelingEdit="GridViewBooks_RowCancelingEdit"
                OnRowUpdating="GridViewBooks_RowUpdating" OnRowDeleting="GridViewBooks_RowDeleting"
                AllowPaging="True" PageSize="5" OnPageIndexChanging="GridViewBooks_PageIndexChanging"
                AllowSorting="True" OnSorting="GridViewBooks_Sorting" CssClass="table table-hover table-bordered">
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" PageButtonCount="5" />
                <Columns>
                    <asp:BoundField DataField="BookID" HeaderText="Book ID" ReadOnly="true" SortExpression="BookID" />
                    <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                    <asp:BoundField DataField="ISBN" HeaderText="ISBN" SortExpression="ISBN" />
                    <asp:TemplateField HeaderText="Genre" SortExpression="Genre">
                        <ItemTemplate>
                            <%# Eval("Genre") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlGenre" runat="server"
                                DataSourceID="sqlDataSourceGenres"
                                DataTextField="Name"
                                DataValueField="GenreID"
                                SelectedValue='<%# Bind("GenreID") %>' CssClass="form-control">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Publisher" SortExpression="Publisher">
                        <ItemTemplate>
                            <%# Eval("Publisher") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlPublisher" runat="server"
                                DataSourceID="sqlDataSourcePublishers"
                                DataTextField="Name"
                                DataValueField="PublisherID"
                                SelectedValue='<%# Bind("PublisherID") %>' CssClass="form-control">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TotalCopies" HeaderText="Total Copies" SortExpression="TotalCopies" />
                    <asp:BoundField DataField="AvailableCopies" HeaderText="Available Copies" SortExpression="AvailableCopies" />
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ControlStyle-CssClass="btn btn-sm btn-action" />
                </Columns>
                <PagerStyle CssClass="pagination justify-content-center" />
            </asp:GridView>
        </div>

       
        <div id="addBookForm" class="modal" runat="server">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2 class="modal-title">Add New Book</h2>
                <div class="form-group">
                    <label for="txtTitle">Title:</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtISBN">ISBN:</label>
                    <asp:TextBox ID="txtISBN" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="ddlNewGenre">Genre:</label>
                    <asp:DropDownList ID="ddlNewGenre" runat="server"
                        DataSourceID="sqlDataSourceGenres"
                        DataTextField="Name"
                        DataValueField="GenreID"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="ddlNewPublisher">Publisher:</label>
                    <asp:DropDownList ID="ddlNewPublisher" runat="server"
                        DataSourceID="sqlDataSourcePublishers"
                        DataTextField="Name"
                        DataValueField="PublisherID"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="txtTotalCopies">Total Copies:</label>
                    <asp:TextBox ID="txtTotalCopies" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtAvailableCopies">Available Copies:</label>
                    <asp:TextBox ID="txtAvailableCopies" runat="server" CssClass="form-control" />
                </div>
                <div class="form-actions">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-success" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-secondary" />
                </div>
            </div>
        </div>
    </div>

  
    <asp:SqlDataSource ID="sqlDataSourceGenres" runat="server"
        ConnectionString="<%$ ConnectionStrings:LibraryDBConnection %>"
        SelectCommand="SELECT GenreID, Name FROM Genre">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDataSourcePublishers" runat="server"
        ConnectionString="<%$ ConnectionStrings:LibraryDBConnection %>"
        SelectCommand="SELECT PublisherID, Name FROM Publisher">
    </asp:SqlDataSource>

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
            transition: opacity 0.3s ease;
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
            transition: transform 0.3s ease-out;
        }

        .close {
            color: #aaa;
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
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .form-actions {
            text-align: right;
            margin-top: 20px;
        }

       
        .btn-primary, .btn-success, .btn-secondary {
            transition: background-color 0.3s ease;
        }

        .btn-primary:hover {
            background-color: #0069d9;
        }

        .btn-success:hover {
            background-color: #28a745;
        }

        .btn-secondary:hover {
            background-color: #6c757d;
        }

        
        .table-striped tbody tr:nth-child(odd) {
            background-color: #f9f9f9;
        }

        .table-hover tbody tr:hover {
            background-color: #f1f1f1;
        }

        .pagination {
            display: flex;
            justify-content: center;
        }

        
        @media (max-width: 768px) {
            .modal-content {
                width: 90%;
                padding: 15px;
            }

            .table {
                font-size: 12px;
            }
        }
    </style>

    
    <script type="text/javascript">
        function openModal() {
            document.getElementById('<%= addBookForm.ClientID %>').style.display = 'block';
            document.getElementById('<%= addBookForm.ClientID %>').style.opacity = '1';
        }

        function closeModal() {
            document.getElementById('<%= addBookForm.ClientID %>').style.display = 'none';
            document.getElementById('<%= addBookForm.ClientID %>').style.opacity = '0';
        }

        document.getElementById('<%= btnAddBook.ClientID %>').onclick = function (e) {
            e.preventDefault();
            openModal();
        };

        window.onclick = function (event) {
            var modal = document.getElementById('<%= addBookForm.ClientID %>');
            if (event.target == modal) {
                closeModal();
            }
        };


        
    </script>
</asp:Content>
