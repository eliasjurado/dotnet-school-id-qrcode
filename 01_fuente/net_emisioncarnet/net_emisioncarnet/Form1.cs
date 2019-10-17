using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using net_emisioncarnet.DAO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace net_emisioncarnet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public OpenFileDialog examinar = new OpenFileDialog();
        private AlumnoDAO alumno = new AlumnoDAO();
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void btnexaminar_Click(object sender, EventArgs e)
        {
            examinar.Filter = "image files|*.jpg;*.png;*.gif;*.ico;.*;";
            DialogResult dres1 = examinar.ShowDialog();
            if (dres1 == DialogResult.Abort)
                return;
            if (dres1 == DialogResult.Cancel)
                return;
            txtexaminar.Text = examinar.FileName;
            //--------------------------------------------------
            //Reducimos la imagen de tamaño pasaporte a 100 x 128

            var srcImage = Image.FromFile(txtexaminar.Text);
            var newWidth = 100;
            var newHeight = 128;
            var newImage = new Bitmap(newWidth, newHeight);
            var graphics = Graphics.FromImage(newImage);

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
            //recortamos la nueva imagen a 100x100

            Rectangle cropRect = new Rectangle(0, 0, 100, 100);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(newImage, new Rectangle(0, 0, target.Width, target.Height), cropRect, GraphicsUnit.Pixel);
            }

            picfotografia.Image = target;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
               
            if(txtdni.Text=="")
            {
                MessageBox.Show("Ingrese DNI");
            }
           else if (!Regex.IsMatch(txtdni.Text, @"^\d{8}"))
            {
                MessageBox.Show("DNI debe tener 8 caracteres");
            }
            else if (txtnom.Text == "")
            {
                MessageBox.Show("Ingrese nombres");
            }
            else if (txtpat.Text == "")
            {
                MessageBox.Show("Ingrese apellido paterno");
            }
            else if (txtmat.Text == "")
            {
                MessageBox.Show("Ingrese apellido materno");
            }
            else if (txtexaminar.Text == "")
            {
                MessageBox.Show("La ruta de la foto está en blanco. Por favor seleccione una imagen");
            }
            else
            {
                //pasamos a byte la imagen acomodada
                byte[] binData = ImageToByte(picfotografia.Image);

                //Doy formato a los nombres y apellidos
                txtnom.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase((txtnom.Text).ToLower());
                txtpat.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtpat.Text.ToLower());
                txtmat.Text = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtmat.Text.ToLower());

                if (txtcod.Text == "")
                {
                    if (MessageBox.Show("¿La información del alumno es correcta?", "Registrar Alumno", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //Ingreso Alumno
                        string mensaje = alumno.RegistrarAlumno(txtdni.Text.Trim(), txtnom.Text.Trim(), txtpat.Text.Trim(), txtmat.Text.Trim(), binData);
                        MessageBox.Show(mensaje);
                        Limpiar();
                    }
                }
                else
                {
                    if (MessageBox.Show("¿La información del alumno es correcta?", "Actualizar Alumno", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //Ingreso Alumno
                        string mensaje = alumno.ActualizarAlumno(txtcod.Text, txtdni.Text.Trim(), txtnom.Text.Trim(), txtpat.Text.Trim(), txtmat.Text.Trim(), binData);
                        MessageBox.Show(mensaje);
                        Limpiar();
                    }
                }
            }
        }

        private void Limpiar()
        {
            txtcod.Text = "";
            txtnom.Text = "";
            txtdni.Text = "";
            txtpat.Text = "";
            txtmat.Text = "";
            txtexaminar.Text = "";
            picfotografia.Image = null;
            txtdni.Focus();
        }



        private void RecuperarDatos(
           string id,
            string dni,
            string nombre,
            string apepat,
            string apetmat,
            DateTime fecreg,
            Image foto
           )
        {
            txtcod.Text = id;
            txtdni.Text = dni;
            txtnom.Text = nombre;
            txtpat.Text = apepat;
            txtmat.Text = apetmat;
            picfotografia.Image = foto;
            txtexaminar.Text = "";

            //Abro el directorio que recibira el archivo
            var directoriof = ConfigurationManager.AppSettings.Get("DirectorioFoto");
            var directoryfinfo = new DirectoryInfo(directoriof);
            if (!directoryfinfo.Exists)
            {
                Directory.CreateDirectory(directoriof);
            }
            //generamos un archivo con la foto del alumno registrado
            Bitmap archivo = new Bitmap(picfotografia.Image);
            archivo.Save(directoriof+id+".jpg");
        }
    

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form2 f1 = new Form2();
            f1.PasarDatos += new Form2.Almacenar_Datos(RecuperarDatos);
            f1.ShowDialog();
        }

        private void txtnom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) { e.Handled = false; }
            else if (Char.IsControl(e.KeyChar)) { e.Handled = false; }
            else if (Char.IsSeparator(e.KeyChar)) { e.Handled = false; }
            else
            {
                e.Handled = true;
                MessageBox.Show("Sólo letras");
            }
        }

        private void txtdni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar)) { e.Handled = false; }
            else if (Char.IsControl(e.KeyChar)) { e.Handled = false; }
            else if (Char.IsSeparator(e.KeyChar)) { e.Handled = false; }
            else
            {
                e.Handled = true;
                MessageBox.Show("Sólo números");
            }
        }

        private void txtnom_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            if(txtcod.Text != "")
            {
                string cod = txtcod.Text;
                string dni = txtdni.Text;
                string nom = txtnom.Text;
                string pat = txtpat.Text;
                string mat = txtmat.Text;
                Image fot = picfotografia.Image;

                Form3 f = new Form3(cod, nom, pat, mat, fot);
                f.ShowDialog();
            }
           else
            {
                MessageBox.Show("Seleccione un alumno de la lista haciendo click en la lupa");
            }
        }

        private void nuevoAlumno_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
