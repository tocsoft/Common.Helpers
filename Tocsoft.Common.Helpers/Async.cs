using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Tocsoft.Common.Helpers
{
    public static class Async
    {
        [Obsolete("Use Tasks")]
        public static void Run<T>(Func<T> action, Action<T> callback) {
            System.Threading.Tasks.Task.Run(()=>{
                return action();
            }).ContinueWith(t =>
            {
                callback(t.Result);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        [Obsolete("Use Tasks")]
        public static void Run(Action action) {
            System.Threading.Tasks.Task.Run(action);
        }
    }
}
