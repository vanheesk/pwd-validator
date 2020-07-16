namespace PwdValidator.Service.Actions
{
    public interface IAction
    {
        void Execute(IActionOptions options);
    }
}