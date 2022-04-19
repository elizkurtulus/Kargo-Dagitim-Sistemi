using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace yazlab1
{
    public partial class Connection : Form
    {
        public Connection()
        {
            InitializeComponent();
        }
        Baglanti connect = new Baglanti();
        private void Connection_Load(object sender, EventArgs e)
        {

        }
       
        private void button1_Click(object sender, EventArgs e)//kayıt olma
        {
            string sorgu = "insert into admin(kullanici_adi,sifre) values('"+ textBox2.Text + "','" + textBox1.Text + "')";
            connect.Sql_sorgu(sorgu);
            MessageBox.Show("İŞLEM BAŞARILI");
        }
        private void button2_Click(object sender, EventArgs e)//giris yapma
        {
            string sorgu = "select kullanici_adi,sifre from admin where kullanici_adi='" + textBox2.Text + "' and sifre='" + textBox1.Text + "'";
            bool giris;
            giris= connect.Sql_ara(sorgu);
            if (giris == true)
            {
                MessageBox.Show("GİRİŞ BAŞARILI");
                uyeGiris ekran = new uyeGiris();
                this.Hide();
                ekran.Show();
            }
            else
            {
                MessageBox.Show("KULLANICI ADI VEYA SİFRE YANLIŞ");
            }

        }

        private void button3_Click(object sender, EventArgs e)//sifre yenileme
        {
            Form2 form = new Form2();
            form.Show();
        }
    }
}