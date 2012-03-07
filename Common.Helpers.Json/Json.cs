using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Helpers.Json
{
    public static class JsonSerialzationHelpers
    {
        static JsonSerialzationHelpers()
        {
            Serializer = new Newtonsoft.Json.JsonSerializer();
        }

        public static JsonSerializer Serializer { get; set; }


        public static T ToObject<T>(this string json, T obj) 
        {
            return json.ToObject<T>();
        }

        public static T ToObject<T>(this string json) {

            using (var sr = new System.IO.StringReader(json))
            {
                using (var jtw = new JsonTextReader(sr))
                {
                    return Serializer.Deserialize<T>(jtw);
                }
            }
        }

        public static string ToJson(this object obj){

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (JsonTextWriter jw = new JsonTextWriter(sw))
                {
                    Serializer.Serialize(jw, obj);
                }
                return sw.ToString();
            }
        }

        public static JObject GenerateDiff(this object updated, object baseObject)
        {
            JObject old = baseObject as JObject;
            if (old == null)
                old = new JObject(baseObject);

            JObject newRec = updated as JObject;
            if (newRec == null)
                newRec = new JObject(updated);

            return GenerateDiff(newRec, old);
        }


        private static JObject GenerateDiff(JObject newRecord, JObject oldRecord)
        {
            var add = new JObject();
            foreach (var i in newRecord)
            {
                var oldprop = oldRecord[i.Key];
                var newprop = i.Value;

                if (oldprop == null)
                {
                    add.Add(i.Key, newprop);
                }
                else if (newprop.ToString() != oldprop.ToString())
                {
                    var newpropObj = newprop as JObject;
                    var oldpropObj = oldprop as JObject;

                    if (oldpropObj != null && newpropObj != null)
                    {
                        var diff = GenerateDiff(newpropObj, oldpropObj);
                        if (diff != null)
                            add.Add(i.Key, diff);
                    }
                    else
                    {
                        add.Add(i.Key, newprop);
                    }
                }
            }

            if (add.Count > 0)
                return add;
            else
                return null;
        }

    }
}
