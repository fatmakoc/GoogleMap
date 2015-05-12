using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace GoogleMaps
{
    /*HTTP protokolü istek ve cevap mantığında çalışır. 
   C# programlama dilinde bu protokolü kullanabileceğimiz çeşitli sınıflar verilmiştir. 
   Bunlar iki tanesi  HttpWebRequest ve HttpWebResponse  sınıflarıdır.
   Bu yapılar sayesinde HTTP protokolünden istek ve cevap mekanizması ile sonuçlar alınabilir. 
   Aşağıda açıklamaları ile birlikte yazılmış olan kodlar mevcuttur.*/

    class HttpRequest
    {

        //Webrequest oluşturulan C# sınıfıdır.

        HttpWebRequest GoogleRequest;

        //Yapılan request sonucunda bize dönen cevabı alınan WebResponse veri tipi ve ona ait olan nesne ismi.

        HttpWebResponse GoogleResponse;

        //Googlelink bilgisini tutulduğu değişken.Bu link ifadesi ile birlikte WebRequest oluşturulur.

        string GoogleLink = string.Empty;

        //JSON formatında dönen sonucun tutulduğu değişkendir.

        string JSON_ResultText = string.Empty;

        //HTTP protokolunü çalıştırdığımız metoddur.

        public void RunHttpProtocol()
        {

            //WebRequest'i create metoduyla oluşturulur.

            GoogleRequest = (HttpWebRequest)WebRequest.Create(this.GoogleLink);

            //Artık istek yaptıktan sonra dönen cevabı yakalamak gereklidir.

            //Bunu GetResponse metodu kullanarak yapıyoruz.
            try
            {
                GoogleResponse = (HttpWebResponse)GoogleRequest.GetResponse();

                //Gelen response cevabı okunabilir karakter katarlarına çevirmek için gelen cevabı Stream şeklinde alıyoruz.

                Stream GoogleResultStream = GoogleResponse.GetResponseStream();



                //StreamReader ile birlikte Stream nesnesi ReadToEnd metoduyla sonuna kadar okunur.

                StreamReader GoogleResultReader = new StreamReader(GoogleResultStream);



                //Stream karakter katarına çevrilir.

                JSON_ResultText = GoogleResultReader.ReadToEnd();

            }
            catch (Exception ex)
            {

            }



        }

        //Google Linkin set edildiği metod.

        public void SetGoogleLink(string LinkGoog)
        {

            GoogleLink = LinkGoog;

        }


        //JSON Sonuç metninin  çağrıldığı yere döndürüldüğü metod.

        public string GetJSON_ResultText()
        {

            return this.JSON_ResultText;

        }



    }
}
