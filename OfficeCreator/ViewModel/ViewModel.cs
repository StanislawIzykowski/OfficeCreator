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
    public class TileViewModel : ViewModelBase
    {
        public string Name { get; set; }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set { _isEnabled = value; OnPropertyChanged(nameof(IsEnabled)); }
        }
    }

    //gridbase inherit after titleviemodel
    public class GridBasedTileViewModel : TileViewModel
    {
        private double _moduleX;
        public double ModuleX { get => _moduleX; set { _moduleX = value; OnPropertyChanged(); } }

        private double _moduleY;
        public double ModuleY { get => _moduleY; set { _moduleY = value; OnPropertyChanged(); } }

        private double _distanceX;
        public double DistanceX { get => _distanceX; set { _distanceX = value; OnPropertyChanged(); } }

        private double _distanceY;
        public double DistanceY { get => _distanceY; set { _distanceY = value; OnPropertyChanged(); } }
    }
    public class StairsTileViewModel : TileViewModel
    {
        private int _numberOfTreads;
        public int NumberOfTreads { get => _numberOfTreads; set { _numberOfTreads = value; OnPropertyChanged(); } }

        private double _stepHeight;
        public double StepHeight { get => _stepHeight; set { _stepHeight = value; OnPropertyChanged(); } }

        private double _width;
        public double Width { get => _width; set { _width = value; OnPropertyChanged(); } }
    }
    public class WallBasedTileViewModel : TileViewModel
    {
        
    }
    public class MainViewModel : ViewModelBase
    {
        public GridBasedTileViewModel GridTile { get; set; }
        public GridBasedTileViewModel WallsTile { get; set; }
        public GridBasedTileViewModel FloorsTile { get; set; }
        public GridBasedTileViewModel ColumnTile { get; set; }
        public StairsTileViewModel StairsTile { get; set; }
        public WallBasedTileViewModel WallBasedTitle { get; set; }

        //closing window part
        public event Action RequestClose;
        public ICommand GenerateCommand { get; }
        public void OnGenerate(object arg) => RequestClose?.Invoke();

        public MainViewModel()
        {
            GridTile = new GridBasedTileViewModel() { Name = "Grid" };
            WallsTile = new GridBasedTileViewModel() { Name = "Walls"};
            ColumnTile = new GridBasedTileViewModel() { Name = "Columns" };
            FloorsTile = new GridBasedTileViewModel() { Name = "Floors" };
            StairsTile = new StairsTileViewModel() { Name = "Stairs"};
            WallBasedTitle = new WallBasedTileViewModel() { Name = "WallBased"};

            GenerateCommand = new RelayCommand(OnGenerate);
        }
        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute; // <== zamiast Action<object>
            private readonly Func<bool> _canExecute;

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
