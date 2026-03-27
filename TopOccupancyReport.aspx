<%@ Page Title="Top Occupancy" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TopOccupancyReport.aspx.cs" Inherits="KumariCinemas.TopOccupancyReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .custom-header th { background-color: #5542f6 !important; color: white !important; text-transform: uppercase; padding: 12px; font-size: 14px; }
        .custom-btn { background-color: #5542f6; border-color: #5542f6; color: white; font-weight: 600; }
        .custom-btn:hover { background-color: #412edb; color: white; }
        .card-custom { border: 1px solid #e0e0e0; border-radius: 8px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <h3 class="mb-3 text-dark">Top 3 Theatre Occupancy Report</h3>
        
        <div class="mb-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold fs-5"></asp:Label>
        </div>

        <div class="card card-custom shadow-sm mb-4">
            <div class="card-body">
                <div class="row align-items-end g-3">
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Select Movie to Analyze:</label>
                        <asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnOccupancyReport" runat="server" Text="Get Top 3 Performers" CssClass="btn custom-btn w-100" OnClick="BtnOccupancyReport_Click" />
                    </div>
                </div>
            </div>
        </div>
        
        <div class="card card-custom shadow-sm mb-5">
            <div class="card-body p-0">
                <div class="table-responsive m-3">
                    <asp:GridView ID="gvTopOccupancy" runat="server" CssClass="table table-bordered table-hover mb-0" AutoGenerateColumns="true">
                        <HeaderStyle CssClass="custom-header" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>