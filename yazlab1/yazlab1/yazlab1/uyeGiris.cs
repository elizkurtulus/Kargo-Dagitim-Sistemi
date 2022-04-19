using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yazlab1
{
    public partial class uyeGiris : Form
    {
        public uyeGiris()
        {
            InitializeComponent();
        }
        Baglanti connect = new Baglanti();
        
        private void uyeGiris_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = connect.Liste("select * from musteri");
        }

        private void button1_Click(object sender, EventArgs e)//ekle butonu
        {
            string sorgu = "insert into musteri(musteriid,musteriadi,secim,enlem,boylam) values("+textBox4.Text+",'"+textBox1.Text+"',0,"+textBox2.Text+","+textBox3.Text+")";
            connect.Sql_sorgu(sorgu);
            dataGridView1.DataSource = connect.Liste("select * from musteri");
            MessageBox.Show("İŞLEM BAŞARILI");  
        }

        private void button2_Click(object sender, EventArgs e)//haritaya git butonu
        {
            Form1 map = new Form1();
            map.Show();

        }

        private void button3_Click(object sender, EventArgs e)//sil butonu
        {
            string sorgu = "delete from musteri where musteriid=" +dataGridView1.CurrentRow.Cells["musteriid"].Value.ToString();
            connect.Sql_sorgu(sorgu);
            dataGridView1.DataSource = connect.Liste("select * from musteri");
            MessageBox.Show("İŞLEM BAŞARILI");
        }

        private void button4_Click(object sender, EventArgs e)//güncelle butonu
        {
            string sorgu = "UPDATE musteri SET musteriadi='" + textBox1.Text + "',secim=0,enlem=" + textBox2.Text +",boylam="+textBox3.Text+" WHERE musteriid='" + textBox4.Text+"'";
            connect.Sql_sorgu(sorgu);
            dataGridView1.DataSource = connect.Liste("select * from musteri");
            MessageBox.Show("İŞLEM BAŞARILI");
        }
    }
}
