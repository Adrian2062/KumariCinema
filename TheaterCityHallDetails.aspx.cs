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
            // Validation: Check if Theatre ID is empty or not a number
            if (!int.TryParse(txtTheatreId.Text.Trim(), out int theatreId))
            {
                lblMessage.Text = "Please enter a valid numeric Theatre ID.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // Validation: Check if Theatre Name is empty
            if (string.IsNullOrWhiteSpace(txtTheatreName.Text))
            {
                lblMessage.Text = "Theatre Name cannot be empty.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("INSERT INTO Theatre (theatre_id, theatre_name) VALUES (:id, :name)", conn);
                cmd.BindByName = true;

                cmd.Parameters.Add("id", OracleDbType.Int32).Value = theatreId;
                cmd.Parameters.Add("name", OracleDbType.Varchar2).Value = txtTheatreName.Text.Trim();

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Theatre Added Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";

                    // Clear textboxes
                    txtTheatreId.Text = "";
                    txtTheatreName.Text = "";

                    BindTheatres();
                }
                catch (OracleException ex)
                {
                    if (ex.Number == 1) lblMessage.Text = "Error: A Theatre with this ID already exists!";
                    else lblMessage.Text = "Database Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Application Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
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
                cmd.BindByName = true;

                cmd.Parameters.Add("name", OracleDbType.Varchar2).Value = (gvTheatres.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim();
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = Convert.ToInt32(gvTheatres.DataKeys[e.RowIndex].Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    gvTheatres.EditIndex = -1;
                    lblMessage.Text = "Theatre Updated Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";
                    BindTheatres();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Update Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
            }
        }

        protected void GvTheatres_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM Theatre WHERE theatre_id = :id", conn);
                cmd.BindByName = true;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = Convert.ToInt32(gvTheatres.DataKeys[e.RowIndex].Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Theatre Deleted Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";
                    BindTheatres();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Delete Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
            }
        }

        // ==========================================
        // HALL METHODS
        // ==========================================
        protected void BtnInsertHall_Click(object sender, EventArgs e)
        {
            // Validation: Check if Hall ID is empty or not a number
            if (!int.TryParse(txtHallId.Text.Trim(), out int hallId))
            {
                lblMessage.Text = "Please enter a valid numeric Hall ID.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // Validation: Check if Capacity is empty or not a number
            if (!int.TryParse(txtCapacity.Text.Trim(), out int capacity))
            {
                lblMessage.Text = "Please enter a valid numeric Capacity.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("INSERT INTO Hall (hall_id, hall_capacity) VALUES (:id, :cap)", conn);
                cmd.BindByName = true;

                cmd.Parameters.Add("id", OracleDbType.Int32).Value = hallId;
                cmd.Parameters.Add("cap", OracleDbType.Int32).Value = capacity;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Hall Added Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";

                    // Clear textboxes
                    txtHallId.Text = "";
                    txtCapacity.Text = "";

                    BindHalls();
                }
                catch (OracleException ex)
                {
                    if (ex.Number == 1) lblMessage.Text = "Error: A Hall with this ID already exists!";
                    else lblMessage.Text = "Database Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Application Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
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
                cmd.BindByName = true;

                cmd.Parameters.Add("cap", OracleDbType.Int32).Value = Convert.ToInt32((gvHalls.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text.Trim());
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = Convert.ToInt32(gvHalls.DataKeys[e.RowIndex].Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    gvHalls.EditIndex = -1;
                    lblMessage.Text = "Hall Updated Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";
                    BindHalls();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Update Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
            }
        }

        protected void GvHalls_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM Hall WHERE hall_id = :id", conn);
                cmd.BindByName = true;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = Convert.ToInt32(gvHalls.DataKeys[e.RowIndex].Value);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Hall Deleted Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";
                    BindHalls();
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