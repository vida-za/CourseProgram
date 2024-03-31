using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProgram.ViewModels
{
    public class MachineListingViewModel : BaseViewModel
    {
        public MachineListingViewModel() 
        {
            
        }

        private bool _stateCheckedBusy;
        public bool StateCheckedBusy
        {
            get => _stateCheckedBusy;
            set
            {
                _stateCheckedBusy = value;
                OnPropertyChanged(nameof(StateCheckedBusy));
            }
        }
    }
}