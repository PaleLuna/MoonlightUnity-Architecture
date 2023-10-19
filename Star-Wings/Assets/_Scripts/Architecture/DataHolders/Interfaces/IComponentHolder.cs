using UnityEngine.Events;

public interface IComponentHolder<T> where T : IGameComponent
{
    UnityEvent OnComponentAdded { get; }
    
    TP Register<TP>(TP newComponent) where TP : T;
    void Unregister<TP>(TP component) where TP : T;
    
    TP Get<TP>() where TP : T;
}