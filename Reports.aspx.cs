using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class Reports : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["OracleDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();
            }
        }

        private void LoadDropdowns()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                try
                {
                    // Load Theatres
                    OracleDataAdapter daT = new OracleDataAdapter("SELECT theatre_id, theatre_name FROM THEATRE ORDER BY theatre_name", conn);
                    DataTable dtT = new DataTable();
                    daT.Fill(dtT);
                    ddlTheatre.DataSource = dtT;
                    ddlTheatre.DataTextField = "theatre_name";
                    ddlTheatre.DataValueField = "theatre_id";
                    ddlTheatre.DataBind();

                    // Load Movies (Using movie_title to match your DB)
                    OracleDataAdapter daM = new OracleDataAdapter("SELECT movie_id, movie_title FROM MOVIE ORDER BY movie_title", conn);
                    DataTable dtM = new DataTable();
                    daM.Fill(dtM);
                    ddlMovies.DataSource = dtM;
                    ddlMovies.DataTextField = "movie_title";
                    ddlMovies.DataValueField = "movie_id";
                    ddlMovies.DataBind();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error Loading Dropdowns: " + ex.Message;
                    lblMessage.CssClass = "text-danger";
                }
            }
        }

        // COMPLEX QUERY 1: User History (Last 6 Months)
        protected void BtnUserReport_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (string.IsNullOrEmpty(txtUserIdSearch.Text)) return;

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string sql = @"SELECT U.user_name, T.ticket_id, T.ticket_price, 
                                      T.ticket_status,
                                      TO_CHAR(T.booking_time, 'YYYY-MM-DD HH24:MI') as Booking_Date, 
                                      T.payment_status 
                               FROM USERS U 
                               JOIN USERMOVIETHEATREHALLSHOWTICKET L ON U.user_id = L.user_id 
                               JOIN TICKET T ON L.ticket_id = T.ticket_id 
                               WHERE U.user_id = :p_userid 
                               AND T.booking_time >= ADD_MONTHS(SYSDATE, -6)";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("p_userid", OracleDbType.Int32).Value = Convert.ToInt32(txtUserIdSearch.Text);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    gvUserTickets.DataSource = dt;
                    gvUserTickets.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        lblMessage.Text = "Found " + dt.Rows.Count + " tickets for User " + txtUserIdSearch.Text;
                        lblMessage.CssClass = "text-success";
                    }
                    else
                    {
                        lblMessage.Text = "No tickets found for this user in the last 6 months.";
                        lblMessage.CssClass = "text-warning";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Query Error: " + ex.Message;
                    lblMessage.CssClass = "text-danger";
                }
            }
        }

        // COMPLEX QUERY 2: Theatre Details
        protected void BtnTheatreReport_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string sql = @"SELECT DISTINCT M.movie_title, S.show_date, TO_CHAR(S.show_time, 'HH24:MI:SS') as Time 
                               FROM THEATRE TH 
                               JOIN USERMOVIETHEATREHALLSHOWTICKET L ON TH.theatre_id = L.theatre_id 
                               JOIN MOVIE M ON L.movie_id = M.movie_id 
                               JOIN SHOWS S ON L.show_id = S.show_id 
                               WHERE TH.theatre_id = :p_tid";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("p_tid", OracleDbType.Int32).Value = Convert.ToInt32(ddlTheatre.SelectedValue);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                try
                {
                    da.Fill(dt);
                    gvTheatreMovies.DataSource = dt;
                    gvTheatreMovies.DataBind();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        // COMPLEX QUERY 3: Top 3 Occupancy
        protected void BtnOccupancyReport_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string sql = @"SELECT * FROM (
                                SELECT TH.theatre_name, 
                                       ROUND((COUNT(TK.ticket_id) / SUM(H.hall_capacity)) * 100, 2) || '%' as Occupancy
                                FROM MOVIE M
                                JOIN USERMOVIETHEATREHALLSHOWTICKET L ON M.movie_id = L.movie_id
                                JOIN TICKET TK ON L.ticket_id = TK.ticket_id
                                JOIN THEATRE TH ON L.theatre_id = TH.theatre_id
                                JOIN HALL H ON L.hall_id = H.hall_id
                                WHERE M.movie_id = :p_mid AND TK.payment_status = 'Paid'
                                GROUP BY TH.theatre_name
                                ORDER BY Occupancy DESC
                               ) WHERE ROWNUM <= 3";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("p_mid", OracleDbType.Int32).Value = Convert.ToInt32(ddlMovies.SelectedValue);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                try
                {
                    da.Fill(dt);
                    gvTopOccupancy.DataSource = dt;
                    gvTopOccupancy.DataBind();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }
    }
}