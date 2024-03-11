using System;
using Client.GameLogic.Inputs.Commands.Movement;
using Core.Ioc;
using UnityEngine;

namespace Client.GameLogic.Inputs.Parts
{
    public class MovementInputPart: IInputPart
    {
        private const float movementTolerance = 0.01f;
        
        private readonly InputBucket _inputBucket;
        
        public MovementInputPart()
        {
            _inputBucket = Ioc.Instance.Get<InputBucket>();
        }
        public void Dispose()
        {
            /* Nothing to do */
        }

        public void Update(in InputPartData data, float delta)
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            if (Math.Abs(x) < movementTolerance && Math.Abs(z) < movementTolerance)
            {
                return;
            }
            
            var command = new MovementCommand(data.OwnerKey, x, z);
            _inputBucket.Invoke(command);
        }
    }
}