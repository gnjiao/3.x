using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Main.Util
{
    internal sealed class PlatformServiceContainer : IServiceProvider, IServiceContainer, IDisposable
    {
        private readonly ConcurrentStack<IServiceProvider> _fallbackProviders = new ConcurrentStack<IServiceProvider>();
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly List<Type> _servicesToDispose = new List<Type>();
        private readonly Dictionary<Type, object> _taskCompletionSources = new Dictionary<Type, object>(); 

        public PlatformServiceContainer()
        {
            _services.Add(typeof(PlatformServiceContainer), this);
            _services.Add(typeof(IServiceContainer), this);
        }

        public void AddFallbackProvider(IServiceProvider provider)
        {
            _fallbackProviders.Push(provider);
        }

        public object GetService(Type serviceType)
        {
            object instance;
            lock (_services)
            {
                if (_services.TryGetValue(serviceType, out instance))
                {
                    if (instance is ServiceCreatorCallback callback)
                    {
                        Console.WriteLine("Service startup: " + serviceType);
                        instance = callback(this, serviceType);
                        if (instance != null)
                        {
                            _services[serviceType] = instance;
                            OnServiceInitialized(serviceType, instance);
                        }
                        else
                        {
                            _services.Remove(serviceType);
                        }
                    }
                }
            }

            if (instance != null)
                return instance;
            foreach (var fallbackProvider in _fallbackProviders)
            {
                instance = fallbackProvider.GetService(serviceType);
                if (instance != null)
                    return instance;
            }

            return null;
        }

        public void Dispose()
        {
            Type[] disposableTypes;
            lock (_services)
            {
                disposableTypes = _servicesToDispose.ToArray();
                //services.Clear();
                _servicesToDispose.Clear();
            }

            // dispose services in reverse order of their creation
            for (int i = disposableTypes.Length - 1; i >= 0; i--)
            {
                IDisposable disposable = null;
                lock (_services)
                {
                    if (_services.TryGetValue(disposableTypes[i], out var serviceInstance))
                    {
                        disposable = serviceInstance as IDisposable;
                        if (disposable != null)
                            _services.Remove(disposableTypes[i]);
                    }
                }

                if (disposable != null)
                {
                    Console.WriteLine("Service shutdown: " + disposableTypes[i]);
                    disposable.Dispose();
                }
            }
        }

        private void OnServiceInitialized(Type serviceType, object serviceInstance)
        {
            if (serviceInstance is IDisposable disposableService)
                _servicesToDispose.Add(serviceType);

            dynamic taskCompletionSource;
            if (_taskCompletionSources.TryGetValue(serviceType, out taskCompletionSource))
            {
                _taskCompletionSources.Remove(serviceType);
                taskCompletionSource.SetResult((dynamic) serviceInstance);
            }
        }

        public void AddService(Type serviceType, object serviceInstance)
        {
            lock (_services)
            {
                _services.Add(serviceType, serviceInstance);
                OnServiceInitialized(serviceType, serviceInstance);
            }
        }

        public void AddService(Type serviceType, object serviceInstance, bool promote)
        {
            AddService(serviceType, serviceInstance);
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            lock (_services)
            {
                _services.Add(serviceType, callback);
            }
        }

        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            AddService(serviceType, callback);
        }

        public void RemoveService(Type serviceType)
        {
            lock (_services)
            {
                object instance;
                if (_services.TryGetValue(serviceType, out instance))
                {
                    _services.Remove(serviceType);
                    IDisposable disposableInstance = instance as IDisposable;
                    if (disposableInstance != null)
                        _servicesToDispose.Remove(serviceType);
                }
            }
        }

        public void RemoveService(Type serviceType, bool promote)
        {
            RemoveService(serviceType);
        }

        public Task<T> GetFutureService<T>()
        {
            Type serviceType = typeof(T);
            lock (_services)
            {
                if (_services.ContainsKey(serviceType))
                {
                    return Task.FromResult((T) GetService(serviceType));
                }
                else
                {
                    if (_taskCompletionSources.TryGetValue(serviceType, out var taskCompletionSource))
                    {
                        return ((TaskCompletionSource<T>) taskCompletionSource).Task;
                    }
                    else
                    {
                        var tcs = new TaskCompletionSource<T>();
                        _taskCompletionSources.Add(serviceType, tcs);
                        return tcs.Task;
                    }
                }
            }
        }
    }
}
