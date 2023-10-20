using UnityEngine.Events;

public interface IServiceHolder 
{
    TP Register<TP>(TP newComponent);
    void Unregister<TP>();
    
    TP Get<TP>();
}