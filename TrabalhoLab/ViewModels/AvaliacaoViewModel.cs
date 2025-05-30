using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TrabalhoLab.Models;

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
                AtualizarAvaliacoes();
            }
        }

        private bool _avaliacaoIndividual;
        public bool AvaliacaoIndividual
        {
            get => _avaliacaoIndividual;
            set
            {
                _avaliacaoIndividual = value;
                OnPropertyChanged();
                AtualizarAvaliacoes();
            }
        }

        public ObservableCollection<Avaliacao> AvaliacoesGrupoSelecionado { get; set; } = new();

        public ICommand GuardarCommand { get; }
        public ICommand RemoverNotaCommand { get; }

        public AvaliacaoViewModel()
        {
            Grupos = new ObservableCollection<Grupo>(DataService<List<Grupo>>.Carregar("grupos.xml") ?? new());
            Tarefas = new ObservableCollection<Tarefa>(DataService<List<Tarefa>>.Carregar("tarefas.xml") ?? new());
            Avaliacoes = new ObservableCollection<Avaliacao>(DataService<List<Avaliacao>>.Carregar("avaliacoes.xml") ?? new());

            GuardarCommand = new RelayCommand(GuardarAvaliacoes);
            RemoverNotaCommand = new RelayCommand<Avaliacao>(RemoverNota);
        }

        private void AtualizarAvaliacoes()
        {
            AvaliacoesGrupoSelecionado.Clear();

            if (GrupoSelecionado == null)
                return;

            if (AvaliacaoIndividual)
            {
                foreach (var aluno in GrupoSelecionado.Alunos)
                {
                    foreach (var tarefa in Tarefas)
                    {
                        var avaliacao = Avaliacoes.FirstOrDefault(a =>
                            a.GrupoId == GrupoSelecionado.Id &&
                            a.TarefaId == tarefa.Id &&
                            a.NumeroAluno == aluno.Numero);

                        if (avaliacao == null)
                        {
                            avaliacao = new Avaliacao
                            {
                                GrupoId = GrupoSelecionado.Id,
                                TarefaId = tarefa.Id,
                                NumeroAluno = aluno.Numero,
                                Nota = 0
                            };
                            Avaliacoes.Add(avaliacao);
                        }

                        AvaliacoesGrupoSelecionado.Add(avaliacao);
                    }
                }
            }
            else
            {
                foreach (var tarefa in Tarefas)
                {
                    var avaliacao = Avaliacoes.FirstOrDefault(a =>
                        a.GrupoId == GrupoSelecionado.Id &&
                        a.TarefaId == tarefa.Id &&
                        a.NumeroAluno == null);

                    if (avaliacao == null)
                    {
                        avaliacao = new Avaliacao
                        {
                            GrupoId = GrupoSelecionado.Id,
                            TarefaId = tarefa.Id,
                            Nota = 0
                        };
                        Avaliacoes.Add(avaliacao);
                    }

                    AvaliacoesGrupoSelecionado.Add(avaliacao);
                }
            }
        }

        private void RemoverNota(Avaliacao avaliacao)
        {
            if (avaliacao != null)
            {
                Avaliacoes.Remove(avaliacao);
                AtualizarAvaliacoes();
            }
        }

        private void GuardarAvaliacoes()
        {
            DataService<List<Avaliacao>>.Guardar("avaliacoes.xml", Avaliacoes.ToList());
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly System.Action<T> _execute;
        private readonly System.Predicate<T>? _canExecute;

        public RelayCommand(System.Action<T> execute, System.Predicate<T>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return parameter is T t && (_canExecute == null || _canExecute(t));
        }

        public void Execute(object? parameter)
        {
            if (parameter is T t)
                _execute(t);
        }

        public event System.EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value!;
            remove => CommandManager.RequerySuggested -= value!;
        }
    }
}
