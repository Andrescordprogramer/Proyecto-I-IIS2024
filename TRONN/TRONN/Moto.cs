using Timer = System.Windows.Forms.Timer;

namespace TRONN
{
    // Clase Moto: Representa a las motos de luz
    public class Moto
    {
        public NodoMoto Cabeza { get; private set; }
        public Estela Estela { get; private set; }
        public int Combustible { get; set; }
        public int Velocidad { get; set; }
        public string Nombre { get; private set; }

        private Direccion direccionActual;
        private int limiteX;
        private int limiteY;
        private Queue<Item> Items;
        private Stack<Poder> Poderes;
        private bool EsInvulnerable;

        public Moto(string nombre, int xInicial, int yInicial, int limiteX, int limiteY)
        {
            Cabeza = new NodoMoto(xInicial, yInicial);
            Estela = new Estela(xInicial, yInicial, 3);
            Combustible = 100;
            Velocidad = new Random().Next(1, 11);
            Nombre = nombre;
            direccionActual = Direccion.Derecha;
            this.limiteX = limiteX;
            this.limiteY = limiteY;
            Items = new Queue<Item>();
            Poderes = new Stack<Poder>();
            EsInvulnerable = false;
        }

        public void Mover()
        {
            if (Combustible > 0)
            {
                int nuevaX = Cabeza.X, nuevaY = Cabeza.Y;
                switch (direccionActual)
                {
                    case Direccion.Arriba: nuevaY--; break;
                    case Direccion.Abajo: nuevaY++; break;
                    case Direccion.Izquierda: nuevaX--; break;
                    case Direccion.Derecha: nuevaX++; break;
                }
                if (nuevaX < 0) nuevaX = limiteX - 1;
                else if (nuevaX >= limiteX) nuevaX = 0;
                if (nuevaY < 0) nuevaY = limiteY - 1;
                else if (nuevaY >= limiteY) nuevaY = 0;

                NodoMoto nuevoNodo = new NodoMoto(nuevaX, nuevaY);
                nuevoNodo.Siguiente = Cabeza;
                Cabeza = nuevoNodo;
                Estela.Mover(nuevaX, nuevaY);

                if (Velocidad % 5 == 0) Combustible--;

                AplicarItems();
            }
        }

        private void AplicarItems()
        {
            // Aplicar ítems automáticamente
            if (Items.Count > 0)
            {
                var item = Items.Dequeue();
                item.Aplicar(this);
            }
        }

        public void CambiarDireccion(Direccion nuevaDireccion)
        {
            direccionActual = nuevaDireccion;
        }

        public void RecogerItem(Item item)
        {
            Items.Enqueue(item);
        }

        public void RecogerPoder(Poder poder)
        {
            Poderes.Push(poder);
        }

        public void UsarPoder()
        {
            if (Poderes.Count > 0)
            {
                Poder poder = Poderes.Pop();
                poder.Aplicar(this);
            }
        }

        public void HacerInvulnerable(int duracion)
        {
            EsInvulnerable = true;
            Timer invulnerabilidadTimer = new Timer { Interval = duracion };
            invulnerabilidadTimer.Tick += (s, e) =>
            {
                EsInvulnerable = false;
                invulnerabilidadTimer.Stop();
            };
            invulnerabilidadTimer.Start();
        }

        public bool EsMotoInvulnerable()
        {
            return EsInvulnerable;
        }
    }


    // Nodo de la moto
    public class NodoMoto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public NodoMoto Siguiente { get; set; }

        public NodoMoto(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
