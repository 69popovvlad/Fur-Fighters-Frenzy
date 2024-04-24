namespace Client.Network.Entities
{
    public class MockNetworkEntityView : NetworkEntityView
    {
        private MockEntity _entity;

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();

            _entity = new MockEntity(ObjectId.ToString());
            Initialize(_entity);
        }
    }
}