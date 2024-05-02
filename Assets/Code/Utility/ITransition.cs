using System.Threading.Tasks;


namespace FormulaBoy.Utility
{
    public interface ITransition<T>
    {
        /// <summary>
        /// Prepare the transition for the given value
        /// </summary>
        /// <param name="value"></param>
        public void PrepareTransition(T value);

        /// <summary>
        /// Transition the given value to the next state, use async to wait for the transition to complete
        /// </summary>
        public Task Transition(T value);

        /// <summary>
        /// End the transition for the given value
        /// </summary>
        public void EndTransition(T value);

        public async Task Play(T value, bool playEnd = true)
        {
            PrepareTransition(value);
            await Transition(value);
            if (playEnd)
                EndTransition(value);
        }
    }
}