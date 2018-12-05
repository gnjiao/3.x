using System;
using Microsoft.Practices.Unity;

namespace Core.Boot
{
    public class ActionBootstrapperExtension : IBootstrapperExtension
    {
        public void Initialize(IUnityContainer container)
        {
            Action(container);
        }

        public ActionBootstrapperExtension()
        {
        }

        public ActionBootstrapperExtension(Action<IUnityContainer> action)
        {
            Action = action;
        }

        public Action<IUnityContainer> Action { get; set; }
    }
}