using System.IO;
using System.Xml.Serialization;

namespace TrabalhoLab.Models
{
    public static class DataService<T>
    {
        public static void Guardar(string nomeFicheiro, T dados)
        {
            string pasta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "TrabalhoLab");

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            string caminho = Path.Combine(pasta, nomeFicheiro);
            using FileStream fs = new(caminho, FileMode.Create);
            XmlSerializer xs = new(typeof(T));
            xs.Serialize(fs, dados);
        }

        public static T? Carregar(string nomeFicheiro)
        {
            string pasta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "TrabalhoLab");

            string caminho = Path.Combine(pasta, nomeFicheiro);
            if (!File.Exists(caminho)) return default;

            using FileStream fs = new(caminho, FileMode.Open);
            XmlSerializer xs = new(typeof(T));
            return (T?)xs.Deserialize(fs);
        }
    }
}
