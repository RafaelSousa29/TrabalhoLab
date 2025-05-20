using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ClosedXML.Excel;
using TrabalhoLab.Models;
using TrabalhoLab.ViewModels;


namespace TrabalhoLab.ViewModels
{
    public class PautaViewModel : BaseViewModel
    {
        public ObservableCollection<AlunoNotaFinal> AlunosNotas { get; set; }

        public ICommand ExportarExcelCommand { get; }

        public PautaViewModel()
        {
            var alunos = DataService<List<Aluno>>.Carregar("alunos.xml") ?? new();
            var grupos = DataService<List<Grupo>>.Carregar("grupos.xml") ?? new();
            var tarefas = DataService<List<Tarefa>>.Carregar("tarefas.xml") ?? new();
            var avaliacoes = DataService<List<Avaliacao>>.Carregar("avaliacoes.xml") ?? new();

            var lista = new List<AlunoNotaFinal>();

            foreach (var aluno in alunos)
            {
                var grupo = grupos.FirstOrDefault(g => g.Alunos.Any(a => a.Numero == aluno.Numero));
                string nomeGrupo = grupo?.Nome ?? "Sem Grupo";

                var avaliacoesDoGrupo = grupo != null
                    ? avaliacoes.Where(a => a.GrupoId == grupo.Id).ToList()
                    : new();

                double somaNotas = 0;
                double somaPesos = 0;

                foreach (var tarefa in tarefas)
                {
                    var avaliacao = avaliacoesDoGrupo.FirstOrDefault(a => a.TarefaId == tarefa.Id);
                    if (avaliacao != null)
                    {
                        somaNotas += avaliacao.Nota * tarefa.Peso;
                        somaPesos += tarefa.Peso;
                    }
                }

                double media = somaPesos > 0 ? somaNotas / somaPesos : 0;

                lista.Add(new AlunoNotaFinal
                {
                    NumeroAluno = aluno.Numero,
                    GrupoNome = nomeGrupo,
                    NotaFinal = media
                });
            }

            AlunosNotas = new ObservableCollection<AlunoNotaFinal>(lista);

            ExportarExcelCommand = new RelayCommand(ExportarParaExcel);
        }

        private void ExportarParaExcel()
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Pauta");

            // Cabeçalhos
            ws.Cell(1, 1).Value = "Número";
            ws.Cell(1, 2).Value = "Grupo";
            ws.Cell(1, 3).Value = "Nota Final";

            int linha = 2;
            foreach (var aluno in AlunosNotas)
            {
                ws.Cell(linha, 1).Value = aluno.NumeroAluno;
                ws.Cell(linha, 2).Value = aluno.GrupoNome;
                ws.Cell(linha, 3).Value = aluno.NotaFinal;
                linha++;
            }

            ws.Columns().AdjustToContents();

            string pasta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "TrabalhoLab");

            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            string caminho = Path.Combine(pasta, "pauta.xlsx");
            wb.SaveAs(caminho);
        }
    }
}
