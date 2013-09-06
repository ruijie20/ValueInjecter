using System;
using NUnit.Framework;
using Omu.ValueInjecter;

namespace ValueInjecterTry
{
    public class Source
    {
        public int? Id { set; get; }
        public String Name { set; get; }
    }

    public class Target
    {
        public String ContentId { set; get; }
        public String ContentName { set; get; }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ContentId != null ? ContentId.GetHashCode() : 0)*397) ^
                       (ContentName != null ? ContentName.GetHashCode() : 0);
            }
        }

        public override bool Equals(Object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            Target other = (Target) obj;
            return String.Equals(ContentId, other.ContentId) && String.Equals(ContentName, other.ContentName);
        }
    }

    public class MyConvention : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return "Content" + c.SourceProp.Name == c.TargetProp.Name
                   && c.SourceProp.Value != null;
        }

        protected override object SetValue(ConventionInfo c)
        {
            return c.SourceProp.Value.ToString();
        }
    }

    public class ValueInjecterTest
    {
        [Test]
        public void should_transfer_object_correctly()
        {
            var source = new Source
                {
                    Name = "my",
                    Id = 123
                };
            var target = new Target();
            var expectedTarget = new Target
                {
                    ContentId = "123",
                    ContentName = "my"
                };

            target.InjectFrom<MyConvention>(source);

            Console.WriteLine(target.ContentName);
            Console.WriteLine(target.ContentId);
            Assert.AreEqual(expectedTarget, target);
        }

        [Test]
        public void should_not_transfer_object_null_porperty()
        {
            var source = new Source
                {
                    Name = "my",
                    Id = null
                };
            var target = new Target();
            var expectedTarget = new Target {ContentName = "my"};

            target.InjectFrom<MyConvention>(source);

            Console.WriteLine(target.ContentName);
            Console.WriteLine(target.ContentId);
            Assert.AreEqual(expectedTarget, target);
        }
    }
}
