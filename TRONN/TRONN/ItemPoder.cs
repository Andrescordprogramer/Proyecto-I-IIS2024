using System;
using Timer = System.Windows.Forms.Timer;

namespace TRONN
{

    // Clase abstracta Item: Base para todos los ítems
    public abstract class Item
    {
        public Point Posicion { get; private set; }

        public Item(int x, int y)
        {
            Posicion = new Point(x, y);
        }

        public abstract void Aplicar(Moto moto);
    }

    // Clase CeldaCombustible: Incrementa el combustible de la moto
    public class CeldaCombustible : Item
    {
        private int cantidad;

        public CeldaCombustible(int x, int y, int cantidad) : base(x, y)
        {
            this.cantidad = cantidad;
        }

        public override void Aplicar(Moto moto)
        {
            moto.Combustible = Math.Min(100, moto.Combustible + cantidad);
        }
    }

    // Clase CrecimientoEstela: Incrementa el tamaño de la estela
    public class CrecimientoEstela : Item
    {
        private int incremento;

        public CrecimientoEstela(int x, int y, int incremento) : base(x, y)
        {
            this.incremento = incremento;
        }

        public override void Aplicar(Moto moto)
        {
            moto.Estela.IncrementarTamano(incremento);
        }
    }

    // Clase Bomba: Representa una bomba que destruye la moto
    public class Bomba : Item
    {
        public Bomba(int x, int y) : base(x, y) { }

        public override void Aplicar(Moto moto)
        {
            moto.Combustible = 0; // Destruye la moto
        }
    }

    // Clase abstracta Poder: Base para todos los poderes
    public abstract class Poder
    {
        public Point Posicion { get; private set; }

        public Poder(int x, int y)
        {
            Posicion = new Point(x, y);
        }

        public abstract void Aplicar(Moto moto);
    }

    // Clase Escudo: Hace la moto invulnerable
    public class Escudo : Poder
    {
        private int duracion;

        public Escudo(int x, int y, int duracion) : base(x, y)
        {
            this.duracion = duracion;
        }

        public override void Aplicar(Moto moto)
        {
            moto.HacerInvulnerable(duracion);
        }
    }

    // Clase HiperVelocidad: Incrementa temporalmente la velocidad de la moto
    public class HiperVelocidad : Poder
    {
        private int incrementoVelocidad;

        public HiperVelocidad(int x, int y, int incrementoVelocidad) : base(x, y)
        {
            this.incrementoVelocidad = incrementoVelocidad;
        }

        public override void Aplicar(Moto moto)
        {
            moto.Velocidad += incrementoVelocidad;
            Timer velocidadTimer = new Timer { Interval = 5000 };
            velocidadTimer.Tick += (s, e) =>
            {
                moto.Velocidad -= incrementoVelocidad;
                velocidadTimer.Stop();
            };
            velocidadTimer.Start();
        }
    }
}
