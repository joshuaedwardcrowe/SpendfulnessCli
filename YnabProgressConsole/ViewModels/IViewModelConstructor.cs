namespace YnabProgressConsole.ViewModels;

public interface IViewModelConstructor<in T>
{
    public ViewModel Construct(T groupCollection);
}