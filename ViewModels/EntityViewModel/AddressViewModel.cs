using CourseProgram.Models;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class AddressViewModel : BaseViewModel
    {
        private readonly Address _model;

        [DisplayName("Номер адреса")]
        public int ID => _model.ID;
        [DisplayName("Город")]
        public string City => _model.City;
        [DisplayName("Улица")]
        public string Street => _model.Street;
        [DisplayName("Дом")]
        public string House => _model.House + (_model.Structure != "" ? "с" + _model.Structure : string.Empty) + (_model.Frame != "" ? "к" + _model.Frame : string.Empty);

        public AddressViewModel(Address address)
        {
            _model = address;
        }

        public Address GetModel() => _model;
    }
}