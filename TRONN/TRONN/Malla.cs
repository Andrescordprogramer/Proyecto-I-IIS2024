namespace TRONN
{
    public class Malla
    {
        public Nodo[,] grid;
        private int tamaño;

        public Malla(int tamaño)
        {
            this.tamaño = tamaño;
            grid = new Nodo[tamaño, tamaño];
            CrearMalla();
        }

        private void CrearMalla()
        {
            // Crear nodos
            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    grid[i, j] = new Nodo();
                }
            }

            // Conectar nodos
            for (int i = 0; i < tamaño; i++)
            {
                for (int j = 0; j < tamaño; j++)
                {
                    if (i > 0) grid[i, j].Arriba = grid[i - 1, j];
                    if (i < tamaño - 1) grid[i, j].Abajo = grid[i + 1, j];
                    if (j > 0) grid[i, j].Izquierda = grid[i, j - 1];
                    if (j < tamaño - 1) grid[i, j].Derecha = grid[i, j + 1];
                }
            }
        }
    }

}