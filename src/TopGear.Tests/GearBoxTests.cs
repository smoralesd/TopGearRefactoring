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
        public void FirstGear()
        {
            var gearBox = new GearBox();
            var spy = ObjectSpy.For(gearBox);

            const int input = 1000;
            gearBox.doit(input);

            var internalE = spy.GetField<int>("e");
            internalE.ShouldBeEquivalentTo(input);

            var internalS = spy.GetField<int>("s");
            internalS.ShouldBeEquivalentTo(1);
        }

        [Fact]
        public void SecondGear()
        {
            var gearBox = new GearBox();
            var spy = ObjectSpy.For(gearBox);

            const int input = 3000;
            gearBox.doit(input);
            gearBox.doit(input);

            var internalE = spy.GetField<int>("e");
            internalE.ShouldBeEquivalentTo(input);

            var internalS = spy.GetField<int>("s");
            internalS.ShouldBeEquivalentTo(2);
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

            public T GetField<T>(string name)
            {
                var field = _type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);

                if (field == null)
                {
                    throw new Exception($"Type {_type} has no field {name}");
                }

                return (T)field.GetValue(_obj);
            }
        }
    }
}