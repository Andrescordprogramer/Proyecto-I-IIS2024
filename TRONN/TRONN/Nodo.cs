namespace TRONN
{
    public class Nodo
    {
        public Nodo Arriba { get; set; }
        public Nodo Abajo { get; set; }
        public Nodo Izquierda { get; set; }
        public Nodo Derecha { get; set; }
        public bool EsEstela { get; set; } // Marcar si es parte de la estela destructiva
        public bool TieneMoto { get; set; } // Para determinar si tiene una moto en esta celda

        // Constructor
        public Nodo()
        {
            Arriba = null;
            Abajo = null;
            Izquierda = null;
            Derecha = null;
            EsEstela = false;
            TieneMoto = false;
        }
    }


    /*
    // Clase NodoMoto: Nodo de la lista enlazada que forma la moto
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

    // Clase NodoEstela: Nodo de la lista enlazada que forma la estela
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
    }*/

}