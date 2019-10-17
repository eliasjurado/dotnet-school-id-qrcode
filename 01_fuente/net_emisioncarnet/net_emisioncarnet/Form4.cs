using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Configuration;

namespace net_emisioncarnet
{
    public partial class Form4 : Form
    {
        public Form4(Image img,string cod)
        {
            InitializeComponent();
            picqr.Image = img;
            lblcodigo.Text = cod;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            string codigoalumno = lblcodigo.Text;

            //Convierto el formulario en imagen
            Graphics gfx = this.CreateGraphics();
            Bitmap bmp = new Bitmap(this.Width, this.Height, gfx);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));

            //Abro el directorio que recibira el archivo
            var directorio = ConfigurationManager.AppSettings.Get("DirectorioCarnet");
            var directoryinfo = new DirectoryInfo(directorio);
            if (!directoryinfo.Exists)
            {
                Directory.CreateDirectory(directorio);
            }

            //Grabo la imagen
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(directorio + codigoalumno + "b.jpg", FileMode.Create, FileAccess.ReadWrite))
                {
                    bmp.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            this.Close();
        }
    }
}
