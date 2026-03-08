<%@ Page Title="Manage Tickets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TicketDetails.aspx.cs" Inherits="KumariCinemas.TicketDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Status Message -->
    <div class="mb-3">
        <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-bold fs-5"></asp:Label>
    </div>

    <!-- Main Card -->
    <div class="row">
        <div class="col-12">
            <div class="card card-custom p-4 shadow-sm">
                <h3 class="mb-4"><i class="fa-solid fa-ticket"></i> Manage Movie Tickets</h3>
                
                <!-- Input Section -->
                <div class="row g-3 mb-4 align-items-end">
                    <div class="col-md-2">
                        <label class="form-label fw-bold">Ticket ID:</label>
                        <asp:TextBox ID="txtID" runat="server" CssClass="form-control" placeholder="ID"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-bold">Price:</label>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="0.00"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-bold">Status:</label>
                        <asp:TextBox ID="txtTStatus" runat="server" CssClass="form-control" placeholder="Booked/Cancelled"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label fw-bold">Booking Time (YYYY-MM-DD HH24:MI:SS):</label>
                        <asp:TextBox ID="txtBTime" runat="server" CssClass="form-control" placeholder="2023-10-27 14:30:00"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-bold">Payment:</label>
                        <asp:TextBox ID="txtPStatus" runat="server" CssClass="form-control" placeholder="Paid/Unpaid"></asp:TextBox>
                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="btnInsert" runat="server" Text="Add" CssClass="btn btn-primary w-100" OnClick="BtnInsert_Click" />
                    </div>
                </div>

                <!-- GridView Section -->
                <div class="table-responsive">
                    <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="False" DataKeyNames="ticket_id" 
                        CssClass="table table-bordered table-striped table-hover grid-view align-middle"
                        OnRowEditing="GvTickets_RowEditing" OnRowUpdating="GvTickets_RowUpdating" 
                        OnRowCancelingEdit="GvTickets_RowCancelingEdit" OnRowDeleting="GvTickets_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ticket_id" HeaderText="Ticket ID" ReadOnly="True" />
                            <asp:BoundField DataField="ticket_price" HeaderText="Price" ReadOnly="True" />
                            <asp:BoundField DataField="ticket_status" HeaderText="Ticket Status" />
                            <asp:BoundField DataField="booking_time" HeaderText="Booking Time" ReadOnly="True" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                            <asp:BoundField DataField="payment_status" HeaderText="Payment Status" />
                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-sm btn-outline-dark m-1" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>