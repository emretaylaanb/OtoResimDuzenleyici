using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MFdeneme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Görsel boyutlandırma ve beyaz alan ekleme işlemi
        private Bitmap ResizeImageWithPadding(Image img, int width, int height)
        {
            // Beyaz bir arka plan oluştur
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.White);

                // Görselin orijinal boyutlarını al
                int originalWidth = img.Width;
                int originalHeight = img.Height;

                // Görsel istenilen boyutlardan büyükse, oranı koruyarak yeniden boyutlandır
                float ratio = Math.Min((float)width / originalWidth, (float)height / originalHeight);
                int newWidth = (int)(originalWidth * ratio);
                int newHeight = (int)(originalHeight * ratio);

                // Görseli ortalamak için konumu hesapla
                int xPos = (width - newWidth) / 2;
                int yPos = (height - newHeight) / 2;

                // Görseli beyaz arka plan üzerine çiz
                g.DrawImage(img, xPos, yPos, newWidth, newHeight);
            }
            return result;
        }

        // Dosyadaki tüm görselleri işle
        private void ProcessImages(string folderPath, int targetWidth, int targetHeight)
        {
            // Dosya yolundaki tüm resim dosyalarını al
            string[] files = Directory.GetFiles(folderPath, "*.jpg"); // İsteğe göre dosya formatını değiştirebilirsiniz.

            int durum=0;
            foreach (var file in files)
            {
                Image img = Image.FromFile(file);
                Bitmap resizedImage = ResizeImageWithPadding(img, targetWidth, targetHeight);

                // Sonuç dosyasını kaydet
                string savePath = Path.Combine(folderPath, Path.GetFileName(file));
                resizedImage.Save(savePath);
                img.Dispose();
                resizedImage.Dispose();
                durum++;
            }
            MessageBox.Show("Tüm resimler başarıyla işlenmiştir.");
            linkLabel1.Text = durum + " Fotoğraf İşlendi";
        }

        private void btnProcessImages_Click(object sender, EventArgs e)
        {

            int gen, yuk;
            if(!int.TryParse(txt_genislik.Text, out gen))
            {
                MessageBox.Show("Genişlik parametresi hatalıdır. Kontrol ediniz.");
                return;
            }

            if (!int.TryParse(txt_yukseklik.Text, out yuk))
            {
                MessageBox.Show("Yukseklik parametresi hatalıdır. Kontrol ediniz.");
                return;
            }

            // Klasör seçme dialogu
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = fbd.SelectedPath;
                    int targetWidth = gen;  // Hedef genişlik
                    int targetHeight = yuk; // Hedef yükseklik

                    // Görselleri işleme fonksiyonu çağrılır
                    ProcessImages(folderPath, targetWidth, targetHeight);
                }
            }
        }
    }
}
