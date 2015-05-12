using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using GMap.NET.MapProviders;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;


namespace GoogleMaps
{
    public partial class Form1 : Form
    {
        GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(-25.966688, 32.580528),
           GMarkerGoogleType.blue);

        int say = 0;

        GMapOverlay polyOverlay = new GMapOverlay("polygons");
        List<PointLatLng> points = new List<PointLatLng>();
        GMapPolygon polygon;

        string yukseklik = "";
        GMapOverlay markersOverlay = new GMapOverlay("markers"); //yeşil konum

        
        double lat = 0.0; //enlem
        double lng = 0.0;  //boylam


        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            gMapControl1.Zoom = trackBar1.Value;  //trackbarın değeri kadar haritayı zoomladık
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             try
            {
                 //  gMapControl1.CanDragMap = true; //sağ tık için 
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.MarkersEnabled = true; //gözükmesi
            gMapControl1.PolygonsEnabled = true; //polygon çizimleri
            gMapControl1.ShowTileGridLines = false; //koordinatları gösteriyor


            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 16; //n-2 zoom yapar.
            gMapControl1.Zoom = 6;



            ///////////////////////////////////////////////
           
                gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance; //GoogleMap bağlantı kuruyor
                GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly; //erişim modu serveronly


                markersOverlay.Markers.Add(marker); //markersoverlay dizi gibi markerları tutuyor
                gMapControl1.Overlays.Add(markersOverlay);
            }
            catch (Exception ex)
            { 
            
            
            }

            gMapControl1.MouseDoubleClick += new MouseEventHandler(gMapControl1_MouseDoubleClick);  //olayları bağladım
            gMapControl1.MouseClick += new MouseEventHandler(gMapControl1_MouseClick);
        }

        private void gMapControl1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //doubleclick olayında sol tuşa tıklandığında gmapcontroldeki koordinatları alıp enlem boylamını
            //markera atıyor

            if (e.Button == MouseButtons.Left)
            {
                lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

           

            say = say + 1;

            if (say % 3 == 0)
            {

                markersOverlay.Markers.Clear();
                polyOverlay.Polygons.Clear();
                polyOverlay.Polygons.Remove(polygon);
                gMapControl1.PolygonsEnabled = false;
             //burası polyline ların kaldırılması icindi

            }
          

            marker = new GMarkerGoogle(new PointLatLng(lat, lng),
            GMarkerGoogleType.blue);

                

           
            gMapControl1.MarkersEnabled = true;
            markersOverlay.Markers.Add(marker);
            gMapControl1.Overlays.Add(markersOverlay);

            points.Add(new PointLatLng(lat, lng));
         }

        }

        private void gMapControl1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                // latitude=enlem,longitude=boylam
                lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

                JsonDondur c = new JsonDondur();
                HttpRequest g = new HttpRequest();
                string url;
                string webdonen = "";


                c.SetParameters(lat, lng);

                url = c.GetGoogleLink();
                g.SetGoogleLink(url);
                g.RunHttpProtocol();
                webdonen = g.GetJSON_ResultText(); //JSon sonuc metnini çağırdık.



                JObject JSONParser = JObject.Parse(webdonen); //JObject.Parse json dokumanını parçalar


                JArray RootJSON = (JArray)JSONParser["results"]; //burasıda results kısmını alıyor

                if (RootJSON != null)  //eğer boş değilse elevation değerini alır.
                {

                    yukseklik = (string)RootJSON[0]["elevation"].ToString();

                }
                else
                {
                   
                }

                marker.ToolTip = new GMapRoundedToolTip(marker);
                marker.ToolTipText = yukseklik;


                /* Yukarıdaki kodları açıklamak gerekirse , JObject nesnesi ile ayrıştırma işlemi gerçekleştirilecek olan 
            JSON formatındaki metinsel ifadeyi Parse adlı metod ile ayrıştırıp , JObject tipinden  bir değişkene atanıyor.
            Ardından ayrıştırılmış ifade üzerinde bize yarayan ilgili kategorilere ait bilgileri okumak gerekiyor.
            İşte bu noktada Linq sayesinde spesifik olan kategori isimleri köşeli parantezler içine yazıp o kategoriye
            erişmiş oluyoruz. Ardından , eğer bu kategori kapsamı  içinde başka bir kategori  var ise , yine köşeli parantez ile
            o kategori ismini yazıp erişiyoruz. Önemli bir nokta ise ilk defa kullandığımda sıkıntı çekmiştim. 
            JArray yapısı eğer ki JSON metinsel ifadesi içinde [ ] (köşeli parantezler arasında) bir ifade varsa bu bir 
            dizi anlamına gelmektedir.  O sebeple diziye işaret eden anahtar kelimeyi yazıp , ardından JArray yapısı içine
            alıyoruz. Böylece yapıyı diziye aktarmış oluyoruz.*/

            }
            else
            {


                //sol tus tıklandıysa 
                
                gMapControl1.PolygonsEnabled = true;
                polygon = new GMapPolygon(points, "mypolygon");
                polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                polygon.Stroke = new Pen(Color.Red, 1);
                polyOverlay.Polygons.Add(polygon);
                gMapControl1.Overlays.Add(polyOverlay);
                
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
           // gMapControl1.Overlays.Clear();
           // gMapControl1.Overlays.Remove(markersOverlay);
            //gMapControl1.Overlays.Remove(polyOverlay);
            gMapControl1.PolygonsEnabled = false;
            gMapControl1.MarkersEnabled = false;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance;
        }

        public string mesafeOlc(double enlem1, double enlem2, double boylam1, double boylam2)
        {
            double R = 6371; // Radius of the earth in km
            double enlemfark = enlem2 - enlem1;
            double boylamfark = boylam2 - boylam1;

            double dLat = enlemfark * (Math.PI / 180);
            double dLng = boylamfark * (Math.PI / 180);

            double d = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) * Math.Cos(enlem1 * (Math.PI / 180)) + Math.Cos(enlem2 * (Math.PI / 180)) *
    Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(d), Math.Sqrt(1 - d));
            double B = R * c; // Distance in km

            return B.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string mesafe = mesafeOlc(points[0].Lat, points[1].Lat, points[0].Lng, points[1].Lng);
            textBox1.Text = mesafe;

        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            polyOverlay.Polygons.Clear();
            markersOverlay.Markers.Clear();
          //  gMapControl1.Overlays.Remove(polyOverlay);
          //  gMapControl1.Overlays.Remove(markersOverlay);
            gMapControl1.PolygonsEnabled = false;
            gMapControl1.MarkersEnabled = false;
        }


    }
}
