using INVENTARIOZAP.Controllers;
using INVENTARIOZAP.Models;
using System;
using System.Windows.Forms;

namespace S6T1_MC
{
    public partial class INV_ZAP : Form
    {
        private readonly ProductoController _productoController;
        public INV_ZAP()
        {
            InitializeComponent();
            _productoController = new ProductoController();
            this.Text = "Inventario Zapatos";
        }

        private void INV_ZAP_Load(object sender, EventArgs e)
        {
            CargaLista();
        }

        public void CargaLista()
        {
            lstInventario.DataSource = null;
            lstInventario.DataSource = _productoController.Todos().ToList(); // <- Aquí es clave
            lstInventario.DisplayMember = "Nombre";
            lstInventario.ValueMember = "ProductoId";
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "" ||
                txtLote.Text.Trim() == "" ||
                txtProveedor.Text.Trim() == "" ||
                txtStock.Text.Trim() == "" ||
                txtPunitario.Text.Trim() == "")
            {
                MessageBox.Show("Completa todos los campos obligatorios.");
                return;
            }

            if (!uint.TryParse(txtStock.Text, out var stock))
            {
                MessageBox.Show("Stock debe ser número positivo.");
                return;
            }

            if (!decimal.TryParse(txtPunitario.Text, out var pUnit))
            {
                MessageBox.Show("Precio unitario inválido.");
                return;
            }

            var producto = new ProductoModel
            {
                Nombre = txtNombre.Text.Trim(),
                Lote = txtLote.Text.Trim(),
                FechaIngreso = dtpFingreso.Value.Date,
                FechaCaducidad = dtpFcaducidad.Value.Date,
                Proveedor = txtProveedor.Text.Trim(),
                Stock = stock,
                PrecioUnitario = pUnit
            };

            string res;
            if (lstInventario.Enabled == false)      // Editar
            {
                producto.ProductoId = Convert.ToInt32(lstInventario.SelectedValue);
                res = _productoController.Actualizar(producto);
            }
            else                                     // Nuevo registro
            {
                res = _productoController.Insertar(producto);
            }

            if (res == "ok")
            {
                MessageBox.Show("Se guardó con éxito");
                LimpiaCajas();
                CargaLista();
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (lstInventario.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un producto de la lista");
                return;
            }

            lstInventario.Enabled = false;
            var prod = _productoController.Uno(Convert.ToInt32(lstInventario.SelectedValue));

            txtNombre.Text = prod.Nombre;
            txtLote.Text = prod.Lote;
            dtpFingreso.Value = prod.FechaIngreso;
            dtpFcaducidad.Value = prod.FechaCaducidad;
            txtProveedor.Text = prod.Proveedor;
            txtStock.Text = prod.Stock.ToString();
            txtPunitario.Text = prod.PrecioUnitario.ToString("0.##");
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            LimpiaCajas();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LimpiaCajas()
        {
            txtNombre.Text = "";
            txtLote.Text = "";
            txtProveedor.Text = "";
            txtStock.Text = "";
            txtPunitario.Text = "";
            dtpFingreso.Value = DateTime.Today;
            dtpFcaducidad.Value = DateTime.Today;
            lstInventario.Enabled = true;
            lstInventario.ClearSelected();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (lstInventario.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un producto de la lista");
                return;
            }

            if (lstInventario.SelectedItem is not ProductoModel productoSeleccionado)
            {
                MessageBox.Show("Error al obtener el producto seleccionado.");
                return;
            }

            var nombre = productoSeleccionado.Nombre;
            var confirmar = MessageBox.Show(
                $"¿Está seguro de eliminar «{nombre}»?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes) return;

            var id = productoSeleccionado.ProductoId;
            var resultado = _productoController.Eliminar(id);

            if (resultado == "ok")
            {
                MessageBox.Show("Se eliminó con éxito");
                LimpiaCajas();
                CargaLista();
            }
            else
            {
                MessageBox.Show($"Error: {resultado}");
            }
        }

        private void DtpFingreso_ValueChanged(object sender, EventArgs e)
        {
        }

        private void Label3_Click(object sender, EventArgs e)
        {
        }
    }
}  
