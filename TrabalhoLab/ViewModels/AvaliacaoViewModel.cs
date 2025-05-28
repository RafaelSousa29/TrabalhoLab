using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TrabalhoLab.Models;
using TrabalhoLab.ViewModels;


namespace TrabalhoLab.ViewModels
{
    public class AvaliacaoViewModel : BaseViewModel
    {
        public ObservableCollection<Grupo> Grupos { get; set; }
        public ObservableCollection<Tarefa> Tarefas { get; set; }
        public ObservableCollection<Avaliacao> Avaliacoes { get; set; }

        private Grupo _grupoSelecionado;
        public Grupo GrupoSelecionado
        {
            get => _grupoSelecionado;
            set
            {
                _grupoSelecionado = value;
                OnPropertyChanged();
                AtualizarAvaliacoesDoGrupo();
            }
        }

        public ICommand GuardarCommand { get; }

        public AvaliacaoViewModel()
        {
            Grupos = new ObservableCollection<Grupo>(
                DataService<List<Grupo>>.Carregar("grupos.xml") ?? new());

            Tarefas = new ObservableCollection<Tarefa>(
                DataService<List<Tarefa>>.Carregar("tarefas.xml") ?? new());

            Avaliacoes = new ObservableCollection<Avaliacao>(
                DataService<List<Avaliacao>>.Carregar("avaliacoes.xml") ?? new());

            GuardarCommand = new RelayCommand(GuardarAvaliacoes);
        }

        private void AtualizarAvaliacoesDoGrupo()
        {
            if (GrupoSelecionado == null)
                return;

            foreach (var tarefa in Tarefas)
            {
                var existe = Avaliacoes.Any(a => a.GrupoId == GrupoSelecionado.Id && a.TarefaId == tarefa.Id);
                if (!existe)
                {
                    Avaliacoes.Add(new Avaliacao
                    {
                        GrupoId = GrupoSelecionado.Id,
                        TarefaId = tarefa.Id,
                        Nota = 0
                    });
                }
            }

            OnPropertyChanged(nameof(AvaliacoesGrupoSelecionado));
        }

        public ObservableCollection<Avaliacao> AvaliacoesGrupoSelecionado
        {
            get
            {
                if (GrupoSelecionado == null)
                    return new ObservableCollection<Avaliacao>();

                var lista = Avaliacoes
                    .Where(a => a.GrupoId == GrupoSelecionado.Id)
                    .OrderBy(a => a.TarefaId)
                    .ToList();

                return new ObservableCollection<Avaliacao>(lista);
            }
        }

        private void GuardarAvaliacoes()
        {
            DataService<List<Avaliacao>>.Guardar("avaliacoes.xml", Avaliacoes.ToList());
        }
    }
}
