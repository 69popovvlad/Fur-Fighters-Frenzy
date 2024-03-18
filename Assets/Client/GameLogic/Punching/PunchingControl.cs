using Client.GameLogic.Collision;
using Client.GameLogic.Collision.Commands;
using Client.GameLogic.Inputs;
using Client.GameLogic.Inputs.Commands.Punching;
using Core.Ioc;
using JetBrains.Annotations;
using UnityEngine;

namespace Client.GameLogic.Punching
{
    public class PunchingControl : MonoBehaviour
    {
        [SerializeField] private ArmPunchingControl _leftArm;
        [SerializeField] private ArmPunchingControl _rightArm;

        private InputBucket _inputBucket;
        private CollisionBucket _collisionBucket;
        
        private void Awake()
        {
            var ioc = Ioc.Instance;
            
            _inputBucket = ioc.Get<InputBucket>();
            _inputBucket.Subscribe<PunchInputCommand>(OnPunchCommand);
            
            _collisionBucket = ioc.Get<CollisionBucket>();
        }

        private void OnDestroy()
        {
            _inputBucket.Unsubscribe<PunchInputCommand>(OnPunchCommand);
        }
        
        [UsedImplicitly]
        public void OnPunchCollision(string entityKey, string partKey, ColliderDataControl colliderData)
        {
            var command = new PunchCollisionCommand(entityKey, partKey, colliderData.CharacterEntityKey, colliderData.OnCollisionEnterKey);
            _collisionBucket.Invoke(command);
        }

        private void OnPunchCommand(PunchInputCommand command)
        {
            if (command.isLeftHand)
            {
                _leftArm.Punch(command);
            }
            else
            {
                _rightArm.Punch(command);
            }
        }
    }
}