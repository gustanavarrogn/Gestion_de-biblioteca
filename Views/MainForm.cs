using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gestion_de_biblioteca
{
    public class MainForm : Form
    {
        private readonly BibliotecaManager manager = new();
        private readonly Color primary = Color.FromArgb(35, 95, 170);
        private readonly Color danger = Color.FromArgb(197, 66, 66);
        private readonly Color success = Color.FromArgb(40, 145, 95);

        private DataGridView dgvLibros = null!;
        private DataGridView dgvUsuarios = null!;
        private DataGridView dgvPrestamos = null!;
        private TextBox txtLibroId = null!, txtTitulo = null!, txtAutor = null!, txtAnio = null!, txtBuscarLibro = null!;
        private TextBox txtUsuarioId = null!, txtNombre = null!, txtCorreo = null!, txtBuscarUsuario = null!;
        private TextBox txtPrestamoId = null!;
        private ComboBox cmbUsuarios = null!, cmbLibros = null!;
        private DateTimePicker dtpPrestamo = null!;
        private Label lblTotalLibros = null!, lblDisponibles = null!, lblUsuarios = null!, lblPrestamos = null!;
        private BarChartControl chartLibros = null!, chartUsuarios = null!, chartInventario = null!;

        public MainForm()
        {
            Text = "Gestión de Biblioteca - MVC";
            Width = 1420;
            Height = 880;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1200, 780);
            BackColor = Color.FromArgb(245, 247, 250);

            InicializarUI();
            RefrescarTodo();
        }

        private void InicializarUI()
        {
            Label titulo = new()
            {
                Text = "Gestión de Biblioteca",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 42, 56),
                Location = new Point(22, 12),
                AutoSize = true
            };
            Controls.Add(titulo);

            Label subtitulo = new()
            {
                Text = "CRUD de libros, usuarios y préstamos con validaciones, persistencia JSON, estadísticas y separación MVC.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.DimGray,
                Location = new Point(26, 58),
                AutoSize = true
            };
            Controls.Add(subtitulo);

            CrearTarjetasResumen();
            CrearPanelLibros();
            CrearPanelUsuarios();
            CrearPanelPrestamos();
            CrearPanelEstadisticas();
        }

        private void CrearTarjetasResumen()
        {
            lblTotalLibros = CrearTarjeta("Libros", 30, 92);
            lblDisponibles = CrearTarjeta("Disponibles", 230, 92);
            lblUsuarios = CrearTarjeta("Usuarios", 430, 92);
            lblPrestamos = CrearTarjeta("Préstamos activos", 630, 92);
        }

        private Label CrearTarjeta(string titulo, int x, int y)
        {
            Panel panel = new()
            {
                Location = new Point(x, y),
                Size = new Size(180, 72),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(panel);

            panel.Controls.Add(new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Location = new Point(12, 9),
                AutoSize = true
            });

            Label valor = new()
            {
                Text = "0",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = primary,
                Location = new Point(12, 30),
                AutoSize = true
            };
            panel.Controls.Add(valor);
            return valor;
        }

        private void CrearPanelLibros()
        {
            GroupBox gb = CrearGrupo("Administrar libros", 30, 178, 650, 300);
            dgvLibros = CrearGrid(15, 58, 405, 222);
            dgvLibros.CellClick += DgvLibros_CellClick;
            gb.Controls.Add(dgvLibros);

            gb.Controls.Add(new Label { Text = "Buscar", Location = new Point(15, 29), AutoSize = true });
            txtBuscarLibro = new TextBox { Location = new Point(70, 25), Width = 350 };
            txtBuscarLibro.TextChanged += (_, _) => RefrescarLibros();
            gb.Controls.Add(txtBuscarLibro);

            CrearLabelYTextBox(gb, "ID", 440, 28, out txtLibroId, true);
            CrearLabelYTextBox(gb, "Título", 440, 78, out txtTitulo);
            CrearLabelYTextBox(gb, "Autor", 440, 128, out txtAutor);
            CrearLabelYTextBox(gb, "Año", 440, 178, out txtAnio);

            Button btnAgregar = CrearBoton("Añadir", 440, 235, 70, primary);
            Button btnEditar = CrearBoton("Editar", 515, 235, 65, success);
            Button btnEliminar = CrearBoton("Eliminar", 585, 235, 70, danger);
            Button btnLimpiar = CrearBoton("Limpiar", 440, 263, 215, Color.Gray);

            btnAgregar.Click += BtnAgregarLibro_Click;
            btnEditar.Click += BtnEditarLibro_Click;
            btnEliminar.Click += BtnEliminarLibro_Click;
            btnLimpiar.Click += (_, _) => LimpiarLibro();
            gb.Controls.AddRange(new Control[] { btnAgregar, btnEditar, btnEliminar, btnLimpiar });
        }

        private void CrearPanelUsuarios()
        {
            GroupBox gb = CrearGrupo("Administrar usuarios", 30, 495, 650, 300);
            dgvUsuarios = CrearGrid(15, 58, 405, 222);
            dgvUsuarios.CellClick += DgvUsuarios_CellClick;
            gb.Controls.Add(dgvUsuarios);

            gb.Controls.Add(new Label { Text = "Buscar", Location = new Point(15, 29), AutoSize = true });
            txtBuscarUsuario = new TextBox { Location = new Point(70, 25), Width = 350 };
            txtBuscarUsuario.TextChanged += (_, _) => RefrescarUsuarios();
            gb.Controls.Add(txtBuscarUsuario);

            CrearLabelYTextBox(gb, "ID", 440, 28, out txtUsuarioId, true);
            CrearLabelYTextBox(gb, "Nombre", 440, 78, out txtNombre);
            CrearLabelYTextBox(gb, "Correo", 440, 128, out txtCorreo);

            Button btnAgregar = CrearBoton("Añadir", 440, 235, 70, primary);
            Button btnEditar = CrearBoton("Editar", 515, 235, 65, success);
            Button btnEliminar = CrearBoton("Eliminar", 585, 235, 70, danger);
            Button btnLimpiar = CrearBoton("Limpiar", 440, 263, 215, Color.Gray);

            btnAgregar.Click += BtnAgregarUsuario_Click;
            btnEditar.Click += BtnEditarUsuario_Click;
            btnEliminar.Click += BtnEliminarUsuario_Click;
            btnLimpiar.Click += (_, _) => LimpiarUsuario();
            gb.Controls.AddRange(new Control[] { btnAgregar, btnEditar, btnEliminar, btnLimpiar });
        }

        private void CrearPanelPrestamos()
        {
            GroupBox gb = CrearGrupo("Gestionar préstamos", 700, 178, 680, 300);
            dgvPrestamos = CrearGrid(15, 90, 650, 190);
            dgvPrestamos.CellClick += DgvPrestamos_CellClick;
            gb.Controls.Add(dgvPrestamos);

            CrearLabelYTextBox(gb, "ID", 15, 28, out txtPrestamoId, true, 60);
            gb.Controls.Add(new Label { Text = "Usuario", Location = new Point(90, 28), AutoSize = true });
            cmbUsuarios = new ComboBox { Location = new Point(90, 48), Width = 175, DropDownStyle = ComboBoxStyle.DropDownList };
            gb.Controls.Add(cmbUsuarios);

            gb.Controls.Add(new Label { Text = "Libro disponible", Location = new Point(280, 28), AutoSize = true });
            cmbLibros = new ComboBox { Location = new Point(280, 48), Width = 190, DropDownStyle = ComboBoxStyle.DropDownList };
            gb.Controls.Add(cmbLibros);

            gb.Controls.Add(new Label { Text = "Fecha", Location = new Point(485, 28), AutoSize = true });
            dtpPrestamo = new DateTimePicker { Location = new Point(485, 48), Width = 180 };
            gb.Controls.Add(dtpPrestamo);

            Button btnAgregar = CrearBoton("Prestar", 15, 63, 80, primary);
            Button btnDevolver = CrearBoton("Devolver", 100, 63, 85, success);
            Button btnEliminar = CrearBoton("Eliminar", 190, 63, 85, danger);
            Button btnLimpiar = CrearBoton("Limpiar", 280, 63, 80, Color.Gray);

            btnAgregar.Click += BtnAgregarPrestamo_Click;
            btnDevolver.Click += BtnDevolver_Click;
            btnEliminar.Click += BtnEliminarPrestamo_Click;
            btnLimpiar.Click += (_, _) => LimpiarPrestamo();
            gb.Controls.AddRange(new Control[] { btnAgregar, btnDevolver, btnEliminar, btnLimpiar });
        }

        private void CrearPanelEstadisticas()
        {
            GroupBox gb = CrearGrupo("Gráficos estadísticos del inventario", 700, 495, 680, 300);

            gb.Controls.Add(new Label { Text = "Libros más prestados", Location = new Point(15, 27), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
            chartLibros = new BarChartControl { Location = new Point(15, 50), Size = new Size(315, 105) };
            gb.Controls.Add(chartLibros);

            gb.Controls.Add(new Label { Text = "Usuarios más activos", Location = new Point(350, 27), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
            chartUsuarios = new BarChartControl { Location = new Point(350, 50), Size = new Size(315, 105) };
            gb.Controls.Add(chartUsuarios);

            gb.Controls.Add(new Label { Text = "Estado del inventario", Location = new Point(15, 170), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
            chartInventario = new BarChartControl { Location = new Point(15, 193), Size = new Size(650, 83) };
            gb.Controls.Add(chartInventario);
        }

        private GroupBox CrearGrupo(string texto, int x, int y, int w, int h)
        {
            GroupBox gb = new()
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(w, h),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(30, 42, 56)
            };
            Controls.Add(gb);
            return gb;
        }

        private DataGridView CrearGrid(int x, int y, int w, int h)
        {
            DataGridView dgv = new()
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = primary;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }

        private void CrearLabelYTextBox(Control parent, string label, int x, int y, out TextBox textBox, bool readOnly = false, int width = 215)
        {
            parent.Controls.Add(new Label { Text = label, Location = new Point(x, y), AutoSize = true });
            textBox = new TextBox { Location = new Point(x, y + 20), Width = width, ReadOnly = readOnly };
            parent.Controls.Add(textBox);
        }

        private Button CrearBoton(string texto, int x, int y, int w, Color color)
        {
            return new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Width = w,
                Height = 26,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            };
        }

        private void RefrescarTodo()
        {
            RefrescarLibros();
            RefrescarUsuarios();
            RefrescarPrestamos();
            RefrescarCombos();
            RefrescarResumen();
            RefrescarGraficos();
        }

        private void RefrescarLibros()
        {
            string filtro = txtBuscarLibro?.Text.Trim().ToLowerInvariant() ?? string.Empty;
            dgvLibros.DataSource = manager.Libros
                .Where(l => string.IsNullOrWhiteSpace(filtro) || l.Titulo.ToLower().Contains(filtro) || l.Autor.ToLower().Contains(filtro) || l.Id.ToString().Contains(filtro))
                .Select(l => new { l.Id, l.Titulo, l.Autor, l.Anio, Disponible = l.Disponible ? "Sí" : "No" })
                .ToList();
        }

        private void RefrescarUsuarios()
        {
            string filtro = txtBuscarUsuario?.Text.Trim().ToLowerInvariant() ?? string.Empty;
            dgvUsuarios.DataSource = manager.Usuarios
                .Where(u => string.IsNullOrWhiteSpace(filtro) || u.Nombre.ToLower().Contains(filtro) || u.CorreoElectronico.ToLower().Contains(filtro) || u.Id.ToString().Contains(filtro))
                .Select(u => new { u.Id, u.Nombre, Correo = u.CorreoElectronico })
                .ToList();
        }

        private void RefrescarPrestamos()
        {
            dgvPrestamos.DataSource = manager.Prestamos.Select(p => new
            {
                p.Id,
                Usuario = manager.Usuarios.FirstOrDefault(u => u.Id == p.UsuarioId)?.Nombre ?? "",
                Libro = manager.Libros.FirstOrDefault(l => l.Id == p.LibroId)?.Titulo ?? "",
                FechaPrestamo = p.FechaPrestamo.ToShortDateString(),
                FechaDevolucion = p.FechaDevolucion.HasValue ? p.FechaDevolucion.Value.ToShortDateString() : "Pendiente",
                p.Estado
            }).ToList();

            string[,] matriz = manager.GenerarMatrizPrestamos();
        }

        private void RefrescarCombos()
        {
            cmbUsuarios.DataSource = manager.Usuarios.Select(u => new { u.Id, Texto = $"{u.Id} - {u.Nombre}" }).ToList();
            cmbUsuarios.DisplayMember = "Texto";
            cmbUsuarios.ValueMember = "Id";

            cmbLibros.DataSource = manager.Libros.Where(l => l.Disponible).Select(l => new { l.Id, Texto = $"{l.Id} - {l.Titulo}" }).ToList();
            cmbLibros.DisplayMember = "Texto";
            cmbLibros.ValueMember = "Id";
        }

        private void RefrescarResumen()
        {
            lblTotalLibros.Text = manager.Libros.Count.ToString();
            lblDisponibles.Text = manager.Libros.Count(l => l.Disponible).ToString();
            lblUsuarios.Text = manager.Usuarios.Count.ToString();
            lblPrestamos.Text = manager.Prestamos.Count(p => p.Estado == "Prestado").ToString();
        }

        private void RefrescarGraficos()
        {
            chartLibros.SetData(manager.ObtenerLibrosMasPrestados());
            chartUsuarios.SetData(manager.ObtenerUsuariosMasActivos());
            chartInventario.SetData(manager.ObtenerDisponibilidadInventario());
        }

        private void BtnAgregarLibro_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() =>
            {
                if (!int.TryParse(txtAnio.Text, out int anio)) throw new Exception("El año debe ser numérico.");
                manager.AgregarLibro(txtTitulo.Text, txtAutor.Text, anio);
                LimpiarLibro();
            });
        }

        private void BtnEditarLibro_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() =>
            {
                if (string.IsNullOrWhiteSpace(txtLibroId.Text)) throw new Exception("Selecciona un libro.");
                if (!int.TryParse(txtAnio.Text, out int anio)) throw new Exception("El año debe ser numérico.");
                manager.EditarLibro(int.Parse(txtLibroId.Text), txtTitulo.Text, txtAutor.Text, anio);
                LimpiarLibro();
            });
        }

        private void BtnEliminarLibro_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() =>
            {
                if (string.IsNullOrWhiteSpace(txtLibroId.Text)) throw new Exception("Selecciona un libro.");
                if (Confirmar("¿Deseas eliminar este libro?")) manager.EliminarLibro(int.Parse(txtLibroId.Text));
                LimpiarLibro();
            });
        }

        private void BtnAgregarUsuario_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() => { manager.AgregarUsuario(txtNombre.Text, txtCorreo.Text); LimpiarUsuario(); });
        }

        private void BtnEditarUsuario_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() =>
            {
                if (string.IsNullOrWhiteSpace(txtUsuarioId.Text)) throw new Exception("Selecciona un usuario.");
                manager.EditarUsuario(int.Parse(txtUsuarioId.Text), txtNombre.Text, txtCorreo.Text);
                LimpiarUsuario();
            });
        }

        private void BtnEliminarUsuario_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() =>
            {
                if (string.IsNullOrWhiteSpace(txtUsuarioId.Text)) throw new Exception("Selecciona un usuario.");
                if (Confirmar("¿Deseas eliminar este usuario?")) manager.EliminarUsuario(int.Parse(txtUsuarioId.Text));
                LimpiarUsuario();
            });
        }

        private void BtnAgregarPrestamo_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() =>
            {
                if (cmbUsuarios.SelectedValue == null) throw new Exception("Selecciona un usuario.");
                if (cmbLibros.SelectedValue == null) throw new Exception("Selecciona un libro disponible.");
                manager.RegistrarPrestamo(Convert.ToInt32(cmbUsuarios.SelectedValue), Convert.ToInt32(cmbLibros.SelectedValue), dtpPrestamo.Value);
                LimpiarPrestamo();
            });
        }

        private void BtnDevolver_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() =>
            {
                if (string.IsNullOrWhiteSpace(txtPrestamoId.Text)) throw new Exception("Selecciona un préstamo.");
                manager.RegistrarDevolucion(int.Parse(txtPrestamoId.Text), DateTime.Now);
                LimpiarPrestamo();
            });
        }

        private void BtnEliminarPrestamo_Click(object? sender, EventArgs e)
        {
            EjecutarAccion(() =>
            {
                if (string.IsNullOrWhiteSpace(txtPrestamoId.Text)) throw new Exception("Selecciona un préstamo.");
                if (Confirmar("¿Deseas eliminar este préstamo?")) manager.EliminarPrestamo(int.Parse(txtPrestamoId.Text));
                LimpiarPrestamo();
            });
        }

        private void DgvLibros_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow fila = dgvLibros.Rows[e.RowIndex];
            txtLibroId.Text = fila.Cells["Id"].Value?.ToString();
            txtTitulo.Text = fila.Cells["Titulo"].Value?.ToString();
            txtAutor.Text = fila.Cells["Autor"].Value?.ToString();
            txtAnio.Text = fila.Cells["Anio"].Value?.ToString();
        }

        private void DgvUsuarios_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];
            txtUsuarioId.Text = fila.Cells["Id"].Value?.ToString();
            txtNombre.Text = fila.Cells["Nombre"].Value?.ToString();
            txtCorreo.Text = fila.Cells["Correo"].Value?.ToString();
        }

        private void DgvPrestamos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            txtPrestamoId.Text = dgvPrestamos.Rows[e.RowIndex].Cells["Id"].Value?.ToString();
        }

        private void LimpiarLibro() { txtLibroId.Clear(); txtTitulo.Clear(); txtAutor.Clear(); txtAnio.Clear(); }
        private void LimpiarUsuario() { txtUsuarioId.Clear(); txtNombre.Clear(); txtCorreo.Clear(); }
        private void LimpiarPrestamo() { txtPrestamoId.Clear(); }

        private void EjecutarAccion(Action accion)
        {
            try
            {
                accion();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static bool Confirmar(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
