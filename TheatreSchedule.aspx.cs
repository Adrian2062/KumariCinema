using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class TheatreSchedule : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["OracleDBConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTheatres();
                // Add a default prompt
                ddlHall.Items.Insert(0, new ListItem("-- Select Theatre First --", "0"));
            }
        }

        private void LoadTheatres()
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                OracleDataAdapter da = new OracleDataAdapter("SELECT THEATRE_ID, THEATRE_NAME FROM THEATRE ORDER BY THEATRE_NAME", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlTheatre.DataSource = dt;
                ddlTheatre.DataTextField = "THEATRE_NAME";
                ddlTheatre.DataValueField = "THEATRE_ID";
                ddlTheatre.DataBind();
                ddlTheatre.Items.Insert(0, new ListItem("-- Select Theatre --", "0"));
            }
        }

        // Triggered automatically when Theatre selection changes
        protected void ddlTheatre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTheatre.SelectedValue != "0")
            {
                LoadHallsForTheatre(ddlTheatre.SelectedValue);
            }
            else
            {
                ddlHall.Items.Clear();
                ddlHall.Items.Insert(0, new ListItem("-- Select Theatre First --", "0"));
            }
        }

        private void LoadHallsForTheatre(string theatreId)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // Filters halls specifically linked to the selected Theatre in your Junction Table
                string sql = @"SELECT DISTINCT H.HALL_ID, 'Hall ' || H.HALL_ID AS DISPLAY_NAME 
                               FROM HALL H
                               JOIN USERMOVIETHEATREHALLSHOWTICKET L ON H.HALL_ID = L.HALL_ID
                               WHERE L.THEATRE_ID = :tid";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("tid", OracleDbType.Int32).Value = Convert.ToInt32(theatreId);

                DataTable dt = new DataTable();
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);

                ddlHall.DataSource = dt;
                ddlHall.DataTextField = "DISPLAY_NAME";
                ddlHall.DataValueField = "HALL_ID";
                ddlHall.DataBind();
                ddlHall.Items.Insert(0, new ListItem("-- Select Hall --", "0"));
            }
        }

        protected void BtnTheatreReport_Click(object sender, EventArgs e)
        {
            if (ddlTheatre.SelectedValue == "0" || ddlHall.SelectedValue == "0")
            {
                lblMessage.Text = "Please select both a Theatre and a Hall.";
                return;
            }

            lblMessage.Text = "";
            lblTheatreResult.Text = $"Showing results for: {ddlTheatre.SelectedItem.Text} - {ddlHall.SelectedItem.Text}";

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                string sql = @"SELECT DISTINCT 
                                   M.MOVIE_TITLE AS ""MOVIETITLE"", 
                                   M.LANGUAGE AS ""MOVIELANGUAGE"", 
                                   M.GENRE AS ""GENRE"", 
                                   TO_CHAR(S.SHOW_DATE, 'YYYY-MM-DD') AS ""SHOWDATE"", 
                                   TO_CHAR(S.SHOW_TIME, 'HH24:MI') AS ""SHOWTIME""
                               FROM USERMOVIETHEATREHALLSHOWTICKET L
                               JOIN MOVIE M ON L.MOVIE_ID = M.MOVIE_ID
                               JOIN SHOWS S ON L.SHOW_ID = S.SHOW_ID
                               WHERE L.THEATRE_ID = :p_tid AND L.HALL_ID = :p_hid
                               ORDER BY ""SHOWDATE"", ""SHOWTIME""";

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.Parameters.Add("p_tid", OracleDbType.Int32).Value = Convert.ToInt32(ddlTheatre.SelectedValue);
                cmd.Parameters.Add("p_hid", OracleDbType.Int32).Value = Convert.ToInt32(ddlHall.SelectedValue);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTheatreMovies.DataSource = dt;
                gvTheatreMovies.DataBind();
            }
        }
    }
}