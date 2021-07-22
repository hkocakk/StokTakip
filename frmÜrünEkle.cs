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
    public partial class frmÜrünEkle : Form
    {
        public frmÜrünEkle()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-4OHKRU8\\SQLEXPRESS05;Initial Catalog=stokTakipUygulama;Integrated Security=True");
        DataSet daset = new DataSet();

        bool durum;
        private void barkodKontrol()  //daha önce girilen barkod no ile tekrar yeni ürün girilmesinin engellenmesi
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodno.Text == read["barkodno"].ToString() || txtBarkodno.Text == "")
                {
                    durum = false;                      
                }
            }
            baglanti.Close();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void frmÜrünEkle_Load(object sender, EventArgs e)
        {

        }

        private void btnYeniEkle_Click(object sender, EventArgs e)
        {

            barkodKontrol();
            if(durum == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into urun(barkodno,urunadi,miktari,satisfiyati,tarih) values(@barkodno,@urunadi,@miktari,@satisfiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodno.Text);
                komut.Parameters.AddWithValue("@urunadi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@miktari", txtMiktarı.Text);
                komut.Parameters.AddWithValue("@satisfiyati", txtSatışFiyatı.Text);
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());

                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ürün Eklendi");
            }
            else
            {
                MessageBox.Show("Böyle bir Barkod no var");
            }
            
            foreach(Control item in groupBox1.Controls)
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
            }
        }


        // Yeni eklenen ürün bilgilerini var olan ürün bölümünde gösterilmesi
        private void Barkodnotxt_TextChanged(object sender, EventArgs e)
        {
            if (Barkodnotxt.Text == "")
            {
                txtMiktarı.Text = "";
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
                {

                }
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodno like '"+Barkodnotxt.Text+"'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                UrunAditxt.Text = read["urunadi"].ToString();
                Miktarıtxt.Text = read["miktari"].ToString();
                SatışFiyatıtxt.Text = read["satisfiyati"].ToString();
            }
            baglanti.Close();
        }

        private void btnVarOlanaEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set miktari=miktari+ '"+ int.Parse(Miktarıtxt.Text) + "' where barkodno='"+ Barkodnotxt.Text+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();

            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var Olan Ürüne Ekleme Yapıldı");

        }
    }
}
