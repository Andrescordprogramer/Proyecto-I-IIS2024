using Timer = System.Windows.Forms.Timer;

namespace TRONN
{
    // Clase Form1: Interfaz gráfica principal
    public partial class Form1 : Form
    {
        private Juego juego;
        private Panel barraCombustible;

        public Form1()
        {
            InitializeComponent();
            juego = new Juego(this);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            // Inicializar barra de combustible
            barraCombustible = new Panel();
            barraCombustible.BackColor = Color.Green;
            barraCombustible.Size = new Size(200, 20); // Tamaño inicial
            barraCombustible.Location = new Point(10, 10);
            this.Controls.Add(barraCombustible);

            Timer combustibleTimer = new Timer { Interval = 100 };
            combustibleTimer.Tick += ActualizarBarraCombustible;
            combustibleTimer.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                juego.CambiarDireccionJugador(Direccion.Arriba);
            }
            else if (e.KeyCode == Keys.Down)
            {
                juego.CambiarDireccionJugador(Direccion.Abajo);
            }
            else if (e.KeyCode == Keys.Left)
            {
                juego.CambiarDireccionJugador(Direccion.Izquierda);
            }
            else if (e.KeyCode == Keys.Right)
            {
                juego.CambiarDireccionJugador(Direccion.Derecha);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            // Dibujar motos
            juego.DibujarMotos(g);

            // Dibujar ítems
            juego.DibujarItems(g);
        }

        private void ActualizarBarraCombustible(object sender, EventArgs e)
        {
            int combustible = juego.Motos[0].Combustible;
            barraCombustible.Width = (int)(200 * (combustible / 100.0));
            barraCombustible.BackColor = combustible > 20 ? Color.Green : Color.Red;
        }
    }

}