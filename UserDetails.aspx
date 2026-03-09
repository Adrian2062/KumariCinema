<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="KumariCinemas.UserDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Page specific CSS can go here if needed -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card card-custom p-4">
        <h2 class="mb-4"><i class="fa-solid fa-users"></i> Manage User Details</h2>
        
        <!-- Input Form -->
        <div class="row mb-3">
            <div class="col-md-3">
                <label class="form-label fw-bold">User ID:</label>
                <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" placeholder="Enter unique ID"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label fw-bold">User Name:</label>
                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-5">
                <label class="form-label fw-bold">Address:</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="mb-4">
            <asp:Button ID="btnInsert" runat="server" Text="Add User" CssClass="btn btn-primary px-4" OnClick="BtnInsert_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success ms-3 fw-bold"></asp:Label>
        </div>

        <!-- Data Grid -->
        <div class="table-responsive">
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="user_id" 
                CssClass="table table-bordered table-striped table-hover grid-view"
                OnRowEditing="GvUsers_RowEditing" OnRowUpdating="GvUsers_RowUpdating" 
                OnRowCancelingEdit="GvUsers_RowCancelingEdit" OnRowDeleting="GvUsers_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="user_id" HeaderText="User ID" ReadOnly="True" />
                    <asp:BoundField DataField="user_name" HeaderText="User Name" />
                    <asp:BoundField DataField="address" HeaderText="Address" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-sm btn-outline-dark m-1" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>