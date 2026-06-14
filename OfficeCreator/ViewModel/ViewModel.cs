using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace OfficeCreator.ViewModel
{
    public class CommandObcject : ViewModelBase
    {
        private int _buttonId;
        public int ButtonId
        {
            get => _buttonId;
            set { _buttonId = value; OnPropertyChanged(nameof(ButtonId)); }
        }

        public CommandObcject(int buttonId)
        {
            ButtonId = buttonId;         
        }

    }
    public class ViewModel : ViewModelBase
    {
        //private string _globalStatus = "default";

        //public string GlobalStatus
        //{
        //    get => _globalStatus;
        //    set { _globalStatus = value; OnPropertyChanged(nameof(GlobalStatus)); }
        //}


        //public ObservableCollection<SuperButtonObcject> _startFiniszCollection = new ObservableCollection<SuperButtonObcject>();

        //public ObservableCollection<SuperButtonObcject> StartFiniszCollection
        //{
        //    get { return _startFiniszCollection; }
        //    set
        //    {
        //        _startFiniszCollection = value;
        //        OnPropertyChanged();
        //    }
        //}

        public ICommand CreateGridCommand
        {
            get;
            
        }


        public void CreateGrid(object arg)
        {
            //create grid
            
            OnPropertyChanged();
            ;
        }

        public ViewModel()
        {
            CreateGridCommand = new RelayCommand(CreateGrid);
        }

        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute; // <== zamiast Action<object>
            private readonly Func<bool> _canExecute;


            // konstruktor poznajemy po tym że jest ot funkcja publiczna której nazwa jest taka sama jak nazwa klasy
            // tak samo tez tworzmy konstruktor

            // jest to odpowiednik funkcji innit w pythonie, czyli inicjaslizuje obiekt - pozwala na wykonanie dodatkowych operacji
            // któe chcesz żeby nastąpiły w momencie tworzenia obiektu, np wykonanmie jakiejs funkji ktora wypełni pola jakiejs lsity albo matrycy

            public RelayCommand(Action<object> execute, Func<bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute();
            public void Execute(object parameter) => _execute(parameter); // <== nie przekazujemy parametru

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }
    }
}
