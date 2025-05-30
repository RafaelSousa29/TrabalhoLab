using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoLab.Models
{
    public class AlunoNotaFinal
    {
        public int NumeroAluno { get; set; }
        public string NomeAluno { get; set; }
        public string GrupoNome { get; set; } = string.Empty;
        public double NotaFinal { get; set; }
    }
}

