using UnityEngine.Events;

public interface IServiceHolder<T> where T : IService
{
    TP Register<TP>(TP newComponent) where TP : T;
    void Unregister<TP>(TP component) where TP : T;
    
    TP Get<TP>() where TP : T;
}