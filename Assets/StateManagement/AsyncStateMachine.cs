using Assets.SpideyActions;
using System.Threading.Tasks;

namespace Assets.SpideyActions
{
    /// <summary>
    /// A state machine which supports asynchronous operations
    ///     If a state handler returns a long-running task, all other attempts to update the state will be blocked
    ///     until the long running task completes
    /// </summary>
    /// <typeparam name="ParamType">The type of object which all state handlers will pull their data from</typeparam>
    public class AsyncStateMachine<ParamType>
    {

        private GenericStateHandler<ParamType> state;

        public AsyncStateMachine(GenericStateHandler<ParamType> initalState)
        {
            state = initalState;
        }

        /// <summary>
        /// boolean used to track if the state machine is currently in the middle of an asynchronous state handler
        ///     all state update requests are discarded while this is true
        /// </summary>
        private bool stateLocked = false;

        /// <summary>
        /// Attempt to step the state machine. Returns true if the machine has stepped, false if the machine is in the middle of an
        ///     asyncronous step and it cannot advance
        /// This method can be fired off and ignored safely, as long as the caller does not care about when the effects are executed
        /// </summary>
        /// <param name="updateParam">The data</param>
        /// <returns>true if the state machine executed a step or attempted to execute a step</returns>
        public async Task<bool> update(ParamType updateParam)
        {
            lock (this)
            {
                if (stateLocked)
                {
                    return false;
                }
                stateLocked = true;
            }

            var updateAction = state;
            var newState = await updateAction.HandleState(updateParam);

            if (!newState.Equals(state))
            {
                updateAction.TransitionOutOfState(updateParam);
                var nextAction = newState;
                nextAction.TransitionIntoState(updateParam);
            }

            state = newState;

            lock (this)
            {
                stateLocked = false;
            }
            return true;
        }
    }
}
