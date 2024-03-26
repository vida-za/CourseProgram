using CourseProgram.Models;
using System;

namespace CourseProgram.ViewModels
{
    public class RouteViewModel : BaseViewModel
    {
        private readonly Route _route;

        public RouteViewModel(Route route) 
        {
            _route = route;
        }

        public int ID => _route.ID;
        public int MachineID => _route.MachineID;
        public int DriverID => _route.DriverID;
        public string Type => _route.Type;
        public float Distance => _route.Distance;
        public string Status => _route.Status;
        public DateTime CompleteTime => _route.CompleteTime;

        public Route GetRoute() => _route;
    }
}