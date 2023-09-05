using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Sintaxis_2
{
    public class Variable
    {
        public enum TiposDatos{Char, Int, Float}

        /*
        char = 1 Byte = 0 -> 255
        int = 2 Bytes = 0 -> 65535
        float = 4 Bytes = 0 -> 4294967295
        */

        private String nombre; 
        private float valor;
        private TiposDatos tipo;

        public Variable(String nombre, TiposDatos tipo){
            this.nombre = nombre;
            this.tipo = tipo;
            this.valor = 0;
        }

        public String getNombre()
        {
            return nombre;
        }
        public TiposDatos getTiposDatos()
        {
            return tipo;
        }
        public void setValor(float valor){
            this.valor = valor;
        }
        public float getValor()
        {
            return valor;
        }
    }
}