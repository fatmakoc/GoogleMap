using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

namespace GoogleMaps
{
    //Aşağıda yazmış olduğum sınıf yapısı, karakter katarı işleme ve 
    //WebRequest için gerekli olan link bilgisinin son halinin oluşturulduğu metodları içermektedir.

    class JsonDondur
    {
        //Linke ait değişiklik göstermeyen karakter katarı ifadeleri

        //Prefix adlı değişkendi ifade google linki ve geo sözcüğü ile bunun bir geo sorgusu olduğunu belirtiyoruz.

        string ilkkisim = "https://maps.googleapis.com/maps/api/elevation/json?locations=";

        //sensor ise GPS sinyal verici gibi sinyal yayan bir cihazdan mı okuma yapacağız kısmıdır. Biz false işaretliyoruz.

        //output ise json formatında alıyoruz. İsterseniz XML formatta alıp, XML parser ile ayrıştırabilirsiniz.

        // key ise google account ile aldığımız tekil bir ifadedir.

        string sonkisim = "&sensor=false&key=AIzaSyBlQDZOmFCWw79EuCZ_0syoGm_gU3oPPPE";



        //Enlem Boylam bilgisini tuttuğumuz değişkenler.

        double enlem = 0.0;

        double boylam = 0.0;

        //Konum bilgilerini set ettiğimiz metod. X ve Y bilgisini set ediyoruz.

        public void SetParameters(double X, double Y)
        {

            this.enlem = X;

            this.boylam = Y;

        }


        //WebRequest için hazırladığımız link bilgisini aldığımız metod.

        public string GetGoogleLink()
        {

            string TempStr = string.Empty;

            TempStr = ilkkisim + this.enlem.ToString().Replace(',', '.') + "," + this.boylam.ToString().Replace(',', '.') + sonkisim;
            return TempStr;

        }

    }
}
