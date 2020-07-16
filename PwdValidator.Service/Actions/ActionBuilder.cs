using System;

namespace PwdValidator.Service.Actions
{
    public static class ActionBuilder
    {

        public static void Execute<T>(IActionOptions options) where T : IAction
        {
            var instance = (T)Activator.CreateInstance(typeof(T));
            if (instance != null) instance.Execute(options);
        }
        
    }
}