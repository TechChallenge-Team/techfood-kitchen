using Bogus;

namespace TechFood.Kitchen.Domain.Tests.Fixtures
{
    public class OrderFixture
    {
        private readonly Faker _faker;

        public OrderFixture()
        {
            _faker = new Faker("pt_BR");
        }

        public Entities.Order CreateValidOrder(Guid? customerId = null, int? number = null) =>
            new(
                number ?? _faker.Random.Int(1, 1000),
                customerId);
    }
}
