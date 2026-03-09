using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class TicketDetails : System.Web.UI.Page
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
                string sql = "SELECT ticket_id, ticket_price, ticket_status, booking_time, payment_status FROM Ticket ORDER BY ticket_id";
                OracleDataAdapter da = new OracleDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTickets.DataSource = dt;
                gvTickets.DataBind();
            }
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            // 1. Validate Ticket ID (Must be a whole number)
            if (!int.TryParse(txtID.Text.Trim(), out int ticketId))
            {
                lblMessage.Text = "Invalid Ticket ID. Please enter a valid number.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // 2. Validate Price (Must be a decimal/number)
            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
            {
                lblMessage.Text = "Invalid Price. Please enter a valid amount (e.g., 250.00).";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // 3. Validate Status fields (Cannot be empty)
            if (string.IsNullOrWhiteSpace(txtTStatus.Text) || string.IsNullOrWhiteSpace(txtPStatus.Text))
            {
                lblMessage.Text = "Ticket Status and Payment Status cannot be empty.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            // 4. Validate Booking Time (Must be a valid Date/Time)
            if (!DateTime.TryParse(txtBTime.Text.Trim(), out DateTime bookingTime))
            {
                lblMessage.Text = "Invalid Booking Time. Please use a format like YYYY-MM-DD HH:mm.";
                lblMessage.CssClass = "text-danger fw-bold fs-5";
                return;
            }

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // Removed TO_DATE. We pass the C# DateTime directly to Oracle.
                string sql = "INSERT INTO Ticket (ticket_id, ticket_price, ticket_status, booking_time, payment_status) " +
                             "VALUES (:id, :price, :tstatus, :btime, :pstatus)";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.BindByName = true;

                cmd.Parameters.Add("id", OracleDbType.Int32).Value = ticketId;
                cmd.Parameters.Add("price", OracleDbType.Decimal).Value = price;
                cmd.Parameters.Add("tstatus", OracleDbType.Varchar2).Value = txtTStatus.Text.Trim();
                cmd.Parameters.Add("btime", OracleDbType.Date).Value = bookingTime;
                cmd.Parameters.Add("pstatus", OracleDbType.Varchar2).Value = txtPStatus.Text.Trim();

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Ticket Added Successfully!";
                    lblMessage.CssClass = "text-success fw-bold fs-5";

                    // Clear the textboxes
                    txtID.Text = "";
                    txtPrice.Text = "";
                    txtTStatus.Text = "";
                    txtBTime.Text = "";
                    txtPStatus.Text = "";

                    BindGrid();
                }
                catch (OracleException ex)
                {
                    if (ex.Number == 1)
                        lblMessage.Text = "Error: A Ticket with this ID already exists!";
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

        protected void GvTickets_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTickets.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GvTickets_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTickets.EditIndex = -1;
            BindGrid();
        }

        protected void GvTickets_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int ticketId = Convert.ToInt32(gvTickets.DataKeys[e.RowIndex].Value);
            string updatedTStatus = (gvTickets.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox).Text.Trim();
            string updatedPStatus = (gvTickets.Rows[e.RowIndex].Cells[4].Controls[0] as TextBox).Text.Trim();

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("UPDATE Ticket SET ticket_status=:ts, payment_status=:ps WHERE ticket_id=:id", conn);
                cmd.BindByName = true;

                cmd.Parameters.Add("ts", OracleDbType.Varchar2).Value = updatedTStatus;
                cmd.Parameters.Add("ps", OracleDbType.Varchar2).Value = updatedPStatus;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = ticketId;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    gvTickets.EditIndex = -1;
                    lblMessage.Text = "Ticket Updated Successfully!";
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

        protected void GvTickets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ticketId = Convert.ToInt32(gvTickets.DataKeys[e.RowIndex].Value);

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM Ticket WHERE ticket_id = :id", conn);
                cmd.BindByName = true;
                cmd.Parameters.Add("id", OracleDbType.Int32).Value = ticketId;

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    lblMessage.Text = "Ticket Deleted!";
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