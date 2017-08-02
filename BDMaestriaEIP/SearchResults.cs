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
    public partial class SearchResults : Form
    {
        public SearchResults()
        {
            InitializeComponent();
        }

        public SearchResults(DataTable dt)
        {

            InitializeComponent();

            int count = dt.Rows.Count;
            if(count>0)
            {
                lbTitle.Text = "Se encontraron " + count + " resultados";
            }
            else
            {
                lbTitle.Text = "No se encontró ningun resultado";
            }

            dgResults.DataSource = dt;
            dgResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgResults.Columns[0].HeaderText = "ID";
            dgResults.Columns[1].HeaderText = "Nombre y Apellido";
            dgResults.Columns[2].HeaderText = "Codigo de Estudinte";
            dgResults.Columns[3].HeaderText = "Codigo de Programa";
            dgResults.Columns[4].HeaderText = "Estado";
            dgResults.Columns[5].HeaderText = "Título del proyecto";
            dgResults.Columns[6].HeaderText = "Director(es) del Proyecto";
            dgResults.Columns[7].HeaderText = "Calificador(es) del Proyecto";
            dgResults.Columns[8].HeaderText = "Cohorte";
            dgResults.Columns[9].HeaderText = "Otros";

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            dgResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgResults.SelectedRows[0];

            int id = int.Parse(row.Cells[0].Value.ToString());

            SQLHandler sql = new SQLHandler();

            DataTable dtEdit = sql.SelectQuery("SELECT * FROM [estudiantes] WHERE Id=" + id);

            EditarEstudiante Edit = new EditarEstudiante(dtEdit);

            if (Edit.ShowDialog() == DialogResult.OK)
            {
                Close();
            }
                        
        }

        private void dgResults_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            button1.PerformClick();
        }

        private void dgResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
