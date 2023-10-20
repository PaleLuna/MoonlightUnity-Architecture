public interface IRegistration<T>
{
    T Registration(T item);
    void Unregistration(T item);
}
public interface ISingleRegistration
{
    TP Registarion<TP>(TP item);
    TP Unregistration<TP>();
}

public interface ISingleRegistration<T>
{
    TP Registarion<TP>(TP item) where TP : T;
    TP Unregistration<TP>() where TP : T;
}