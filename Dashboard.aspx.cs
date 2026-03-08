using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace KumariCinemas
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["OracleDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                try
                {
                    conn.Open();

                    // 1. Total Movies Count
                    using (OracleCommand cmdMovies = new OracleCommand("SELECT COUNT(*) FROM MOVIE", conn))
                    {
                        lblTotalMovies.Text = cmdMovies.ExecuteScalar()?.ToString() ?? "0";
                    }

                    // 2. Registered Users Count
                    using (OracleCommand cmdUsers = new OracleCommand("SELECT COUNT(*) FROM USERS", conn))
                    {
                        lblTotalUsers.Text = cmdUsers.ExecuteScalar()?.ToString() ?? "0";
                    }

                    // 3. Tickets Sold
                    using (OracleCommand cmdTickets = new OracleCommand("SELECT COUNT(*) FROM TICKET", conn))
                    {
                        lblTotalTickets.Text = cmdTickets.ExecuteScalar()?.ToString() ?? "0";
                    }

                    // 4. Total Revenue (Rs.)
                    string revSql = "SELECT SUM(ticket_price) FROM TICKET WHERE UPPER(payment_status) = 'PAID'";
                    using (OracleCommand cmdRev = new OracleCommand(revSql, conn))
                    {
                        object result = cmdRev.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            decimal totalRev = Convert.ToDecimal(result);
                            // Formatted with Rs. prefix
                            lblTotalRevenue.Text = string.Format("Rs. {0:N2}", totalRev);
                        }
                        else
                        {
                            lblTotalRevenue.Text = "Rs. 0.00";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Error Fallbacks
                    lblTotalMovies.Text = "0";
                    lblTotalUsers.Text = "0";
                    lblTotalTickets.Text = "0";
                    lblTotalRevenue.Text = "Rs. 0.00";

                    System.Diagnostics.Debug.WriteLine("Dashboard Error: " + ex.Message);
                }
            }
        }
    }
}