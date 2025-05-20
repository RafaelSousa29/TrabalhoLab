using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using TrabalhoLab.Models;
using TrabalhoLab.ViewModels;


namespace TrabalhoLab.ViewModels
{
    public class UtilizadorViewModel : BaseViewModel
    {
        private Utilizador _utilizador;
        public Utilizador Utilizador
        {
            get => _utilizador;
            set { _utilizador = value; OnPropertyChanged(); }
        }

        public ICommand GuardarCommand { get; }
        public ICommand SelecionarFotoCommand { get; }
        public ICommand EntrarCommand { get; }


        public UtilizadorViewModel()
        {
            Utilizador = DataService<Utilizador>.Carregar("utilizador.xml") ?? new Utilizador();

            GuardarCommand = new RelayCommand(Guardar);
            SelecionarFotoCommand = new RelayCommand(SelecionarFoto);
            EntrarCommand = new RelayCommand(Entrar);
        }

        private void Guardar()
        {
            DataService<Utilizador>.Guardar("utilizador.xml", Utilizador);
        }

        private void SelecionarFoto()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Imagens|*.jpg;*.png;*.jpeg",
                Title = "Selecionar Foto de Perfil"
            };

            if (dlg.ShowDialog() == true)
            {
                Utilizador.CaminhoFoto = dlg.FileName;
                OnPropertyChanged(nameof(Utilizador));
            }
        }

        private void Entrar()
        {
            Guardar(); // Garante que os dados são salvos antes de entrar

            var janelaMenu = new MainWindow();
            janelaMenu.Show();

            // Fechar a janela atual (UtilizadorView)
            foreach (var janela in System.Windows.Application.Current.Windows)
            {
                if (janela is Window w && w.DataContext == this)
                {
                    w.Close();
                    break;
                }
            }
        }

    }

}
