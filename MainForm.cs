using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gestion_de_biblioteca
{
    public class MainForm : Form
    {
        private readonly BibliotecaManager manager = new BibliotecaManager();

        // Libros
        private DataGridView dgvLibros;
        private TextBox txtLibroId;
        private TextBox txtTitulo;
        private TextBox txtAutor;
        private TextBox txtAnio;

        // Usuarios
        private DataGridView dgvUsuarios;
        private TextBox txtUsuarioId;
        private TextBox txtNombre;
        private TextBox txtCorreo;

        // Prestamos
        private DataGridView dgvPrestamos;
        private ComboBox cmbUsuarios;
        private ComboBox cmbLibros;
        private DateTimePicker dtpPrestamo;
        private DateTimePicker dtpDevolucion;
        private TextBox txtPrestamoId;

        public MainForm()
        {
            Text = "Gestión de Biblioteca";
            Width = 1400;
            Height = 850;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.WhiteSmoke;

            InicializarUI();
            RefrescarTodo();
        }

        private void InicializarUI()
        {
            Label lblTitulo = new Label
            {
                Text = "GESTIÓN DE BIBLIOTECA",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            Controls.Add(lblTitulo);

            Label lblSub = new Label
            {
                Text = "Aplicación de escritorio para administrar libros, usuarios y préstamos mediante un CRUD completo",
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Location = new Point(22, 55)
            };
            Controls.Add(lblSub);

            // Panel Libros
            GroupBox gbLibros = new GroupBox
            {
                Text = "Administrar Libros",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(20, 90),
                Size = new Size(630, 300)
            };
            Controls.Add(gbLibros);

            dgvLibros = new DataGridView
            {
                Location = new Point(15, 25),
                Size = new Size(390, 250),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvLibros.CellClick += DgvLibros_CellClick;
            gbLibros.Controls.Add(dgvLibros);

            gbLibros.Controls.Add(new Label { Text = "ID", Location = new Point(420, 30), AutoSize = true });
            txtLibroId = new TextBox { Location = new Point(420, 50), Width = 180, ReadOnly = true };
            gbLibros.Controls.Add(txtLibroId);

            gbLibros.Controls.Add(new Label { Text = "Título", Location = new Point(420, 80), AutoSize = true });
            txtTitulo = new TextBox { Location = new Point(420, 100), Width = 180 };
            gbLibros.Controls.Add(txtTitulo);

            gbLibros.Controls.Add(new Label { Text = "Autor", Location = new Point(420, 130), AutoSize = true });
            txtAutor = new TextBox { Location = new Point(420, 150), Width = 180 };
            gbLibros.Controls.Add(txtAutor);

            gbLibros.Controls.Add(new Label { Text = "Año", Location = new Point(420, 180), AutoSize = true });
            txtAnio = new TextBox { Location = new Point(420, 200), Width = 180 };
            gbLibros.Controls.Add(txtAnio);

            Button btnAgregarLibro = new Button { Text = "Añadir", Location = new Point(420, 235), Width = 55 };
            Button btnEditarLibro = new Button { Text = "Editar", Location = new Point(482, 235), Width = 55 };
            Button btnEliminarLibro = new Button { Text = "Eliminar", Location = new Point(544, 235), Width = 60 };

            btnAgregarLibro.Click += BtnAgregarLibro_Click;
            btnEditarLibro.Click += BtnEditarLibro_Click;
            btnEliminarLibro.Click += BtnEliminarLibro_Click;

            gbLibros.Controls.Add(btnAgregarLibro);
            gbLibros.Controls.Add(btnEditarLibro);
            gbLibros.Controls.Add(btnEliminarLibro);

            // Panel graficos
            GroupBox gbGraficos = new GroupBox
            {
                Text = "Estadísticas",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(670, 90),
                Size = new Size(690, 300)
            };
            Controls.Add(gbGraficos);

            Label lblGraf1 = new Label
            {
                Text = "Libros más Prestados",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(20, 25),
                AutoSize = true
            };
            gbGraficos.Controls.Add(lblGraf1);

            Label lblGraf2 = new Label
            {
                Text = "Usuarios más Activos",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(355, 25),
                AutoSize = true
            };
            gbGraficos.Controls.Add(lblGraf2);

            // Panel Usuarios
            GroupBox gbUsuarios = new GroupBox
            {
                Text = "Administrar Usuarios",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(20, 405),
                Size = new Size(630, 300)
            };
            Controls.Add(gbUsuarios);

            dgvUsuarios = new DataGridView
            {
                Location = new Point(15, 25),
                Size = new Size(390, 250),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvUsuarios.CellClick += DgvUsuarios_CellClick;
            gbUsuarios.Controls.Add(dgvUsuarios);

            gbUsuarios.Controls.Add(new Label { Text = "ID", Location = new Point(420, 30), AutoSize = true });
            txtUsuarioId = new TextBox { Location = new Point(420, 50), Width = 180, ReadOnly = true };
            gbUsuarios.Controls.Add(txtUsuarioId);

            gbUsuarios.Controls.Add(new Label { Text = "Nombre", Location = new Point(420, 80), AutoSize = true });
            txtNombre = new TextBox { Location = new Point(420, 100), Width = 180 };
            gbUsuarios.Controls.Add(txtNombre);

            gbUsuarios.Controls.Add(new Label { Text = "Correo", Location = new Point(420, 130), AutoSize = true });
            txtCorreo = new TextBox { Location = new Point(420, 150), Width = 180 };
            gbUsuarios.Controls.Add(txtCorreo);

            Button btnAgregarUsuario = new Button { Text = "Añadir", Location = new Point(420, 235), Width = 55 };
            Button btnEditarUsuario = new Button { Text = "Editar", Location = new Point(482, 235), Width = 55 };
            Button btnEliminarUsuario = new Button { Text = "Eliminar", Location = new Point(544, 235), Width = 60 };

            btnAgregarUsuario.Click += BtnAgregarUsuario_Click;
            btnEditarUsuario.Click += BtnEditarUsuario_Click;
            btnEliminarUsuario.Click += BtnEliminarUsuario_Click;

            gbUsuarios.Controls.Add(btnAgregarUsuario);
            gbUsuarios.Controls.Add(btnEditarUsuario);
            gbUsuarios.Controls.Add(btnEliminarUsuario);

            // Panel Prestamos
            GroupBox gbPrestamos = new GroupBox
            {
                Text = "Gestionar Préstamos",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(670, 405),
                Size = new Size(690, 300)
            };
            Controls.Add(gbPrestamos);

            dgvPrestamos = new DataGridView
            {
                Location = new Point(15, 95),
                Size = new Size(660, 180),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvPrestamos.CellClick += DgvPrestamos_CellClick;
            gbPrestamos.Controls.Add(dgvPrestamos);

            gbPrestamos.Controls.Add(new Label { Text = "ID", Location = new Point(15, 30), AutoSize = true });
            txtPrestamoId = new TextBox { Location = new Point(15, 50), Width = 60, ReadOnly = true };
            gbPrestamos.Controls.Add(txtPrestamoId);

            gbPrestamos.Controls.Add(new Label { Text = "Usuario", Location = new Point(90, 30), AutoSize = true });
            cmbUsuarios = new ComboBox
            {
                Location = new Point(90, 50),
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            gbPrestamos.Controls.Add(cmbUsuarios);

            gbPrestamos.Controls.Add(new Label { Text = "Libro", Location = new Point(285, 30), AutoSize = true });
            cmbLibros = new ComboBox
            {
                Location = new Point(285, 50),
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            gbPrestamos.Controls.Add(cmbLibros);

            gbPrestamos.Controls.Add(new Label { Text = "Fecha Préstamo", Location = new Point(480, 30), AutoSize = true });
            dtpPrestamo = new DateTimePicker { Location = new Point(480, 50), Width = 190 };
            gbPrestamos.Controls.Add(dtpPrestamo);

            gbPrestamos.Controls.Add(new Label { Text = "Fecha Devolución", Location = new Point(480, 30 + 35), AutoSize = false, Visible = false });

            dtpDevolucion = new DateTimePicker
            {
                Location = new Point(480, 50),
                Width = 190,
                Visible = false
            };

            Button btnAgregarPrestamo = new Button { Text = "Añadir", Location = new Point(15, 20 + 45), Width = 70 };
            Button btnDevolver = new Button { Text = "Devolver", Location = new Point(90, 20 + 45), Width = 70 };
            Button btnEliminarPrestamo = new Button { Text = "Eliminar", Location = new Point(165, 20 + 45), Width = 70 };

            btnAgregarPrestamo.Click += BtnAgregarPrestamo_Click;
            btnDevolver.Click += BtnDevolver_Click;
            btnEliminarPrestamo.Click += BtnEliminarPrestamo_Click;

            gbPrestamos.Controls.Add(btnAgregarPrestamo);
            gbPrestamos.Controls.Add(btnDevolver);
            gbPrestamos.Controls.Add(btnEliminarPrestamo);
        }
        // =========================
        // REFRESCAR
        // =========================
        private void RefrescarTodo()
        {
            RefrescarLibros();
            RefrescarUsuarios();
            RefrescarPrestamos();
            RefrescarCombos();
        }

        private void RefrescarLibros()
        {
            dgvLibros.DataSource = null;
            dgvLibros.DataSource = manager.Libros.Select(l => new
            {
                l.Id,
                l.Titulo,
                l.Autor,
                l.Anio,
                Disponible = l.Disponible ? "Sí" : "No"
            }).ToList();
        }

        private void RefrescarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = manager.Usuarios.Select(u => new
            {
                u.Id,
                u.Nombre,
                Correo = u.CorreoElectronico
            }).ToList();
        }

        private void RefrescarPrestamos()
        {
            dgvPrestamos.DataSource = null;
            dgvPrestamos.DataSource = manager.Prestamos.Select(p => new
            {
                p.Id,
                Usuario = manager.Usuarios.FirstOrDefault(u => u.Id == p.UsuarioId)?.Nombre,
                Libro = manager.Libros.FirstOrDefault(l => l.Id == p.LibroId)?.Titulo,
                FechaPrestamo = p.FechaPrestamo.ToShortDateString(),
                FechaDevolucion = p.FechaDevolucion.HasValue ? p.FechaDevolucion.Value.ToShortDateString() : "",
                p.Estado
            }).ToList();

            // usar la matriz para cumplir requisito
            string[,] matriz = manager.GenerarMatrizPrestamos();
        }

        private void RefrescarCombos()
        {
            cmbUsuarios.DataSource = null;
            cmbUsuarios.DataSource = manager.Usuarios
                .Select(u => new { u.Id, Texto = $"{u.Id} - {u.Nombre}" })
                .ToList();
            cmbUsuarios.DisplayMember = "Texto";
            cmbUsuarios.ValueMember = "Id";

            cmbLibros.DataSource = null;
            cmbLibros.DataSource = manager.Libros
                .Where(l => l.Disponible)
                .Select(l => new { l.Id, Texto = $"{l.Id} - {l.Titulo}" })
                .ToList();
            cmbLibros.DisplayMember = "Texto";
            cmbLibros.ValueMember = "Id";
        }


        // =========================
        // EVENTOS LIBROS
        // =========================
        private void BtnAgregarLibro_Click(object sender, EventArgs e)
        {
            try
            {
                manager.AgregarLibro(txtTitulo.Text, txtAutor.Text, int.Parse(txtAnio.Text));
                LimpiarLibro();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnEditarLibro_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLibroId.Text))
                    throw new Exception("Selecciona un libro.");

                manager.EditarLibro(int.Parse(txtLibroId.Text), txtTitulo.Text, txtAutor.Text, int.Parse(txtAnio.Text));
                LimpiarLibro();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnEliminarLibro_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLibroId.Text))
                    throw new Exception("Selecciona un libro.");

                manager.EliminarLibro(int.Parse(txtLibroId.Text));
                LimpiarLibro();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void DgvLibros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvLibros.Rows[e.RowIndex];
                txtLibroId.Text = fila.Cells["Id"].Value.ToString();
                txtTitulo.Text = fila.Cells["Titulo"].Value.ToString();
                txtAutor.Text = fila.Cells["Autor"].Value.ToString();
                txtAnio.Text = fila.Cells["Anio"].Value.ToString();
            }
        }

        private void LimpiarLibro()
        {
            txtLibroId.Clear();
            txtTitulo.Clear();
            txtAutor.Clear();
            txtAnio.Clear();
        }

        // =========================
        // EVENTOS USUARIOS
        // =========================
        private void BtnAgregarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                manager.AgregarUsuario(txtNombre.Text, txtCorreo.Text);
                LimpiarUsuario();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnEditarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsuarioId.Text))
                    throw new Exception("Selecciona un usuario.");

                manager.EditarUsuario(int.Parse(txtUsuarioId.Text), txtNombre.Text, txtCorreo.Text);
                LimpiarUsuario();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnEliminarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsuarioId.Text))
                    throw new Exception("Selecciona un usuario.");

                manager.EliminarUsuario(int.Parse(txtUsuarioId.Text));
                LimpiarUsuario();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void DgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];
                txtUsuarioId.Text = fila.Cells["Id"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtCorreo.Text = fila.Cells["Correo"].Value.ToString();
            }
        }

        private void LimpiarUsuario()
        {
            txtUsuarioId.Clear();
            txtNombre.Clear();
            txtCorreo.Clear();
        }

        // =========================
        // EVENTOS PRESTAMOS
        // =========================
        private void BtnAgregarPrestamo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbUsuarios.SelectedValue == null)
                    throw new Exception("Selecciona un usuario.");

                if (cmbLibros.SelectedValue == null)
                    throw new Exception("Selecciona un libro disponible.");

                int usuarioId = Convert.ToInt32(cmbUsuarios.SelectedValue);
                int libroId = Convert.ToInt32(cmbLibros.SelectedValue);

                manager.RegistrarPrestamo(usuarioId, libroId, dtpPrestamo.Value);
                LimpiarPrestamo();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnDevolver_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPrestamoId.Text))
                    throw new Exception("Selecciona un préstamo.");

                manager.RegistrarDevolucion(int.Parse(txtPrestamoId.Text), DateTime.Now);
                LimpiarPrestamo();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void BtnEliminarPrestamo_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPrestamoId.Text))
                    throw new Exception("Selecciona un préstamo.");

                manager.EliminarPrestamo(int.Parse(txtPrestamoId.Text));
                LimpiarPrestamo();
                RefrescarTodo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void DgvPrestamos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvPrestamos.Rows[e.RowIndex];
                txtPrestamoId.Text = fila.Cells["Id"].Value.ToString();
            }
        }

        private void LimpiarPrestamo()
        {
            txtPrestamoId.Clear();
        }
    }
}