public interface IStateHolder<T> : ISingleRegistration<T> where T : State
{
    void ChangeState<TP>() where TP : T;
}