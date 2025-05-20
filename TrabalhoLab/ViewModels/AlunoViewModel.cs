using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TrabalhoLab.Models;
using TrabalhoLab.ViewModels;

namespace TrabalhoLab.ViewModels
{
    public class AlunoViewModel : BaseViewModel
    {
        private string _numeroPesquisa;
        public string NumeroPesquisa
        {
            get => _numeroPesquisa;
            set
            {
                _numeroPesquisa = value;
                OnPropertyChanged();
                FiltrarAlunos();
            }
        }

        public ObservableCollection<Aluno> Alunos { get; set; }
        public ObservableCollection<Aluno> AlunosFiltrados { get; set; }

        private Aluno _alunoSelecionado;
        public Aluno AlunoSelecionado
        {
            get => _alunoSelecionado;
            set
            {
                _alunoSelecionado = value;
                OnPropertyChanged();
            }
        }

        public ICommand AdicionarCommand { get; }
        public ICommand RemoverCommand { get; }
        public ICommand GuardarCommand { get; }

        public AlunoViewModel()
        {
            Alunos = new ObservableCollection<Aluno>(
                DataService<List<Aluno>>.Carregar("alunos.xml") ?? new());

            AlunosFiltrados = new ObservableCollection<Aluno>(Alunos);

            AdicionarCommand = new RelayCommand(AdicionarAluno);
            RemoverCommand = new RelayCommand(RemoverAluno, () => AlunoSelecionado != null);
            GuardarCommand = new RelayCommand(GuardarAlunos);
        }

        private void AdicionarAluno()
        {
            var novo = new Aluno { Numero = 0, Nome = "Novo Aluno", Email = "email@exemplo.com" };
            Alunos.Add(novo);
            FiltrarAlunos();
            AlunoSelecionado = novo;
        }

        private void RemoverAluno()
        {
            if (AlunoSelecionado != null)
            {
                Alunos.Remove(AlunoSelecionado);
                FiltrarAlunos();
            }
        }

        private void GuardarAlunos()
        {
            DataService<List<Aluno>>.Guardar("alunos.xml", Alunos.ToList());
        }

        private void FiltrarAlunos()
        {
            if (string.IsNullOrWhiteSpace(NumeroPesquisa))
            {
                AlunosFiltrados = new ObservableCollection<Aluno>(Alunos);
            }
            else
            {
                AlunosFiltrados = new ObservableCollection<Aluno>(
                    Alunos.Where(a => a.Numero.ToString().Contains(NumeroPesquisa)));
            }

            OnPropertyChanged(nameof(AlunosFiltrados));
        }
    }
}
