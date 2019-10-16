using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using net_emisioncarnet.DAO;
using System.IO;

namespace net_emisioncarnet
{
    public partial class Form3 : Form
    {
        public Form3(string cod, string nom, string pat, string mat, Image fot)
        {
            InitializeComponent();
            lblcod.Text = cod;
            lblnom.Text = nom;
            lblpat.Text = pat;
            lblmat.Text = mat;
            picfoto.Image = fot;

            string a = cod.Remove(0, 1);
            string b = a.Remove(4, 5);
            lblano.Text = b;
        }


        private void imprimir_click(object sender, EventArgs e)
        {
            imprimir();
        }

        private void imprimir()
        {
            if (MessageBox.Show("¿Desea imprimir el carnet?", "Imprimir Carnet", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                Graphics gfx = this.CreateGraphics();

                Bitmap bmp = new Bitmap(this.Width, this.Height, gfx);
                    
               this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
                    
                /*continuar aqui*/


                // Displays a SaveFileDialog so the user can save the Image
                // assigned to Button2.
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = ""+lblcod.Text+"";
                saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
                saveFileDialog1.Title = "Guardar carnet de alumno";
                saveFileDialog1.ShowDialog();

                // If the file name is not an empty string open it for saving.
                if (saveFileDialog1.FileName != "")
                {
                    // Saves the Image via a FileStream created by the OpenFile method.
                    System.IO.FileStream fs =
                        (System.IO.FileStream)saveFileDialog1.OpenFile();
                    // Saves the Image in the appropriate ImageFormat based upon the
                    // File type selected in the dialog box.
                    // NOTE that the FilterIndex property is one-based.
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            bmp.Save(fs,
                              System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;

                        case 2:
                            bmp.Save(fs,
                              System.Drawing.Imaging.ImageFormat.Bmp);
                            break;

                        case 3:
                            bmp.Save(fs,
                              System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                    }
                    this.Dispose();
                }

            }
            else
            {
                this.Dispose();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
        }
    }
}
