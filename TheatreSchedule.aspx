<%@ Page Title="Theatre Schedule" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheatreSchedule.aspx.cs" Inherits="KumariCinemas.TheatreSchedule" %>

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
        <h3 class="mb-3 text-dark">Theatre and Hall Movie Schedule</h3>
        
        <div class="mb-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold fs-5"></asp:Label>
        </div>

        <div class="card card-custom shadow-sm mb-4">
            <div class="card-body">
                <div class="row align-items-end g-3">
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Select Theatre:</label>
                        <asp:DropDownList ID="ddlTheatre" runat="server" CssClass="form-select" 
                            AutoPostBack="true" OnSelectedIndexChanged="ddlTheatre_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Select Hall:</label>
                        <asp:DropDownList ID="ddlHall" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnTheatreReport" runat="server" Text="View Schedule" CssClass="btn custom-btn w-100" OnClick="BtnTheatreReport_Click" />
                    </div>
                </div>
            </div>
        </div>
        
        <div class="card card-custom shadow-sm mb-5">
            <div class="card-body">
                <asp:Label ID="lblTheatreResult" runat="server" CssClass="fw-bold mb-3 d-block" Style="color: #5542f6;"></asp:Label>
                <div class="table-responsive">
                    <asp:GridView ID="gvTheatreMovies" runat="server" CssClass="table table-bordered table-hover mb-0" 
                        AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" 
                        EmptyDataText="No movies are scheduled for this combination.">
                        <HeaderStyle CssClass="custom-header" />
                        <EmptyDataRowStyle CssClass="empty-data-msg" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>