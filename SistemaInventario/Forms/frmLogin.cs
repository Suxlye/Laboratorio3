using SistemaInventario.Data;
using SistemaInventario.Helpers;
using SistemaInventario.Models;
using System;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Windows.Forms;

namespace SistemaInventario
{
    public partial class frmLogin : Form
    {
        private Repository<Usuario> _usuarioRepo;

        public frmLogin()
        {
            InitializeComponent();

            try
            {
                _usuarioRepo = new Repository<Usuario>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el repositorio:\n\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsuario.Clear();
            txtPassword.Clear();
            txtUsuario.Focus();
        }

      
        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                MessageBox.Show("Por favor ingrese el usuario.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor ingrese la contraseña.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                string passwordCifrada = SecurityHelper.ComputeSHA256Hash(txtPassword.Text);

                var usuarios = _usuarioRepo.Query(
                    "Usuario = @Usuario AND Password = @Password AND Activo = 1",
                    new { Usuario = txtUsuario.Text, Password = passwordCifrada }
                );

                var usuario = usuarios.FirstOrDefault();

                if (usuario != null)
                {
                    MessageBox.Show($"Bienvenido {usuario.NombreUsuario}", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir formulario principal
                    frmRegistrarProducto frmPrincipal = new frmRegistrarProducto();
                    this.Hide();
                    frmPrincipal.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

      

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnIniciarSesion.PerformClick();
                e.Handled = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
