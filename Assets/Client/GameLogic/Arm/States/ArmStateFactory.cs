using System;
using System.Collections.Generic;
using Client.GameLogic.Eating;
using Client.GameLogic.Inputs.Commands;
using Client.GameLogic.Punching;
using Client.GameLogic.Throwing;
using Client.GameLogic.Throwing.Taking;
using Core.FSM;

namespace Client.GameLogic.Arm.States
{
    public static class ArmStateFactory
    {
        private static readonly Dictionary<Type, Type> _stateByComponent = new Dictionary<Type, Type>()
        {
            {typeof(ArmPunchingControl), typeof(ArmPunchingState)},
            {typeof(TakingArmControl), typeof(ArmTakingState)},
            {typeof(ThrowingArmControl), typeof(ArmThrowingState)},
            {typeof(EatingArmControl), typeof(ArmEatingState)},
        };

        public static StateBase<ArmStateType> CreateState<TCommand>(ArmStateControlBase<TCommand> component, object[] args)
            where TCommand : struct, IInputCommand
        {
            var componentType = _stateByComponent[component.GetType()];
            return (StateBase<ArmStateType>)Activator.CreateInstance(componentType, args);
        }
    }
}