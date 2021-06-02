using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arbol_Binario_Steven_Rocha
{
    public partial class Form1 : Form
    {
        //Variables para trabajar.
        public int cantClientes { get; set; }

        int axu,
            posX,
            posY,
            i = 0;

        public double iva = 15;

        //Variable de tipo arbol la cual posteriormente instanciaremos. Y variable de tipo Graphics la cual nos servira para dibujar el arbol
        Arbol arbol;
        Graphics nodo;

        //Arreglo de registro para almacenar valores
        public struct datos
        {
            public string cliente { get; set; }
            public double precio { get; set; }
            public int dias { get; set; }
            public double total { get; set; }
        }

        //Arreglo de registro unidimensional
        datos[] dato;

        //Constructor de la clase form
        public Form1()
        {
            InitializeComponent();
            nodo = CreateGraphics();
            arbol = new Arbol(nodo, Font);
            gbEliminar.Enabled = false;
            gbRecorridos.Enabled = false;
            gbDatos.Enabled = false;
        }

        #region Metodo Cantidad
        public void Cantidad()
        {
            try
            {
                cantClientes = Convert.ToInt32(txtCantClientes.Text);
                axu = cantClientes;

                if (cantClientes < 0 || txtCantClientes.Text.Length > 8)
                {
                    MessageBox.Show("Cantidad ingresada no valida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    dato = new datos[cantClientes];
                    MessageBox.Show($"Se podran registrar un total de {cantClientes} clientes", ":)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    gbDatos.Enabled = true;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Cantidad ingresada no valida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }
        #endregion

        private void btnCantClientes_Click(object sender, EventArgs e)
        {
            Cantidad();
        }

        #region Metodo para insertar nodos/datos
        public void Insertar()
        {
            if (i == cantClientes)
            {
                MessageBox.Show("Ya no puede ingresar mas elementos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    dato[i].cliente = txtCliente.Text;
                    dato[i].precio = double.Parse(txtPrecio.Text);
                    dato[i].dias = Convert.ToInt32(txtNumDias.Text);
                    dato[i].total = (((dato[i].precio * dato[i].dias) * iva) / 100) + (dato[i].precio * dato[i].dias);

                    if (!arbol.InsertarDatos(dato[i].total))
                    {
                        MessageBox.Show("No se pueden ingresar valores duplicados.\nIngrese su precio nuevamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPrecio.Clear();
                        txtPrecio.Focus();
                    }
                    else
                    {
                        dataGridView1.Rows.Add(dato[i].cliente, dato[i].total);
                        Refresh();
                        LimpiarCampos();
                        txtCliente.Focus();
                        i++;
                    }

                    if (i > 0)
                    {
                        gbRecorridos.Enabled = true;
                    }
                    if (i == cantClientes)
                    {
                        gbEliminar.Enabled = true;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Datos erroneos.\nRevisar los datos previamente ingresados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Limpiar los campos
        public void LimpiarCampos()
        {
            txtCliente.Clear();
            txtNumDias.Clear();
            txtPrecio.Clear();
            txtParqueo.Clear();
        }
        #endregion

        private void btnDatos_Click(object sender, EventArgs e)
        {
            Insertar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            arbol.InOrden(lstRecorridos, lblRecorridos);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            arbol.MostrarArbol(e, this.BackColor);
        }

        private void btnPosOrden_Click(object sender, EventArgs e)
        {
            arbol.PosOrden(lstRecorridos, lblRecorridos);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPreOrden_Click(object sender, EventArgs e)
        {
            arbol.PreOrden(lstRecorridos, lblRecorridos);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            double x = double.Parse(textBox1.Text);

            if (arbol.Eliminar(x))
            {
                Refresh();
                Eliminar(x);
            }
            else return;
        }


        #region Eliminar del datagridview
        public void Eliminar(double x)
        {
            for (int i = 0; i < dato.Length; i++)
            {
                if (dato[i].total == x)
                {
                    for (int j = i; j < dato.Length - 1; j++)
                    {
                        dato[j].total = dato[j + 1].total;
                        dato[j].cliente = dato[j + 1].cliente;
                        dato[j].dias = dato[j + 1].dias;

                        if (j == dato.Length)
                        {
                            dato[j].total = Convert.ToDouble(""); ;
                            dato[j].cliente = null;
                            dato[j].dias = Convert.ToInt32(null);
                        }
                    }
                }
            }

            axu--;
            dataGridView1.Rows.Clear();

            for (int i = 0; i < axu; i++)
            {
                dataGridView1.Rows.Add(dato[i].cliente, dato[i].total);
            }
        }
        #endregion

        private void barraNavegacion_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                posX = e.X;
                posY = e.Y;
            }
            else
            {
                Left += (e.X - posX);
                Top += (e.Y - posY);
            }
        }
    }
}
