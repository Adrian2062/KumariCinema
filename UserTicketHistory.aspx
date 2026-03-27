<%@ Page Title="User Ticket History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserTicketHistory.aspx.cs" Inherits="KumariCinemas.UserTicketHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .custom-header th { background-color: #5542f6 !important; color: white !important; text-transform: uppercase; padding: 12px; font-size: 14px; }
        .custom-btn { background-color: #5542f6; border-color: #5542f6; color: white; font-weight: 600; }
        .custom-btn:hover { background-color: #412edb; color: white; }
        .card-custom { border: 1px solid #e0e0e0; border-radius: 8px; }
        .empty-data-msg { text-align: center; padding: 20px; font-weight: bold; color: #6c757d; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h3 class="mb-3 text-dark">Ticket History (Last 6 Months)</h3>
        
        <div class="mb-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold fs-5"></asp:Label>
        </div>

        <div class="card card-custom shadow-sm mb-4">
            <div class="card-body">
                <div class="row align-items-end g-3">
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Select User:</label>
                        <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnUserReport" runat="server" Text="Generate Report" CssClass="btn custom-btn w-100" OnClick="BtnUserReport_Click" />
                    </div>
                </div>
            </div>
        </div>
        
        <div class="card card-custom shadow-sm mb-5">
            <div class="card-body p-0">
                <div class="table-responsive m-3">
                    <!-- Added ShowHeaderWhenEmpty and EmptyDataText to prevent page from going blank -->
                    <asp:GridView ID="gvUserTickets" runat="server" 
                        CssClass="table table-bordered table-hover mb-0" 
                        AutoGenerateColumns="true" 
                        ShowHeaderWhenEmpty="true" 
                        EmptyDataText="No ticket history records found for this user.">
                        <HeaderStyle CssClass="custom-header" />
                        <EmptyDataRowStyle CssClass="empty-data-msg" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>