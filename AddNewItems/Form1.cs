using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace AddNewItems
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = DBUtils.GetDBConnection();
        public static MySqlConnection
            GetDBConnection(string host, int port, string database, string username, string password)
        {
            // Connection String.
            String connString = "Server=" + host + ";Database=" + database
                                + ";port=" + port + ";User Id=" + username + ";password=" + password;

            MySqlConnection conn = new MySqlConnection(connString);

            return conn;
        }



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

                

                //IEnumerable<string> query1 =
                //    from current in input
                //    let currentFields = current.Split(';')
                //    select "happygifts_" + currentFields[2] + ";" + currentFields[9] + ";" + currentFields[52] + ";" + currentFields[51] + ";"
                //    + currentFields[10] + ";" + currentFields[11] + ";" + currentFields[49].Split(',').First() + ";" + currentFields[13].Split(',').First() + ";"
                //    + currentFields[54] + ";" + currentFields[23] + ";" + currentFields[85] + ";" 
                //    + "Временная" + ";" + pushList(ref imgUrls, currentFields[142]) + insertData("happygifts_" + currentFields[2], currentFields[9], currentFields[51], 
                //        currentFields[52], currentFields[10], currentFields[11], currentFields[49].Split(',').First(), currentFields[13].Split(',').First(),
                //        currentFields[54],currentFields[23], currentFields[85], currentFields[14], pushList(ref imgUrls, currentFields[142]), "RUB", currentFields[57], currentFields[24]) +
                //    currentFields[142] +  ";RUB" + ";" + currentFields[57] + ";" + currentFields[47] + ";" + currentFields[24];
                //resList.AddRange(query1.ToList());
                IEnumerable<string> query1 =
                    from current in input
                    let currentFields = current.Split(';')
                    select insertData("happygifts_" + currentFields[2], currentFields[9], currentFields[51],
                        currentFields[52], currentFields[10], currentFields[11], currentFields[49].Split(',').First(),
                        currentFields[13].Split(',').First(),
                        currentFields[54], currentFields[23], currentFields[85], currentFields[14].Split(',').First(),
                        pushList(ref imgUrls, currentFields[142]), "RUB", currentFields[57], currentFields[24]);
                resList.AddRange(query1.ToList());
                //MessageBox.Show(query1.GetEnumerator().Current);
                IEnumerable<string> query2 =
                   from current in input
                   let currentField = current.Split(';')
                   select currentField[142];
                images.AddRange(query2.ToList());

            }
            splitCommas(images);

            List<List<string>> abc = new List<List<string>>();
            File.WriteAllLines(@"D:\XML_ADD\images.csv", images, Encoding.UTF8);
            //abc = splitList(images, 500);
            
            //Thread[] threads = new Thread[100];
            //for (int i = 0; i < abc.Count; i++)
            //{

            //    threads[i] = new Thread(new ParameterizedThreadStart(downloadThread));
            //    threads[i].Start(abc[i]);


            //}

            
            



            //foreach (string image in images)
            //{
            //    string[] arr = image.Split(',');
            //    foreach (string s in arr)
            //    {
            //        if(s!="")
            //            ImageDownload(s);
            //    }


            //}


            
            
            File.WriteAllLines(@"D:\XML_ADD\files\res.csv", resList, Encoding.UTF8);
            return;
            File.WriteAllLines(@"D:\XML_ADD\images.csv", images, Encoding.UTF8);

        }
        List<string> paths = new List<string>();
        string[] stringSeparators = new string[] { ",h", ", "};
        public void splitCommas(List<string> images)
        {
            
            foreach (string image in images)
            {
                string temp = "";
                string[] arr = image.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in arr)
                {
                    
                    if (s.IndexOf("©") == -1)
                        if (s != "")
                        {
                            temp += s.Substring(s.LastIndexOf('/'));
                        }
                }

                temp = temp.Replace(".jpg,", ".jpg");
                paths.Add(temp.Replace(".jpg",".jpg,"));

            }
            File.WriteAllLines(@"D:\XML_ADD\files\paths.csv", paths, Encoding.UTF8);
        }


        //public static List<List<string>> SplitList(List<string> numbers, int nSize = 5)
        //{
        //    var list = new List<List<string>>();

        //    int splitter = list.Count / 5;



        //    return list;
        //}

        public string insertData(string xml_id1, string name1, string articul1, string description1, string weight1, string volume1, string color1, string material1, string pack_quan1, string price1, string quantity1, string appl_type1, string photo1, string valuta1, string pack_size1, string brand1)
        {
            MySqlConnection connection = DBUtils.GetDBConnection();
            connection.Open();
            try
            {
                // Команда Insert.
                string sql = "Insert into happygifts (ID,XML_ID,NAME,ARTICUL,DESCRIPTION,WEIGHT,VOLUME,COLOR,MATERIAL,PACKAGE_QUANTITY,PRICE,QUANTITY,APPLICATION_TYPE,PHOTO,VALUTA,PACK_SIZE,BRAND)"
                +" values(@id,@xml_id,@name,@articul,@description,@weight,@volume,@color,@material,@package_quantity,@price,@quantity,@application_type,@photo,@valuta,@pack_size,@brand)";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;

                MySqlParameter id = new MySqlParameter("@id", 0);
                id.Value = 0;
                cmd.Parameters.Add(id);

                // Создать объект Parameter.
                MySqlParameter xml_id = new MySqlParameter("@xml_id", xml_id1);
                cmd.Parameters.Add(xml_id);

                MySqlParameter name = new MySqlParameter("@name", name1);
                cmd.Parameters.Add(name);

                MySqlParameter articul = new MySqlParameter("@articul", articul1);
                cmd.Parameters.Add(articul);

                MySqlParameter description = new MySqlParameter("@description", description1);
                cmd.Parameters.Add(description);
                
                MySqlParameter weight = new MySqlParameter("@weight", weight1);
                cmd.Parameters.Add(weight);

                MySqlParameter volume = new MySqlParameter("@volume", volume1);

                cmd.Parameters.Add(volume);

                MySqlParameter color = new MySqlParameter("@color", color1);

                cmd.Parameters.Add(color);

                MySqlParameter material = new MySqlParameter("@material", material1);
                cmd.Parameters.Add(material);

                MySqlParameter package_quantity = new MySqlParameter("@package_quantity", pack_quan1);
                cmd.Parameters.Add(package_quantity);

                MySqlParameter price = new MySqlParameter("@price", price1);
                cmd.Parameters.Add(price);

                MySqlParameter quantity = new MySqlParameter("@quantity", quantity1);
                cmd.Parameters.Add(quantity);

                MySqlParameter application_type = new MySqlParameter("@application_type", appl_type1);
                cmd.Parameters.Add(application_type);

                MySqlParameter photo = new MySqlParameter("@photo", photo1);
                cmd.Parameters.Add(photo);

                MySqlParameter valuta = new MySqlParameter("@valuta", valuta1);
                cmd.Parameters.Add(valuta);

                MySqlParameter pack_size = new MySqlParameter("@pack_size", pack_size1);
                cmd.Parameters.Add(pack_size);

                MySqlParameter brand = new MySqlParameter("@brand", brand1);
                cmd.Parameters.Add(brand);
                Application.DoEvents();

                cmd.ExecuteNonQuery();

                //richTextBox1.AppendText("Row Count affected = "+ rowCount +"\r\n");
            }
            catch (Exception e)
            {
                //richTextBox1.AppendText("Error: " + e + "\r\n");
                //richTextBox1.AppendText(e.StackTrace + "\r\n");
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }


            Console.Read();
            return "";
        }


        public string GetData()
        {
            
            return "";
        }

        public static List<List<string>> splitList(List<string> locations, int nSize)
        {
            var list = new List<List<string>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }

        //public static void downloadThread(object x)
        //{
        //    List<string> images =  x as List<string>;
        //    foreach (string image in images)
        //    {
        //        string[] arr = image.Split(',');
        //        foreach (string s in arr)
        //        {

        //            if (s.IndexOf("©") == -1)
        //                if (s != "")
        //                {
        //                    ImageDownload(s);
        //                }
        //        }
        //    }
        //}

        public string pushList(ref List<string> images,string imageLink)
        {
            images.Add(imageLink);
            return "";
        }

        

        //загрузка изображения
        public static string ImageDownload(string ImageLink)
        {
            //List<string> paths = new List<string>();
            ////if (File.Exists(@"D:\XML_ADD\uploads\" + ImageLink.Substring(ImageLink.LastIndexOf('/'))))
            ////    return "";
            //using (WebClient webClient = new WebClient())
            //{
            //    try
            //    {
            //        byte[] data = webClient.DownloadData(ImageLink);

            //        using (MemoryStream mem = new MemoryStream(data))
            //        {
            //            using (var yourImage = Image.FromStream(mem))
            //            {
            //                Task.Delay(100);
            //                //yourImage.Save(@"D:\XML_ADD\uploads\" + ImageLink.Substring(ImageLink.LastIndexOf('/')),
            //                 //   ImageFormat.Png);
            //                //paths.Add(ImageLink.Substring(ImageLink.LastIndexOf('/')));
                                
            //            }
            //        }
            //    }
            //    catch (Exception e)
            //    {
                        
            //    }
                    

            //}
            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.AppendText("Openning Connection ...\r\n");

                conn.Open();

                richTextBox1.AppendText("Connection successful!\r\n");
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText("Error: " + ex.Message + "\r\n");
            }

            //Pass a query variable to a method and execute it  
            // in the method. The query itself is unchanged.
            //OutputQueryResults2(scoreQuery1, "Merge two spreadsheets:");
            //insertData();
        }
    }
}
