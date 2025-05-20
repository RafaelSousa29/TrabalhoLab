using System.Windows;
using TrabalhoLab.ViewModels;

namespace TrabalhoLab.Views
{
    public partial class TarefaView : Window
    {
        public TarefaView()
        {
            InitializeComponent();
            DataContext = new TarefaViewModel();
        }
    }
}
