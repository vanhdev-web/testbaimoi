using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace storeProcedure
{
    public partial class Form1 : Form
    {
        string strcon = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\dovie\\OneDrive\\Documents\\testbaimoi\\storeProcedure\\storeProcedure\\QLBH.mdf;Integrated Security=True";

        SqlConnection sqlcon = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (sqlcon == null)
            {
                sqlcon = new SqlConnection(strcon);
            }

            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType  = CommandType.Text;
            sqlCommand.CommandText = "select * from PNHAP";

            sqlCommand.Connection = sqlcon;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while(reader.Read())
            {
                string soPN = reader.GetString(0);
                DateTime ngay = reader.GetDateTime(1);
                string soDH = reader.GetString(2);  

                ListViewItem item = new ListViewItem(soPN);
                item.SubItems.Add(ngay.ToString("dd/mm/yyyy"));
                item.SubItems.Add(soDH);

                listViewDSPN.Items.Add(item);
            }
            reader.Close();
        }

        private void listViewDSPN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewDSPN.SelectedItems.Count > 0)
            {
                string soPN = listViewDSPN.SelectedItems[0].SubItems[0].Text;
                hienThiChiTietPhieuNhap(soPN);
            }
        }

        private void hienThiChiTietPhieuNhap(string soPN)
        {
            if (sqlcon == null)
            {
                sqlcon = new SqlConnection(strcon);
            }

            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
            }

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "hienThiPhieuNhap";

            SqlParameter para = new SqlParameter("@MaPN", SqlDbType.Char);
            para.Value = soPN;
            sqlCommand.Parameters.Add(para);

            sqlCommand.Connection = sqlcon;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            listViewTT.Items.Clear();   
            while (reader.Read())
            {
                string maVtu = reader.GetString(1);
                string sl = reader.GetInt32(2).ToString();
                string dg = reader.GetDecimal(3).ToString();
                ListViewItem  item = new ListViewItem(soPN);
                item.SubItems.Add(maVtu);
                item.SubItems.Add(sl);
                item.SubItems.Add(dg);

                listViewTT.Items.Add(item);
                
            }
            reader.Close();
        }
    }
}
