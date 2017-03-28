namespace TopGear.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var gearBox = new GearBox();
            gearBox.doit(1000);
        }
    }

    public class GearBox
    {
        private int s = 0;

        private const int MaxRpm = 2000;
        private const int MinRpm = 500;

        private const int MaxGear = 6;
        private const int MinGear = 1;

        public void doit(int i)
        {
            if (s > 0)
            {
                if (i > MaxRpm)
                {
                    s++;
                }
                else if (i < MinRpm)
                {
                    s--;
                }
            }
            if (s > MaxGear)
            {
                s--;
            }
            else if (s < MinGear)
            {
                s++;
            }
        }
    }
}
