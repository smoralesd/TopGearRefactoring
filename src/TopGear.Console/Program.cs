using System;
using System.Collections.Generic;
using System.Linq;

namespace TopGear.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var gearBox = new GearBox.Builder().AddGear(0, 3000). AddGear(0, 3000).Build();
            gearBox.Update(1000);
        }
    }

    public class GearBoxFactory
    {
        public static GearBox CreateDefault(int amount)
        {
            var builder = new GearBox.Builder();

            for (int current = 0; current < amount; current++)
            {
                builder.AddGear(500, 2000);
            }

            return builder.Build();
        }
    }

    public class GearBox
    {
        private int _currentGear;
        private Gear CurrentGear;

        private const int MaxRpm = 2000;
        private const int MinRpm = 500;

        private const int MaxGear = 6;
        private const int MinGear = 1;

        private GearBox(Gear initialGear)
        {
            CurrentGear = initialGear;
        }

        public void Update(int newRpm)
        {
            CurrentGear.Update(this, newRpm);

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

        private class Gear
        {
            private Gear _nextGearUp;
            private Gear _nextGearDown;

            private readonly int _minRpm;
            private readonly int _maxRpm;

            private readonly int _gearNumber;

            public Gear(int gearNumber, int minRpm, int maxRpm)
            {
                _gearNumber = gearNumber;
                _minRpm = minRpm;
                _maxRpm = maxRpm;
            }

            public void SetNextGearDown(Gear nextDown)
            {
                _nextGearDown = nextDown;
            }

            public void SetNextGearUp(Gear nextUp)
            {
                _nextGearUp = nextUp;
            }

            public void Update(GearBox gearBox, int newRpm)
            {
                if (newRpm < _minRpm)
                {
                    gearBox.CurrentGear = _nextGearDown ?? this;
                }

                if (newRpm > _maxRpm)
                {
                    gearBox.CurrentGear = _nextGearUp ?? this;
                }
            }
        }

        public class Builder
        {
            private readonly IList<Gear> _gears = new List<Gear>();

            public Builder AddGear(int minRpm, int maxRpm)
            {
                var newGear = new Gear(_gears.Count + 1, minRpm, maxRpm);

                if (_gears.Count != 0)
                {
                    var previousGear = _gears.Last();
                    previousGear.SetNextGearUp(newGear);
                    newGear.SetNextGearDown(previousGear);
                }

                _gears.Add(newGear);
                return this;
            }

            public GearBox Build()
            {
                if (_gears.Count == 0)
                {
                    throw new Exception("not enought gears for this gear box");
                }

                return new GearBox(_gears.First());
            }
        }
    }
}
