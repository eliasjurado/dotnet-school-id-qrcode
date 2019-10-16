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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private AlumnoDAO alumno = new AlumnoDAO();

        private void CargarGrilla()
        {
            dgvAlumno.DataSource = alumno.ListarAlumnos();
            dgvAlumno.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlumno.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvAlumno.AllowUserToAddRows = false;
            dgvAlumno.AllowUserToDeleteRows = false;
            dgvAlumno.ReadOnly = true;
        }
        private void ListarXDnioCod()
        {
            if (txtbuscar.Text == "") { CargarGrilla(); }
            else
            {
                dgvAlumno.DataSource = alumno.ListarXDnioCod(txtbuscar.Text);
                dgvAlumno.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAlumno.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvAlumno.AllowUserToAddRows = false;
                dgvAlumno.AllowUserToDeleteRows = false;
                dgvAlumno.ReadOnly = true;
            }
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            CargarGrilla();
        }

     public delegate void Almacenar_Datos(
        string id,
        string dni, 
        string nombre,
        string apepat, 
        string apetmat, 
        DateTime fecreg,
        Image foto
     );
        public event Almacenar_Datos PasarDatos;

        private void dgvAlumno_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                dgvAlumno.CurrentRow.Selected = true;
                string id= dgvAlumno.CurrentRow.Cells[0].Value.ToString();
                string dni = dgvAlumno.CurrentRow.Cells[1].Value.ToString();
                string nombre = dgvAlumno.CurrentRow.Cells[2].Value.ToString();
                string apepat = dgvAlumno.CurrentRow.Cells[3].Value.ToString();
                string apetmat = dgvAlumno.CurrentRow.Cells[4].Value.ToString();
                DateTime fecreg = DateTime.Parse(dgvAlumno.CurrentRow.Cells[5].Value.ToString());
                DataGridViewImageCell cell = dgvAlumno.CurrentRow.Cells[6] as DataGridViewImageCell;
                byte[] imagen = (byte[])cell.Value;
                var ms = new MemoryStream(imagen);
                Image foto = Image.FromStream(ms);

                PasarDatos(
                    id,
                    dni,
                    nombre,
                    apepat,
                    apetmat,
                    fecreg,
                    foto
                );
            }
            this.Close();
        }

        private void buscar_Click(object sender, EventArgs e)
        {
            ListarXDnioCod();
        }
    }
}
