using System.Windows.Input;
using TrabalhoLab.ViewModels;
using TrabalhoLab.Views;

namespace TrabalhoLab.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        public ICommand AbrirAlunosCommand { get; }
        public ICommand AbrirGruposCommand { get; }
        public ICommand AbrirTarefasCommand { get; }
        public ICommand AbrirAvaliacoesCommand { get; }
        public ICommand AbrirPautaCommand { get; }
        public ICommand AbrirPerfilCommand { get; }

        public MainMenuViewModel()
        {
            AbrirAlunosCommand = new RelayCommand(() => new AlunoView().Show());
            AbrirGruposCommand = new RelayCommand(() => new GrupoView().Show());
            AbrirTarefasCommand = new RelayCommand(() => new TarefaView().Show());
            AbrirAvaliacoesCommand = new RelayCommand(() => new AvaliacaoView().Show());
            AbrirPautaCommand = new RelayCommand(() => new PautaView().Show());
            AbrirPerfilCommand = new RelayCommand(() => new UtilizadorView().Show());
        }
    }
}
