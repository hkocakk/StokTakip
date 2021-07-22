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

namespace stokTakipUygulama
{
    public partial class frmÜrünListele : Form
    {
        public frmÜrünListele()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-4OHKRU8\\SQLEXPRESS05;Initial Catalog=stokTakipUygulama;Integrated Security=True");
        DataSet daset = new DataSet();

        private void frmÜrünListele_Load(object sender, EventArgs e)
        {
            ÜrünListele();
        }

        private void ÜrünListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun", baglanti);
            adtr.Fill(daset, "urun");
            dataGridView1.DataSource = daset.Tables["urun"];
            baglanti.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Barkodnotxt.Text = dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString();
            UrunAditxt.Text = dataGridView1.CurrentRow.Cells["urunadi"].Value.ToString();
            Miktarıtxt.Text = dataGridView1.CurrentRow.Cells["miktari"].Value.ToString();
            SatışFiyatıtxt.Text = dataGridView1.CurrentRow.Cells["satisfiyati"].Value.ToString();
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set urunadi = @urunadi, miktari=@miktari, satisfiyati=@satisfiyati where barkodno=@barkodno", baglanti);
            komut.Parameters.AddWithValue("@barkodno", Barkodnotxt.Text);
            komut.Parameters.AddWithValue("@urunadi", UrunAditxt.Text);
            komut.Parameters.AddWithValue("@miktari", int.Parse(Miktarıtxt.Text));
            komut.Parameters.AddWithValue("@satisfiyati", SatışFiyatıtxt.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["urun"].Clear();
            ÜrünListele();
            MessageBox.Show("Güncelleme yapıldı");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from urun where barkodno='" +dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString()+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["urun"].Clear();
            ÜrünListele();
            MessageBox.Show("Kayıt silindi");
        }

        private void txtBarkodNoAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun where barkodno like '%" + txtBarkodNoAra.Text + "%'", baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
