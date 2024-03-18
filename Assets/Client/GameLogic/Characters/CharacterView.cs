using Core.Entities.Views;

namespace Client.GameLogic.Characters
{
    public class CharacterView : EntityView
    {
        private CharacterEntity _entity;

        private void Awake()
        {
            _entity = new CharacterEntity(); // TODO: set guid from network (need update CORE for it)
            Initialize(_entity);
        }
    }
}