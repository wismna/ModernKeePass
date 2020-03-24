using System.Reflection;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IProxyInvocationHandler
    {
        object Invoke(object proxy, MethodInfo method, object[] parameters);
    }
}