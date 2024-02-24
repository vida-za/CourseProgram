using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CourseProgram.Models;

namespace CourseProgram.ViewModels
{
    public class DriverViewModel : BaseViewModel<Driver>
    {
        public DriverViewModel()
        {
            Title = "";
            IsBusy = false;

            Items = new List<Driver>();
            SelectedItems = new List<Driver>();
            LoadItemsCommand();
        }

        public List<Driver> Items { get; set; }
        public List<Driver> SelectedItems { get; set; }

        private async void LoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var _drivers = await Data.GetItemsAsync(true);
                Task load = new(() =>
                {
                    Items.Clear();
                    foreach (var drv in _drivers)
                        Items.Add(drv);
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}