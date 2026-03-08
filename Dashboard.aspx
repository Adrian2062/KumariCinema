<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="KumariCinemas.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h2 class="mb-4">System Overview</h2>
        <div class="row">
            <!-- Users Card -->
            <div class="col-md-4 mb-4">
                <div class="card card-custom bg-primary text-white">
                    <div class="card-body text-center">
                        <i class="fa-solid fa-users fa-3x mb-3"></i>
                        <h3>Manage Users</h3>
                        <p>Add, Edit, and Delete Customers.</p>
                        <a href="UserDetails.aspx" class="btn btn-light mt-2">Go to Users</a>
                    </div>
                </div>
            </div>
            <!-- Movies Card -->
            <div class="col-md-4 mb-4">
                <div class="card card-custom bg-success text-white">
                    <div class="card-body text-center">
                        <i class="fa-solid fa-video fa-3x mb-3"></i>
                        <h3>Manage Movies</h3>
                        <p>Update currently showing movies.</p>
                        <a href="MovieDetails.aspx" class="btn btn-light mt-2">Go to Movies</a>
                    </div>
                </div>
            </div>
            <!-- Theatre & Halls Card -->
            <div class="col-md-4 mb-4">
                <div class="card card-custom bg-danger text-white">
                    <div class="card-body text-center">
                        <i class="fa-solid fa-building fa-3x mb-3"></i>
                        <h3>Theatres & Halls</h3>
                        <p>Control locations and seating capacity.</p>
                        <a href="TheaterCityHallDetails.aspx" class="btn btn-light mt-2">Go to Theatres</a>
                    </div>
                </div>
            </div>
            <!-- Shows Card -->
            <div class="col-md-4 mb-4">
                <div class="card card-custom bg-warning text-dark">
                    <div class="card-body text-center">
                        <i class="fa-solid fa-calendar-days fa-3x mb-3"></i>
                        <h3>Manage Shows</h3>
                        <p>Schedule dates, times, and holidays.</p>
                        <a href="ShowDetails.aspx" class="btn btn-dark mt-2">Go to Shows</a>
                    </div>
                </div>
            </div>
            <!-- Tickets Card -->
            <div class="col-md-4 mb-4">
                <div class="card card-custom bg-info text-white">
                    <div class="card-body text-center">
                        <i class="fa-solid fa-ticket fa-3x mb-3"></i>
                        <h3>Manage Tickets</h3>
                        <p>Track bookings and payment statuses.</p>
                        <a href="TicketDetails.aspx" class="btn btn-light mt-2">Go to Tickets</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>