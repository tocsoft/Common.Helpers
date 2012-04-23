using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tocsoft.Common.Helpers
{
    public static class Async
    {
        public static void Run<T>(Func<T> action, Action<T> callback) {
            var bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                e.Result = action();
            };
            if (callback != null)
            {
                bw.RunWorkerCompleted += (s, e) =>
                {
                    callback((T)e.Result);
                };
            }
        }
        public static void Run(Action action) {
            Run(() =>
            {
                action();
                return (object)null;
            }, null);
        }
    }
}
