using System;
using System.Collections.Generic;

namespace PaleLuna.Architecture.Services
{
    public class SceneBaggage
    {
        private Dictionary<string, IBaggage> _baggages = new Dictionary<string, IBaggage>();

        public SceneBaggage SetString(string key,string value)
        {
            AddOrUpdate(key, new StringBaggage(value));
            return this;
        }
        public SceneBaggage SetInt(string key,int value)
        {
            AddOrUpdate(key, new IntBaggage(value));
            return this;
        }
        public SceneBaggage SetFloat(string key, float value)
        {
            AddOrUpdate(key, new FloatBaggage(value));
            return this ;
        }
        public SceneBaggage SetBool(string key, bool value)
        {
            AddOrUpdate(key, new BoolBaggage(value));
            return this;
        }

        public SceneBaggage AddBaggage(string key, IBaggage baggage)
        {
            AddOrUpdate(key, baggage);
            return this;
        }

        public string GetString(string key)
        {
            TryCheckValueByKey(key, out IBaggage rawBaggage);
            
            StringBaggage baggage = rawBaggage as StringBaggage;

            return baggage?.GetBaggage() ??
                   throw new NullReferenceException($"The key \"{key}\" does not refer to the specified data type");
        }

        public int GetInt(string key)
        {
            TryCheckValueByKey(key, out IBaggage rawBaggage);
            
            IntBaggage baggage = _baggages[key] as IntBaggage;

            return baggage?.GetBaggage() ??
                   throw new NullReferenceException($"The key \"{key}\" does not refer to the specified data type");
        }
        public float GetFloat(string key)
        {
            TryCheckValueByKey(key, out IBaggage rawBaggage);
            
            FloatBaggage baggage = _baggages[key] as FloatBaggage;

            return baggage?.GetBaggage() ??
                   throw new NullReferenceException($"The key \"{key}\" does not refer to the specified data type");
        }
        public bool GetBool(string key)
        {
            TryCheckValueByKey(key, out IBaggage rawBaggage);
            
            BoolBaggage baggage = _baggages[key] as BoolBaggage;

            return baggage?.GetBaggage() ??
                   throw new NullReferenceException($"The key \"{key}\" does not refer to the specified data type");
        }

        private void AddOrUpdate(string key, IBaggage rawBaggage)
        {
            if(IsContainsKey(key)) _baggages[key] = rawBaggage;
            else _baggages.Add(key, rawBaggage);
        }

        private bool IsContainsKey(string key) => _baggages.ContainsKey(key);

        private bool TryCheckValueByKey(string key, out IBaggage value)
        {
            if(!IsContainsKey(key))
                throw new KeyNotFoundException("The key is not contained in the dictionary");

            value = _baggages[key];
            return true;
        }
    }
}