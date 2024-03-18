namespace CourseProgram.DataClasses
{
    public class InitData
    {
        private DriverData _driverData;
        public DriverData DriverData { get { return _driverData; } }
        public InitData()
        {
            _driverData = new DriverData();
        }
    }
}