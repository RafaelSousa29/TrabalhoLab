using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TrabalhoLab.Models;
using TrabalhoLab.ViewModels;

namespace TrabalhoLab.ViewModels
{
    public class GrupoViewModel : BaseViewModel
    {
        public ObservableCollection<Grupo> Grupos { get; set; }
        public ObservableCollection<Aluno> AlunosDisponiveis { get; set; }

        private Grupo _grupoSelecionado;
        public Grupo GrupoSelecionado
        {
            get => _grupoSelecionado;
            set
            {
                _grupoSelecionado = value;
                OnPropertyChanged();
                AtualizarAlunosNoGrupo();
            }
        }

        private ObservableCollection<Aluno> _alunosNoGrupo = new();
        public ObservableCollection<Aluno> AlunosNoGrupo
        {
            get => _alunosNoGrupo;
            set
            {
                _alunosNoGrupo = value;
                OnPropertyChanged();
            }
        }

        private Aluno _alunoSelecionadoParaAdicionar;
        public Aluno AlunoSelecionadoParaAdicionar
        {
            get => _alunoSelecionadoParaAdicionar;
            set { _alunoSelecionadoParaAdicionar = value; OnPropertyChanged(); }
        }

        private Aluno _alunoSelecionadoParaRemover;
        public Aluno AlunoSelecionadoParaRemover
        {
            get => _alunoSelecionadoParaRemover;
            set { _alunoSelecionadoParaRemover = value; OnPropertyChanged(); }
        }

        public ICommand AdicionarGrupoCommand { get; }
        public ICommand RemoverGrupoCommand { get; }
        public ICommand GuardarGruposCommand { get; }
        public ICommand AdicionarAlunoAoGrupoCommand { get; }
        public ICommand RemoverAlunoDoGrupoCommand { get; }

        public GrupoViewModel()
        {
            Grupos = new ObservableCollection<Grupo>(
                DataService<List<Grupo>>.Carregar("grupos.xml") ?? new());

            AlunosDisponiveis = new ObservableCollection<Aluno>(
                DataService<List<Aluno>>.Carregar("alunos.xml") ?? new());

            AdicionarGrupoCommand = new RelayCommand(AdicionarGrupo);
            RemoverGrupoCommand = new RelayCommand(RemoverGrupo);
            GuardarGruposCommand = new RelayCommand(GuardarGrupos);
            AdicionarAlunoAoGrupoCommand = new RelayCommand(AdicionarAlunoAoGrupo);
            RemoverAlunoDoGrupoCommand = new RelayCommand(RemoverAlunoDoGrupo);
        }

        private void AdicionarGrupo()
        {
            var novo = new Grupo
            {
                Id = Grupos.Count > 0 ? Grupos.Max(g => g.Id) + 1 : 1,
                Nome = "Novo Grupo",
                Alunos = new List<Aluno>()
            };
            Grupos.Add(novo);
            GrupoSelecionado = novo;
        }

        private void RemoverGrupo()
        {
            if (GrupoSelecionado != null)
            {
                Grupos.Remove(GrupoSelecionado);
                GrupoSelecionado = null;
                AtualizarAlunosNoGrupo();
            }
        }

        private void GuardarGrupos()
        {
            DataService<List<Grupo>>.Guardar("grupos.xml", Grupos.ToList());
        }

        private void AdicionarAlunoAoGrupo()
        {
            if (GrupoSelecionado == null || AlunoSelecionadoParaAdicionar == null)
                return;

            bool jaEstaNoutroGrupo = Grupos
                .Any(g => g.Alunos.Any(a => a.Numero == AlunoSelecionadoParaAdicionar.Numero));

            if (jaEstaNoutroGrupo)
                return;

            GrupoSelecionado.Alunos.Add(AlunoSelecionadoParaAdicionar);
            AtualizarAlunosNoGrupo();
            AlunoSelecionadoParaAdicionar = null;
        }

        private void RemoverAlunoDoGrupo()
        {
            if (GrupoSelecionado == null || AlunoSelecionadoParaRemover == null)
                return;

            GrupoSelecionado.Alunos.RemoveAll(a => a.Numero == AlunoSelecionadoParaRemover.Numero);
            AtualizarAlunosNoGrupo();
            AlunoSelecionadoParaRemover = null;
        }

        private void AtualizarAlunosNoGrupo()
        {
            AlunosNoGrupo = GrupoSelecionado != null
                ? new ObservableCollection<Aluno>(GrupoSelecionado.Alunos)
                : new ObservableCollection<Aluno>();
        }
    }
}
