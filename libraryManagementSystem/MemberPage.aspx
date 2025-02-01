<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberPage.aspx.cs" Inherits="libraryManagementSystem.MemberPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        
        <div class="page-header">
            <h1>Member Management</h1>
            <asp:Button ID="btnAddMember" runat="server" Text="Add New Member" OnClick="btnAddMember_Click" CssClass="btn btn-primary" />
        </div>

        
        <div class="table-responsive">
            <asp:GridView ID="GridViewMembers" runat="server" AutoGenerateColumns="False" DataKeyNames="MemberID"
                OnRowEditing="GridViewMembers_RowEditing" OnRowCancelingEdit="GridViewMembers_RowCancelingEdit"
                OnRowUpdating="GridViewMembers_RowUpdating" OnRowDeleting="GridViewMembers_RowDeleting"
                AllowPaging="True" PageSize="4" OnPageIndexChanging="GridViewBooks_PageIndexChanging"
                AllowSorting="True" OnSorting="GridViewMembers_Sorting" CssClass="table table-striped table-hover">
                <PagerSettings Mode="NumericFirstLast" FirstPageText="First" LastPageText="Last" PageButtonCount="5" />
                <Columns>
                    <asp:BoundField DataField="MemberID" HeaderText="Member ID" ReadOnly="true" SortExpression="MemberID" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
                    <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                    <asp:TemplateField HeaderText="Membership Start Date" SortExpression="MembershipStartDate">
                        <ItemTemplate>
                            <asp:Label ID="lblMembershipStartDate" runat="server" Text='<%# Eval("MembershipStartDate", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMembershipStartDateEdit" runat="server" Text='<%# Bind("MembershipStartDate", "{0:yyyy-MM-dd}") %>' CssClass="form-control" />
                            <asp:ImageButton ID="imgStartDateEdit" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click2" CssClass="calendar-icon" />
                            <asp:Calendar ID="CalendarStartDateEdit" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged2" Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Membership End Date" SortExpression="MembershipEndDate">
                        <ItemTemplate>
                            <asp:Label ID="lblMembershipEndDate" runat="server" Text='<%# Eval("MembershipEndDate", "{0:yyyy-MM-dd}") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMembershipEndDateEdit" runat="server" Text='<%# Bind("MembershipEndDate", "{0:yyyy-MM-dd}") %>' CssClass="form-control" />
                            <asp:ImageButton ID="imgEndDateEdit" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click2" CssClass="calendar-icon"  />
                            <asp:Calendar ID="CalendarEndDateEdit" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged2" Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ControlStyle-CssClass="btn btn-sm btn-action" />
                </Columns>
                <PagerStyle CssClass="pagination" />
            </asp:GridView>
        </div>

        <!-- Add Member Modal -->
        <div id="addMemberForm" class="modal" runat="server">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h2>Add New Member</h2>
                <div class="form-group">
                    <label for="txtFirstName">First Name:</label>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtLastName">Last Name:</label>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtEmail">Email:</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtPhone">Phone:</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtAddress">Address:</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtMembershipStartDate">Membership Start Date:</label>
                    <asp:TextBox ID="txtMembershipStartDate" runat="server" ReadOnly="true" CssClass="form-control" />
                    <asp:ImageButton ID="imgStartDate" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click" CssClass="calendar-icon"  />
                    <asp:Calendar ID="CalendarStartDate" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged" Visible="false" />
                </div>
                <div class="form-group">
                    <label for="txtMembershipEndDate">Membership End Date:</label>
                    <asp:TextBox ID="txtMembershipEndDate" runat="server" ReadOnly="true" CssClass="form-control" />
                    <asp:ImageButton ID="imgEndDate" runat="server" ImageUrl="~/Images/calendar.png" OnClick="imgCalendar_Click" CssClass="calendar-icon"  />
                    <asp:Calendar ID="CalendarEndDate" runat="server" CssClass="calendar" OnSelectionChanged="Calendar_SelectionChanged" Visible="false" />
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
}</style>
    
    <script type="text/javascript">
        
        function openModal() {
            document.getElementById('<%= addMemberForm.ClientID %>').style.display = 'block';
        }

        // Function to close the modal
        function closeModal() {
            document.getElementById('<%= addMemberForm.ClientID %>').style.display = 'none';
        }

        
       

        var modalContent = document.getElementById('<%= addMemberForm.ClientID %>').getElementsByClassName('modal-content')[0];
        modalContent.onclick = function (event) {
            event.stopPropagation(); 
        };

        
        window.onclick = function (event) {
            var modal = document.getElementById('<%= addMemberForm.ClientID %>');
            if (event.target == modal) {
                closeModal();
            }
        };

        
        
    </script>
</asp:Content>