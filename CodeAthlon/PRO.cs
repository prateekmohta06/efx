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
    public partial class PRO : Form
    {
        public PRO(string proname, string category, int zipcode, string phone, double rating, int noofjobs, double rateusd, string skills, string lat, string lng)
        {
            InitializeComponent();
            txtPRONAME.Text = proname;
            txtCategory.Text = category;
            txtZip.Text = zipcode.ToString();
            txtPhone.Text = phone;
            txtRateUSD.Text = rateusd.ToString();
            

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
                                skillGRID.Rows = skillGRID.Rows + 1;
                                skillGRID.Row = skillGRID.Rows - 1;
                                skillGRID.Col = 0;
                                skillGRID.Text = reader["B_AvailabilityDate"].ToString();
                                skillGRID.Col = 1;
                                skillGRID.Text = reader["B_StartTime"].ToString();
                                skillGRID.Col = 2;
                                skillGRID.Text = reader["B_EndTime"].ToString();
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
        }

        public string connectionString = @"Data Source=C:\Users\atman\Desktop\homedepot.db; Version=3; FailIfMissing=True; Foreign Keys=True;";

        
        private void PRO_Load(object sender, EventArgs e)
        {
            skillGRID.Row = 0;

            skillGRID.set_ColWidth(0, 1500);
            skillGRID.Col = 0;
            skillGRID.Text = "Start Date";

            skillGRID.set_ColWidth(1, 1500);
            skillGRID.Col = 1;
            skillGRID.Text = "Start Time";

            skillGRID.set_ColWidth(2, 1500);
            skillGRID.Col = 2;
            skillGRID.Text = "End Time";
        }

        private void lstSkills_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            lstSkills.Items.Add(textBox1.Text );
            textBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lstSkills.Items.RemoveAt(lstSkills.SelectedIndex);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            skillGRID.Rows = skillGRID.Rows + 1;
            skillGRID.Row = skillGRID.Rows - 1;
            skillGRID.Col = 0;
            skillGRID.Text = textBox3.Text;
            skillGRID.Col = 1;
            skillGRID.Text = textBox2.Text;
            skillGRID.Col = 2;
            skillGRID.Text = textBox4.Text;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "INSERT INTO BOOKING (B_Pro_name,B_AvailabilityDate,B_StartTime,B_EndTime) VALUES ('" + txtPRONAME.Text + "','" +
                        textBox3.Text + "','" + textBox2.Text + "','" + textBox4.Text + "'); commit;";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    //SQLiteDataReader reader = cmd.ExecuteReader();
                    //cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.StackTrace);
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            skillGRID.RemoveItem(skillGRID.Row);

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "DELETE FROM BOOKING WHERE B_Pro_name='" + txtPRONAME.Text + "' AND B_AvailabilityDate = '" +
                        textBox3.Text + "' AND B_StartTime = '" + textBox2.Text + "' AND B_EndTime ='" + textBox4.Text + "'; commit;";
                    textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    

                    conn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        private void skillGRID_Enter(object sender, EventArgs e)
        {
            
        }

        private void skillGRID_EnterCell(object sender, EventArgs e)
        {
           
        }

        private void skillGRID_ClickEvent(object sender, EventArgs e)
        {
            skillGRID.Col = 0;
            textBox3.Text = skillGRID.Text;
            skillGRID.Col = 1;
            textBox2.Text = skillGRID.Text;
            skillGRID.Col = 2;
            textBox4.Text = skillGRID.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE WHERE B_Pro_name='" + txtPRONAME.Text + "' AND B_AvailabilityDate = '" +
                        textBox3.Text + "' AND B_StartTime = '" + textBox2.Text + "' AND B_EndTime ='" + textBox4.Text + "'; commit;";
                    textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);

                    cmd.ExecuteNonQuery();


                    conn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.StackTrace);
            }
        }
    }
}
