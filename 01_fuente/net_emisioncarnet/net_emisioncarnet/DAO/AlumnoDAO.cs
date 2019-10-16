using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace net_emisioncarnet.DAO
{
    class AlumnoDAO
    {
        //Conexion para métodos
        private ConexionBD cnx = new ConexionBD();
        private SqlConnection cn;
        private SqlConnection cn1 = new SqlConnection
          (ConfigurationManager.ConnectionStrings["cadenaconexion"].ConnectionString);


        public DataTable ListarAlumnos()
        {
            DataTable dtb = new DataTable();

            SqlDataAdapter dap = new SqlDataAdapter
                ("sp_listaralumno", cn1);
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.Fill(dtb);

            return dtb;
        }
        public DataTable ListarXDnioCod(string filtro)
        {
            DataTable dtb = new DataTable();

            SqlDataAdapter dap = new SqlDataAdapter
                ("sp_listaralumnoxdniocod", cn1);
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@filtro", filtro);
            dap.Fill(dtb);

            return dtb;
        }
        public string RegistrarAlumno(string dni, string nombre, string apepat, string apetmat, byte[] foto)
        {
            cn = cnx.Conectar();
            string mensaje = string.Empty;
            SqlTransaction tr = null;
            SqlCommand cmd = new SqlCommand("sp_registraralumno", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dni", dni);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apepat", apepat);
            cmd.Parameters.AddWithValue("@apetmat", apetmat);
            cmd.Parameters.AddWithValue("@foto", foto);
            int q = 0;
            try
            {
                cn.Open();
                tr = cn.BeginTransaction(IsolationLevel.Serializable);
                cmd.Transaction = tr;
                q = cmd.ExecuteNonQuery();
                tr.Commit();
                mensaje = "Registro creado";
            }
            catch (Exception ex)
            {
                tr.Rollback();
                mensaje = ex.Message;
            }
            finally
            {
                cn.Close();
            }
            return mensaje;
        }

        public string ActualizarAlumno(string id,string dni, string nombre, string apepat, string apetmat, byte[] foto)
        {
            cn = cnx.Conectar();
            string mensaje = string.Empty;
            SqlTransaction tr = null;
            SqlCommand cmd = new SqlCommand("sp_actualizaralumno", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@dni", dni);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@apepat", apepat);
            cmd.Parameters.AddWithValue("@apetmat", apetmat);
            cmd.Parameters.AddWithValue("@foto", foto);
            int q = 0;
            try
            {
                cn.Open();
                tr = cn.BeginTransaction(IsolationLevel.Serializable);
                cmd.Transaction = tr;
                q = cmd.ExecuteNonQuery();
                tr.Commit();
                mensaje = "Registro actualizado";
            }
            catch (Exception ex)
            {
                tr.Rollback();
                mensaje = ex.Message;
            }
            finally
            {
                cn.Close();
            }
            return mensaje;
        }
    }
}
