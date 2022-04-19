using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
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


namespace yazlab1
{
    public partial class Form1 : Form
    {
        private List<PointLatLng> _points;
        public Form1()
        {
            InitializeComponent();
            _points = new List<PointLatLng>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GMapProviders.GoogleMap.ApiKey = @"AIzaSyCuChtVps2ofy6TBaP3TZ8q1lRGTXGw4Vo";
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            map.CacheLocation = @"cache";
            map.ShowCenter = false;// Disable the red cross in map center
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            map.Position = new PointLatLng(38.963745, 35.243322);
            kargolarıgetir();
            haritaCiz();
            map.MinZoom = 5;   // Minimum map zoom
            map.MaxZoom = 100; // Maximum map zoom 
            map.Zoom = 10;     //Current map zoom
        }


        GMapOverlay markers = new GMapOverlay("markers");
        private void kargolarıgetir()
        {
            Baglanti bag = new Baglanti();
            {
                try
                {
                    bag.baglanti.Open();

                    MySqlDataReader oku;
                    MySqlCommand komut = new MySqlCommand("select enlem,boylam FROM musteri WHERE secim=0", bag.baglanti);
                    oku = komut.ExecuteReader();
                    while (oku.Read())
                    {
                        PointLatLng point = new PointLatLng(oku.GetDouble(0), oku.GetDouble(1));
                        _points.Add(point);
                        GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_pushpin);
                        markers.Markers.Add(marker);
                        map.Overlays.Add(markers);
                    }
                    bag.baglanti.Close();
                }
                catch (Exception hata)
                {
                    MessageBox.Show("baglantı olmadı nedeni \n " + hata.ToString());
                    throw;
                }
            }
        }
        GMapOverlay routes = new GMapOverlay("routes");
        private void haritaCiz()
        {
            int i;
            for (i = 0; i < _points.Count - 1; i++)
            {
                var route = GoogleMapProvider.Instance
                    .GetRoute(_points[i], _points[i + 1], false, false, 14);
                var r = new GMapRoute(route.Points, "My Route")
                {
                    Stroke = new Pen(Color.Red, 5)
                };
                
                routes.Routes.Add(r);
                map.Overlays.Add(routes);
            }
        }

        private void buttonbuttonKargoGotur_Click(object sender, EventArgs e)
        {
            if (routes.Routes.Count() > 0)
            {
                routes.Routes.RemoveAt(0);
                markers.Markers.RemoveAt(0);
            }

        }

        private void map_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var point = map.FromLocalToLatLng(e.X, e.Y);
                double x= point.Lat;
                double y = point.Lng;
                textBox3.Text = x+"";
                textBox4.Text = y+"";
                _points.Add(new PointLatLng(x, y));
                LoadMap(new PointLatLng(x, y));
                AddMarker(new PointLatLng(x, y));
                var addresses = GetAddresss(point);
                if (addresses != null)
                    richTextBox1.Text = "Address: \n-----\n" + addresses[0];
                else
                    richTextBox1.Text = "Bilinmeyen Adres";
            }
            
        }


        private void LoadMap(PointLatLng point)
        {
            map.Position = point;
        }
        private void AddMarker(PointLatLng pointToAdd, GMarkerGoogleType markerType = GMarkerGoogleType.red_pushpin)
        {
            var markers = new GMapOverlay("markers");
            var marker = new GMarkerGoogle(pointToAdd, markerType);
            markers.Markers.Add(marker);
            map.Overlays.Add(markers);

        }
        private List<String> GetAddresss(PointLatLng point)
        {
            List<Placemark> placemarks = null;
            var statusCode = GMapProviders.GoogleMap.GetPlacemarks(point, out placemarks);
            if (placemarks != null)
            {
                List<string> addresses = new List<string>();
                foreach (var placemark in placemarks)
                {
                    addresses.Add(placemark.Address);
                }
                return addresses;
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _points.Add(new PointLatLng(Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text)));
            Baglanti bag = new Baglanti();
            try
            {
                bag.baglanti.Open();
                string sorgu = "INSERT INTO musteri(musteriid,musteriadi,secim,enlem,boylam) VALUES(@p1,@p2,@p3,@p4,@p5)";
                MySqlCommand komut = new MySqlCommand(sorgu, bag.baglanti);
                komut.Parameters.AddWithValue("@p1", textBox1.Text);
                komut.Parameters.AddWithValue("@p2", textBox2.Text);
                komut.Parameters.AddWithValue("@p3", '0');
                komut.Parameters.AddWithValue("@p4", (Convert.ToDouble(textBox3.Text)));
                komut.Parameters.AddWithValue("@p5", (Convert.ToDouble(textBox4.Text)));
                komut.ExecuteNonQuery();
                bag.baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("baglantı olmadı nedeni \n " + hata.ToString());
                throw;
            }
            haritaCiz();
        }
    }
}
