using Core.Entities;

namespace Client.Network.Entities
{
    public class MockEntity : EntityBase
    {
        public MockEntity(string guidBase) : base(guidBase + $"_{nameof(MockEntity)}")
        {
            /* Nothing to do */
        }
    }
}