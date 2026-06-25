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
       public class MainViewModel : ViewModelBase
    {
        private float _moduleX = 6;

        public float ModuleX
        {
            get => _moduleX; set { _moduleX = value; OnPropertyChanged(nameof(ModuleX)); }
        }

        private float _moduleY = 6;

        public float ModuleY
        {
            get => _moduleY; set { _moduleY = value; OnPropertyChanged(nameof(ModuleY)); }
        }

        private float _distanceX = 6;

        public float DistanceX
        {
            get => _distanceX; set { _distanceX = value; OnPropertyChanged(nameof(DistanceX)); }
        }

        private float _distanceY = 6;
        public float DistanceY
        {
            get => _distanceY; set { _distanceY = value; OnPropertyChanged(nameof(DistanceY)); }
        }

        private bool _gridChecker = true;
        public bool GridChecker
        {
            get => _gridChecker; set { _gridChecker = value; OnPropertyChanged(nameof(GridChecker)); }
        }

        private bool _wallChecker = true;
        public bool WallChecker
        {
            get => _wallChecker; set { _wallChecker = value; OnPropertyChanged(nameof(WallChecker)); }
        }

        private bool _windowChecker = true;
        public bool WindowChecker
        {
            get => _windowChecker; set { _windowChecker = value; OnPropertyChanged(nameof(WindowChecker)); }
        }
        private bool _doorChecker = true;
        public bool DoorChecker
        {
            get => _doorChecker; set { _doorChecker = value; OnPropertyChanged(nameof(DoorChecker)); }
        }

        private bool _stairsChecker = true;
        public bool StairsChecker
        {
            get => _stairsChecker; set { _stairsChecker = value; OnPropertyChanged(nameof(StairsChecker)); }
        }

        private bool _floorChecker = true;
        public bool FloorChecker
        {
            get => _floorChecker; set { _floorChecker = value; OnPropertyChanged(nameof(FloorChecker)); }
        }

        private bool _columnChecker = true;
        public bool ColumnChecker
        {
            get => _columnChecker; set { _columnChecker = value; OnPropertyChanged(nameof(ColumnChecker)); }
        }


        //Vm does not connect to View 
        //creating event to say i want to clsoe this window
        public event Action RequestClose;
        public ICommand GenerateCommand { get; }

        public void OnGenerate(object arg) => RequestClose?.Invoke();


        public MainViewModel()
        {
            GenerateCommand = new RelayCommand(OnGenerate);
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
