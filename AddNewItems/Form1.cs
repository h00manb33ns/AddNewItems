using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AddNewItems
{
    public partial class Form1 : Form
    {
        string hGifts_path_stock = @"D:\XML_ADD\1409_export.xml";
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Parse_file_Click(object sender, EventArgs e)
        {
            XMLtoCSV_hgifts();
            //ImageDownload();
        }

        public void XMLtoCSV_hgifts()
        {
            //List<string> result = new List<string>();
            //List<string> URLs = new List<string>{"1364_export.csv",
            //"1042_export.csv", "621_export.csv", "1179_export.csv", "1043_export.csv", "1011_export.csv", "627_export.csv",
            //"701_export.csv", "708_export.csv", "712_export.csv", "1180_export.csv", "638_export.csv",
            //"662_export.csv", "668_export.csv", "678_export.csv", "733_export.csv",
            //"979_export.csv", "1044_export.csv", "1045_export.csv", "1049_export.csv",
            //"1064_export.csv", "1072_export.csv"};

            //Downloader downloader = new Downloader();

            //foreach (string urL in URLs)
            //{
            //    result.Add(downloader.DownloadXmlHappyGifts("https://happygifts.ru/XML/" + urL));
            //}

            //for (int i = 0; i < URLs.Count; i++)
            //{
            //    File.WriteAllText(@"D:\XML_ADD\" + URLs[i].Replace("xlm", "xml"), result[i]);
            //}

            //MessageBox.Show("Загрузка завершена!");

            List<string> files = new List<string>{"1364_export.csv",
                "1042_export.csv", "621_export.csv", "1179_export.csv", "1043_export.csv", "1011_export.csv", "627_export.csv",
                "701_export.csv", "708_export.csv", "712_export.csv", "1180_export.csv", "638_export.csv",
                "662_export.csv", "668_export.csv", "678_export.csv", "733_export.csv",
                "979_export.csv", "1044_export.csv", "1045_export.csv", "1049_export.csv",
                "1064_export.csv", "1072_export.csv"};
            List<string> resList = new List<string>();
            List<string> images = new List<string>();
            List<string> imgUrls = new List<string>();
            foreach (string currentFile in files)
            {
                List<string> input = new List<string>(System.IO.File.ReadAllLines(@"D:\XML_ADD\" + currentFile));
                if (input.Count < 1)
                {
                    continue;
                }
                input.RemoveAt(0);

                

                IEnumerable<string> query1 =
                    from current in input
                    let currentFields = current.Split(';')
                    select "happygifts_" + currentFields[2] + ";" + currentFields[9] + ";" + currentFields[51] + ";" + currentFields[52] + ";"
                    + currentFields[10] + ";" + currentFields[11] + ";" + currentFields[49].Split(',').First() + ";" + currentFields[13].Split(',').First() + ";"
                    + currentFields[54] + ";" + currentFields[23] + ";" + currentFields[85] + ";" + currentFields[3] + ";"
                    + currentFields[4] + ";" + currentFields[134].Split(',').First() + ";" + pushList(ref imgUrls, currentFields[142]) + currentFields[142] +  ";RUB" + ";" + currentFields[57] + ";" + currentFields[47] + ";" + currentFields[24];
                resList.AddRange(query1.ToList());
                // MessageBox.Show(query1.GetEnumerator().Current);
                IEnumerable<string> query2 =
                   from current in input
                   let currentField = current.Split(';')
                   select currentField[142];
                images.AddRange(query2.ToList());

            }

            


            IEnumerable<string[]> query3 =
                   from current in imgUrls
                   let currentFields = current.Split(',')
                   select currentFields;
            foreach(string[] a in query3)
            {
                foreach(string b in a)
                    MessageBox.Show(b);
            }
            return;
            File.WriteAllLines(@"D:\XML_ADD\res.csv", resList, Encoding.Default);
            File.WriteAllLines(@"D:\XML_ADD\images.csv", images, Encoding.Default);

        }

        public string pushList(ref List<string> images,string imageLink)
        {
            images.Add(imageLink);
            return "";
        }

        public void SaveImage(string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://happygifts.ru/catalog-images/4564fac3-e9f6-11e2-960d-18a9053c0de9/photo/4564fac3-e9f6-11e2-960d-18a9053c0de9.jpg");
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }


        //загрузка изображения
        public string ImageDownload(string ImageLink)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(ImageLink);

                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    { 
                        yourImage.Save(@"D:\XML_ADD\"+ ImageLink.Substring(ImageLink.LastIndexOf('/')), ImageFormat.Png);
                    }
                }

            }
            return "";
        }
    }
}
