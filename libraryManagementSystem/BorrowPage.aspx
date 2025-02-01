<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BorrowPage.aspx.cs" Inherits="libraryManagementSystem.BorrowPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
     
        <div class="page-header">
            <h1>Borrow Management</h1>
            <asp:Button ID="btnAddBorrow" runat="server" Text="Add New Borrow" OnClick="btnAddBorrow_Click" CssClass="btn btn-primary" />
        </div>

        
        <div class="table-responsive">
            <asp:GridView ID="GridViewBorrows" runat="server" AutoGenerateColumns="False" DataKeyNames="BorrowID"
                OnRowEditing="GridViewBorrows_RowEditing" OnRowCancelingEdit="GridViewBorrows_RowCancelingEdit"
                OnRowUpdating="GridViewBorrows_RowUpdating" OnRowDeleting="GridViewBorrows_RowDeleting"
                AllowPaging="True" PageSize="4" OnPageIndexChanging="GridViewBooks_PageIndexChanging"
                AllowSorting="True" OnSorting="GridViewBorrows_Sorting" CssClass="table table-striped table-hover">
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" PageButtonCount="5" />
                <Columns>
                    <asp:BoundField DataField="BorrowID" HeaderText="Borrow ID" ReadOnly="true" SortExpression="BorrowID" />
                    <asp:TemplateField HeaderText="Book" SortExpression="BookTitle">
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
                    <asp:TemplateField HeaderText="Member" SortExpression="MemberName">
                        <ItemTemplate>
                            <%# Eval("MemberName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlMember" runat="server"
                                DataSourceID="sqlDataSourceMembers"
                                DataTextField="FullName"
                                DataValueField="MemberID"
                                SelectedValue='<%# Bind("MemberID") %>' 
                                CssClass="form-control">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Borrow Date" SortExpression="BorrowDate">
                        <ItemTemplate>
                            <%# Eval("BorrowDate", "{0:yyyy-MM-dd}") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBorrowDateGV" runat="server" Text='<%# Bind("BorrowDate", "{0:yyyy-MM-dd}") %>' ReadOnly="true" CssClass="form-control" />
                            <asp:ImageButton ID="imgBorrowDate" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click2" CssClass="calendar-icon" />
                            <asp:Calendar ID="CalendarBorrowDateGV" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged2" Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Due Date" SortExpression="DueDate">
                        <ItemTemplate>
                            <%# Eval("DueDate", "{0:yyyy-MM-dd}") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDueDateGV" runat="server" Text='<%# Bind("DueDate", "{0:yyyy-MM-dd}") %>' ReadOnly="true" CssClass="form-control" />
                            <asp:ImageButton ID="imgDueDate" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click2" CssClass="calendar-icon" />
                            <asp:Calendar ID="CalendarDueDateGV" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged2" Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Return Date" SortExpression="ReturnDate">
                        <ItemTemplate>
                            <%# Eval("ReturnDate", "{0:yyyy-MM-dd}") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtReturnDateGV" runat="server" Text='<%# Bind("ReturnDate", "{0:yyyy-MM-dd}") %>' ReadOnly="true" CssClass="form-control" />
                            <asp:ImageButton ID="imgReturnDate" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click2" CssClass="calendar-icon" />
                            <asp:Calendar ID="CalendarReturnDateGV" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged2" Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ControlStyle-CssClass="btn btn-sm btn-action" />
                </Columns>
                <PagerStyle CssClass="pagination" />
            </asp:GridView>
        </div>

       
        <div id="addBorrowForm" class="modal" runat="server">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Add New Borrow Record</h2>
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
                    <label for="ddlNewMember">Member:</label>
                    <asp:DropDownList ID="ddlNewMember" runat="server"
                        DataSourceID="sqlDataSourceMembers"
                        DataTextField="FullName"
                        DataValueField="MemberID"
                        CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="txtBorrowDate">Borrow Date:</label>
                    <asp:TextBox ID="txtBorrowDate" runat="server" CssClass="form-control" />
                    <asp:ImageButton ID="imgBorrowDate" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click" CssClass="calendar-icon"  />
                    <asp:Calendar ID="CalendarBorrowDate" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged" Visible="false" />
                </div>
                <div class="form-group">
                    <label for="txtDueDate">Due Date:</label>
                    <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" />
                    <asp:ImageButton ID="imgDueDate" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click" CssClass="calendar-icon"  
   />
                    <asp:Calendar ID="CalendarDueDate" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged" Visible="false" />
                </div>
                <div class="form-group">
                    <label for="txtReturnDate">Return Date:</label>
                    <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-control" />
                    <asp:ImageButton ID="imgReturnDate" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click" CssClass="calendar-icon" />
                    <asp:Calendar ID="CalendarReturnDate" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged" Visible="false" />
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
    <asp:SqlDataSource ID="sqlDataSourceMembers" runat="server"
        ConnectionString="<%$ ConnectionStrings:LibraryDBConnection %>"
        SelectCommand="SELECT MemberID, FirstName + ' ' + LastName AS FullName FROM Member">
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
        }
        .modal-content {
            background-color: #fff;
            margin: 10% auto;
            padding: 20px;
            border-radius: 8px;
            max-width: 600px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }
        .close {
            color: #aaa;
            float: right;
            font-size: 28px;
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
            border-radius: 4px;
        }
        .form-actions {
            text-align: right;
        }
        .calendar-icon {
            cursor: pointer;
            margin-left: 10px;
        }
        .calendar {
            position: absolute;
            z-index: 1001;
            background-color: #fff;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }
    </style>
    
  
    <script>
        function openModal() {
            document.getElementById('<%= addBorrowForm.ClientID %>').style.display = 'block';
        }

        function closeModal() {
            document.getElementById('<%= addBorrowForm.ClientID %>').style.display = 'none';
        }

        var modalContent = document.getElementById('<%= addBorrowForm.ClientID %>').getElementsByClassName('modal-content')[0];
        modalContent.onclick = function (event) {
            event.stopPropagation(); 
        };

        window.onclick = function (event) {
            var modal = document.getElementById('<%= addBorrowForm.ClientID %>');
            if (event.target == modal) {
                closeModal();
            }
        };
    </script>
</asp:Content>
