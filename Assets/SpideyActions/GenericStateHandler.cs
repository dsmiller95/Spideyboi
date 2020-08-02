using System.Threading.Tasks;

namespace Assets.SpideyActions
{
    public interface GenericStateHandler<ParamType>
    {
        Task<GenericStateHandler<ParamType>> HandleState(ParamType data);
        void TransitionIntoState(ParamType data);
        void TransitionOutOfState(ParamType data);
    }
}