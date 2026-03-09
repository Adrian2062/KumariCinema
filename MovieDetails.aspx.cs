using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class MovieDetails : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["OracleDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleDataAdapter da = new OracleDataAdapter("SELECT movie_id, movie_title, duration, language, genre, release_date, is_new_release FROM Movie ORDER BY movie_id", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvMovies.DataSource = dt;
                gvMovies.DataBind();
            }
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            // 1. Check if Movie ID is empty or not a number
            if (!int.TryParse(txtID.Text.Trim(), out int movieId))
            {
                lblMessage.Text = "Please enter a valid numeric Movie ID.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // 2. NEW: Check if Movie Title is empty
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                lblMessage.Text = "Movie Title cannot be empty.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // 3. Validate Dates (Duration and Release Date)
            if (!DateTime.TryParse(txtDuration.Text.Trim(), out DateTime durationDate))
            {
                lblMessage.Text = "Please select a valid Duration Date.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            if (!DateTime.TryParse(txtRelDate.Text.Trim(), out DateTime releaseDate))
            {
                lblMessage.Text = "Please select a valid Release Date.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // Removed TO_DATE! We pass the C# dates directly to Oracle.
                string sql = "INSERT INTO Movie (movie_id, movie_title, duration, language, genre, release_date, is_new_release) " +
                             "VALUES (:id, :title, :dur, :lang, :genre, :rdate, :isnew)";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.BindByName = true; // Prevents parameter mixing

                // Add parameters with exact Oracle types
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = movieId;
                cmd.Parameters.Add("title", OracleDbType.Varchar2).Value = txtTitle.Text.Trim();
                cmd.Parameters.Add("dur", OracleDbType.Date).Value = durationDate;
                cmd.Parameters.Add("lang", OracleDbType.Varchar2).Value = txtLang.Text.Trim();
                cmd.Parameters.Add("genre", OracleDbType.Varchar2).Value = txtGenre.Text.Trim();
                cmd.Parameters.Add("rdate", OracleDbType.Date).Value = releaseDate;
                cmd.Parameters.Add("isnew", OracleDbType.Varchar2).Value = txtIsNew.Text.Trim().ToUpper();

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Movie Added Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";

                    // Clear the textboxes after saving
                    txtID.Text = ""; txtTitle.Text = ""; txtDuration.Text = "";
                    txtLang.Text = ""; txtGenre.Text = ""; txtRelDate.Text = ""; txtIsNew.Text = "";

                    BindGrid();
                }
                catch (OracleException ex)
                {
                    // Check if it's ORA-00001 (Duplicate ID)
                    if (ex.Number == 1)
                        lblMessage.Text = "Error: A Movie with this ID already exists!";
                    else
                        lblMessage.Text = "Database Error: " + ex.Message;

                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Application Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
            }
        }

        protected void GvMovies_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMovies.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GvMovies_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMovies.EditIndex = -1;
            BindGrid();
        }

        protected void GvMovies_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string sql = "UPDATE Movie SET movie_title=:t, language=:l, genre=:g, is_new_release=:n WHERE movie_id=:id";
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.BindByName = true;

                cmd.Parameters.Add("t", OracleDbType.Varchar2).Value = (gvMovies.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
                cmd.Parameters.Add("l", OracleDbType.Varchar2).Value = (gvMovies.Rows[e.RowIndex].Cells[3].Controls[0] as TextBox).Text.Trim();
                cmd.Parameters.Add("g", OracleDbType.Varchar2).Value = (gvMovies.Rows[e.RowIndex].Cells[4].Controls[0] as TextBox).Text.Trim();
                cmd.Parameters.Add("n", OracleDbType.Varchar2).Value = (gvMovies.Rows[e.RowIndex].Cells[6].Controls[0] as TextBox).Text.Trim().ToUpper();
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = Convert.ToInt32(gvMovies.DataKeys[e.RowIndex].Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    gvMovies.EditIndex = -1;
                    lblMessage.Text = "Movie Updated Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";
                    BindGrid();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Update Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
            }
        }

        protected void GvMovies_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM Movie WHERE movie_id = :id", conn);
                cmd.BindByName = true;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = Convert.ToInt32(gvMovies.DataKeys[e.RowIndex].Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Movie Deleted Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";
                    BindGrid();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Delete Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
            }
        }
    }
}