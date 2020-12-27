using System;
using System.Collections.Generic;
using System.Text;

namespace VendyClovece.Backend
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T instance = null;
        public static T Instance => instance;

        public Singleton()
        {
            if (Instance != null)
                throw new Exception("More than one instance of singleton");
            instance = (T)this;
        }
    }
}
