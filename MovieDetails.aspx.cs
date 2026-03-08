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
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string sql = "INSERT INTO Movie (movie_id, movie_title, duration, language, genre, release_date, is_new_release) " +
                             "VALUES (:id, :title, TO_DATE(:dur, 'YYYY-MM-DD'), :lang, :genre, TO_DATE(:rdate, 'YYYY-MM-DD'), :isnew)";
                OracleCommand cmd = new OracleCommand(sql, conn);

                cmd.Parameters.Add("id", txtID.Text);
                cmd.Parameters.Add("title", txtTitle.Text);
                cmd.Parameters.Add("dur", txtDuration.Text);
                cmd.Parameters.Add("lang", txtLang.Text);
                cmd.Parameters.Add("genre", txtGenre.Text);
                cmd.Parameters.Add("rdate", txtRelDate.Text);
                cmd.Parameters.Add("isnew", txtIsNew.Text.ToUpper());

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Movie Added Successfully!";

                // Clear the textboxes after saving
                txtID.Text = ""; txtTitle.Text = ""; txtDuration.Text = "";
                txtLang.Text = ""; txtGenre.Text = ""; txtRelDate.Text = ""; txtIsNew.Text = "";

                BindGrid();
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

                cmd.Parameters.Add("t", (gvMovies.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text);
                cmd.Parameters.Add("l", (gvMovies.Rows[e.RowIndex].Cells[3].Controls[0] as TextBox).Text);
                cmd.Parameters.Add("g", (gvMovies.Rows[e.RowIndex].Cells[4].Controls[0] as TextBox).Text);
                cmd.Parameters.Add("n", (gvMovies.Rows[e.RowIndex].Cells[6].Controls[0] as TextBox).Text.ToUpper());
                cmd.Parameters.Add("id", Convert.ToInt32(gvMovies.DataKeys[e.RowIndex].Value));

                conn.Open();
                cmd.ExecuteNonQuery();
                gvMovies.EditIndex = -1;
                lblMessage.Text = "Movie Updated Successfully!";
                BindGrid();
            }
        }

        protected void GvMovies_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM Movie WHERE movie_id = :id", conn);
                cmd.Parameters.Add("id", Convert.ToInt32(gvMovies.DataKeys[e.RowIndex].Value));

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Movie Deleted Successfully!";
                BindGrid();
            }
        }
    }
}