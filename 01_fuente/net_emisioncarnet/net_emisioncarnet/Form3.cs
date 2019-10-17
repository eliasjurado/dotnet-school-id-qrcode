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
using System.Configuration;

namespace net_emisioncarnet
{
    public partial class Form3 : Form
    {
        //los datos que recibe este formulario son los que vienen de la primera vista
        public Form3(string cod, string nom, string pat, string mat, Image fot)
        {
            InitializeComponent();
            lblcod.Text = cod;
            lblnom.Text = nom;
            lblpat.Text = pat;
            lblmat.Text = mat;
            picfoto.Image = fot;

            string i = cod.Remove(0, 1);
            string ano = i.Remove(4, 5);
            lblano.Text = ano;
        }

        private async void Form3_Load(object sender, EventArgs e)
        {
            string cod = lblcod.Text;
            //espero un segundo y...
            await Task.Delay(1000);
            //lanzo el mensaje de confirmación
            if (MessageBox.Show("¿Desea imprimir el carnet?", "Imprimir Carnet", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
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
                    using (FileStream fs = new FileStream(directorio+cod+".jpg", FileMode.Create, FileAccess.ReadWrite))
                    {
                        bmp.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                //Abro la carpeta con la imagen
                Process.Start(directorio);
            }
            else
            {
                this.Dispose();
            }
        }
    }
}
