using System.Reflection;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Aop
{
    public class DatabaseChangedProxy<T>: IProxyInvocationHandler
    {
        private readonly T _decorated;
        private readonly IDatabaseService _databaseService;

        public DatabaseChangedProxy(T decorated, IDatabaseService databaseService)
        {
            _decorated = decorated;
            _databaseService = databaseService;
        }

        public object Invoke(object proxy, MethodInfo method, object[] parameters)
        {
            object retVal = null;
            retVal = method.Invoke(proxy, parameters);
            _databaseService.HasChanged = true;

            return retVal;
        }
    }
}
