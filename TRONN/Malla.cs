namespace TRONN
{
    public class Malla
    {
        public Nodo[,] Nodos { get; private set; }
        public int Filas { get; private set; }
        public int Columnas { get; private set; }
        public Nodo PuntoActual { get; private set; }

        public Malla(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
            Nodos = new Nodo[filas, columnas];

            // Inicializar nodos y conectar sus referencias
            for (int i = 0; i < filas; i++)
            {
                for (int j = 0; j < columnas; j++)
                {
                    Nodos[i, j] = new Nodo();

                    if (i > 0)
                        Nodos[i, j].Arriba = Nodos[i - 1, j];
                    if (i < filas - 1)
                        Nodos[i, j].Abajo = Nodos[i + 1, j];
                    if (j > 0)
                        Nodos[i, j].Izquierda = Nodos[i, j - 1];
                    if (j < columnas - 1)
                        Nodos[i, j].Derecha = Nodos[i, j + 1];
                }
            }

            // Colocar el punto negro en el primer nodo (centro del primer cuadro)
            PuntoActual = Nodos[0, 0];
            PuntoActual.TienePuntoNegro = true;
        }

        public void MoverPunto(int deltaX, int deltaY)
        {
            int nuevaFila = Array.IndexOf(Nodos, PuntoActual) + deltaX;
            int nuevaColumna = Array.IndexOf(Nodos, PuntoActual) + deltaY;

            if (nuevaFila >= 0 && nuevaFila < Filas && nuevaColumna >= 0 && nuevaColumna < Columnas)
            {
                PuntoActual.TienePuntoNegro = false;
                PuntoActual = Nodos[nuevaFila, nuevaColumna];
                PuntoActual.TienePuntoNegro = true;
            }
        }
    }
}