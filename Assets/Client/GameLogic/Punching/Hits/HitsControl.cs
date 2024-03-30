using Client.GameLogic.Characters;
using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Core.Ioc;
using UnityEngine;

namespace Client.Punching.Hits
{
    public class HitsControl: MonoBehaviour
    {
        private readonly int HitHeadHash = Animator.StringToHash("HitHead");
        private readonly int HitBodyHash = Animator.StringToHash("HitBody");

        [SerializeField] private CharacterView _characterView;
        [SerializeField] private Animator _animator;

        private CollisionBucket _collisionBucket;

        private void Awake()
        {
            _collisionBucket = Ioc.Instance.Get<CollisionBucket>();
            _collisionBucket.Subscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        private void OnDestroy()
        {
            _collisionBucket.Unsubscribe<PunchCollisionCommand>(OnPunchCollisionCommand);
        }

        private void OnPunchCollisionCommand(PunchCollisionCommand command)
        {
            if(!command.ToKey.Equals(_characterView.Guid))
            {
                return;
            }

            switch(command.ToPartKey)
            {
                case "head":
                    _animator.SetTrigger(HitHeadHash);
                    break;

                case "body":
                    _animator.SetTrigger(HitBodyHash);
                    break;
            }
        }
    }
}