using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ex = Microsoft.Office.Interop.Excel;

namespace BDMaestriaEIP
{
    public partial class Form1 : Form
    {
        string searchmode="";
        SQLHandler sql;

        public Form1()
        {
            sql = new SQLHandler();
            InitializeComponent();            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rbNombre.PerformClick();
        }

        private void rbSelected(object sender, EventArgs e)
        {
            tbNombre.Enabled = false;
            tbNombre.Text = "";
            tbKeywords.Enabled = false;
            tbKeywords.Text = "";
            tbCohorte.Enabled = false;
            tbCohorte.Text = "";
            tbCodigo.Enabled = false;
            tbCodigo.Text = "";

            switch (((RadioButton)sender).Name)
            {
                case "rbCodigo": tbCodigo.Enabled = true; searchmode = "codigo"; break;
                case "rbCohorte": tbCohorte.Enabled = true; searchmode = "cohorte"; break;
                case "rbKeywords": tbKeywords.Enabled = true; searchmode = "keywords"; break;
                case "rbNombre": tbNombre.Enabled = true; searchmode = "nombre"; break;
            }
        }


        private DataTable Busqueda()
        {
            string query="";
            DataTable dt = new DataTable();
            switch (searchmode)
            {
                case "codigo":
                    query = "SELECT * FROM [estudiantes] WHERE codigo_estudiante LIKE '%" + tbCodigo.Text +"%' ";
                    dt = sql.SelectQuery(query);
                    return dt;                    
                case "cohorte":
                    query = "SELECT * FROM [estudiantes] WHERE cohorte LIKE '%" + tbCohorte.Text + "%' ";
                    dt = sql.SelectQuery(query);
                    return dt;
                case "keywords":
                    query = "SELECT * FROM [estudiantes]";
                    dt = sql.SelectQuery(query);

                    DataTable dt2 = new DataTable(dt.TableName);
                    dt2 = dt.Clone();

                    foreach (DataRow row in dt.Rows)
                    {
                        string rowstring = row[1] + " " + row[2] + " " + row[3] + " " + row[4] + " " + row[5] + " " + row[6] + " " + row[7] + " " + row[8] + " " + row[9];

                        if (rowstring.IndexOf(tbKeywords.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            dt2.ImportRow(row);
                        }
                        
                    }
                    return dt2;
                case "nombre":
                    query = "SELECT * FROM [estudiantes] WHERE nombre LIKE '%" + tbNombre.Text + "%' ";
                    dt = sql.SelectQuery(query);
                    return dt;
            }

            return dt;

        }

        private void btBusquedaClick(object sender, EventArgs e)
        {
            DataTable dt = Busqueda();

            SearchResults resultados = new SearchResults(dt);
            if (dt.Rows.Count > 0) resultados.ShowDialog();
            else MessageBox.Show("La busqueda ingresada no produjo ningun resultado", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            CreacioEstudiante newStudent = new CreacioEstudiante();

            newStudent.ShowDialog();
        }

        private void btToExcel_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            ex.Application xlApp = new ex.Application();

            progressBar1.Value = 0;

            if(xlApp == null)
            {
                MessageBox.Show("Para poder usar esta funcionalidad, Microsoft Excel debe estar instalado", "No se encontró Excel", MessageBoxButtons.OK , MessageBoxIcon.Error);
            }

            progressBar1.Value = 10;

            ex.Workbook wb = xlApp.Workbooks.Add(ex.XlWBATemplate.xlWBATWorksheet);
            ex.Worksheet ws = wb.ActiveSheet;

            string query = "SELECT * FROM [estudiantes]";
            DataTable dt = sql.SelectQuery(query);

            ws.Cells[1, 1] = "ID";
            ws.Cells[1, 2] = "Nombre del Estudiante";
            ws.Cells[1, 3] = "Código del Estudiante";
            ws.Cells[1, 4] = "Código del Programa";
            ws.Cells[1, 5] = "Estado";
            ws.Cells[1, 6] = "Título del Proyecto";
            ws.Cells[1, 7] = "Director(es) del proyecto";
            ws.Cells[1, 8] = "Calificador(es) del proyecto";
            ws.Cells[1, 9] = "Cohorte";
            ws.Cells[1, 10] = "Otros / Observaciones";

            progressBar1.Value = 20;

            int count = 2;
            foreach(DataRow row in dt.Rows)
            {
                ws.Cells[count, 1] = row[0];
                ws.Cells[count, 2] = row[1];
                ws.Cells[count, 3] = row[2];
                ws.Cells[count, 4] = row[3];
                ws.Cells[count, 5] = row[4];
                ws.Cells[count, 6] = row[5];
                ws.Cells[count, 7] = row[6];
                ws.Cells[count, 8] = row[7];
                ws.Cells[count, 9] = row[8];
                ws.Cells[count, 10] = row[9];

                count++;
            }

            progressBar1.Value = 70;

            ws.Rows.RowHeight = 25;

            ex.Range dataRange = ws.UsedRange;

            dataRange.Font.Color = ColorTranslator.FromHtml("#720303");
            dataRange.Font.Name = "Segoe UI";
            dataRange.Font.Size = 13;
            ws.Columns.AutoFit();
            dataRange.Font.Size = 11;

            ex.Range hRange = ws.Range[ws.Cells[1, 1], ws.Cells[1, 10]];
            hRange.Interior.Color = ColorTranslator.FromHtml("#474747");
            hRange.Font.Color = Color.White;
            hRange.Font.Name = "Segoe UI";
            hRange.Font.Size = 14;

            ws.Columns.AutoFit();

            dataRange.WrapText = true;

            hRange.Font.Size = 12;

            progressBar1.Value = 85;

            for (int i = 1; i <= 10; i++)
            {
                if (ws.Columns[i].ColumnWidth > 60)
                {
                    ws.Columns[i].ColumnWidth = 60;
                }
            }

            int lastRow = dataRange.Row + dataRange.Rows.Count - 1;

            ws.Rows.AutoFit();

            for (int i = 1; i <= lastRow; i++)
            {
                if (dataRange.Rows[i].RowHeight < 25)
                {
                    dataRange.Rows[i].RowHeight = 25;
                }
            }
            
            dataRange.VerticalAlignment = ex.XlVAlign.xlVAlignCenter;
            dataRange.HorizontalAlignment = ex.XlHAlign.xlHAlignCenter;

            progressBar1.Value = 100;

            //ESTA TIENE MUCHOS COMENTARIOS JHUHUHEUHUE
            //ESTA TIENE MUCHOS COMENTARIOS JHUHUHEUHUE
            //ESTA TIENE MUCHOS COMENTARIOS JHUHUHEUHUE
            //ESTA TIENE MUCHOS COMENTARIOS JHUHUHEUHUE
            //ESTA TIENE MUCHOS COMENTARIOS JHUHUHEUHUE
            //ESTA TIENE MUCHOS COMENTARIOS JHUHUHEUHUE
            //ESTA TIENE MUCHOS COMENTARIOS JHUHUHEUHUE
            //ESTA TIENE MUCHOS COMENTARIOS JHUHUHEUHUE

            xlApp.WindowState = ex.XlWindowState.xlMaximized;
            xlApp.ActiveWindow.Activate();
            xlApp.Visible = true;

            progressBar1.Value = 0;
            progressBar1.Visible = false;


        }

        private void ValidateKey(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btBuscar.PerformClick();
            }
        }
    }
}
