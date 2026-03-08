<%@ Page Title="Manage Movies" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieDetails.aspx.cs" Inherits="KumariCinemas.MovieDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card card-custom p-4">
        <h2 class="mb-4"><i class="fa-solid fa-video"></i> Manage Movies</h2>
        
        <!-- Input Form in Bootstrap Grid -->
        <div class="row mb-3">
            <div class="col-md-2">
                <label class="form-label fw-bold">Movie ID:</label>
                <asp:TextBox ID="txtID" runat="server" CssClass="form-control" placeholder="e.g. 12"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="form-label fw-bold">Title:</label>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Movie Title"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label fw-bold">Duration (Date):</label>
                <!-- Using HTML5 Date Picker -->
                <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label fw-bold">Language:</label>
                <asp:TextBox ID="txtLang" runat="server" CssClass="form-control" placeholder="e.g. English"></asp:TextBox>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-md-3">
                <label class="form-label fw-bold">Genre:</label>
                <asp:TextBox ID="txtGenre" runat="server" CssClass="form-control" placeholder="e.g. Action, Sci-Fi"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label class="form-label fw-bold">Release Date:</label>
                <!-- Using HTML5 Date Picker -->
                <asp:TextBox ID="txtRelDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <label class="form-label fw-bold">New Release?</label>
                <asp:TextBox ID="txtIsNew" runat="server" CssClass="form-control" placeholder="Y or N" MaxLength="1"></asp:TextBox>
            </div>
            <div class="col-md-4 d-flex align-items-end">
                <asp:Button ID="btnInsert" runat="server" Text="Add Movie" CssClass="btn btn-primary px-4 w-100" OnClick="BtnInsert_Click" />
            </div>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success fw-bold"></asp:Label>
        </div>

        <!-- Data Grid -->
        <div class="table-responsive">
            <asp:GridView ID="gvMovies" runat="server" AutoGenerateColumns="False" DataKeyNames="movie_id" 
                CssClass="table table-bordered table-striped table-hover grid-view align-middle"
                OnRowEditing="GvMovies_RowEditing" OnRowUpdating="GvMovies_RowUpdating" 
                OnRowCancelingEdit="GvMovies_RowCancelingEdit" OnRowDeleting="GvMovies_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="movie_id" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="movie_title" HeaderText="Title" />
                    <asp:BoundField DataField="duration" HeaderText="Duration" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="language" HeaderText="Language" />
                    <asp:BoundField DataField="genre" HeaderText="Genre" />
                    <asp:BoundField DataField="release_date" HeaderText="Release Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="is_new_release" HeaderText="New?" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-sm btn-outline-dark m-1" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>