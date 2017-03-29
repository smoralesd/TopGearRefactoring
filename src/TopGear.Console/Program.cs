namespace TopGear.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var gearBox = new GearBox();
            gearBox.Update(1000);
        }
    }

    public class GearBox
    {
        private int _currentGear = 0;

        private const int MaxRpm = 2000;
        private const int MinRpm = 500;

        private const int MaxGear = 6;
        private const int MinGear = 1;

        public void Update(int newRpm)
        {
            if (_currentGear > 0)
            {
                if (newRpm > MaxRpm)
                {
                    _currentGear++;
                }
                else if (newRpm < MinRpm)
                {
                    _currentGear--;
                }
            }
            if (_currentGear > MaxGear)
            {
                _currentGear--;
            }
            else if (_currentGear < MinGear)
            {
                _currentGear++;
            }
        }
    }
}
