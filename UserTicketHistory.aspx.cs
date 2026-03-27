using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class UserTicketHistory : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["OracleDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
                BindEmptyGrid();
            }
        }

        private void LoadUsers()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                try
                {
                    OracleDataAdapter daU = new OracleDataAdapter("SELECT USER_ID, USER_ID || ' - ' || USER_NAME AS DISPLAY_NAME FROM USERS ORDER BY USER_ID", conn);
                    DataTable dtU = new DataTable();
                    daU.Fill(dtU);
                    ddlUsers.DataSource = dtU;
                    ddlUsers.DataTextField = "DISPLAY_NAME";
                    ddlUsers.DataValueField = "USER_ID";
                    ddlUsers.DataBind();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error Loading Users: " + ex.Message;
                    lblMessage.CssClass = "text-danger";
                }
            }
        }

        private void BindEmptyGrid()
        {
            DataTable dtEmpty = new DataTable();
            string[] cols = { "USERID", "USERNAME", "PHONENUMBER", "TICKETID", "TICKETPRICE", "TICKETSTATUS", "BOOKINGTIME", "PAYMENTSTATUS", "MOVIE", "THEATRE", "HALL", "SHOWDATE" };
            foreach (string col in cols) dtEmpty.Columns.Add(col);
            gvUserTickets.DataSource = dtEmpty;
            gvUserTickets.DataBind();
        }

        protected void BtnUserReport_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (string.IsNullOrEmpty(ddlUsers.SelectedValue)) return;

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // Updated SQL: Starting with LEFT JOIN on the link table itself 
                // ensures even users with NO tickets still appear in the report headers.
                string sql = @"SELECT 
                                   U.USER_ID AS ""USERID"", 
                                   U.USER_NAME AS ""USERNAME"", 
                                   U.ADDRESS AS ""PHONENUMBER"", 
                                   T.TICKET_ID AS ""TICKETID"", 
                                   T.TICKET_PRICE AS ""TICKETPRICE"", 
                                   T.TICKET_STATUS AS ""TICKETSTATUS"", 
                                   TO_CHAR(T.BOOKING_TIME, 'YYYY-MM-DD HH:MI') AS ""BOOKINGTIME"", 
                                   T.PAYMENT_STATUS AS ""PAYMENTSTATUS"", 
                                   M.MOVIE_TITLE AS ""MOVIE"", 
                                   TH.THEATRE_NAME AS ""THEATRE"", 
                                   'Hall ' || H.HALL_ID AS ""HALL"", 
                                   TO_CHAR(S.SHOW_DATE, 'YYYY-MM-DD') AS ""SHOWDATE""
                               FROM USERS U
                               LEFT JOIN USERMOVIETHEATREHALLSHOWTICKET L ON U.USER_ID = L.USER_ID
                               LEFT JOIN TICKET T ON L.TICKET_ID = T.TICKET_ID
                               LEFT JOIN MOVIE M ON L.MOVIE_ID = M.MOVIE_ID
                               LEFT JOIN THEATRE TH ON L.THEATRE_ID = TH.THEATRE_ID
                               LEFT JOIN HALL H ON L.HALL_ID = H.HALL_ID
                               LEFT JOIN SHOWS S ON L.SHOW_ID = S.SHOW_ID
                               WHERE U.USER_ID = :p_userid 
                               ORDER BY T.BOOKING_TIME DESC NULLS LAST";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("p_userid", OracleDbType.Int32).Value = Convert.ToInt32(ddlUsers.SelectedValue);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        gvUserTickets.DataSource = dt;
                        gvUserTickets.DataBind();
                    }
                    else
                    {
                        BindEmptyGrid();
                        lblMessage.Text = "No ticket history records found for this user.";
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
    }
}