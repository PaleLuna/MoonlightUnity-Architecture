using UnityEngine.Events;

public interface IServiceHolder : ISingleRegistration
{
    TP Get<TP>();
}