namespace TRONN
{
    // Clase Estela: Representa la estela de la moto
    public class Estela
    {
        public NodoEstela cabeza;
        private int tamanoMaximo;

        public Estela(int xInicial, int yInicial, int tamanoMaximo = 3)
        {
            this.tamanoMaximo = tamanoMaximo;
            cabeza = new NodoEstela(xInicial, yInicial);
            NodoEstela actual = cabeza;
            for (int i = 1; i < tamanoMaximo; i++)
            {
                actual.Siguiente = new NodoEstela(xInicial, yInicial);
                actual = actual.Siguiente;
            }
        }

        public void Mover(int nuevaX, int nuevaY)
        {
            NodoEstela nuevoNodo = new NodoEstela(nuevaX, nuevaY);
            nuevoNodo.Siguiente = cabeza;
            cabeza = nuevoNodo;

            NodoEstela actual = cabeza;
            int contador = 1;
            while (contador < tamanoMaximo && actual.Siguiente != null)
            {
                actual = actual.Siguiente;
                contador++;
            }
            if (actual.Siguiente != null)
            {
                actual.Siguiente = null;
            }
        }

        public IEnumerable<NodoEstela> ObtenerNodos()
        {
            NodoEstela actual = cabeza;
            while (actual != null)
            {
                yield return actual;
                actual = actual.Siguiente;
            }
        }

        public bool Contiene(int x, int y, bool ignorarCabeza = false)
        {
            NodoEstela actual = cabeza;
            if (ignorarCabeza) actual = actual.Siguiente; // Omitir la cabeza de la estela

            while (actual != null)
            {
                if (actual.X == x && actual.Y == y)
                    return true;
                actual = actual.Siguiente;
            }
            return false;
        }

        public void IncrementarTamano(int incremento)
        {
            tamanoMaximo += incremento;
        }
    }

    // Nodo de la estela
    public class NodoEstela
    {
        public int X { get; set; }
        public int Y { get; set; }
        public NodoEstela Siguiente { get; set; }

        public NodoEstela(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
