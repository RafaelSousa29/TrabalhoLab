using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TrabalhoLab.Models;
using TrabalhoLab.ViewModels;

namespace TrabalhoLab.ViewModels
{
    public class TarefaViewModel : BaseViewModel
    {
        public ObservableCollection<Tarefa> Tarefas { get; set; }

        private Tarefa _tarefaSelecionada;
        public Tarefa TarefaSelecionada
        {
            get => _tarefaSelecionada;
            set
            {
                _tarefaSelecionada = value;
                OnPropertyChanged();
            }
        }

        public ICommand AdicionarCommand { get; }
        public ICommand RemoverCommand { get; }
        public ICommand GuardarCommand { get; }

        public TarefaViewModel()
        {
            Tarefas = new ObservableCollection<Tarefa>(
                DataService<List<Tarefa>>.Carregar("tarefas.xml") ?? new());

            AdicionarCommand = new RelayCommand(AdicionarTarefa);
            RemoverCommand = new RelayCommand(RemoverTarefa, () => TarefaSelecionada != null);
            GuardarCommand = new RelayCommand(GuardarTarefas);
        }

        private void AdicionarTarefa()
        {
            var nova = new Tarefa
            {
                Id = Tarefas.Count > 0 ? Tarefas.Max(t => t.Id) + 1 : 1,
                Titulo = "Nova Tarefa",
                Descricao = "",
                Peso = 1
            };
            Tarefas.Add(nova);
            TarefaSelecionada = nova;
            OnPropertyChanged(nameof(TarefaSelecionada));
        }

        private void RemoverTarefa()
        {
            if (TarefaSelecionada != null)
            {
                Tarefas.Remove(TarefaSelecionada);
                TarefaSelecionada = null;
            }
        }

        private void GuardarTarefas()
        {
            DataService<List<Tarefa>>.Guardar("tarefas.xml", Tarefas.ToList());
        }
    }
}