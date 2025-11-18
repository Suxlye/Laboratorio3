using System;
using System.Linq;
using System.Windows.Forms;
using SistemaInventario.Data;
using SistemaInventario.Models;

namespace SistemaInventario
{
    public partial class frmRegistrarProductoCmd : Form
    {
        private Repository<Producto> _productoRepo;
        private Producto _productoActual;

        public frmRegistrarProductoCmd(Producto producto)
        {
            InitializeComponent();
            _productoRepo = new Repository<Producto>();
            _productoActual = producto;
        }

        private void frmRegistrarProductoCmd_Load(object sender, EventArgs e)
        {
            if (_productoActual != null)
            {
                txtNombre.Text = _productoActual.Nombre;
                txtCodigo.Text = _productoActual.Codigo;
                txtCategoria.Text = _productoActual.Categoria;
                txtPrecioUnitario.Text = _productoActual.PrecioUnitario.ToString("F2");
                txtCantidad.Text = _productoActual.Cantidad.ToString();
                txtStockMinimo.Text = _productoActual.StockMinimo.ToString();

                txtCodigo.ReadOnly = true;
                txtCodigo.BackColor = System.Drawing.SystemColors.Control;
            }

            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                MessageBox.Show("La categoría es obligatoria.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoria.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario) || precioUnitario <= 0)
            {
                MessageBox.Show("El precio unitario debe ser un número válido mayor a 0.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioUnitario.Focus();
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad < 0)
            {
                MessageBox.Show("La cantidad debe ser un número entero válido mayor o igual a 0.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCantidad.Focus();
                return;
            }

            if (!int.TryParse(txtStockMinimo.Text, out int stockMinimo) || stockMinimo < 0)
            {
                MessageBox.Show("El stock mínimo debe ser un número entero válido mayor o igual a 0.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStockMinimo.Focus();
                return;
            }

            try
            {
                _productoActual.Nombre = txtNombre.Text.Trim();
                _productoActual.Categoria = txtCategoria.Text.Trim();
                _productoActual.PrecioUnitario = precioUnitario;
                _productoActual.Cantidad = cantidad;
                _productoActual.StockMinimo = stockMinimo;

                int resultado = _productoRepo.Update(_productoActual);

                if (resultado > 0)
                {
                    MessageBox.Show("Producto actualizado correctamente.", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el producto.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtCantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtStockMinimo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
