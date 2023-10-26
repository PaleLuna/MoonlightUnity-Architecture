public interface IStateHolder<T> where T : State
{
    void ChangeState<TP>() where TP : T;
}