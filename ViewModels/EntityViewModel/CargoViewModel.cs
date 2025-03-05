using CourseProgram.Models;
using CourseProgram.Stores;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class CargoViewModel : BaseViewModel
    {
        private readonly ControllersStore _controllersStore;
        private Nomenclature _nomenclatureModel;

        private int _nomenclatureID;
        private float? _volume;
        private float _weight;
        private int _count;

        public int ID;

        public int NomenclatureID
        {
            get => _nomenclatureID;
            set
            {
                if (_nomenclatureID != value)
                {
                    _nomenclatureID = value;
                    UpdateNomenclature();
                    OnPropertyChanged(nameof(NomenclatureID));
                }
            }
        }

        [DisplayName("Название номенклатуры")]
        public string NomenclatureName => _nomenclatureModel?.Name ?? "Не указано";
        [DisplayName("Объём")]
        public string Volume 
        { 
            get => _volume?.ToString() ?? "Не указано";
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _volume = null;
                }
                else if (float.TryParse(value, out float fValue))
                {
                    _volume = fValue;
                }
                OnPropertyChanged(nameof(Volume));
            }
        }
        [DisplayName("Вес")]
        public string Weight
        {
            get => _weight.ToString();
            set
            {
                if (float.TryParse(value, out float fValue))
                {
                    _weight = fValue;
                    OnPropertyChanged(nameof(Weight));
                }
            }
        }
        [DisplayName("Количество")]
        public string Count
        {
            get => _count.ToString();
            set
            {
                if (int.TryParse(value, out int iValue))
                {
                    _count = iValue;
                    OnPropertyChanged(nameof(Count));
                }
            }
        }

        public CargoViewModel(Cargo cargo, ControllersStore controllersStore)
        {
            ID = cargo.ID;
            _nomenclatureID = cargo.NomenclatureID;
            _volume = cargo.Volume;
            _weight = cargo.Weight;
            _count = cargo.Count;

            _controllersStore = controllersStore;
            UpdateNomenclature();
        }

        private async void UpdateNomenclature()
        {
            if (NomenclatureID > 0)
            {
                _nomenclatureModel = await _controllersStore.GetController<Nomenclature>().GetItemByID(NomenclatureID);
                OnPropertyChanged(nameof(NomenclatureName));
            }
        }

        public Cargo ToCargo(int BudID) => new Cargo(ID, BudID, NomenclatureID, _volume, _weight, _count);
    }
}