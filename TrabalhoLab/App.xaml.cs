using System.Configuration;
using System.Data;
using System.Windows;

namespace TrabalhoLab
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var janela = new Views.PautaView();
            janela.Show();
        }
    }
}
