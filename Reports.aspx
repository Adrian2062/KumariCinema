<%@ Page Title="Complex Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="KumariCinemas.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4 text-primary"><i class="fa-solid fa-chart-line"></i> Advanced Analytics & Reports</h2>

        <!-- Status Message Area -->
        <div class="mb-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="fw-bold fs-5"></asp:Label>
        </div>

        <!-- REPORT 1: USER TICKETS -->
        <div class="card card-custom mb-4 shadow-sm border-primary">
            <div class="card-header bg-primary text-white">
                <h5 class="mb-0"><i class="fa-solid fa-user-tag"></i> 1. User Ticket History (Last 6 Months)</h5>
            </div>
            <div class="card-body">
                <div class="row g-3 align-items-end mb-3">
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Enter User ID:</label>
                        <asp:TextBox ID="txtUserIdSearch" runat="server" CssClass="form-control" placeholder="e.g. 2"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnUserReport" runat="server" Text="Generate Report" CssClass="btn btn-dark w-100" OnClick="BtnUserReport_Click" />
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvUserTickets" runat="server" CssClass="table table-hover table-bordered" AutoGenerateColumns="true">
                        <HeaderStyle CssClass="table-dark" />
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- REPORT 2: THEATRE SCHEDULE -->
        <div class="card card-custom mb-4 shadow-sm border-success">
            <div class="card-header bg-success text-white">
                <h5 class="mb-0"><i class="fa-solid fa-building"></i> 2. Theatre Movie & Showtime Details</h5>
            </div>
            <div class="card-body">
                <div class="row g-3 align-items-end mb-3">
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Select Theatre:</label>
                        <asp:DropDownList ID="ddlTheatre" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnTheatreReport" runat="server" Text="View Schedule" CssClass="btn btn-dark w-100" OnClick="BtnTheatreReport_Click" />
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvTheatreMovies" runat="server" CssClass="table table-hover table-bordered" AutoGenerateColumns="true">
                        <HeaderStyle CssClass="table-dark" />
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- REPORT 3: TOP PERFORMERS -->
        <div class="card card-custom mb-4 shadow-sm border-warning">
            <div class="card-header bg-warning text-dark">
                <h5 class="mb-0"><i class="fa-solid fa-trophy"></i> 3. Top 3 Theatre Occupancy (Paid Only)</h5>
            </div>
            <div class="card-body">
                <div class="row g-3 align-items-end mb-3">
                    <div class="col-md-4">
                        <label class="form-label fw-bold">Select Movie:</label>
                        <asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnOccupancyReport" runat="server" Text="Show Top 3" CssClass="btn btn-dark w-100" OnClick="BtnOccupancyReport_Click" />
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvTopOccupancy" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="true">
                        <HeaderStyle CssClass="table-dark" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>