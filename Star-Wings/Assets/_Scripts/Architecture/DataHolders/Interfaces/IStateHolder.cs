public interface IStateHolder<T> where T : State
{
    TP Register<TP>(TP state) where TP : T;
    TP UnRegister<TP>() where TP : T;

    void ChangeState<TP>();
}