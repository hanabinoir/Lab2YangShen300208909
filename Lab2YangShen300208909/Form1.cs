using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Lab2YangShen300208909
{
    public partial class Form1 : Form
    {
        MySqlConnection connection;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlTextReader reader = new XmlTextReader("data.xml");
            string connectionString = "";

            string[] names = { "SERVER", "database", "uid", "password" };
            string[] values = new string[3];
            for (int i = 0; i < names.Length; i++)
            {
                reader.ReadToFollowing(names[i]);
                reader.Read();
                connectionString += names[i] + "=" + reader.Value + ";";
            }

            reader.Close();
            connection = new MySqlConnection(connectionString);

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.FullRowSelect = true;
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Name");
            listView1.Columns.Add("Phone");


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null)
                connection.Close();

        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (connection != null)
            {
                connection.Open();
                string query = "Select * from Shippers ";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter mcmd = new MySqlDataAdapter();
                MySqlDataReader reader;

                mcmd.SelectCommand = cmd;
                /* mcmd contains the data
                   mcmd.Fill "transfer" data to DataSet ds */
                DataSet ds = new DataSet();
                mcmd.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                reader = cmd.ExecuteReader();
                string[] data = new string[3];
                listView1.Clear();
                listView1.Columns.Add("ID");
                listView1.Columns.Add("Name");
                listView1.Columns.Add("Phone");
                ListViewItem item;
                while (reader.Read())
                {
                    data[0] = reader.GetString(0);
                    data[1] = reader.GetString(1);
                    data[2] = reader.GetString(2);
                    item = new ListViewItem(data);
                    listView1.Items.Add(item);
                }
                reader.Close();

                connection.Close();
            }

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string instruction = "INSERT INTO Shippers(shippername,phone) ";
            instruction += " values (@a,@b) ";

            //create command and assign the query and connection from the constructor
            if (connection != null)
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(instruction, connection);

                cmd.Parameters.AddWithValue("@a", nameTxt.Text);
                cmd.Parameters.AddWithValue("@b", phoneTxt.Text);
                cmd.ExecuteNonQuery();
                connection.Close();
                btnDisplay_Click(sender, e);
            }

        }

        private void btnUD_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (selectedRowCount > 0)
            {
                // using DataGridView
                string instruction = "Update Shippers set shippername = @a, phone = @b where id = @c ";

                connection.Open();
                MySqlCommand cmd = new MySqlCommand(instruction, connection);

                cmd.Parameters.AddWithValue("@a", nameTxt.Text);
                cmd.Parameters.AddWithValue("@b", phoneTxt.Text);
                cmd.Parameters.Add("@c", MySqlDbType.String);
                for (int y = 0; y < selectedRowCount; y++)
                {
                    // cell[0] is the id
                    cmd.Parameters["@c"].Value = dataGridView1.SelectedRows[y].Cells[0].Value.ToString();
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
                btnDisplay_Click(sender, e);
            }            

        }

        private void btnUL_Click(object sender, EventArgs e)
        {
            //using ListView
            ListView.SelectedIndexCollection indexes = listView1.SelectedIndices;
            if (indexes.Count > 0)
            {
                string instruction = "Update Shippers set shippername = @a, phone = @b where id = @c ";

                connection.Open();
                MySqlCommand cmd = new MySqlCommand(instruction, connection);

                cmd.Parameters.AddWithValue("@a", nameTxt.Text);
                cmd.Parameters.AddWithValue("@b", phoneTxt.Text);
                cmd.Parameters.Add("@c", MySqlDbType.String);
                foreach (int y in indexes)
                {
                    // cell[0] is the id
                    cmd.Parameters["@c"].Value = listView1.Items[y].SubItems[0].Text;
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
                btnDisplay_Click(sender, e);
            }         

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);

            if (selectedRowCount > 0)
            {
                // using DataGridView
                string instruction = "Delete from Shippers where id = @c ";

                connection.Open();
                MySqlCommand cmd = new MySqlCommand(instruction, connection);

                cmd.Parameters.Add("@c", MySqlDbType.String);
                for (int y = 0; y < selectedRowCount; y++)
                {
                    // cell[0] is the id
                    cmd.Parameters["@c"].Value = dataGridView1.SelectedRows[y].Cells[0].Value.ToString();
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
                btnDisplay_Click(sender, e);
            }          

        }
    }
}
