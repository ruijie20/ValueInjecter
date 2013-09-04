using System;
using System.Globalization;
using NUnit.Framework;
using Omu.ValueInjecter;

namespace ValueInjecterTry
{
    public class Foo
    {

        public Foo Foo1 { set; get; }
        public Foo Foo2 { set; get; }
        public String Name { set; get; }
        public int Age { set; get; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Foo1 != null ? Foo1.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Foo2 != null ? Foo2.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Age;
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            Foo other = (Foo) obj;
            return Equals(Foo1, other.Foo1) && Equals(Foo2, other.Foo2) 
                   && string.Equals(Name, other.Name) && Age == other.Age;
        }
    }

    public class FlatFoo
    {
        public String Foo1Foo2Foo1Name { set; get; }
        public String Foo1Name { set; get; }
        public String Foo2Name { set; get; }
        public String Foo1Age { set; get; }
    }


    public class ValueInjecterTest
    {
        [Test]
        public void should_transfer_correct()
        {
            var flat = new FlatFoo
                {
                    Foo1Foo2Foo1Name = "foo1foo2foo1name",
                    Foo1Name = "abc",
                    Foo2Name = "efg",
                };

            var expectedUnflat = new Foo
                {
                    Foo1 = new Foo
                        {
                            Name = "abc",
                            Foo2 = new Foo
                                {
                                    Foo1 = new Foo
                                        {
                                            Name = "foo1foo2foo1name"
                                        }
                                }
                        },
                    Foo2 = new Foo
                        {
                            Name = "efg"
                        }
                };
            var unflat = new Foo();

            unflat.InjectFrom<UnflatLoopValueInjection>(flat);
            Console.WriteLine(unflat.Foo1.Foo2.Foo1.Name);
            Console.WriteLine(unflat.Foo1.Name);
            Console.WriteLine(unflat.Foo2.Name);
            Assert.That(unflat, Is.EqualTo(expectedUnflat));
        }
    }
}
