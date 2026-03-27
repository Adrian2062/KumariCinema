using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class TopOccupancyReport : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["OracleDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMovies();
            }
        }

        private void LoadMovies()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                try
                {
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
                    lblMessage.Text = "Error Loading Movies: " + ex.Message;
                    lblMessage.CssClass = "text-danger";
                }
            }
        }

        protected void BtnOccupancyReport_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string sql = @"SELECT * FROM (
                                   SELECT 
                                       TH.theatre_name AS ""THEATRENAME"",
                                       'Hall ' || H.hall_id AS ""HALLNAME"",
                                       H.hall_capacity AS ""HALLCAPACITY"",
                                       COUNT(DISTINCT CASE WHEN TK.payment_status = 'Paid' THEN TK.ticket_id END) AS ""PAIDTICKETS"",
                                       ROUND(
                                           (COUNT(DISTINCT CASE WHEN TK.payment_status = 'Paid' THEN TK.ticket_id END) / 
                                           (COUNT(DISTINCT L.show_id) * H.hall_capacity)) * 100, 2
                                       ) AS ""OCCUPANCYPERCENT""
                                   FROM USERMOVIETHEATREHALLSHOWTICKET L
                                   JOIN THEATRE TH ON L.theatre_id = TH.theatre_id
                                   JOIN HALL H ON L.hall_id = H.hall_id
                                   JOIN TICKET TK ON L.ticket_id = TK.ticket_id
                                   WHERE L.movie_id = :p_mid
                                   GROUP BY TH.theatre_name, H.hall_id, H.hall_capacity
                                   ORDER BY ""OCCUPANCYPERCENT"" DESC
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
                    lblMessage.CssClass = "text-danger";
                }
            }
        }
    }
}