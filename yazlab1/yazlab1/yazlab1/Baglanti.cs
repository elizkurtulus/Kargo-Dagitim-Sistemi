using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace yazlab1
{
    class Baglanti
    {
      public static string ConnStr = ConfigurationManager.ConnectionStrings["MySQL"].ToString();

       public MySqlConnection baglanti = new MySqlConnection(ConnStr);

        public void Sql_sorgu(string sorgu)
        {
            baglanti.Open();
            MySqlCommand komut = new MySqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = sorgu;
            komut.ExecuteNonQuery();
            baglanti.Close();
        }

        public bool Sql_ara(string sorgu)
        {
            baglanti.Open();
            MySqlCommand komut = new MySqlCommand();
            komut.Connection = baglanti;
            komut.CommandText = sorgu;
            komut.ExecuteNonQuery();
            MySqlDataReader read = komut.ExecuteReader();//komuttan dönen bilgileri satır satır okur
            if (read.Read())
            {
                baglanti.Close();
                return true;
            }
            else
            {
                baglanti.Close();
                return false;
            }
        }
        
        public DataTable Liste(string sorgu)
        {
            DataTable ekran = new DataTable();
            baglanti.Open();
            MySqlDataAdapter adt = new MySqlDataAdapter(sorgu,baglanti);
            adt.Fill(ekran);
            baglanti.Close();
            return ekran;
        }


    }
}
