using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDMaestriaEIP
{
    public partial class EditarEstudiante : Form
    {
        int id;

        public EditarEstudiante()
        {
            InitializeComponent();
        }

        public EditarEstudiante(DataTable dt)
        {
            InitializeComponent();

            id = int.Parse(dt.Rows[0][0].ToString());

            tbNombre.Text = dt.Rows[0][1].ToString();
            tbCodigo.Text = dt.Rows[0][2].ToString();
            tbCodigoPrograma.Text = dt.Rows[0][3].ToString();
            tbEstado.Text = dt.Rows[0][4].ToString();
            tbTitulo.Text = dt.Rows[0][5].ToString();
            tbDirectores.Text = dt.Rows[0][6].ToString();
            tbCalificadores.Text = dt.Rows[0][7].ToString();
            tbCohorte.Text = dt.Rows[0][8].ToString();
            tbOtros.Text = dt.Rows[0][9].ToString();
            tbCodigo.Enabled = false;


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM estudiantes WHERE Id=" + id;

            SQLHandler sql = new SQLHandler();

            if(MessageBox.Show("¿ Está seguro que desea eliminar el estudiante ?","Confirmacion",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                if (sql.NonQuery(query))
                {
                    MessageBox.Show("Se elimino correctamente al estudiante","Eliminacion correcta",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Hubo un error realizando la eliminación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
        }

        private void btModify_Click(object sender, EventArgs e)
        {
            string nombre, codigo, codigoPrograma, estado, titulo, director, calificador, cohorte, otros;

            nombre = tbNombre.Text;
            codigo = tbCodigo.Text;
            codigoPrograma = tbCodigoPrograma.Text;
            estado = tbEstado.Text;
            titulo = tbTitulo.Text;
            director = tbDirectores.Text;
            calificador = tbCalificadores.Text;
            cohorte = tbCohorte.Text;
            otros = tbOtros.Text;

            string query = "UPDATE [estudiantes] SET [nombre] = '{0}', [codigo_estudiante] = '{1}', [codigo_programa] = '{2}' , [estado] = '{3}' , [titulo] = '{4}', [director] = '{5}', [calificador] = '{6}', [cohorte]='{7}', [otros]='{8}' WHERE [Id] = {9} ";

            query = string.Format(query, nombre, codigo, codigoPrograma, estado, titulo, director, calificador, cohorte, otros,id);

            Clipboard.SetText(query);

            SQLHandler sql = new SQLHandler();

            if (sql.NonQuery(query))
            {
                MessageBox.Show("Los datos se actualizaron correctamente","Modificación correcta",MessageBoxButtons.OK,MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Hubo un error realizando la modificación","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
        
    }
}
