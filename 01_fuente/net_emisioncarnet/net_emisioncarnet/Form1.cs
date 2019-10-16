using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using net_emisioncarnet.DAO;
using System.Text.RegularExpressions;

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
            picfotografia.Image = Image.FromFile(examinar.FileName);
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
                FileStream stream = new FileStream(txtexaminar.Text, FileMode.Open, FileAccess.Read);
                //Se inicailiza un flujo de archivo con la imagen seleccionada desde el disco.
                BinaryReader br = new BinaryReader(stream);
                FileInfo fi = new FileInfo(txtexaminar.Text);

                //Se inicializa un arreglo de Bytes del tamaño de la imagen
                byte[] binData = new byte[stream.Length];
                //Se almacena en el arreglo de bytes la informacion que se obtiene del flujo de archivos(foto)
                //Lee el bloque de bytes del flujo y escribe los datos en un búfer dado.
                stream.Read(binData, 0, Convert.ToInt32(stream.Length));

                ////Se muetra la imagen obtenida desde el flujo de datos
                picfotografia.Image = Image.FromStream(stream);

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
