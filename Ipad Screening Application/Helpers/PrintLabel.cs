using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;
using Ipad_Screening_Application;

namespace Ipad_Screening_Application
{
    /// <summary>
    /// Class for printing Labels Passing Object properties in a FlowDocument
    /// </summary>
    public class PrintLabel
    {
        FlowDocument doc;
        PrintDialog printDlg = new PrintDialog();
        

        public PrintLabel()
        {
            doc = new FlowDocument();
            printDlg = new PrintDialog();
            
        }

        public void PrintUsingFlowDocument(Device obj)
        {            

            //We need to use paragraphs to addthe content in the blocks of flow document.  
            DateTime dateTime = DateTime.UtcNow.Date;

            string date = dateTime.ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(obj.Jobnumber))
            {
                // Adding Jobnumber barcode
                BarCode(obj.Jobnumber, "JOBNUMBER");
            }

            if (!string.IsNullOrEmpty(obj.IMEI))
            {
                string imei2 = obj.IMEI.TrimEnd('\0');
                BarCode(imei2, "IMEI");
            }

            

            Paragraph p = new Paragraph();
            Span s = new Span();

            Span d = new Span();
            //d = new Span(new Run("JN: " + obj.Jobnumber + " IMEI: " + obj.IMEI ));
            //d.Inlines.Add(new LineBreak());
            d.Inlines.Add(new Bold(new Run("JN: ")));
            d.Inlines.Add(new Run(obj.Jobnumber + " "));
            d.Inlines.Add(new Bold(new Run("IMEI: ")));
            d.Inlines.Add(new Run(obj.IMEI + " "));
            d.Inlines.Add(new LineBreak());//Line break is used for next line.  
            p.Inlines.Add(d);

            // Location of connection
            //Span location = new Span();    
            //location.Inlines.Add(new Bold(new Run("USBPORT: ")));
            //location.Inlines.Add(new Run(port + " "));
            //location.Inlines.Add(new Run(workStation + " "));
            //location.Inlines.Add(new LineBreak());
            //p.Inlines.Add(location);

            // Jobnumber
            //Span j = new Span();
            //j.Inlines.Add(new Bold(new Run("JOBNUMBER: ")));
            //j.Inlines.Add(new Run(obj.Jobnumber));
            //j.Inlines.Add(new LineBreak());
            //p.Inlines.Add(j);

            // Imei
            //Span i = new Span();
            //i.Inlines.Add(new Bold(new Run("IMEI: ")));
            //i.Inlines.Add(new Run(obj.IMEI));
            //i.Inlines.Add(new LineBreak());
            //p.Inlines.Add(i);

            // Device model, color, capacity
            Span device = new Span();
            //string model = "Port: " + obj.Port + " Box: " + obj.Box + " Pos: " + obj.Position + " " + obj.Serial_Number;

            string port_adjusted = Application_Methods.AdjustPort(obj.Port);

            device.Inlines.Add(new Bold(new Run("PORT: ")));
            device.Inlines.Add(new Run(port_adjusted + " "));
            //device.Inlines.Add(new Bold(new Run("BOX: ")));
            //device.Inlines.Add(new Run(obj.Box + " "));
            //device.Inlines.Add(new Bold(new Run("POSITION: ")));
            //device.Inlines.Add(new Run(obj.Position + " "));
            device.Inlines.Add(new LineBreak());
            p.Inlines.Add(device);

            // Codecli and sku
            Span mdm = new Span();
            string line1 = obj.Model + " " + obj.Capacity + " " + obj.Color;
            mdm.Inlines.Add(new Run(line1));
            mdm.Inlines.Add(new LineBreak());
            p.Inlines.Add(mdm);

            Span line4 = new Span();
            line4.Inlines.Add(new Bold(new Run("SN: ")));
            line4.Inlines.Add(new Run(obj.Serial_Number + " "));
            line4.Inlines.Add(new Bold(new Run("FMIP: ")));
            line4.Inlines.Add(new Run(obj.FMIP + " "));
            line4.Inlines.Add(new Bold(new Run("MDM\\DEP: ")));
            line4.Inlines.Add(new Run(obj.Dep_MDM_Status + " "));
            line4.Inlines.Add(new LineBreak());
            p.Inlines.Add(line4);

            //Give style and formatting to paragraph content.  
            p.FontFamily = new System.Windows.Media.FontFamily("Arial");
            p.FontSize = 12;
            p.FontStyle = FontStyles.Normal;
            p.TextAlignment = TextAlignment.Left;
            doc.Blocks.Add(p);
            //Print Image or barcode in flow document.  
            //let we need to print barcode for 123456.  

            doc.Name = "FlowDoc";
            doc.PageWidth = 400;
            doc.PagePadding = new Thickness(3, 5, 2, 4);
            // Create IDocumentPaginatorSource from FlowDocument
            IDocumentPaginatorSource idpSource = doc;
            // Call PrintDocument method to send document to printer
            printDlg.PrintDocument(idpSource.DocumentPaginator, "Receipt Printing.");
            // Set headers to Label Printed
            Application_Methods.SetHeaders(obj, PortStatusIconPath.SuccessIcon, "Label Printed");


        }

        public void BarCode(string stringForBarCode, string text)
        {
            if (stringForBarCode != String.Empty)
            {
                string path = @"C:\\Login_Integration_Application\\BarCodeImages\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    System.IO.DirectoryInfo di = new DirectoryInfo(path);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                }
                else
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(path);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();

                    }
                    string saveLocation = "C:\\Login_Integration_Application\\BarCodeImages\\" + Convert.ToString(stringForBarCode) + ".png"; //"/" + filename; \ 
                    GenerateImageString(Convert.ToString(Convert.ToString(stringForBarCode))).Save(saveLocation, ImageFormat.Png);
                    AddBarCode(saveLocation, text, stringForBarCode);




                }
            }
        }

        //Convert string to barcode image.  
        private System.Drawing.Image GenerateImageString(string uniqueCode)
        {
            //Read in the parameters  
            string strData = uniqueCode;
            int imageHeight = 50;
            int imageWidth = 275;

            BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;
            type = BarcodeLib.TYPE.CODE128;
            System.Drawing.Image barcodeImage = null;
            try
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                b.IncludeLabel = false;
                b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                barcodeImage = b.Encode(type, strData.Trim(), imageWidth, imageHeight);
                System.IO.MemoryStream MemStream = new System.IO.MemoryStream();
                barcodeImage.Save(MemStream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = MemStream.ToArray();
                return byteArrayToImage(imageBytes);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                barcodeImage.Dispose();
            }
        }

        //Convert byte to image.  
        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        //Add barcode to document.  
        public void AddBarCode(string ImagePath, string text ,string value)
        {
            Paragraph p = new Paragraph();
            p.Margin = new Thickness(2); // setting margin
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            BitmapImage bimg = new BitmapImage();

            using (var stream = File.OpenRead(ImagePath))
            {
                bimg.BeginInit();
                bimg.CacheOption = BitmapCacheOption.OnLoad;
                bimg.StreamSource = stream;
                bimg.EndInit();
            }

            //bimg.BeginInit();
            //bimg.CacheOption = BitmapCacheOption.OnLoad;
            //bimg.UriSource = new Uri(ImagePath, UriKind.Absolute);
            //bimg.EndInit();       
            image.Source = bimg;
            image.Width = 200;
            image.Height = 25;
            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            p.Inlines.Add(image);
            p.Inlines.Add(new LineBreak());
            Span j = new Span();
            j.FontFamily = new System.Windows.Media.FontFamily("Arial");
            j.FontSize = 8;
            j.FontStyle = FontStyles.Normal;
            j.Inlines.Add(new Bold(new Run(text + ": " + value)));
            p.Inlines.Add(j);


            //p.Margin = new Thickness(0);
            //doc.Blocks.Add(new BlockUIContainer(image));
            doc.Blocks.Add(p);

        }

        public string ReplaceInvalidChars(string filename)
        {
            return string.Join("", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
