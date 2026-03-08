using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class TheaterCityHallDetails : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["OracleDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTheatres();
                BindHalls();
            }
        }

        private void BindTheatres()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleDataAdapter da = new OracleDataAdapter("SELECT theatre_id, theatre_name FROM Theatre ORDER BY theatre_id", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTheatres.DataSource = dt;
                gvTheatres.DataBind();
            }
        }

        private void BindHalls()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleDataAdapter da = new OracleDataAdapter("SELECT hall_id, hall_capacity FROM Hall ORDER BY hall_id", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvHalls.DataSource = dt;
                gvHalls.DataBind();
            }
        }

        // ==========================================
        // THEATRE METHODS
        // ==========================================
        protected void BtnInsertTheatre_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("INSERT INTO Theatre (theatre_id, theatre_name) VALUES (:id, :name)", conn);
                cmd.Parameters.Add("id", txtTheatreId.Text);
                cmd.Parameters.Add("name", txtTheatreName.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Theatre Added Successfully!";

                // Clear textboxes
                txtTheatreId.Text = "";
                txtTheatreName.Text = "";

                BindTheatres();
            }
        }

        protected void GvTheatres_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTheatres.EditIndex = e.NewEditIndex;
            BindTheatres();
        }

        protected void GvTheatres_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTheatres.EditIndex = -1;
            BindTheatres();
        }

        protected void GvTheatres_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("UPDATE Theatre SET theatre_name = :name WHERE theatre_id = :id", conn);
                cmd.Parameters.Add("name", (gvTheatres.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text);
                cmd.Parameters.Add("id", Convert.ToInt32(gvTheatres.DataKeys[e.RowIndex].Value));

                conn.Open();
                cmd.ExecuteNonQuery();
                gvTheatres.EditIndex = -1;
                lblMessage.Text = "Theatre Updated Successfully!";

                BindTheatres();
            }
        }

        protected void GvTheatres_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM Theatre WHERE theatre_id = :id", conn);
                cmd.Parameters.Add("id", Convert.ToInt32(gvTheatres.DataKeys[e.RowIndex].Value));

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Theatre Deleted Successfully!";

                BindTheatres();
            }
        }

        // ==========================================
        // HALL METHODS
        // ==========================================
        protected void BtnInsertHall_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("INSERT INTO Hall (hall_id, hall_capacity) VALUES (:id, :cap)", conn);
                cmd.Parameters.Add("id", txtHallId.Text);
                cmd.Parameters.Add("cap", txtCapacity.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Hall Added Successfully!";

                // Clear textboxes
                txtHallId.Text = "";
                txtCapacity.Text = "";

                BindHalls();
            }
        }

        protected void GvHalls_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvHalls.EditIndex = e.NewEditIndex;
            BindHalls();
        }

        protected void GvHalls_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvHalls.EditIndex = -1;
            BindHalls();
        }

        protected void GvHalls_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("UPDATE Hall SET hall_capacity = :cap WHERE hall_id = :id", conn);
                cmd.Parameters.Add("cap", (gvHalls.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text);
                cmd.Parameters.Add("id", Convert.ToInt32(gvHalls.DataKeys[e.RowIndex].Value));

                conn.Open();
                cmd.ExecuteNonQuery();
                gvHalls.EditIndex = -1;
                lblMessage.Text = "Hall Updated Successfully!";

                BindHalls();
            }
        }

        protected void GvHalls_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM Hall WHERE hall_id = :id", conn);
                cmd.Parameters.Add("id", Convert.ToInt32(gvHalls.DataKeys[e.RowIndex].Value));

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "Hall Deleted Successfully!";

                BindHalls();
            }
        }
    }
}