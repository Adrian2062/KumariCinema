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
            // 1. Safely validate Show ID
            if (!int.TryParse(txtID.Text.Trim(), out int showId))
            {
                lblMessage.Text = "Invalid Show ID. Must be a number.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // 2. Safely validate Date
            if (!DateTime.TryParse(txtDate.Text.Trim(), out DateTime parsedDate))
            {
                lblMessage.Text = "Invalid Date format. Please use YYYY-MM-DD.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // 3. Safely validate Time (Allows "14:30" or "14:30:00")
            if (!TimeSpan.TryParse(txtTime.Text.Trim(), out TimeSpan parsedTime))
            {
                // Fallback in case they typed a full date-time string anyway
                if (DateTime.TryParse(txtTime.Text.Trim(), out DateTime fullDateTime))
                {
                    parsedTime = fullDateTime.TimeOfDay;
                }
                else
                {
                    lblMessage.Text = "Invalid Time format. Please use HH:mm (e.g., 14:30).";
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                    return;
                }
            }

            // Combine the validated Date and Time into one C# DateTime object
            DateTime combinedDateTime = parsedDate.Date + parsedTime;

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // TO_DATE removed. We pass the native C# DateTime parameters directly.
                string sql = "INSERT INTO SHOWS (show_id, show_date, show_time, holiday_status) " +
                             "VALUES (:id, :sdate, :stime, :hol)";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.BindByName = true;

                cmd.Parameters.Add("id", OracleDbType.Int32).Value = showId;
                cmd.Parameters.Add("sdate", OracleDbType.Date).Value = parsedDate;
                cmd.Parameters.Add("stime", OracleDbType.Date).Value = combinedDateTime;
                cmd.Parameters.Add("hol", OracleDbType.Varchar2).Value = txtHol.Text.Trim().ToUpper();

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Show Added Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";

                    // Clear the textboxes
                    txtID.Text = "";
                    txtDate.Text = "";
                    txtTime.Text = "";
                    txtHol.Text = "";

                    BindGrid();
                }
                catch (OracleException ex)
                {
                    // Catch the Unique Constraint error specifically
                    if (ex.Number == 1)
                    {
                        lblMessage.Text = "Error: A Show with ID " + showId + " already exists!";
                    }
                    else
                    {
                        lblMessage.Text = "Database Error: " + ex.Message;
                    }
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Application Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger fw-bold fs-5";
                }
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

                OracleCommand cmd = new OracleCommand("UPDATE SHOWS SET holiday_status=:h WHERE show_id=:id", conn);
                cmd.Parameters.Add("h", updatedHol);
                cmd.Parameters.Add("id", showId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    gvShows.EditIndex = -1;
                    lblMessage.Text = "Show Updated Successfully!";
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

        protected void GvShows_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                int showId = Convert.ToInt32(gvShows.DataKeys[e.RowIndex].Value);

                OracleCommand cmd = new OracleCommand("DELETE FROM SHOWS WHERE show_id = :id", conn);
                cmd.Parameters.Add("id", showId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Show Deleted!";
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