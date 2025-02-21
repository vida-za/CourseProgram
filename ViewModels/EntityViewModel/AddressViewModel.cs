using CourseProgram.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class AddressViewModel : BaseViewModel
    {
        private readonly Address _model;
        public readonly int ID;

        [DisplayName("Город")]
        public string City => _model.City;
        [DisplayName("Улица")]
        public string Street => _model.Street;
        [DisplayName("Дом")]
        public string House => CalcHouse();
        [DisplayName("Активен")]
        public string Active => _model.Active ? "Да" : "Нет";

        private string CalcHouse()
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(_model.House))
                parts.Add(_model.House);

            if (!string.IsNullOrWhiteSpace(_model.Structure))
                parts.Add($"с{_model.Structure}");

            if (!string.IsNullOrWhiteSpace(_model.Frame))
                parts.Add($"к{_model.Frame}");

            return parts.Count == 0 ? "Не определен" : string.Join("", parts);
        }

        public AddressViewModel(Address address)
        {
            _model = address;

            ID = _model.ID;
        }

        public Address GetModel() => _model;
    }
}