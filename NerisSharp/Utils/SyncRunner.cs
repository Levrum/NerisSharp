using System;
using System.Threading.Tasks;

namespace NerisSharp.Utils
{
    public static class SyncRunner
    {
        public static T RunSync<T>(Func<Task<T>> func)
        {
            Task<T> task = Task.Run(func);
            task.Wait();
            T result = task.Result;
            return result;
        }
    }
}
