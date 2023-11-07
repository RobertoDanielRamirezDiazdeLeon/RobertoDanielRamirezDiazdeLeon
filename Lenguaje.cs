using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

/*
    Requerimiento 1: Programar scanf                                                        LISTO
    Requerimiento 2: Programar printf                                                       LISTO
    Requerimiento 3: Programar ++,--,+=,-=,*=,/=,%=                                         LISTO
    Requerimiento 4: Programar else                                                         LISTO
    Requerimiento 5: Programar do para que gerenre una sola vez el codigo                   LISTO
    Requerimiento 6: Programar while para que gerenre una sola vez el codigo                LISTO
    Requerimiento 7: Programar el for para que gerenre una sola vez el codigo               LISTO
    Requerimiento 8: Programar el CAST                                                      LISTO
*/

namespace Sintaxis_2
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> lista;
        Stack<float> stack;
        int contIf,contFor,contWhile,contElse,contDo;

        Variable.TiposDatos tipoDatoExpresion;
        public Lenguaje()
        {
            lista = new List<Variable>();
            stack = new Stack<float>();
            tipoDatoExpresion = Variable.TiposDatos.Char;
            contIf = contFor = contWhile = contDo = 1;
            contElse = 0;
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            lista = new List<Variable>();
            stack = new Stack<float>();
            tipoDatoExpresion = Variable.TiposDatos.Char;
            contIf = contFor = contWhile = contDo = 1;
            contElse = 0;
        }

        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            asm.WriteLine("include 'emu8086.inc'");
            asm.WriteLine("org 100h");
            if (getContenido() == "#")
            {
                Librerias();
            }
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
            Main(true);
            asm.WriteLine("int 20h");
            asm.WriteLine("RET");
            asm.WriteLine("define_scan_num");
            asm.WriteLine("define_clear_screen");
            asm.WriteLine("define_print_num");
            asm.WriteLine("define_print_num_uns");
            Imprime();
            asm.WriteLine("END");
        }

        private void Imprime()
        {
            log.WriteLine("-----------------");
            log.WriteLine("V a r i a b l e s");
            log.WriteLine("-----------------");
            asm.WriteLine("; V a r i a b l e s");
            foreach (Variable v in lista)
            {
                log.WriteLine(v.getNombre() + " " + v.getTipoDato() + " = " + v.getValor());
                asm.WriteLine(v.getNombre() + " dw 0h " );
            }
            log.WriteLine("-----------------");
        }

        private bool Existe(string nombre)
        {
            foreach (Variable v in lista)
            {
                if (v.getNombre() == nombre)
                {
                    return true;
                }
            }
            return false;
        }
        private void Modifica(string nombre, float nuevoValor)
        {
            foreach (Variable v in lista)
            {
                if (v.getNombre() == nombre)
                {
                    v.setValor(nuevoValor);
                }
            }
        }
        private float getValor(string nombre)
        {
            foreach (Variable v in lista)
            {
                if (v.getNombre() == nombre)
                {
                    return v.getValor();
                }
            }
            return 0;
        }
        private Variable.TiposDatos getTipo(string nombre)
        {
            foreach (Variable v in lista)
            {
                if (v.getNombre() == nombre)
                {
                    return v.getTipoDato();
                }
            }
            return Variable.TiposDatos.Char;
        }
        private Variable.TiposDatos getTipo(float resultado)
        {
            if (resultado % 1 != 0)
            {
                return Variable.TiposDatos.Float;
            }
            else if (resultado < 256)
            {
                return Variable.TiposDatos.Char;
            }
            else if (resultado < 65536)
            {
                return Variable.TiposDatos.Int;
            }
            return Variable.TiposDatos.Float;
        }

        // Libreria -> #include<Identificador(.h)?>
        private void Libreria()
        {
            match("#");
            match("include");
            match("<");
            match(Tipos.Identificador);
            if (getContenido() == ".")
            {
                match(".");
                match("h");
            }
            match(">");
        }
        //Librerias -> Libreria Librerias?
        private void Librerias()
        {
            Libreria();
            if (getContenido() == "#")
            {
                Librerias();
            }
        }
        //Variables -> tipo_dato ListaIdentificadores; Variables?
        private void Variables()
        {
            Variable.TiposDatos tipo = Variable.TiposDatos.Char;
            switch (getContenido())
            {
                case "int": tipo = Variable.TiposDatos.Int; break;
                case "float": tipo = Variable.TiposDatos.Float; break;
            }
            match(Tipos.TipoDato);
            ListaIdentificadores(tipo);
            match(";");
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
        }
        //ListaIdentificadores -> identificador (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TiposDatos tipo)
        {
            if (!Existe(getContenido()))
            {
                lista.Add(new Variable(getContenido(), tipo));
            }
            else
            {
                throw new Error("de sintaxis, la variable <" + getContenido() + "> está duplicada", log, linea, columna);
            }
            match(Tipos.Identificador);
            if (getContenido() == ",")
            {
                match(",");
                ListaIdentificadores(tipo);
            }
        }
        //BloqueInstrucciones -> { ListaInstrucciones ? }
        private void BloqueInstrucciones(bool ejecuta, bool primeraVez)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(ejecuta,primeraVez);
            }
            match("}");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool ejecuta, bool primeraVez)
        {
            Instruccion(ejecuta,primeraVez);
            if (getContenido() != "}")
            {
                ListaInstrucciones(ejecuta,primeraVez);
            }
        }
        //Instruccion -> Printf | Scanf | If | While | Do | For | Asignacion
        private void Instruccion(bool ejecuta, bool primeraVez)
        {
            if (getContenido() == "printf")
            {
                Printf(ejecuta,primeraVez);
            }
            else if (getContenido() == "scanf")
            {
                Scanf(ejecuta,primeraVez);
            }
            else if (getContenido() == "if")
            {
                If(ejecuta,primeraVez);
            }
            else if (getContenido() == "while")
            {
                While(ejecuta,primeraVez);
            }
            else if (getContenido() == "do")
            {
                Do(ejecuta,primeraVez);
            }
            else if (getContenido() == "for")
            {
                For(ejecuta,primeraVez);
            }
            else
            {
                Asignacion(ejecuta,primeraVez);
            }
        }
        //Asignacion -> identificador = Expresion;
        private void Asignacion(bool ejecuta, bool primeraVez)
        {
            float resultado = 0;
            tipoDatoExpresion = Variable.TiposDatos.Char;
            if (!Existe(getContenido()))
            {
                throw new Error("de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
            }
            log.Write(getContenido() + " = ");
            string variable = getContenido();
            match(Tipos.Identificador);
            if (getContenido() == "=")
            {
                match("=");
                Expresion(primeraVez);
                resultado = stack.Pop();
                if(primeraVez)
                {
                    asm.WriteLine("POP AX");
                    asm.WriteLine("; Asignacion "+variable);
                    asm.WriteLine("MOV "+variable+", AX");
                }
            }
            else if (getClasificacion() == Tipos.IncrementoTermino)
            {
                if (getContenido() == "++")
                {
                    match("++");
                    resultado = getValor(variable) + 1;
                    if(primeraVez)
                    {
                        asm.WriteLine("MOV AX, "+variable);
                        asm.WriteLine("INC AX");
                        asm.WriteLine("MOV "+variable+", AX");
                    }
                }
                else if (getContenido() == "--")
                {
                    match("--");
                    resultado = getValor(variable) - 1;
                    if(primeraVez)
                    {
                        asm.WriteLine("MOV AX, "+variable);
                        asm.WriteLine("DEC AX");
                        asm.WriteLine("MOV "+variable+", AX");
                    }
                }
                else if (getContenido() == "+=")
                {
                    match("+=");
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado += getValor(variable);
                    if(primeraVez)
                    {
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV BX, "+variable);
                        asm.WriteLine("ADD AX, BX");
                        asm.WriteLine("MOV "+variable+", AX");
                    }
                    
                }
                else
                {
                    match("-=");
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado = getValor(variable) - resultado;
                    if(primeraVez)
                    {
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV BX, "+variable);
                        asm.WriteLine("SUB BX, AX");
                        asm.WriteLine("MOV "+variable+", BX");
                    }
                }
            }
            else if (getClasificacion() == Tipos.IncrementoFactor)
            {
                if (getContenido() == "*=")
                {
                    match("*=");
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado *= getValor(variable);
                    if(primeraVez)
                    {
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV BX, "+variable);
                        asm.WriteLine("IMUL BX");
                        asm.WriteLine("MOV "+variable+", AX");
                    }
                }
                else if (getContenido() == "/=")
                {
                    match("/=");
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado = getValor(variable) / resultado;
                    if(primeraVez)
                    {
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV BX, AX");
                        asm.WriteLine("MOV AX, "+variable);
                        asm.WriteLine("IDIV BX");
                        asm.WriteLine("MOV "+variable+", AX");
                    }
                }
                else if (getContenido() == "%=")
                {
                    match("%=");
                    Expresion(primeraVez);
                    resultado = stack.Pop();
                    resultado = getValor(variable) % resultado;
                    if(primeraVez)
                    {
                        asm.WriteLine("POP AX");
                        asm.WriteLine("MOV BX, AX");
                        asm.WriteLine("MOV AX, "+variable);
                        asm.WriteLine("IDIV BX");
                        asm.WriteLine("MOV "+variable+", DX");
                    }
                }
            }
            log.WriteLine(" = " + resultado);
            if (ejecuta)
            {
                Variable.TiposDatos tipoDatoVariable = getTipo(variable);
                Variable.TiposDatos tipoDatoResultado = getTipo(resultado);

                // Console.WriteLine(variable + " = "+tipoDatoVariable);
                // Console.WriteLine(resultado + " = "+tipoDatoResultado);
                // Console.WriteLine("expresion = "+tipoDatoExpresion);

                if (tipoDatoExpresion > tipoDatoResultado)
                {
                    tipoDatoResultado = tipoDatoExpresion;
                }
                if (tipoDatoVariable >= tipoDatoResultado)
                {
                    Modifica(variable, resultado);
                }
                else
                {
                    throw new Error("de semantica, no se puede asignar in <" + tipoDatoResultado + "> a un <" + tipoDatoVariable + ">", log, linea, columna);
                }
            }
            match(";");
        }
        //While -> while(Condicion) BloqueInstrucciones | Instruccion
        private void While(bool ejecuta, bool primeraVez)
        {
            if(primeraVez)
            {
                asm.WriteLine("; While: "+contWhile);
            }   
            
            string etiquetaInicio = "InicioWhile"+ contWhile;
            string etiquetaFin    = "FinWhile"+ contWhile++;

            if(primeraVez)
            {
                asm.WriteLine(etiquetaInicio+":");
            }
            do
            {
                int inicia = caracter;
                int lineaInicio = linea;
                match("while");
                match("(");
                ejecuta = Condicion(etiquetaFin,primeraVez) && ejecuta;
                match(")");
                
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(ejecuta,primeraVez);
                }
                else
                {
                    Instruccion(ejecuta,primeraVez);
                }
                
                if (ejecuta)
                {
                    archivo.DiscardBufferedData();
                    caracter = inicia-6;
                    archivo.BaseStream.Seek(caracter, SeekOrigin.Begin);
                    nextToken();
                    linea = lineaInicio;
                }
                if(primeraVez)
                {
                    asm.WriteLine("JMP "+etiquetaInicio);
                    asm.WriteLine(etiquetaFin+":");
                }
                primeraVez = false;
            }
            while (ejecuta);
            
        }

        //Do -> do BloqueInstrucciones | Instruccion while(Condicion)
        private void Do(bool ejecuta, bool primeraVez)
        {
            if(primeraVez)
            {
                asm.WriteLine("; Do: "+contDo); 
            }  
            
            string etiquetaInicio = "InicioDo"+ contDo;
            string etiquetaFin    = "FinDo"+ contDo++;

            
            if(primeraVez)
            {
                asm.WriteLine(etiquetaInicio+":");
            }
            do
            {
                int inicia = caracter;
                int lineaInicio = linea;
                match("do"); 
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(ejecuta,primeraVez);
                }
                else
                {
                    Instruccion(ejecuta,primeraVez);
                }
                match("while");
                match("(");
                ejecuta = Condicion(etiquetaFin,primeraVez) && ejecuta;
                match(")");
                match(";");
                if (ejecuta)
                {
                    archivo.DiscardBufferedData();
                    caracter = inicia - 2;
                    archivo.BaseStream.Seek(caracter, SeekOrigin.Begin);
                    nextToken();
                    linea = lineaInicio;
                }
                if(primeraVez)
                {
                    asm.WriteLine("JMP "+etiquetaInicio);
                    asm.WriteLine(etiquetaFin+":");
                }
                primeraVez = false;
            }
            while (ejecuta);
            
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstrucciones | Instruccion
        private void For(bool ejecuta, bool primeraVez)
        {
            
            
            if(primeraVez)
            {
                asm.WriteLine("; FOR: "+contFor);
            }
        
            match("for");
            match("(");
            Asignacion(ejecuta,primeraVez);
            string etiquetaInicio = "InicioFor"+contFor;
            string etiquetaFin = "FinFor"+contFor++;

            int inicia = caracter;
            int lineaInicio = linea;
            float resultado = 0;
            string variable = getContenido();
            
            log.WriteLine("for: "+variable);
            
            if(primeraVez)
            {
                asm.WriteLine(etiquetaInicio+":");
            }
            

            do
            {
                ejecuta = Condicion(etiquetaFin,primeraVez) && ejecuta;
                match(";");
                resultado = Incremento(ejecuta);
                match(")");
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(ejecuta,primeraVez);
                }
                else
                {
                    Instruccion(ejecuta,primeraVez);
                }
                if (getValor(variable) < resultado)
                {
                    if(primeraVez)
                    {
                        asm.WriteLine("INC " + variable);
                    }
                }
                else if (getValor(variable) > resultado)
                {
                    if(primeraVez)
                    {
                        asm.WriteLine("DEC " + variable);
                    }
                }

                if (ejecuta)
                {
                    Variable.TiposDatos tipoDatoVariable = getTipo(variable);
                    Variable.TiposDatos tipoDatoResultado = getTipo(resultado);
                    if (tipoDatoVariable >= tipoDatoResultado)
                    {
                        Modifica(variable, resultado);
                    }
                    else
                    {
                        throw new Error("de semantica, no se puede asignar un <" + getTipo(resultado) + "> a un <" + getTipo(variable) + ">", log, linea, columna);
                    }
                    archivo.DiscardBufferedData();
                    caracter = inicia - variable.Length - 1;
                    archivo.BaseStream.Seek(caracter, SeekOrigin.Begin);
                    nextToken();
                    linea = lineaInicio;
                    
                }
                
                if(primeraVez){
                    asm.WriteLine("JMP "+etiquetaInicio);
                    asm.WriteLine(etiquetaFin+":");
                }

                primeraVez = false;
            }
            while (ejecuta);
           
            
            
            
        }
        //Incremento -> Identificador ++ | --
        private float Incremento(bool ejecuta)
        {
            float resultado = 0;
            if (!Existe(getContenido()))
            {
                throw new Error("de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
            }
            stack.Push(getValor(getContenido()));

            string variable = getContenido();
            match(Tipos.Identificador);
            if (getContenido() == "++")
            {
                match("++");
                resultado = stack.Pop()  + 1;
            }
            else
            {
                match("--");
                resultado = stack.Pop() - 1;
            }
            return resultado;
        }
        //Condicion -> Expresion OperadorRelacional Expresion
        private bool Condicion(string etiqueta, bool primeraVez)
        {
            Expresion(primeraVez);
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion(primeraVez);
            float R1 = stack.Pop();
            float R2 = stack.Pop();
            

            if (primeraVez)
            {
                asm.WriteLine("POP BX"); // Expresion 2
                asm.WriteLine("POP AX"); // Expresion 1
                asm.WriteLine("CMP AX, BX");
            }

            switch (operador)
            {
                case "==": 
                    if (primeraVez) asm.WriteLine("JNE "+etiqueta);
                return R2 == R1;
                case ">": 
                    if (primeraVez) asm.WriteLine("JBE "+etiqueta);
                return R2 > R1;
                case ">=":
                    if (primeraVez) asm.WriteLine("JB "+etiqueta);
                return R2 >= R1;
                case "<": 
                    if (primeraVez) asm.WriteLine("JAE "+etiqueta);
                return R2 < R1;
                case "<=": 
                    if (primeraVez) asm.WriteLine("JA "+etiqueta);
                return R2 <= R1;
                default: 
                    if (primeraVez) asm.WriteLine("JE "+etiqueta);
                return R2 != R1;
            }
        }
        //If -> if (Condicion) BloqueInstrucciones | Instruccion (else BloqueInstrucciones | Instruccion)?
        private void If(bool ejecuta,bool primeraVez)
        {
            match("if");
            match("(");
            
            if(primeraVez)
            {
                asm.WriteLine("; if: "+contIf);
            }
            
            string etiqueta = "Eif"+ contIf;
            if(primeraVez)
            {
                contIf++;
                contElse++;
            }
            string etiquetaE = "Eelse"+ contElse;
            bool evaluacion = Condicion(etiqueta,primeraVez);
            
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(evaluacion && ejecuta, primeraVez);
                
            }
            else
            {
                Instruccion(evaluacion && ejecuta, primeraVez);
                
            }
        
            if (getContenido() == "else")
            {
                match("else");
                if(primeraVez){
                    asm.WriteLine("; else: "+contElse);
                    asm.WriteLine("JMP "+etiquetaE);
                    asm.WriteLine(etiqueta+":");
                }
                
                
                if (getContenido() == "{")
                {
                    BloqueInstrucciones(!evaluacion && ejecuta, primeraVez);
                }
                else
                {
                    Instruccion(!evaluacion && ejecuta, primeraVez);
                }
                if(primeraVez)
                {
                    asm.WriteLine(etiquetaE+":");
                }
                /*if(primeraVez)
                {
                    contElse++;
                }*/
                
            }
            primeraVez = false;
        }
        //Printf -> printf(cadena(,Identificador)?);
        private void Printf(bool ejecuta,bool primeraVez)
        {
            match("printf");
            match("(");
            if (ejecuta)
            {
                Console.Write(getContenido().Replace("\"", "").Replace("\\n", "\n").Replace("\\t", "\t"));
                if(primeraVez)
                {
                 asm.WriteLine("PRINT '"+getContenido().Replace("\"", "").Replace("\\n", "'\nPRINTN ' '\nPRINT '").Replace("\\t", "  ")+"'");   
                }
            }
            else{
                if(primeraVez)
                {
                 asm.WriteLine("PRINT '"+getContenido().Replace("\"", "").Replace("\\n", "'\nPRINTN ' '\nPRINT '").Replace("\\t", "  ")+"'");   
                }
            }
            match(Tipos.Cadena);
            
            if (getContenido() == ",")
            {
                match(",");
                if (!Existe(getContenido()))
                {
                    throw new Error("de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
                }
                if (ejecuta)
                {
                    if(primeraVez)
                    {
                        asm.WriteLine("MOV AX," + getContenido());
                        asm.WriteLine("CALL PRINT_NUM");
                        asm.WriteLine("PRINTN ''");
                    }
                    Console.Write(getValor(getContenido()));
                }
                else{
                    if(primeraVez)
                    {
                        asm.WriteLine("MOV AX," + getContenido());
                        asm.WriteLine("CALL PRINT_NUM");
                        asm.WriteLine("PRINTN ''");
                    }
                }
                match(Tipos.Identificador);
            }
            match(")");
            match(";");
        }
        //Scanf -> scanf(cadena,&Identificador);
        private void Scanf(bool ejecuta, bool primeraVez)
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            if (!Existe(getContenido()))
            {
                throw new Error("de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
            }
            string variable = getContenido();
            
            match(Tipos.Identificador);
            if (ejecuta)
            {
                if(primeraVez)
                {
                    asm.WriteLine("CALL SCAN_NUM");
                    asm.WriteLine("MOV "+variable +", CX");
                    asm.WriteLine("PRINTN ' '");
                }
                string captura = "" + Console.ReadLine();
                float resultado;
                if (!float.TryParse(captura, out resultado))
                {
                    throw new Error("No se puede capturar una cadena", log, linea, columna);
                }
                else
                {
                    if (getTipo(variable) >= getTipo(resultado))
                    {
                        stack.Push(float.Parse(captura));
                        Modifica(variable, resultado);
                    }
                    else
                    {
                        throw new Error("de semantica, no se puede asignar in <" + getTipo(resultado) + "> a un <" + getTipo(variable) + ">", log, linea, columna);
                    }
                }
            }
            else
            {
               if(primeraVez)
                {
                    asm.WriteLine("CALL SCAN_NUM");
                    asm.WriteLine("MOV "+variable +", CX");
                    asm.WriteLine("PRINTN ' '");
                } 
            }
            match(")");
            match(";");
        }
        //Main -> void main() BloqueInstrucciones
        private void Main(bool ejecuta)
        {
            match("void");
            match("main");
            match("(");
            match(")");
            BloqueInstrucciones(ejecuta,true);
        }
        //Expresion -> Termino MasTermino
        private void Expresion(bool primeraVez)
        {
            Termino(primeraVez);
            MasTermino(primeraVez);
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino(bool primeraVez)
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino(primeraVez);
                log.Write(" " + operador);
                float R2 = stack.Pop();
                float R1 = stack.Pop();
                if (primeraVez)
                {
                    asm.WriteLine("POP BX");
                    asm.WriteLine("POP AX");
                }
                if (operador == "+")
                {
                    stack.Push(R1 + R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("ADD AX, BX");
                        asm.WriteLine("PUSH AX");
                    }
                }
                else
                {
                    stack.Push(R1 - R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("SUB AX, BX");
                        asm.WriteLine("PUSH AX");
                    }
                }
            }
        }
        //Termino -> Factor PorFactor
        private void Termino(bool primeraVez)
        {
            Factor(primeraVez);
            PorFactor(primeraVez);
        }
        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor(bool primeraVez)
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor(primeraVez);
                log.Write(" " + operador);
                float R2 = stack.Pop();
                float R1 = stack.Pop();
                if (primeraVez)
                {
                    asm.WriteLine("POP BX");
                    asm.WriteLine("POP AX");
                }
                if (operador == "*")
                {
                    stack.Push(R1 * R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("MUL  BX");
                        asm.WriteLine("PUSH AX");
                    }
                }
                else if (operador == "/")
                {
                    stack.Push(R1 / R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("DIV  BX");
                        asm.WriteLine("PUSH AX");
                    }
                }
                else
                {
                    stack.Push(R1 % R2);
                    if (primeraVez)
                    {
                        asm.WriteLine("DIV  BX");
                        asm.WriteLine("PUSH DX");
                    }
                }
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor(bool primeraVez)
        {
            if (getClasificacion() == Tipos.Numero)
            {
                log.Write(" " + getContenido());
                if(primeraVez)
                {
                    asm.WriteLine("MOV AX, "+getContenido());
                    asm.WriteLine("PUSH AX");
                }
                stack.Push(float.Parse(getContenido()));
                if (tipoDatoExpresion < getTipo(float.Parse(getContenido())))
                {
                    tipoDatoExpresion = getTipo(float.Parse(getContenido()));
                }
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                if (!Existe(getContenido()))
                {
                    throw new Error("de sintaxis, la variable <" + getContenido() + "> no está declarada", log, linea, columna);
                }
                if (primeraVez)
                {
                    asm.WriteLine("MOV AX, "+getContenido());
                    asm.WriteLine("PUSH AX");
                }
                stack.Push(getValor(getContenido()));
                match(Tipos.Identificador);
                if (tipoDatoExpresion < getTipo(getContenido()))
                {
                    tipoDatoExpresion = getTipo(getContenido());
                }
            }
            else
            {
                bool huboCast = false;
                Variable.TiposDatos tipoDatoCast = Variable.TiposDatos.Char;
                match("(");
                if (getClasificacion() == Tipos.TipoDato)
                {
                    huboCast = true;
                    switch (getContenido())
                    {
                        case "int":
                            tipoDatoCast = Variable.TiposDatos.Int;
                            break;
                        case "float":
                            tipoDatoCast = Variable.TiposDatos.Float;
                            break;
                    }
                    match(Tipos.TipoDato);
                    match(")");
                    match("(");
                }
                Expresion(primeraVez);
                match(")");
                if (huboCast)
                {
                    tipoDatoExpresion = tipoDatoCast;
                    stack.Push(castea(stack.Pop(), tipoDatoCast));
                    /*if (primeraVez)
                    {
                        asm.WriteLine("POP AX");
                    }*/
                }
            }
        }

        //Castear
        float castea(float resultado, Variable.TiposDatos tipoDato)
        {
            if (tipoDato == Variable.TiposDatos.Int)
            {
                resultado = (float)Math.Round(resultado);
                resultado = resultado % 65536;
            }
            else if (tipoDato == Variable.TiposDatos.Char)
            {
                resultado = (float)Math.Round(resultado);
                asm.WriteLine("MOV BX, 256");
                resultado = resultado % 256;
                asm.WriteLine("DIV BX");
                
                asm.WriteLine("PUSH DX");

            }

            return resultado;
        }
    }
}