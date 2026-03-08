<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="KumariCinemas.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Google Fonts for Modern Feel -->
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;600;700&display=swap" rel="stylesheet">

    <style>
        :root {
            --primary-slate: #1e293b;
            --accent-blue: #3b82f6;
            --soft-gray: #f8fafc;
            --border-color: #e2e8f0;
        }

        body { font-family: 'Inter', sans-serif; background-color: var(--soft-gray); }

        /* Stats Cards Styling */
        .stat-card {
            background: #ffffff;
            border: 1px solid var(--border-color);
            border-radius: 12px;
            padding: 1.5rem;
            transition: all 0.3s ease;
        }
        .stat-card:hover { transform: translateY(-3px); box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1); }
        .stat-label { color: #64748b; font-size: 0.85rem; font-weight: 600; text-transform: uppercase; letter-spacing: 0.5px; }
        .stat-value { color: var(--primary-slate); font-size: 1.75rem; font-weight: 700; margin-top: 5px; }
        .stat-icon-box {
            width: 48px; height: 48px; border-radius: 10px;
            display: flex; align-items: center; justify-content: center;
        }

        /* Module Cards Styling */
        .module-card {
            background: #ffffff;
            border: 1px solid var(--border-color);
            border-radius: 16px;
            overflow: hidden;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            height: 100%;
        }
        .module-card:hover { border-color: var(--accent-blue); transform: translateY(-5px); box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.05); }
        .module-icon { font-size: 2.5rem; margin-bottom: 1.25rem; }
        .module-title { font-weight: 700; color: var(--primary-slate); font-size: 1.25rem; }
        .module-desc { color: #64748b; font-size: 0.95rem; line-height: 1.5; }
        
        .btn-action {
            background: var(--soft-gray);
            color: var(--primary-slate);
            font-weight: 600; border: 1px solid var(--border-color);
            border-radius: 8px; padding: 10px 20px;
            transition: 0.2s;
        }
        .module-card:hover .btn-action { background: var(--accent-blue); color: white; border-color: var(--accent-blue); }
        
        /* Layout Tweaks */
        .page-header { border-bottom: 1px solid var(--border-color); padding-bottom: 1.5rem; margin-bottom: 2rem; }
    </style>

    <div class="container-fluid px-4">
        
        <div class="page-header d-flex align-items-center justify-content-between mt-4">
            <div>
                <h2 class="fw-bold text-slate mb-1">System Overview</h2>
                <p class="text-muted mb-0">Kumari Cinemas Operational Management Portal</p>
            </div>
            <div class="text-end">
                <span class="badge bg-light text-dark border p-2">
                    <i class="fa-solid fa-circle text-success me-1"></i> System Online
                </span>
            </div>
        </div>

        <!-- 1. CLEAN STATS ROW -->
        <div class="row g-4 mb-5">
            <div class="col-md-3">
                <div class="stat-card">
                    <div class="d-flex justify-content-between">
                        <div>
                            <div class="stat-label">Total Movies</div>
                            <div class="stat-value"><asp:Label ID="lblTotalMovies" runat="server" Text="0"></asp:Label></div>
                        </div>
                        <div class="stat-icon-box bg-blue-light" style="background: #eff6ff; color: #3b82f6;">
                            <i class="fa-solid fa-film fa-lg"></i>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card">
                    <div class="d-flex justify-content-between">
                        <div>
                            <div class="stat-label">Active Users</div>
                            <div class="stat-value"><asp:Label ID="lblTotalUsers" runat="server" Text="0"></asp:Label></div>
                        </div>
                        <div class="stat-icon-box" style="background: #f0fdf4; color: #22c55e;">
                            <i class="fa-solid fa-user-check fa-lg"></i>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card">
                    <div class="d-flex justify-content-between">
                        <div>
                            <div class="stat-label">Total Revenue</div>
                            <!-- Changed default text to Rs. -->
                            <div class="stat-value">
                                <asp:Label ID="lblTotalRevenue" runat="server" Text="Rs. 0.00"></asp:Label></div>
                        </div>
                        <!-- Changed icon to fa-money-bill-trend-up for a more professional look -->
                        <div class="stat-icon-box" style="background: #fffbeb; color: #f59e0b;">
                            <i class="fa-solid fa-money-bill-trend-up fa-lg"></i>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="stat-card">
                    <div class="d-flex justify-content-between">
                        <div>
                            <div class="stat-label">Tickets Sold</div>
                            <div class="stat-value"><asp:Label ID="lblTotalTickets" runat="server" Text="0"></asp:Label></div>
                        </div>
                        <div class="stat-icon-box" style="background: #fef2f2; color: #ef4444;">
                            <i class="fa-solid fa-ticket fa-lg"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- 2. REFINED MODULE CARDS -->
        <h4 class="fw-bold mb-4" style="color: var(--primary-slate);">Core Modules</h4>
        <div class="row g-4 mb-5">
            <div class="col-md-4">
                <div class="module-card p-4">
                    <div class="module-icon text-primary"><i class="fa-solid fa-users-viewfinder"></i></div>
                    <h5 class="module-title">User Portal</h5>
                    <p class="module-desc">Manage authentication, customer profiles, and user-level access controls across the system.</p>
                    <a href="UserDetails.aspx" class="btn btn-action w-100">Access User Records <i class="fa-solid fa-arrow-right ms-2 small"></i></a>
                </div>
            </div>
            <div class="col-md-4">
                <div class="module-card p-4">
                    <div class="module-icon text-success"><i class="fa-solid fa-clapperboard"></i></div>
                    <h5 class="module-title">Movie Library</h5>
                    <p class="module-desc">Centralized repository for movie metadata, ratings, duration, and promotional content management.</p>
                    <a href="MovieDetails.aspx" class="btn btn-action w-100">Manage Content <i class="fa-solid fa-arrow-right ms-2 small"></i></a>
                </div>
            </div>
            <div class="col-md-4">
                <div class="module-card p-4">
                    <div class="module-icon text-danger"><i class="fa-solid fa-building-columns"></i></div>
                    <h5 class="module-title">Infrastructure</h5>
                    <p class="module-desc">Configure theatre locations, hall seating charts, and physical facility parameters.</p>
                    <a href="TheaterCityHallDetails.aspx" class="btn btn-action w-100">Configure Facilities <i class="fa-solid fa-arrow-right ms-2 small"></i></a>
                </div>
            </div>
            <div class="col-md-6">
                <div class="module-card p-4 d-flex align-items-center">
                    <div class="me-4 module-icon text-warning mb-0"><i class="fa-solid fa-clock-rotate-left"></i></div>
                    <div class="flex-grow-1">
                        <h5 class="module-title">Showtime Scheduler</h5>
                        <p class="module-desc mb-3">Sync movies with halls and dates to prevent scheduling conflicts.</p>
                        <a href="ShowDetails.aspx" class="btn btn-action">Open Scheduler</a>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="module-card p-4 d-flex align-items-center">
                    <div class="me-4 module-icon text-info mb-0"><i class="fa-solid fa-chart-line"></i></div>
                    <div class="flex-grow-1">
                        <h5 class="module-title">Advanced Analytics</h5>
                        <p class="module-desc mb-3">Generate data-driven reports on occupancy, revenue trends, and sales performance.</p>
                        <a href="Reports.aspx" class="btn btn-action">Generate Reports</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>