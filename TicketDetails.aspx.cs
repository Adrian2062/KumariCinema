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
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string sql = "INSERT INTO Ticket (ticket_id, ticket_price, ticket_status, booking_time, payment_status) " +
                             "VALUES (:id, :price, :tstatus, TO_DATE(:btime, 'YYYY-MM-DD HH24:MI:SS'), :pstatus)";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("id", txtID.Text);
                cmd.Parameters.Add("price", txtPrice.Text);
                cmd.Parameters.Add("tstatus", txtTStatus.Text);
                cmd.Parameters.Add("btime", txtBTime.Text);
                cmd.Parameters.Add("pstatus", txtPStatus.Text);

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Ticket Added Successfully!";

                // Clear the textboxes
                txtID.Text = "";
                txtPrice.Text = "";
                txtTStatus.Text = "";
                txtBTime.Text = "";
                txtPStatus.Text = "";

                BindGrid();
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
            string updatedTStatus = (gvTickets.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox).Text;
            string updatedPStatus = (gvTickets.Rows[e.RowIndex].Cells[4].Controls[0] as TextBox).Text;

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("UPDATE Ticket SET ticket_status=:ts, payment_status=:ps WHERE ticket_id=:id", conn);
                cmd.Parameters.Add("ts", updatedTStatus);
                cmd.Parameters.Add("ps", updatedPStatus);
                cmd.Parameters.Add("id", ticketId);

                conn.Open();
                cmd.ExecuteNonQuery();

                gvTickets.EditIndex = -1;
                lblMessage.Text = "Ticket Updated Successfully!";
                BindGrid();
            }
        }

        protected void GvTickets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ticketId = Convert.ToInt32(gvTickets.DataKeys[e.RowIndex].Value);

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleCommand cmd = new OracleCommand("DELETE FROM Ticket WHERE ticket_id = :id", conn);
                cmd.Parameters.Add("id", ticketId);

                conn.Open();
                cmd.ExecuteNonQuery();

                lblMessage.Text = "Ticket Deleted!";
                BindGrid();
            }
        }
    }
}