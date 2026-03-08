using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace KumariCinemas
{
    public partial class UserDetails : System.Web.UI.Page
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
                // FIXED: Changed \"User\" to USERS
                OracleDataAdapter da = new OracleDataAdapter("SELECT user_id, user_name, address FROM USERS ORDER BY user_id", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // FIXED: Changed \"User\" to USERS
                OracleCommand cmd = new OracleCommand("INSERT INTO USERS (user_id, user_name, address) VALUES (:id, :name, :address)", conn);
                cmd.Parameters.Add("id", txtUserId.Text);
                cmd.Parameters.Add("name", txtUserName.Text);
                cmd.Parameters.Add("address", txtAddress.Text);

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "User Added successfully!";

                // Clear textboxes after insert
                txtUserId.Text = ""; txtUserName.Text = ""; txtAddress.Text = "";
                BindGrid();
            }
        }

        protected void GvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            BindGrid();
        }

        protected void GvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // FIXED: Changed \"User\" to USERS
                OracleCommand cmd = new OracleCommand("UPDATE USERS SET user_name = :name, address = :addr WHERE user_id = :id", conn);
                cmd.Parameters.Add("name", (gvUsers.Rows[e.RowIndex].Cells[1].Controls[0] as TextBox).Text);
                cmd.Parameters.Add("addr", (gvUsers.Rows[e.RowIndex].Cells[2].Controls[0] as TextBox).Text);
                cmd.Parameters.Add("id", Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value));

                conn.Open();
                cmd.ExecuteNonQuery();
                gvUsers.EditIndex = -1;
                lblMessage.Text = "User Updated successfully!";
                BindGrid();
            }
        }

        protected void GvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connStr))
            {
                // FIXED: Changed \"User\" to USERS
                OracleCommand cmd = new OracleCommand("DELETE FROM USERS WHERE user_id = :id", conn);
                cmd.Parameters.Add("id", Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value));

                conn.Open();
                cmd.ExecuteNonQuery();
                lblMessage.Text = "User Deleted successfully!";
                BindGrid();
            }
        }
    }
}