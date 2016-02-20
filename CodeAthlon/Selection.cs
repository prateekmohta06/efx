using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace CodeAthlon
{
    public partial class Selection : Form
    {
        string lat;
        string lng;
        public string connectionString = @"Data Source=C:\Users\atman\Desktop\homedepot.db; Version=3; FailIfMissing=True; Foreign Keys=True;";

        public Selection(string proname, string category, string zipcode, string phone, string rating, string noofjobs, string rateusd, string skills, string lat, string lng)
        {
            InitializeComponent();
            this.webKitBrowser1.Navigated +=
                new WebBrowserNavigatedEventHandler(webKitBrowser1_Navigated);

            txtPRONAME.Text = proname;
            txtCategory.Text = category;
            txtZip.Text = zipcode;
            txtPhone.Text = phone;
            txtRatings.Text = rating;
            txtJobs.Text = noofjobs;
            txtRateUSD.Text = rateusd;
            this.lat = lat;
            this.lng = lng;

            string[] listofskills = skills.Split(',');
            for (int i = 0; i < listofskills.Length; i++)
            {
                lstSkills.Items.Add(listofskills[i].Trim(' '));
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM BOOKING WHERE B_PRO_NAME = '" + txtPRONAME.Text + "'";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GRD.Rows = GRD.Rows + 1;
                                GRD.Row = GRD.Rows - 1;
                                GRD.Col = 0;
                                GRD.Text = reader["B_AvailabilityDate"].ToString();
                                GRD.Col = 1;
                                GRD.Text = reader["B_StartTime"].ToString();
                                GRD.Col = 2;
                                GRD.Text = reader["B_EndTime"].ToString();
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.StackTrace);
            }

            try {
                string urlstr = "http://maps.google.com/maps?q=" + lat + "," + lng;
                webKitBrowser1.Navigate(urlstr);
            }
            catch(Exception ex)
            {
                Console.Write(ex.StackTrace);
            }

        }

        void webKitBrowser1_Navigated(object sender,
            WebBrowserNavigatedEventArgs e)
        {
            
        }

        private void Selection_Load(object sender, EventArgs e)
        {
            GRD.Row = 0;

            GRD.set_ColWidth(0, 1500);
            GRD.Col = 0;
            GRD.Text = "Start Date";

            GRD.set_ColWidth(1, 1500);
            GRD.Col = 1;
            GRD.Text = "Start Time";

            GRD.set_ColWidth(2, 1500);
            GRD.Col = 2;
            GRD.Text = "End Time";

        }
    }
}
