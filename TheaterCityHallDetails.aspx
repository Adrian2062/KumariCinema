<%@ Page Title="Manage Theatres & Halls" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterCityHallDetails.aspx.cs" Inherits="KumariCinemas.TheaterCityHallDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <!-- Status Message -->
    <div class="mb-3">
        <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-bold fs-5"></asp:Label>
    </div>

    <!-- Row for Side-by-Side Layout -->
    <div class="row">
        
        <!-- ==============================
             THEATRES SECTION
        =============================== -->
        <div class="col-lg-6 mb-4">
            <div class="card card-custom p-4 h-100">
                <h3 class="mb-4"><i class="fa-solid fa-building"></i> Manage Theatres</h3>
                
                <div class="row mb-4 align-items-end">
                    <div class="col-md-3">
                        <label class="form-label fw-bold">ID:</label>
                        <asp:TextBox ID="txtTheatreId" runat="server" CssClass="form-control" placeholder="ID"></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <label class="form-label fw-bold">Theatre Name:</label>
                        <asp:TextBox ID="txtTheatreName" runat="server" CssClass="form-control" placeholder="e.g. PVR Cinemas"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnInsertTheatre" runat="server" Text="Add Theatre" CssClass="btn btn-primary w-100" OnClick="BtnInsertTheatre_Click" />
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="gvTheatres" runat="server" AutoGenerateColumns="False" DataKeyNames="theatre_id" 
                        CssClass="table table-bordered table-striped table-hover grid-view align-middle"
                        OnRowEditing="GvTheatres_RowEditing" OnRowUpdating="GvTheatres_RowUpdating" 
                        OnRowCancelingEdit="GvTheatres_RowCancelingEdit" OnRowDeleting="GvTheatres_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="theatre_id" HeaderText="Theatre ID" ReadOnly="True" />
                            <asp:BoundField DataField="theatre_name" HeaderText="Theatre Name" />
                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-sm btn-outline-dark m-1" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- ==============================
             HALLS SECTION
        =============================== -->
        <div class="col-lg-6 mb-4">
            <div class="card card-custom p-4 h-100">
                <h3 class="mb-4"><i class="fa-solid fa-door-open"></i> Manage Halls</h3>
                
                <div class="row mb-4 align-items-end">
                    <div class="col-md-3">
                        <label class="form-label fw-bold">ID:</label>
                        <asp:TextBox ID="txtHallId" runat="server" CssClass="form-control" placeholder="ID"></asp:TextBox>
                    </div>
                    <div class="col-md-6">
                        <label class="form-label fw-bold">Capacity:</label>
                        <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control" placeholder="e.g. 150"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnInsertHall" runat="server" Text="Add Hall" CssClass="btn btn-primary w-100" OnClick="BtnInsertHall_Click" />
                    </div>
                </div>

                <div class="table-responsive">
                    <asp:GridView ID="gvHalls" runat="server" AutoGenerateColumns="False" DataKeyNames="hall_id" 
                        CssClass="table table-bordered table-striped table-hover grid-view align-middle"
                        OnRowEditing="GvHalls_RowEditing" OnRowUpdating="GvHalls_RowUpdating" 
                        OnRowCancelingEdit="GvHalls_RowCancelingEdit" OnRowDeleting="GvHalls_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="hall_id" HeaderText="Hall ID" ReadOnly="True" />
                            <asp:BoundField DataField="hall_capacity" HeaderText="Capacity" />
                            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-sm btn-outline-dark m-1" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>
</asp:Content>