namespace TRON
{
    public partial class Form1 : Form
    {
        private Malla malla;
        private Moto moto;
        private Image motoImg;
        private Image itemImg;
        public List<string> motoImgs;

        public Form1()
        {
            InitializeComponent();
            malla = new Malla(18, 32);
            moto = new Moto(malla, 0, 0); // Iniciamos la moto en la posición (0, 0)
            motoImg = Image.FromFile("moto_abajo.png"); // Cargar la imagen de la moto
            itemImg = Image.FromFile("caja.png"); // Cargar la imagen del item(en caja)

            //Generar 10 items en la malla
            malla.GenerarItemsAleatorios(10, itemImg);

            motoImgs = new List<string>()
            {
                "moto_arriba.png",
                "moto_abajo.png",
                "moto_izquierda.png",
                "moto_derecha.png"
            };


            // Configurar el evento KeyDown para capturar las teclas
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.DoubleBuffered = true; // Para evitar parpadeos al redibujar
        }

        // Evento que maneja el teclado
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    moto.Mover(-1, 0, "arriba");
                    motoImg = Image.FromFile(motoImgs[0]);
                    break;
                case Keys.Down:
                    moto.Mover(1, 0, "abajo");
                    motoImg = Image.FromFile(motoImgs[1]);
                    break;
                case Keys.Left:
                    moto.Mover(0, -1, "izquierda");
                    motoImg = Image.FromFile(motoImgs[2]);
                    break;
                case Keys.Right:
                    moto.Mover(0, 1, "derecha"); 
                    motoImg = Image.FromFile(motoImgs[3]);
                    break;
            }

            // Redibujar la malla y la moto
            this.Invalidate();
        }

        // Método para dibujar la malla, la moto y los ítems
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int tamañoCelda = 40; // Tamaño de cada celda

            // Dibujar la moto y su estela
            moto.DibujarMotoYEstela(e.Graphics, motoImg, tamañoCelda);

            // Dibujar los ítems
            malla.DibujarItems(e.Graphics, tamañoCelda);

            // Mostrar el estado del combustible y otros atributos
            e.Graphics.DrawString($"Combustible: {moto.Combustible}", new Font("Arial", 14), Brushes.White, new PointF(10, 10));
            e.Graphics.DrawString($"Velocidad: {moto.Velocidad}", new Font("Arial", 14), Brushes.White, new PointF(10, 40));
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            // Código que quieres ejecutar al cargar el formulario
            
        }

    }

}