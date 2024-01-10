using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosWebApi.ArchivoJson
{
    /// <summary>
    /// Escribir a un archivo de texto "Json", 
    /// Patrón de diseño "Decorator" junto con la clase StreamWriter. Este patrón Decorator permite agregar funcionalidad adicional a objetos de manera dinámica. 
    /// En este caso, StreamWriter puede ser usado para proporcionar funcionalidades adicionales, 
    /// como el registro de información antes de escribir en el archivo.
    /// este registro puede ser utilizado para escribir a los logs por ejemplo.
    /// </summary>
    public interface ITextWriter
    {
        void Inicializa();
        void WriteText(string text);
        void FlushText();
        string ToStringText();
    }

    // Implementación básica de la interfaz
    public class SimpleTextWriter : ITextWriter
    {
        private readonly string filePath;

        public SimpleTextWriter(string filePath)
        {
            this.filePath = filePath;
        }
        public void Inicializa()
        {
            using (StreamWriter writer = File.Exists(filePath) ? File.AppendText(filePath) : File.CreateText(filePath))
            {
            }
        }
        public void WriteText(string text)
        {
            // Implementación básica de escritura en un archivo
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(text);
            }
        }

        public void FlushText()
        {
            // Implementación básica de escritura en un archivo
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.Flush();
            }
            // Cerrar y volver a abrir el archivo en modo de escritura
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                // El archivo ahora está limpio y listo para nuevas escrituras
            }
        }
        public string ToStringText()
        {
            string regresar = "";
            regresar = File.ReadAllText(filePath);
            return regresar;
        }
    }

    // Decorador para agregar funcionalidad de registro antes de la escritura
    public class LoggingTextWriterDecorator : ITextWriter
    {
        private readonly ITextWriter textWriter;

        public LoggingTextWriterDecorator(ITextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Inicializa()
        {
            // Funcionalidad de registro antes de la escritura
            Console.WriteLine($"Registrando: {"ToStringText"}");

            // Llamada al método original de escritura
            textWriter.Inicializa();
        }
        public void WriteText(string text)
        {
            // Funcionalidad de registro antes de la escritura
            Console.WriteLine($"Registrando: {text}");

            // Llamada al método original de escritura
            textWriter.WriteText(text);
        }
        public void FlushText()
        {
            // Funcionalidad de registro antes de la escritura
            Console.WriteLine($"Registrando: {"Flush"}");

            // Llamada al método original de escritura
            textWriter.FlushText();
        }
        public string ToStringText()
        {
            // Funcionalidad de registro antes de la escritura
            Console.WriteLine($"Registrando: {"ToStringText"}");

            // Llamada al método original de escritura
            return textWriter.ToStringText();
        }
    }
}
