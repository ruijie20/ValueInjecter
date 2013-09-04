using System;
using NUnit.Framework;
using Omu.ValueInjecter;

namespace ValueInjecterTry
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Conuntry1 { get; set; }
        public int Conuntry2 { get; set; }
    }

    public class PersonDto
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public Country Country1 { get; set; }
        public Country Country2 { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id;
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) Country1;
                hashCode = (hashCode*397) ^ (int) Country2;
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            PersonDto other = (PersonDto) obj;
            return Id == other.Id && string.Equals(Name, other.Name) && Country1 == other.Country1 && Country2 == other.Country2;
        }
    }

    public enum Country
    {
        China, US, UK
    }

    public class IntToEnum : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return c.SourceProp.Name == c.TargetProp.Name && c.SourceProp.Type.IsSubclassOf(typeof (Enum))
                   && c.TargetProp.Type == typeof (int);
        }
    } 

    [TestFixture]
    public class PersonTest
    {
        [Test]
        public void should_transfer_int_to_enum_correctly()
        {
            var person = new Person
                {
                    Id = 1, Conuntry1 = 2, Conuntry2 = 1, Name = "abc"
                };
            var expectedPersonDto = new PersonDto
                {
                    Id = 1, Country1 = Country.US, Country2 = Country.China, Name = "abc"
                };
            var personDto = new PersonDto();
            personDto.InjectFrom<IntToEnum>(person)
                .InjectFrom(person);
            Console.WriteLine(personDto.Country1);
            Console.WriteLine(personDto.Country2);
            Console.WriteLine(personDto.Name);
            Console.WriteLine(personDto.Id);
            Assert.That(personDto, Is.EqualTo(expectedPersonDto));
        }
    } 
}
