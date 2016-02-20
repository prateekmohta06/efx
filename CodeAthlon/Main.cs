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
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace CodeAthlon
{
    public partial class Main : Form
    {
        public string connectionString = @"Data Source=C:\Users\atman\Desktop\homedepot.db; Version=3; FailIfMissing=True; Foreign Keys=True;";
        List<PRO_CUSTOMER> pros_list = new List<PRO_CUSTOMER>();
        //static ManualResetEvent _completed = null;

        public Main()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            GRD.Row = 0;

            GRD.set_ColWidth(0, 3000);
            GRD.Col = 0;
            GRD.Text = "PRO Name";

            GRD.set_ColWidth(1, 4500);
            GRD.Col = 1;
            GRD.Text = "Category";

            GRD.set_ColWidth(2, 1000);
            GRD.Col = 2;
            GRD.Text = "Zip";

            GRD.set_ColWidth(3, 2000);
            GRD.Col = 3;
            GRD.Text = "Contact";

            GRD.set_ColWidth(4, 1000);
            GRD.Col = 4;
            GRD.Text = "Rating";

            GRD.set_ColWidth(5, 1000);
            GRD.Col = 5;
            GRD.Text = "Jobs";

            GRD.set_ColWidth(6, 1000);
            GRD.Col = 6;
            GRD.Text = "Pay";

            GRD.set_ColWidth(7, 3000);
            GRD.Col = 7;
            GRD.Text = "Availability";

            GRD.set_ColWidth(8, 0);
            GRD.Col = 8;
            GRD.Text = "Skills";

            GRD.set_ColWidth(9, 0);
            GRD.Col = 9;
            GRD.Text = "Lat";

            GRD.set_ColWidth(10, 0);
            GRD.Col = 10;
            GRD.Text = "Long";

            lstCategory.Items.Clear();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT S_CATEGORY FROM CATEGORY_SERVICE_MAPPING";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstCategory.Items.Add(reader["S_CATEGORY"].ToString());
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


            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM PRO_CUSTOMER_LIST";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PRO_CUSTOMER cust = new PRO_CUSTOMER();
                                cust.Pro_Customer_Name = reader["Pro_Name"].ToString();
                                cust.Pro_Category_Name = reader["Pro_Category"].ToString();
                                cust.Pro_Zip_Code = int.Parse(reader["Pro_Zip"].ToString());
                                cust.Pro_Phone_Number = reader["Pro_Phone"].ToString();
                                cust.Pro_Rating_from_5 = double.Parse(reader["Pro_Rating"].ToString());
                                cust.Pro_No_Of_Orders = int.Parse(reader["Pro_NoOfOrders"].ToString());
                                cust.Pro_Rate_in_USD = double.Parse(reader["Pro_Rate"].ToString());
                                cust.Pro_Skills_List = reader["Pro_Skills"].ToString();
                                cust.Lat = reader["Latitude"].ToString();
                                cust.Lng = reader["Longitude"].ToString();
                                cust.Username = reader["username"].ToString();
                                cust.Passwd = reader["password"].ToString();
                                pros_list.Add(cust);
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

        private void lstCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstServices.Items.Clear();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT S_SERVICE FROM CATEGORY_SERVICE_MAPPING where S_CATEGORY = '" + lstCategory.SelectedItem.ToString() + "'";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lstServices.Items.Add(reader["S_SERVICE"].ToString());
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

        private void lstServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            GRD.Rows=1;
            GRD.Visible = false;        

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT DISTINCT PCS_PRONAME from PRO_Customer_service where PCS_service = '" + lstServices.SelectedItem.ToString() + "'";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {


                                using (SQLiteConnection conn_for_details = new SQLiteConnection(connectionString))
                                {
                                    conn_for_details.Open();
                                    string sql_for_details = "SELECT * FROM PRO_Customer_List WHERE PRO_NAME = '" + reader["PCS_PRONAME"].ToString() + "'";

                                    using (SQLiteCommand cmd_for_details = new SQLiteCommand(sql_for_details, conn_for_details))
                                    {
                                        using (SQLiteDataReader reader_for_details = cmd_for_details.ExecuteReader())
                                        {
                                            while (reader_for_details.Read())
                                            {
                                                GRD.Rows = GRD.Rows + 1;
                                                GRD.Row = GRD.Rows - 1;
                                                GRD.Col = 0;
                                                GRD.Text = reader_for_details["Pro_Name"].ToString();
                                                GRD.Col = 1;
                                                GRD.Text = reader_for_details["Pro_Category"].ToString();
                                                GRD.Col = 2;
                                                GRD.Text = reader_for_details["Pro_Zip"].ToString();
                                                GRD.Col = 3;
                                                GRD.Text = reader_for_details["Pro_Phone"].ToString();
                                                GRD.Col = 4;
                                                GRD.Text = reader_for_details["Pro_Rating"].ToString();
                                                GRD.Col = 5;
                                                GRD.Text = reader_for_details["Pro_NoOfOrders"].ToString();
                                                GRD.Col = 6;
                                                GRD.Text = reader_for_details["Pro_Rate"].ToString();
                                                GRD.Col = 7;
                                                GRD.Text = "Check Availability";
                                                GRD.Col = 8;
                                                GRD.Text = reader_for_details["Pro_Skills"].ToString();
                                                GRD.Col = 9;
                                                GRD.Text = reader_for_details["Latitude"].ToString();
                                                GRD.Col = 10;
                                                GRD.Text = reader_for_details["Longitude"].ToString();
                                            }
                                        }
                                    }
                                    conn_for_details.Close();
                                }

                              
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
            GRD.Visible = true;
        }

        private void GRD_Enter(object sender, EventArgs e)
        {

        }

        private void GRD_EnterCell(object sender, EventArgs e)
        {
            // Selection profile = new Selection();
            // GRD.Col = 0;
            // profile.txtPRONAME.Text = GRD.Text;
            // GRD.Col = 1;
            
        }

        private void GRD_ClickEvent(object sender, EventArgs e)
        {
            string proname, category,  zipcode,  phone,  rating,  noofjobs,  rateusd,  skills, lat, lng;
            GRD.Col = 0;
            proname = GRD.Text;
            GRD.Col = 1;
            category = GRD.Text;
            GRD.Col = 2;
            zipcode = GRD.Text;
            GRD.Col = 3;
            phone = GRD.Text;
            GRD.Col = 4;
            rating = GRD.Text;
            GRD.Col = 5;
            noofjobs = GRD.Text;
            GRD.Col = 6;
            rateusd = GRD.Text;
            GRD.Col = 8;
            skills = GRD.Text;
            GRD.Col = 9;
            lat = GRD.Text;
            GRD.Col = 10;
            lng = GRD.Text;
            Selection profile = new Selection(proname, category, zipcode, phone, rating, noofjobs, rateusd, skills, lat, lng);
            profile.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string search_str = textBox1.Text;
            string[] tokens = search_str.Split(' ');
            List<count_dictionary> countset = new List<count_dictionary>();
            GRD.Visible = false;
            GRD.Rows = 1;

           
                for (int j = 0; j < pros_list.Count; j++)
                {
                    if (pros_list[j].Pro_Skills_List.ToString().ToLower().Contains(search_str))
                    {
                        GRD.Rows = GRD.Rows + 1;
                        GRD.Row = GRD.Rows - 1;
                        GRD.Col = 0;
                        GRD.Text = pros_list[j].Pro_Customer_Name;
                        GRD.Col = 1;
                        GRD.Text = pros_list[j].Pro_Category_Name;
                        GRD.Col = 2;
                        GRD.Text = pros_list[j].Pro_Zip_Code.ToString();
                        GRD.Col = 3;
                        GRD.Text = pros_list[j].Pro_Phone_Number.ToString();
                        GRD.Col = 4;
                        GRD.Text = pros_list[j].Pro_Rating_from_5.ToString();
                        GRD.Col = 5;
                        GRD.Text = pros_list[j].Pro_No_Of_Orders.ToString();
                        GRD.Col = 6;
                        GRD.Text = pros_list[j].Pro_Rate_in_USD.ToString();
                        GRD.Col = 7;
                        GRD.Text = "Check Availability";
                        GRD.Col = 8;
                        GRD.Text = pros_list[j].Pro_Skills_List;
                        GRD.Col = 9;
                        GRD.Text = pros_list[j].Lat;
                        GRD.Col = 10;
                        GRD.Text = pros_list[j].Lng;
                    }
                }
            

            for (int i = 0; i < tokens.Length; i++)
            {
                for (int j = 0; j < pros_list.Count; j++)
                {
                    if (pros_list[j].Pro_Skills_List.ToString().ToLower().Contains(tokens[i].ToLower()))
                    {
                        GRD.Rows = GRD.Rows + 1;
                        GRD.Row = GRD.Rows - 1;
                        GRD.Col = 0;
                        GRD.Text = pros_list[j].Pro_Customer_Name;
                        GRD.Col = 1;
                        GRD.Text = pros_list[j].Pro_Category_Name;
                        GRD.Col = 2;
                        GRD.Text = pros_list[j].Pro_Zip_Code.ToString();
                        GRD.Col = 3;
                        GRD.Text = pros_list[j].Pro_Phone_Number.ToString();
                        GRD.Col = 4;
                        GRD.Text = pros_list[j].Pro_Rating_from_5.ToString();
                        GRD.Col = 5;
                        GRD.Text = pros_list[j].Pro_No_Of_Orders.ToString();
                        GRD.Col = 6;
                        GRD.Text = pros_list[j].Pro_Rate_in_USD.ToString();
                        GRD.Col = 7;
                        GRD.Text = "Check Availability";
                        GRD.Col = 8;
                        GRD.Text = pros_list[j].Pro_Skills_List;
                        GRD.Col = 9;
                        GRD.Text = pros_list[j].Lat;
                        GRD.Col = 10;
                        GRD.Text = pros_list[j].Lng;
                    }
                }
            }
            GRD.Visible = true;
            lblSM.Text = (GRD.Rows - 1).ToString();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (button1.Text == "Listen ? No") button1.Text = "Listen ? Yes";
            else button1.Text = "Listen ? No";
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (txtPROLoginPassword.Text == "" || txtProLoginUsername.Text == "")
            {
                MessageBox.Show("Username and Password is requried");
            }
            else {
                for (int i = 0; i < pros_list.Count; i++)
                {
                    if (pros_list[i].Username == txtProLoginUsername.Text && pros_list[i].Passwd == txtPROLoginPassword.Text)
                    {
                        PRO proCustomer = new PRO(pros_list[i].Pro_Customer_Name, pros_list[i].Pro_Category_Name, pros_list[i].Pro_Zip_Code, pros_list[i].Pro_Phone_Number,
                            pros_list[i].Pro_Rating_from_5, pros_list[i].Pro_No_Of_Orders, pros_list[i].Pro_Rate_in_USD,
                            pros_list[i].Pro_Skills_List, pros_list[i].Lat, pros_list[i].Lng);
                        proCustomer.Show();
                    }

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }
    }

    class count_dictionary
    {
        string key;
        int count;

        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }

            set
            {
                count = value;
            }
        }
    }


    // SELECT DISTINCT PCS_PRONAME from PRO_Customer_service where PCS_Category = 
    // This class is entity class for PRO Customer / Service Mapping
    class PRO_CUSTOMER
    {
        int Pro_ID;
        string Pro_Name;
        string Pro_Category;
        int Pro_Zip;
        string Pro_Phone;
        double Pro_Rating;
        string Pro_BackgroundCheck;
        int Pro_NoOfOrders;
        double Pro_Rate;
        string Pro_Skills;
        string lat;
        string lng;
        string username;
        string passwd;

        public int Pro_ID_No
        {
            get
            {
                return Pro_ID;
            }

            set
            {
                Pro_ID = value;
            }
        }

        public string Pro_Customer_Name
        {
            get
            {
                return Pro_Name;
            }

            set
            {
                Pro_Name = value;
            }
        }

        public string Pro_Category_Name
        {
            get
            {
                return Pro_Category;
            }

            set
            {
                Pro_Category = value;
            }
        }

        public int Pro_Zip_Code
        {
            get
            {
                return Pro_Zip;
            }

            set
            {
                Pro_Zip = value;
            }
        }

        public string Pro_Phone_Number
        {
            get
            {
                return Pro_Phone;
            }

            set
            {
                Pro_Phone = value;
            }
        }

        public double Pro_Rating_from_5
        {
            get
            {
                return Pro_Rating;
            }

            set
            {
                Pro_Rating = value;
            }
        }

        public string Pro_Background_Check
        {
            get
            {
                return Pro_BackgroundCheck;
            }

            set
            {
                Pro_BackgroundCheck = value;
            }
        }

        public int Pro_No_Of_Orders
        {
            get
            {
                return Pro_NoOfOrders;
            }

            set
            {
                Pro_NoOfOrders = value;
            }
        }

        public double Pro_Rate_in_USD
        {
            get
            {
                return Pro_Rate;
            }

            set
            {
                Pro_Rate = value;
            }
        }

        public string Pro_Skills_List
        {
            get
            {
                return Pro_Skills;
            }

            set
            {
                Pro_Skills = value;
            }
        }

        public string Lat
        {
            get
            {
                return lat;
            }

            set
            {
                lat = value;
            }
        }

        public string Lng
        {
            get
            {
                return lng;
            }

            set
            {
                lng = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }

        public string Passwd
        {
            get
            {
                return passwd;
            }

            set
            {
                passwd = value;
            }
        }
    }

    class CATEGORY_SERVICE_MAPPING {
        int S_ID;
        string S_Category;
        string S_Service;

        public int S_ID_No
        {
            get
            {
                return S_ID;
            }

            set
            {
                S_ID = value;
            }
        }

        public string S_Category_Name
        {
            get
            {
                return S_Category;
            }

            set
            {
                S_Category = value;
            }
        }

        public string S_Service_Name
        {
            get
            {
                return S_Service;
            }

            set
            {
                S_Service = value;
            }
        }
    }

    // This class is entity class for PRO Customer / Service Mapping
    class PRO_CUSTOMER_SERVICE {
        int PCS_Id;
        string PCS_ProName;
        string PCS_Category;
        string PCS_Service;

        public int PCS_Id_No
        {
            get
            {
                return PCS_Id;
            }

            set
            {
                PCS_Id = value;
            }
        }

        public string PCS_Pro_Name
        {
            get
            {
                return PCS_ProName;
            }

            set
            {
                PCS_ProName = value;
            }
        }

        public string PCS_Category_Name
        {
            get
            {
                return PCS_Category;
            }

            set
            {
                PCS_Category = value;
            }
        }

        public string PCS_Service_Name
        {
            get
            {
                return PCS_Service;
            }

            set
            {
                PCS_Service = value;
            }
        }
    }




    // This class is entity class for PRO Customer Appointment Booking
    class BOOKING
    {
        int B_Id;
        string B_Pro_name;
        string B_AvailabilityDate;
        string B_StartTime;
        string B_EndTime;

        public int B_Id_no
        {
            get
            {
                return B_Id;
            }

            set
            {
                B_Id = value;
            }
        }

        public string B_Pro_customer_name
        {
            get
            {
                return B_Pro_name;
            }

            set
            {
                B_Pro_name = value;
            }
        }

        public string B_Availability_Date
        {
            get
            {
                return B_AvailabilityDate;
            }

            set
            {
                B_AvailabilityDate = value;
            }
        }

        public string B_Start_Time
        {
            get
            {
                return B_StartTime;
            }

            set
            {
                B_StartTime = value;
            }
        }

        public string B_End_Time
        {
            get
            {
                return B_EndTime;
            }

            set
            {
                B_EndTime = value;
            }
        }
    }
}
