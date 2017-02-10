
// // CTA Ridership analysis using C# and SQL Serer. // 
// <<Krunal Patel>> 
// U. of Illinois, Chicago
// CS341, Fall 2016 
// Homework 6 // 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";

            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;",version, filename);

            db = new SqlConnection(connectionInfo);
            db.Open();
            string sql =  string.Format(@"SELECT Name FROM Stations ORDER BY Name ASC;");
            // MessageBox.Show(db.State.ToString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);

            //object result = cmd.ExecuteScalar();

            db.Close();

            foreach (DataRow row in ds.Tables["TABLE"].Rows) {
                string msg = string.Format("{0}", Convert.ToString(row["Name"]));
                this.listBox1.Items.Add(msg);
            }


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show( Convert.ToString(this.listBox1.SelectedItem));
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";

            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);

            db = new SqlConnection(connectionInfo);
            db.Open();
            string sql = string.Format(@"SELECT StationID FROM Stations WHERE Name = '{0}';", this.listBox1.SelectedItem);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            object result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            int stationID = Convert.ToInt32(result);
            //-------------------------------------Stops ar this Station-----------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT Name FROM Stops WHERE StationID = {0} ORDER BY Name ASC;", stationID);
            // MessageBox.Show(db.State.ToString());
            cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);

            //object result = cmd.ExecuteScalar();

            db.Close();
            this.listBox2.Items.Clear();
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                string msg = string.Format("{0}", Convert.ToString(row["Name"]));
                this.listBox2.Items.Add(msg);
            }
            //-----------------------------------------Total Ridership---------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT SUM(DailyTotal) AS Output FROM Riderships WHERE StationID = '{0}';", stationID);
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            int totalRidership = Convert.ToInt32(result);
            this.textBox1.Text = Convert.ToString(result);

            //------------------------------------------Avg. Ridership-------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT COUNT(*) FROM Riderships WHERE StationID = '{0}';", stationID);
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            db.Close();
            int numDays = Convert.ToInt32(result);
            int averageRidership = totalRidership / numDays;
            // MessageBox.Show(Convert.ToString(result));
            String msg1 = string.Format("{0}/day", Convert.ToString(averageRidership));
            this.textBox2.Text = msg1;

            //----------------------------------------%Ridership ---------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT SUM(CAST(DailyTotal AS BIGINT)) AS Output FROM Riderships;");
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            //msg1 = string.Format("{0}%", Convert.ToString(result));
            long totalforAllRidership = Convert.ToInt64(result);
            long outputRidership = ((long)totalRidership); 
            string msg3 = ((float)totalRidership/totalforAllRidership).ToString("0.00%");
            this.textBox3.Text = Convert.ToString(msg3);

            //----------------------------------------WeekDay---------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT SUM(DailyTotal) AS Output FROM Riderships WHERE StationID = '{0}' AND TypeOfDay = 'W';", stationID);
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            msg1 = string.Format("{0}", Convert.ToString(result));
            this.textBox7.Text = msg1;
            //----------------------------------------Saturday---------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT SUM(DailyTotal) AS Output FROM Riderships WHERE StationID = '{0}' AND TypeOfDay = 'A';", stationID);
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            msg1 = string.Format("{0}", Convert.ToString(result));
            this.textBox8.Text = msg1;
            //----------------------------------------Sunday/Holiday---------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT SUM(DailyTotal) AS Output FROM Riderships WHERE StationID = '{0}' AND TypeOfDay = 'U';", stationID);
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            msg1 = string.Format("{0}", Convert.ToString(result));
            this.textBox9.Text =msg1;

        }
    private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBox3.Items.Clear();
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";
            string userInput;

            userInput = this.listBox2.GetItemText(listBox2.SelectedItem);

            userInput = userInput.Replace("'", "''");

            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);

            db = new SqlConnection(connectionInfo);
            db.Open();
            string sql = string.Format(@"SELECT StopID FROM Stops WHERE Name = '{0}';", userInput);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            object result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            int stopID = Convert.ToInt32(result);
            string msg1 = string.Format("{0}", Convert.ToString(stopID));
            //-------------------------------------Stops ar this Station-----------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT LineID FROM StopDetails WHERE StopID = {0};", stopID);
            // MessageBox.Show(db.State.ToString());
            cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);

            //object result = cmd.ExecuteScalar();
            
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                sql = string.Format(@"SELECT Color FROM Lines WHERE LineID = {0};", row["LineID"]);
                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                result = cmd.ExecuteScalar();
                string msg = string.Format("{0}", Convert.ToString(result));
                this.listBox3.Items.Add(msg);
            }
            db.Close();
            //-------------------------------------Loaction-----------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT Latitude, Longitude FROM Stops WHERE StopID = {0};", stopID);
            cmd = new SqlCommand();
            cmd.Connection = db;
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                msg1 = string.Format("({0},{1})", Convert.ToString(row["Latitude"]), Convert.ToString(row["Longitude"]));
                this.textBox4.Text = msg1;
            }

            //----------------------------------------Handicap---------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT ADA FROM Stops WHERE StopID = {0};", stopID);
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            
            if ((Boolean)result)
            {
                this.textBox6.Text = "Yes";
            }
            else
            {
                this.textBox6.Text = "No";
            }

            //----------------------------------------Direction---------------------------------------------------
            db = new SqlConnection(connectionInfo);
            db.Open();
            sql = string.Format(@"SELECT Direction FROM Stops WHERE StopID = {0};", stopID);
            cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            result = cmd.ExecuteScalar();
            db.Close();
            // MessageBox.Show(Convert.ToString(result));
            msg1 = string.Format("{0}", Convert.ToString(result));
            this.textBox5.Text = msg1;
            //-------------------------------------END-----------------------------------------------------


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            string filename, version, connectionInfo;
            SqlConnection db;
            version = "MSSQLLocalDB";
            filename = "CTA.mdf";

            connectionInfo = String.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename=|DataDirectory|\{1};Integrated Security=True;", version, filename);

            db = new SqlConnection(connectionInfo);
            db.Open();
            string sql = string.Format(@"SELECT TOP 10 Stations.Name FROM Riderships, Stations WHERE Riderships.StationID = Stations.StationID 
                                    GROUP BY Name ORDER BY SUM(Riderships.DailyTotal)DESC;");
            // MessageBox.Show(db.State.ToString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            cmd.CommandText = sql;
            adapter.Fill(ds);

            //object result = cmd.ExecuteScalar();

            db.Close();

            foreach (DataRow row in ds.Tables["TABLE"].Rows)
            {
                string msg = string.Format("{0}", Convert.ToString(row["name"]));
                this.listBox1.Items.Add(msg);
            }
            //-------------------------------------------------------------------------------------------------
        }
    }
}
