using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrabalhoLab.Models;

namespace TrabalhoLab.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Aluno> Alunos { get; set; } = new();
    }
}

