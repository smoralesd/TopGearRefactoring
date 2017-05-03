using System;
using System.Reflection;
using Xunit;
using FluentAssertions;
using TopGear.Console;

namespace TopGear.Tests
{
    public class GearBoxTests
    {
        [Fact]
        public void HighRpmOnFirstGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(1).Build();
            runner.AccelerateToGearUpAndAssert();
        }

        [Fact]
        public void HighRpmOnSecondGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(2).Build();
            runner.AccelerateToGearUpAndAssert();
        }

        [Fact]
        public void HighRpmOnThirdGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(3).Build();
            runner.AccelerateToGearUpAndAssert();
        }

        [Fact]
        public void HighRpmOnFourthGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(4).Build();
            runner.AccelerateToGearUpAndAssert();
        }

        [Fact]
        public void HighRpmOnFifthGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(4).Build();
            runner.AccelerateToGearUpAndAssert();
        }

        [Fact]
        public void HighRpmOnSixthGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(4).Build();
            runner.AccelerateToGearUpAndAssert();
        }

        [Fact]
        public void StayOnFirst()
        {
            var runner = new TestRunner.Builder().SetInitialGear(1).Build();
            runner.StayOnSameGearAndAssert();
        }

        [Fact]
        public void StayOnSecond()
        {
            var runner = new TestRunner.Builder().SetInitialGear(2).Build();
            runner.StayOnSameGearAndAssert();
        }

        [Fact]
        public void StayOnThird()
        {
            var runner = new TestRunner.Builder().SetInitialGear(3).Build();
            runner.StayOnSameGearAndAssert();
        }

        [Fact]
        public void StayOnFourth()
        {
            var runner = new TestRunner.Builder().SetInitialGear(4).Build();
            runner.StayOnSameGearAndAssert();
        }

        [Fact]
        public void StayOnFifth()
        {
            var runner = new TestRunner.Builder().SetInitialGear(5).Build();
            runner.StayOnSameGearAndAssert();
        }

        [Fact]
        public void StayOnSixth()
        {
            var runner = new TestRunner.Builder().SetInitialGear(6).Build();
            runner.StayOnSameGearAndAssert();
        }

        [Fact]
        public void LowRpmOnFirstGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(1).Build();
            runner.DecelerateToGearDownAndAssert();
        }

        [Fact]
        public void LowRpmOnSecondGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(2).Build();
            runner.DecelerateToGearDownAndAssert();
        }

        [Fact]
        public void LowRpmOnThirdGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(3).Build();
            runner.DecelerateToGearDownAndAssert();
        }

        [Fact]
        public void LowRpmOnFourthGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(4).Build();
            runner.DecelerateToGearDownAndAssert();
        }

        [Fact]
        public void LowRpmOnFifhGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(5).Build();
            runner.DecelerateToGearDownAndAssert();
        }

        [Fact]
        public void LowRpmOnSixthGear()
        {
            var runner = new TestRunner.Builder().SetInitialGear(6).Build();
            runner.DecelerateToGearDownAndAssert();
        }

        [Fact]
        public void UpAndDownRun()
        {
            var runner = new TestRunner.Builder().Build();

            runner.AccelerateToGearUpAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.AccelerateToGearUpAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.AccelerateToGearUpAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.DecelerateToGearDownAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.AccelerateToGearUpAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.AccelerateToGearUpAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.AccelerateToGearUpAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.StayOnSameGearAndAssert();
            runner.DecelerateToGearDownAndAssert();
        }

        private class TestRunner
        {
            private const int RpmToGoUpOneGear = 3000;
            private const int RpmToGoDownOneGear = 0;
            private const int RpmToStayOnSameGear = 1000;

            private const int totalGears = 6;

            private readonly ObjectSpy _spy;
            private readonly GearBox _gearBox;

            private TestRunner()
            {
                _gearBox = GearBoxFactory.CreateDefault(totalGears);
                _spy = ObjectSpy.For(_gearBox);
            }

            private void SetInitialGearValue(int initialValue)
            {
                if (initialValue <= 1)
                {
                    return;
                }

                var currentGear = _spy.GetFieldValue<GearBox.Gear>("CurrentGear");

                for (var gearIndex = 2; gearIndex <= initialValue; ++gearIndex)
                {
                    var gearSpy = ObjectSpy.For(currentGear);
                    var nextGear = gearSpy.GetFieldValue<GearBox.Gear>("_nextGearDown");

                    if (nextGear == null)
                    {
                        break;
                    }

                    currentGear = nextGear;
                }
            }

            private int GetCurrentGearValue()
            {
                var currentGear = _spy.GetFieldValue<GearBox.Gear>("CurrentGear");
                var gearSpy = ObjectSpy.For(currentGear);
                return gearSpy.GetFieldValue<int>("_gearNumber");
            }

            public void AccelerateToGearUpAndAssert()
            {
                var originalS = GetCurrentGearValue();

                _gearBox.Update(RpmToGoUpOneGear);

                var newS = GetCurrentGearValue();

                var expectedS = Math.Min(totalGears, originalS + 1);
                newS.ShouldBeEquivalentTo(expectedS, "currentGear value");
            }

            public void StayOnSameGearAndAssert()
            {
                var originalS = GetCurrentGearValue();

                _gearBox.Update(RpmToStayOnSameGear);

                var newS = GetCurrentGearValue();
                newS.ShouldBeEquivalentTo(originalS, "currentGear value");
            }

            public void DecelerateToGearDownAndAssert()
            {
                var originalS = GetCurrentGearValue();

                _gearBox.Update(RpmToGoDownOneGear);

                var newS = GetCurrentGearValue();

                var expectedS = Math.Max(1, originalS - 1);
                newS.ShouldBeEquivalentTo(expectedS, "currentGear value");
            }

            public class Builder
            {
                private int? _initialGear;

                public TestRunner Build()
                {
                    var runner = new TestRunner();

                    if (_initialGear != null)
                    {
                        runner.SetInitialGearValue(_initialGear.Value);
                    }

                    return runner;
                }

                public Builder SetInitialGear(int initialValue)
                {
                    _initialGear = initialValue;
                    return this;
                }
            }
        }

        private class ObjectSpy
        {
            private readonly object _obj;
            private readonly Type _type;

            private ObjectSpy(object obj)
            {
                _type = obj.GetType();
                _obj = obj;
            }

            public static ObjectSpy For(object obj)
            {
                return new ObjectSpy(obj);
            }

            public T GetFieldValue<T>(string name)
            {
                var field = GetFieldInfo(name);
                return (T)field.GetValue(_obj);
            }

            private FieldInfo GetFieldInfo(string name)
            {
                var field = _type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);

                if (field == null)
                {
                    throw new Exception($"Type {_type} has no field {name}");
                }

                return field;
            }

            public void SetFieldValue<T>(string name, T value)
            {
                var field = GetFieldInfo(name);
                field.SetValue(_obj, value);
            }
        }
    }
}