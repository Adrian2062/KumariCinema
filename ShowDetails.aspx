<%@ Page Title="Manage Showtimes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowDetails.aspx.cs" Inherits="KumariCinemas.ShowDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Status Message -->
    <div class="mb-3">
        <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-bold fs-5"></asp:Label>
    </div>

    <!-- Professional Card Layout -->
    <div class="row">
        <div class="col-12">
            <div class="card card-custom p-4 shadow-sm">
                <h3 class="mb-4"><i class="fa-solid fa-clock"></i> Manage Movie Showtimes</h3>
                
                <!-- Input Row (Aligned for speed) -->
                <div class="row g-3 mb-4 align-items-end">
                    <div class="col-md-2">
                        <label class="form-label fw-bold">Show ID:</label>
                        <asp:TextBox ID="txtID" runat="server" CssClass="form-control" placeholder="ID"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label fw-bold">Show Date:</label>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="YYYY-MM-DD"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label class="form-label fw-bold">Show Time:</label>
                        <asp:TextBox ID="txtTime" runat="server" CssClass="form-control" placeholder="HH:mm (e.g. 14:30)"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <label class="form-label fw-bold">Holiday Status:</label>
                        <asp:TextBox ID="txtHol" runat="server" CssClass="form-control" placeholder="Y / N"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnInsert" runat="server" Text="Add Show" CssClass="btn btn-primary w-100" OnClick="BtnInsert_Click" />
                    </div>
                </div>

                <!-- GridView Row -->
                <div class="table-responsive">
                    <asp:GridView ID="gvShows" runat="server" AutoGenerateColumns="False" DataKeyNames="show_id" 
                        CssClass="table table-bordered table-striped table-hover grid-view align-middle"
                        OnRowEditing="GvShows_RowEditing" OnRowUpdating="GvShows_RowUpdating" 
                        OnRowCancelingEdit="GvShows_RowCancelingEdit" OnRowDeleting="GvShows_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="show_id" HeaderText="Show ID" ReadOnly="True" />
                            <asp:BoundField DataField="show_date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="True" />
                            <asp:BoundField DataField="show_time" HeaderText="Time" ReadOnly="True" />
                            <asp:BoundField DataField="holiday_status" HeaderText="Holiday?" />
                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-sm btn-outline-dark m-1" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>