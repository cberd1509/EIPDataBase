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
    public partial class CreacioEstudiante : Form
    {
        public CreacioEstudiante()
        {
            InitializeComponent();
        }

        private void CreacioEstudiante_Load(object sender, EventArgs e)
        {

        }

        private void btAdd_Click(object sender, EventArgs e)
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

            string query = "INSERT INTO [estudiantes] ([nombre],[codigo_estudiante],[codigo_programa],[estado],[titulo],[director],[calificador],[cohorte],[otros]) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}') ";

            query = string.Format(query, nombre, codigo, codigoPrograma, estado, titulo, director, calificador, cohorte, otros);

            SQLHandler sql = new SQLHandler();

            if (sql.NonQuery(query))
            {
                if(MessageBox.Show("El estudiante se agregó correctamente. ¿Desea agregar otro estudiante?", "Inserción exitosa", MessageBoxButtons.YesNo, MessageBoxIcon.Information)==DialogResult.Yes)
                {
                    foreach(Control c in panel2.Controls)
                    {
                        if(c is TextBox)
                        {
                            c.Text = "";
                        }
                    }
                }
                else
                {
                    Close();
                }
            }
            else
            {
                if (MessageBox.Show("Hubo un error al agregar el estudiante, ¿Desea volver a intentarlo?", "Error al agregar el estudiante", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    Close();
                }
            }
        }
    }
}

