using System;
using System.Windows.Forms;
using SistemaInventario.Data;

namespace SistemaInventario
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            try
            {
                DatabaseConnection.Initialize();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmLogin());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar la aplicación:\n\n{ex.Message}\n\n{ex.StackTrace}",
                    "Error Fatal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}

//AUTOEVALUACION
//en esta cacion pues considero que mi autoevalucion sera un 7/10