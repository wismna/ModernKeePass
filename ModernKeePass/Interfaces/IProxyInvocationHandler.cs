using System.Reflection;

namespace ModernKeePass.Interfaces
{
    public interface IProxyInvocationHandler
    {
        object Invoke(object proxy, MethodInfo method, object[] parameters);
    }
}