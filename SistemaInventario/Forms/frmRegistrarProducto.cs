using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SistemaInventario.Data;
using SistemaInventario.Forms;
using SistemaInventario.Models;

namespace SistemaInventario
{
    public partial class frmRegistrarProducto : Form
    {
        private Repository<Producto> _productoRepo;
        private bool _modoEdicion = false;
        private int _idProductoEditar = 0;

        public frmRegistrarProducto()
        {
            InitializeComponent();
            _productoRepo = new Repository<Producto>();
        }

        private void frmRegistrarProducto_Load(object sender, EventArgs e)
        {
            LimpiarControles();
            CargarProductos();
            ConfigurarDataGridView();
        }

        private void ConfigurarDataGridView()
        {
            dgvRegistro.AutoGenerateColumns = false;
            dgvRegistro.Columns.Clear();

            dgvRegistro.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Name = "Id",
                Width = 50,
                Visible = false
            });

            dgvRegistro.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Name = "Nombre",
                Width = 150
            });

            dgvRegistro.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Codigo",
                HeaderText = "Código",
                Name = "Codigo",
                Width = 100
            });

            dgvRegistro.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Categoria",
                HeaderText = "Categoría",
                Name = "Categoria",
                Width = 120
            });

            dgvRegistro.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PrecioUnitario",
                HeaderText = "Precio Unitario",
                Name = "PrecioUnitario",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvRegistro.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Cantidad",
                HeaderText = "Cantidad",
                Name = "Cantidad",
                Width = 80
            });

            dgvRegistro.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StockMinimo",
                HeaderText = "Stock Mínimo",
                Name = "StockMinimo",
                Width = 100
            });

            dgvRegistro.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FechaCreacion",
                HeaderText = "Fecha Creación",
                Name = "FechaCreacion",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });
        }

        private void CargarProductos()
        {
            try
            {
                var productos = _productoRepo.GetAll().ToList();
                dgvRegistro.DataSource = null;
                dgvRegistro.DataSource = productos;

                // BONUS :(
                foreach (DataGridViewRow row in dgvRegistro.Rows)
                {
                    var producto = row.DataBoundItem as Producto;
                    if (producto != null && producto.Cantidad < producto.StockMinimo)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarControles()
        {
            txtNombre.Clear();
            txtCodigo.Clear();
            txtCategoria.Clear();
            txtPrecioUnitario.Clear();
            txtCantidad.Clear();
            txtStockMinimo.Clear();

            _modoEdicion = false;
            _idProductoEditar = 0;
            btnAgregar.Text = "Agregar";

            txtNombre.Focus();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El código es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
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
                if (_modoEdicion)
                {
                    var producto = new Producto
                    {
                        Id = _idProductoEditar,
                        Nombre = txtNombre.Text.Trim(),
                        Codigo = txtCodigo.Text.Trim(),
                        Categoria = txtCategoria.Text.Trim(),
                        PrecioUnitario = precioUnitario,
                        Cantidad = cantidad,
                        StockMinimo = stockMinimo
                    };

                    int resultado = _productoRepo.Update(producto);

                    if (resultado > 0)
                    {
                        MessageBox.Show("Producto actualizado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarControles();
                        CargarProductos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo actualizar el producto.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var productoExistente = _productoRepo.Query(
                        "Codigo = @Codigo",
                        new { Codigo = txtCodigo.Text.Trim() }
                    ).FirstOrDefault();

                    if (productoExistente != null)
                    {
                        MessageBox.Show("Ya existe un producto con ese código.", "Validación",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtCodigo.Focus();
                        return;
                    }

                    var producto = new Producto
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Codigo = txtCodigo.Text.Trim(),
                        Categoria = txtCategoria.Text.Trim(),
                        PrecioUnitario = precioUnitario,
                        Cantidad = cantidad,
                        StockMinimo = stockMinimo,
                        FechaCreacion = DateTime.Now
                    };

                    int id = _productoRepo.Insert(producto);

                    if (id > 0)
                    {
                        MessageBox.Show("Producto agregado correctamente.", "Éxito",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarControles();
                        CargarProductos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo agregar el producto.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRegistro_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var producto = dgvRegistro.Rows[e.RowIndex].DataBoundItem as Producto;

                if (producto != null)
                {
                    // Abrir formulario modal para editar
                    frmRegistrarProductoCmd frmEditar = new frmRegistrarProductoCmd(producto);

                    if (frmEditar.ShowDialog() == DialogResult.OK)
                    {
                        CargarProductos();
                    }
                }
            }
        }

        private void txtPrecioUnitario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            // Permitir solo un punto decimal
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