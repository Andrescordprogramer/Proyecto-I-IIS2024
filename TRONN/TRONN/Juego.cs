using Timer = System.Windows.Forms.Timer;

namespace TRONN
{
    // Clase Juego: Controla el estado general del juego
    public class Juego
    {
        public List<Moto> Motos { get; private set; }
        private List<Item> ItemsEnMapa;
        private Timer movimientoTimer;
        private Random random = new Random();
        private Form1 form;

        public Juego(Form1 form)
        {
            this.form = form;
            Motos = new List<Moto>();
            ItemsEnMapa = new List<Item>();
            InicializarMotos();
            GenerarItems();

            movimientoTimer = new Timer { Interval = 100 };
            movimientoTimer.Tick += MovimientoTimer_Tick;
            movimientoTimer.Start();
        }

        private void InicializarMotos()
        {
            // Moto del jugador
            Motos.Add(new Moto("Jugador", 25, 25, 50, 50));

            // Motos de los bots
            for (int i = 0; i < 4; i++)
            {
                Motos.Add(new Moto($"Bot{i + 1}", random.Next(0, 50), random.Next(0, 50), 50, 50));
            }
        }

        private void GenerarItems()
        {
            // Generar celdas de combustible
            for (int i = 0; i < 5; i++)
            {
                ItemsEnMapa.Add(new CeldaCombustible(random.Next(0, 50), random.Next(0, 50), random.Next(10, 30)));
            }
            // Generar ítems de crecimiento de estela
            for (int i = 0; i < 5; i++)
            {
                ItemsEnMapa.Add(new CrecimientoEstela(random.Next(0, 50), random.Next(0, 50), random.Next(1, 10)));
            }
            // Generar bombas
            for (int i = 0; i < 3; i++)
            {
                ItemsEnMapa.Add(new Bomba(random.Next(0, 50), random.Next(0, 50)));
            }
        }

        private void MovimientoTimer_Tick(object sender, EventArgs e)
        {
            foreach (var moto in Motos)
            {
                if (moto.Nombre.StartsWith("Bot"))
                {
                    moto.CambiarDireccion((Direccion)random.Next(0, 4));
                }
                moto.Mover();

                // Detectar colisión con ítems
                DetectarColisionConItems(moto);

                // Verificar colisión con otras motos o estelas
                if (moto.Combustible <= 0 || DetectarColision(moto))
                {
                    // Lógica de fin de juego o respawn
                    Motos.Remove(moto);
                    break; // Salir del bucle para evitar problemas de enumeración modificada
                }
            }
            form.Invalidate(); // Redibujar el formulario
        }

        private void DetectarColisionConItems(Moto moto)
        {
            for (int i = ItemsEnMapa.Count - 1; i >= 0; i--)
            {
                if (moto.Cabeza.X == ItemsEnMapa[i].Posicion.X && moto.Cabeza.Y == ItemsEnMapa[i].Posicion.Y)
                {
                    moto.RecogerItem(ItemsEnMapa[i]);
                    ItemsEnMapa.RemoveAt(i);
                }
            }
        }

        private bool DetectarColision(Moto moto)
        {
            // Verificar colisión con la propia estela, ignorando la cabeza
            if (moto.Estela.Contiene(moto.Cabeza.X, moto.Cabeza.Y, ignorarCabeza: true))
            {
                return true;
            }

            // Verificar colisión con la estela de otras motos
            foreach (var otraMoto in Motos)
            {
                if (otraMoto != moto && otraMoto.Estela.Contiene(moto.Cabeza.X, moto.Cabeza.Y))
                {
                    return true;
                }
            }

            // Verificar colisión directa con otras motos
            foreach (var otraMoto in Motos)
            {
                if (otraMoto != moto && otraMoto.Cabeza.X == moto.Cabeza.X && otraMoto.Cabeza.Y == moto.Cabeza.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public void DibujarMotos(Graphics g)
        {
            foreach (var moto in Motos)
            {
                Brush brush = moto.Nombre == "Jugador" ? Brushes.Blue : Brushes.Orange;
                foreach (var nodo in moto.Estela.ObtenerNodos())
                {
                    g.FillRectangle(brush, nodo.X * 10, nodo.Y * 10, 10, 10);
                }
            }
        }

        public void CambiarDireccionJugador(Direccion nuevaDireccion)
        {
            Motos[0].CambiarDireccion(nuevaDireccion);
        }

        public void DibujarItems(Graphics g)
        {
            foreach (var item in ItemsEnMapa)
            {
                Brush brush = item is CeldaCombustible ? Brushes.Green :
                              item is CrecimientoEstela ? Brushes.Yellow :
                              Brushes.Red; // Bomba

                g.FillRectangle(brush, item.Posicion.X * 10, item.Posicion.Y * 10, 10, 10);
            }
        }
    }

}
