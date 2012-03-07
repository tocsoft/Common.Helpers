using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;
using System.IO;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;
using System.Web;

namespace Common.Helpers
{
        public static class ObjectExtentions
        { 
            ///<summary>
            /// Convert an object into a dictionary
            ///</summary>
            ///<param name="object">The object</param>
            ///<param name="nullHandler">Handler for null value</param>
            ///<returns></returns>
            public static Dictionary<string, object> ToDictionary(this object @object)
            {
                if (@object == null)
                {
                    return new Dictionary<string, object>();
                }

                var properties = TypeDescriptor.GetProperties(@object);
                DynamicObject dyn = @object as DynamicObject;

                int count = properties.Count;
                List<string> dynNames = null;
                if (dyn != null)
                {
                    dynNames = dyn.GetDynamicMemberNames().ToList();
                    count += dynNames.Count;
                }

                var hash = new Dictionary<string, object>(count);

                foreach (PropertyDescriptor descriptor in properties)
                {
                    var key = descriptor.Name;
                    var value = descriptor.GetValue(@object);

                    hash.Add(key, value);

                }
                
                if (dynNames != null)
                {

                    Type objType = @object.GetType();

                    foreach (string dynName in dynNames)
                    {
                        var key = dynName;
                        var value = GetValue(dynName, dyn, objType);

                        hash.Add(key, value);

                    }
                }
                return hash;
            }

            public static object GetValue(string name, DynamicObject dyn, Type objType)
            {
                try
                {
                    var callSite =
                        CallSite<Func<CallSite, object, object>>.Create(
                            Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, name, objType,
                                             new[]
                                             {
                                                 CSharpArgumentInfo.Create(
                                                     CSharpArgumentInfoFlags.None, null)
                                             }));

                    return callSite.Target(callSite, dyn); ;
                }
                catch (RuntimeBinderException)
                {

                    return null;
                }
            }


            public static string ToNiceString(this object o)
            {
                if (o == null)
                    return "";

                var con = TypeDescriptor.GetConverter(o);

                if (con == null)
                    return o.ToString();
                else
                    return con.ConvertToString(o);

            }

        }
    }
