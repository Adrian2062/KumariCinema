using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class ShowDetails : System.Web.UI.Page
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
                // Updated table name to SHOWS (matching your database screenshot)
                string sql = "SELECT show_id, show_date, TO_CHAR(show_time, 'HH24:MI:SS') as show_time, holiday_status FROM SHOWS ORDER BY show_id";
                OracleDataAdapter da = new OracleDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvShows.DataSource = dt;
                gvShows.DataBind();
            }
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // Updated table name to SHOWS
                string sql = "INSERT INTO SHOWS (show_id, show_date, show_time, holiday_status) " +
                             "VALUES (:id, TO_DATE(:sdate, 'YYYY-MM-DD'), TO_DATE(:stime, 'YYYY-MM-DD HH24:MI:SS'), :hol)";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("id", txtID.Text);
                cmd.Parameters.Add("sdate", txtDate.Text);
                cmd.Parameters.Add("stime", txtTime.Text);
                cmd.Parameters.Add("hol", txtHol.Text.ToUpper());

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Show Added Successfully!";

                // Clear the textboxes
                txtID.Text = "";
                txtDate.Text = "";
                txtTime.Text = "";
                txtHol.Text = "";

                BindGrid();
            }
        }

        protected void GvShows_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvShows.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GvShows_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvShows.EditIndex = -1;
            BindGrid();
        }

        protected void GvShows_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string updatedHol = (gvShows.Rows[e.RowIndex].Cells[3].Controls[0] as TextBox).Text.ToUpper();
                int showId = Convert.ToInt32(gvShows.DataKeys[e.RowIndex].Value);

                // Updated table name to SHOWS
                OracleCommand cmd = new OracleCommand("UPDATE SHOWS SET holiday_status=:h WHERE show_id=:id", conn);
                cmd.Parameters.Add("h", updatedHol);
                cmd.Parameters.Add("id", showId);

                conn.Open();
                cmd.ExecuteNonQuery();

                gvShows.EditIndex = -1;
                lblMessage.Text = "Show Updated Successfully!";
                BindGrid();
            }
        }

        protected void GvShows_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                int showId = Convert.ToInt32(gvShows.DataKeys[e.RowIndex].Value);
                // Updated table name to SHOWS
                OracleCommand cmd = new OracleCommand("DELETE FROM SHOWS WHERE show_id = :id", conn);
                cmd.Parameters.Add("id", showId);

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Show Deleted!";
                BindGrid();
            }
        }
    }
}