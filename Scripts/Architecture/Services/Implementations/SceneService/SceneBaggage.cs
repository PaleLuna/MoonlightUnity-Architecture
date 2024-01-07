using System;
using System.Collections.Generic;

namespace PaleLuna.Architecture.Services
{
    public class SceneBaggage
    {
        private Dictionary<string, IBaggage> _baggages = new Dictionary<string, IBaggage>();

        public SceneBaggage SetInt(string key,int num)
        {
            _baggages.Add(key, new IntBaggage(num));
            return this;
        }
        public SceneBaggage SetFloat(string key, float num)
        {
            _baggages.Add(key, new FloatBaggage(num));
            return this;
        }
        public SceneBaggage SetBool(string key, bool val)
        {
            _baggages.Add(key, new BoolBaggage(val));
            return this;
        }

        public int GetInt(string key)
        {
            if (!IsContainsKey(key))
                throw new KeyNotFoundException("The key is not contained in the dictionary");
            
            IntBaggage baggage = _baggages[key] as IntBaggage;

            return baggage?.GetBaggage() ??
                   throw new NullReferenceException($"The key \"{key}\" does not refer to the specified data type");
        }
        public float GetFloat(string key)
        {
            if (!IsContainsKey(key))
                throw new KeyNotFoundException("The key is not contained in the dictionary");
            
            FloatBaggage baggage = _baggages[key] as FloatBaggage;

            return baggage?.GetBaggage() ??
                   throw new NullReferenceException($"The key \"{key}\" does not refer to the specified data type");
        }
        public bool GetBool(string key)
        {
            if (!IsContainsKey(key))
                throw new KeyNotFoundException("The key is not contained in the dictionary");
            
            BoolBaggage baggage = _baggages[key] as BoolBaggage;


            return baggage?.GetBaggage() ??
                   throw new NullReferenceException($"The key \"{key}\" does not refer to the specified data type");
        }

        private bool IsContainsKey(string key)
        {
            return _baggages.ContainsKey(key);
        }
    }
}