using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoLab.Models;

namespace TrabalhoLab.Models
{
    public class Pauta
    {
        public List<Avaliacao> Avaliacoes { get; set; } = new();
    }
}
