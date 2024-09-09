using static TRON.Program;

namespace TRON
{
    partial class Form1
    {

        public class Nodo
        {
            public int Valor { get; set; }
            public Nodo Arriba { get; set; }
            public Nodo Abajo { get; set; }
            public Nodo Izquierda { get; set; }
            public Nodo Derecha { get; set; }

            public Nodo(int valor)
            {
                this.Valor = valor;
                this.Arriba = null;
                this.Abajo = null;
                this.Izquierda = null;
                this.Derecha = null;
            }
        }



        public class Malla
        {
            private Nodo[,] grid;
            public List<Item> Items { get; set; }
            public int filas { get; set; }
            public int columnas { get; set; }

            public Malla(int Filas, int Columnas)
            {
                this.filas = Filas;
                this.columnas = Columnas;
                grid = new Nodo[Filas, Columnas];
                Items = new List<Item>();
                InicializarMalla();
            }

            public void MostrarMoto(Graphics g, int fila, int columna, Image motoImg, string direccion)
            {
                int tamañoCelda = 40; // Definimos un tamaño para cada celda de la malla

                if (direccion == "derecha" || direccion == "izquierda")
                    g.DrawImage(motoImg, columna * tamañoCelda, fila * tamañoCelda, tamañoCelda, tamañoCelda-10);
                else
                    g.DrawImage(motoImg, columna * tamañoCelda, fila * tamañoCelda, tamañoCelda-10, tamañoCelda);
            }

            // Método para inicializar la malla
            private void InicializarMalla()
            {
                for (int i = 0; i < filas; i++)
                {
                    for (int j = 0; j < columnas; j++)
                    {
                        grid[i, j] = new Nodo(0); // Inicializamos cada nodo con valor 0

                        // Conectar los nodos entre sí (arriba, abajo, izquierda, derecha)
                        if (i > 0)
                        {
                            grid[i, j].Arriba = grid[i - 1, j];
                            grid[i - 1, j].Abajo = grid[i, j];
                        }
                        if (j > 0)
                        {
                            grid[i, j].Izquierda = grid[i, j - 1];
                            grid[i, j - 1].Derecha = grid[i, j];
                        }
                    }
                }
            }

            // Método para generar ítems aleatorios al iniciar el juego
            public void GenerarItemsAleatorios(int cantidadItems, Image imagenItem)
            {
                Random rnd = new Random();
                for (int i = 0; i < cantidadItems; i++)
                {
                    int fila = rnd.Next(0, filas);
                    int columna = rnd.Next(0, columnas);

                    // Elegir un tipo de ítem aleatorio
                    string tipoItem = rnd.Next(0, 3) switch
                    {
                        0 => "Combustible",
                        1 => "Crecimiento",
                        _ => "Bomba"
                    };

                    // Crear el ítem y agregarlo a la lista
                    Items.Add(new Item(fila, columna, tipoItem, imagenItem));
                }
            }

            // Método para dibujar los ítems en la malla
            public void DibujarItems(Graphics g, int tamañoCelda)
            {
                foreach (var item in Items)
                {
                    item.DibujarItem(g, tamañoCelda);
                }
            }

            // Método para insertar un valor en un nodo específico
            public void Insertar(int fila, int columna, int valor)
            {
                if (fila >= 0 && fila < filas && columna >= 0 && columna < columnas)
                {
                    grid[fila, columna].Valor = valor;
                }
                else
                {
                    Console.WriteLine("Coordenadas fuera de rango");
                }
            }

            // Método para eliminar el valor de un nodo (lo deja en 0)
            public void Eliminar(int fila, int columna)
            {
                if (fila >= 0 && fila < filas && columna >= 0 && columna < columnas)
                {
                    grid[fila, columna].Valor = 0;
                }
                else
                {
                    Console.WriteLine("Coordenadas fuera de rango");
                }
            }

            // Método para mostrar la malla
            public void Mostrar()
            {
                for (int i = 0; i < filas; i++)
                {
                    for (int j = 0; j < columnas; j++)
                    {
                        Console.Write(grid[i, j].Valor + " ");
                    }
                    Console.WriteLine();
                }
            }
            
            // Método para mostrar la estela
            public void MostrarEstela(Graphics g, int fila, int columna, int tamañoCelda)
            {
                Brush brochaEstela = Brushes.Blue; // Puedes cambiar el color de la estela
                g.FillRectangle(brochaEstela, columna * tamañoCelda, fila * tamañoCelda, tamañoCelda, tamañoCelda);
            }
        }



        public class Moto
        {
            private Queue<string> items;
            private Stack<string> poderes;
            private List<Estela> estela;
            private Malla malla;
            public int Fila { get; set; }
            public int Columna { get; set; }
            public string Direccion { get; set; }
            public int Velocidad { get; set; }
            public int TamañoEstela { get; set; }
            public int Combustible { get; set; }
            public bool MotoDestruida { get; private set; }



            private int recorrido; // Para controlar el consumo de combustible

            public Moto(Malla malla, int filaInicial, int columnaInicial)
            {
                this.malla = malla;
                this.Fila = filaInicial;
                this.Columna = columnaInicial;
                this.Direccion = "abajo";

                // Inicializar los atributos
                Random rnd = new Random();
                this.Velocidad = rnd.Next(1, 11); // Velocidad aleatoria entre 1 y 10
                this.TamañoEstela = 3; // Tamaño inicial de la estela
                this.Combustible = 100; // Combustible inicial 100
                this.items = new Queue<string>();
                this.poderes = new Stack<string>();
                this.recorrido = 0;

                // Inicializar la estela detrás de la moto
                estela = new List<Estela>();
                for (int i = 0; i < TamañoEstela; i++)
                {
                    estela.Add(new Estela(malla, Fila - i, Columna));
                }
            }

            // Método para mover la moto y actualizar estela, combustible y velocidad
            public void Mover(int deltaFila, int deltaColumna, string nuevaDireccion)
            {
                if (Combustible > 0)
                {
                    int nuevaFila = Fila + deltaFila;
                    int nuevaColumna = Columna + deltaColumna;

                    // Verificar si la nueva posición está dentro de la malla
                    if (nuevaFila >= 0 && nuevaFila < malla.filas && nuevaColumna >= 0 && nuevaColumna < malla.columnas)
                    {
                        // Mover la estela
                        for (int i = estela.Count - 1; i > 0; i--)
                        {
                            estela[i].Fila = estela[i - 1].Fila;
                            estela[i].Columna = estela[i - 1].Columna;
                        }
                        estela[0].Fila = Fila;
                        estela[0].Columna = Columna;

                        // Actualizar la posición de la moto
                        Fila = nuevaFila;
                        Columna = nuevaColumna;
                        Direccion = nuevaDireccion;

                        // Actualizar combustible
                        recorrido++;
                        if (recorrido >= 5) // Consumir combustible cada 5 celdas recorridas
                        {
                            Combustible -= Velocidad;
                            recorrido = 0; // Reiniciar el contador de celdas recorridas
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Moto sin combustible");
                }
            }

            // Método para usar un ítem
            public void UsarItem(string item)
            {
                items.Enqueue(item);
                switch (item)
                {
                    case "AumentarEstela":
                        TamañoEstela++;
                        estela.Add(new Estela(malla, Fila, Columna)); // Añadir un nuevo segmento a la estela
                        break;
                    case "AumentarCombustible":
                        Combustible = Math.Min(Combustible + 20, 100); // Aumenta el combustible sin pasar de 100
                        break;
                        // Agrega otros casos según los ítems disponibles
                }
            }

            // Método para activar un poder temporal
            public void ActivarPoder(string poder)
            {
                poderes.Push(poder);
                switch (poder)
                {
                    case "SuperVelocidad":
                        Velocidad += 5; // Aumenta temporalmente la velocidad
                        break;
                        // Agrega otros poderes según sea necesario
                }
            }
            public void Destruir()
            {
                // Liberar los ítems y poderes en la malla de forma aleatoria
                Random rnd = new Random();
                foreach (var item in items)
                {
                    int fila = rnd.Next(0, malla.filas);
                    int columna = rnd.Next(0, malla.columnas);
                    malla.Items.Add(new Item(fila, columna, item, Image.FromFile("caja.png"))); // Colocar el ítem en una posición aleatoria
                }

                foreach (var poder in poderes)
                {
                    int fila = rnd.Next(0, malla.filas);
                    int columna = rnd.Next(0, malla.columnas);
                    malla.Items.Add(new Item(fila, columna, poder, Image.FromFile("caja.png"))); // Colocar el poder en una posición aleatoria
                }

                // Agregar lógica para manejar la destrucción de la moto
            }


            // Método para dibujar la moto y la estela
            public void DibujarMotoYEstela(Graphics g, Image motoImg, int tamañoCelda)
            {
                // Dibujar la moto
                malla.MostrarMoto(g, Fila, Columna, motoImg, Direccion);

                // Dibujar la estela
                foreach (var segmento in estela)
                {
                    malla.MostrarEstela(g, segmento.Fila, segmento.Columna, tamañoCelda);
                }

            }

        }

        public class Item
        {
            public int Fila { get; set; }
            public int Columna { get; set; }
            public string Tipo { get; set; } // Tipo de ítem (e.g., "Combustible", "Crecimiento", "Bomba")
            public Image Imagen { get; set; }

            public Item(int fila, int columna, string tipo, Image imagen)
            {
                Fila = fila;
                Columna = columna;
                Tipo = tipo;
                Imagen = imagen;
            }

            // Método para dibujar el ítem
            public void DibujarItem(Graphics g, int tamañoCelda)
            {
                g.DrawImage(Imagen, Columna * tamañoCelda, Fila * tamañoCelda, tamañoCelda, tamañoCelda);
            }
        }

        public class Estela
        {
            public int Fila { get; set; }
            public int Columna { get; set; }

            public Estela(Malla malla, int fila, int columna)
            {
                this.Fila = fila;
                this.Columna = columna;
            }
        }



        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 686);
            label1.Name = "label1";
            label1.Size = new Size(63, 25);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 800);
            Controls.Add(label1);
            Name = "Form1";
            Text = "TRON";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
    }
}
