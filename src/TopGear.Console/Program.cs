using System.Collections.Generic;

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
        private int e = 0;

        public void doit(int i)
        {
            if (s < 0) {
                // do nothing!
                e = i;
            } else {
                if (s > 0) {
                    if (i > 2000)
                        s++;
                    } else if (i < 500) {
                        s--;
                    }
                } if (s > 6) {
                    s--;
                } else if (s < 1) {
                    s++;
                }
                e = i;
            }
    }
}
