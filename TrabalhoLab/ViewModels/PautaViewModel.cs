using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ClosedXML.Excel;
using TrabalhoLab.Models;
using TrabalhoLab.ViewModels;
using LiveCharts;
using LiveCharts.Wpf;

namespace TrabalhoLab.ViewModels
{
    public class PautaViewModel : BaseViewModel
    {
        public ObservableCollection<AlunoNotaFinal> AlunosNotas { get; set; }

        public ICommand ExportarExcelCommand { get; }

        public SeriesCollection Series { get; set; }
        public string[] Labels { get; set; }

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
                    // Tenta encontrar nota INDIVIDUAL primeiro
                    var avaliacaoIndividual = avaliacoesDoGrupo.FirstOrDefault(a =>
                        a.TarefaId == tarefa.Id &&
                        a.NumeroAluno == aluno.Numero);

                    double? notaConsiderada = null;

                    if (avaliacaoIndividual != null)
                    {
                        notaConsiderada = avaliacaoIndividual.Nota;
                    }
                    else
                    {
                        // Se não houver individual, usa a GERAL do grupo
                        var avaliacaoGeral = avaliacoesDoGrupo.FirstOrDefault(a =>
                            a.TarefaId == tarefa.Id &&
                            a.NumeroAluno == null);

                        if (avaliacaoGeral != null)
                            notaConsiderada = avaliacaoGeral.Nota;
                    }

                    if (notaConsiderada.HasValue)
                    {
                        somaNotas += notaConsiderada.Value * tarefa.Peso;
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

            Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Notas",
                    Values = new ChartValues<double>(AlunosNotas.Select(a => a.NotaFinal))
                }
            };

            Labels = AlunosNotas.Select(a => a.NumeroAluno.ToString()).ToArray();
        }

        private void ExportarParaExcel()
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Pauta");

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