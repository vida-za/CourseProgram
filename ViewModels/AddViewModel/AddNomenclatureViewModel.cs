using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Services;
using CourseProgram.Stores;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.ObjectModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddNomenclatureViewModel : BaseAddViewModel
    {
        public AddNomenclatureViewModel(ServicesStore servicesStore, INavigationService nomenclatureViewNavigationService)
        {
            SubmitCommand = new AddNomenclatureCommand(this, servicesStore, nomenclatureViewNavigationService);
            CancelCommand = new NavigateCommand(nomenclatureViewNavigationService);
        }

        public static ObservableCollection<string> TypeArray => GetFullEnumDescription(typeof(NomenclatureTypeValues));
        public static ObservableCollection<string> CategoryCargoArray => GetFullEnumDescription(typeof(NomenclatureCategoriesValues));
        public static ObservableCollection<string> UnitArray => GetFullEnumDescription(typeof(NomenclatureMeasureValues));
        public static ObservableCollection<string> PackArray => GetFullEnumDescription(typeof(NomenclaturePackingValues));
        public static ObservableCollection<string> DangerousClassArray => GetFullEnumDescription(typeof(NomenclatureDangerousValues));

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value.Length < 101)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                if (value.Length < 51)
                {
                    _type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        private string _categoryCargo;
        public string CategoryCargo
        {
            get => _categoryCargo;
            set
            {
                if (value.Length < 51)
                {
                    _categoryCargo = value;
                    OnPropertyChanged(nameof(CategoryCargo));
                }
            }
        }

        private string _length;
        public string Length
        {
            get => _length;
            set
            {
                if (float.TryParse(value, out float res) && res > 0)
                {
                    _length = value;
                    OnPropertyChanged(nameof(Length));
                }
            }
        }

        private string _width;
        public string Width
        {
            get => _width;
            set
            {
                if (float.TryParse(value, out float res) && res > 0)
                {
                    _width = value;
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        private string _height;
        public string Height
        {
            get => _height;
            set
            {
                if (float.TryParse(value, out float res) && res > 0)
                {
                    _height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        private string _weight;
        public string Weight
        {
            get => _weight;
            set
            {
                if (float.TryParse(value, out float res) && res > 0)
                {
                    _weight = value;
                    OnPropertyChanged(nameof(Weight));
                }
            }
        }

        private string _unit;
        public string Unit
        {
            get => _unit;
            set
            {
                if (value.Length < 11)
                {
                    _unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }

        private string _pack;
        public string Pack
        {
            get => _pack;
            set
            {
                if (value.Length < 51)
                {
                    _pack = value;
                    OnPropertyChanged(nameof(Pack));
                }
            }
        }

        private string _needTemperature;
        public string NeedTemperature
        {
            get => _needTemperature;
            set
            {
                if (value.Length < 51)
                {
                    _needTemperature = value;
                    OnPropertyChanged(nameof(NeedTemperature));
                }
            }
        }

        private string _dangerousClass;
        public string DangerousClass
        {
            get => _dangerousClass;
            set
            {
                if (value.Length < 51)
                {
                    _dangerousClass = value;
                    OnPropertyChanged(nameof(DangerousClass));
                }
            }
        }

        private string _manufacturer;
        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                if (value.Length < 101)
                {
                    _manufacturer = value;
                    OnPropertyChanged(nameof(Manufacturer));
                }
            }
        }

        private string _expiryDate;
        public string ExpiryDate
        {
            get => _expiryDate;
            set
            {
                if (int.TryParse(value, out int res) && res > 0)
                {
                    _expiryDate = value;
                    OnPropertyChanged(nameof(ExpiryDate));
                }
            }
        }
    }
}
