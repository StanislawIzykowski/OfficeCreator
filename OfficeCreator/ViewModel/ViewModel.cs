using Autodesk.Revit.DB;
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

    }
    public class WallBasedTileViewModel : TileViewModel
    {
        private Dictionary<string, ElementId> _listOne;
        public Dictionary<string, ElementId> ListOne { get => _listOne; set { _listOne = value; OnPropertyChanged(); } }

        private ElementId _pickedElemIdFromListOne;
        public ElementId PickedElemIdFromListOne { get => _pickedElemIdFromListOne; set { _pickedElemIdFromListOne = value; OnPropertyChanged(); } }

        private double _wallSideX;
        public double WallSideX { get => _wallSideX; set { _wallSideX = value; OnPropertyChanged(); } }

        private double _wallSideY;
        public double WallSideY { get => _wallSideY; set { _wallSideY = value; OnPropertyChanged(); } }
    }

    public class DropboxTileViewModel : TileViewModel
    {
        private Dictionary<string, ElementId> _listOne;
        public Dictionary<string, ElementId> ListOne{ get => _listOne; set { _listOne = value; OnPropertyChanged(); } }

        private Dictionary<string, ElementId> _listTwo;
        public Dictionary<string, ElementId> ListTwo{ get => _listTwo; set { _listTwo = value; OnPropertyChanged(); } }

        private Dictionary<string, ElementId> _listThree;
        public Dictionary<string, ElementId> ListThree { get => _listThree; set { _listThree = value; OnPropertyChanged(); } }

        private ElementId _pickedElemIdFromListOne;
        public ElementId PickedElemIdFromListOne { get => _pickedElemIdFromListOne; set { _pickedElemIdFromListOne = value; OnPropertyChanged(); } }

        private ElementId _pickedElemIdFromListTwo;
        public ElementId PickedElemIdFromListTwo { get => _pickedElemIdFromListTwo; set { _pickedElemIdFromListTwo = value; OnPropertyChanged(); } }

        private ElementId _pickedElemIdFromListThree;
        public ElementId PickedElemIdFromListThree { get => _pickedElemIdFromListThree; set { _pickedElemIdFromListThree = value; OnPropertyChanged(); } }

    }
    public class MainViewModel : ViewModelBase
    {
        public GridBasedTileViewModel GridTile { get; set; }
        public DropboxTileViewModel ColumnsTile { get; set; }
        public DropboxTileViewModel WallsTile { get; set; }
        public DropboxTileViewModel FloorsTile { get; set; }
        public StairsTileViewModel StairsTile { get; set; }
        public WallBasedTileViewModel WindowTile { get; set; }
        public WallBasedTileViewModel DoorTile { get; set; }

        //closing window part
        public event Action RequestClose;
        public ICommand GenerateCommand { get; }
        public void OnGenerate(object arg) => RequestClose?.Invoke();

        public MainViewModel()
        {
            GridTile = new GridBasedTileViewModel() { Name = "Grid" };
            WallsTile = new DropboxTileViewModel() { Name = "Walls"};
            ColumnsTile = new DropboxTileViewModel() { Name = "Columns"};
            FloorsTile = new DropboxTileViewModel() { Name = "Floors" };
            StairsTile = new StairsTileViewModel() { Name = "Stairs"};
            WindowTile = new WallBasedTileViewModel() { Name = "Window"};
            DoorTile = new WallBasedTileViewModel() { Name = "Door"};

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
