using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExpressionDynamicTest.Parsing.Models
{
    [DebuggerDisplay("{value}", Name = "{key}")]
    internal class KeyValuePairs
    {
        private IDictionary dictionary;
        private object key;
        private object value;
        public KeyValuePairs(IDictionary dictionary, object key, object value)
        {
            this.value = value;
            this.key = key;
            this.dictionary = dictionary;
        }

        public object Key
        {
            get { return key; }
            set
            {
                object tempValue = dictionary[key];
                dictionary.Remove(key);
                key = value;
                dictionary.Add(key, tempValue);
            }
        }

        public object Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                dictionary[key] = this.value;
            }
        }
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(HashtableDebugView))]
    class MyHashtable
    {
        public Hashtable hashtable;

        public MyHashtable()
        {
            hashtable = new Hashtable();
        }

        private string DebuggerDisplay { get { return "Count = " + hashtable.Count; } }

        private class HashtableDebugView
        {
            private MyHashtable myhashtable;
            public HashtableDebugView(MyHashtable myhashtable)
            {
                this.myhashtable = myhashtable;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePairs[] Keys
            {
                get
                {
                    KeyValuePairs[] keys = new KeyValuePairs[myhashtable.hashtable.Count];

                    int i = 0;
                    foreach (object key in myhashtable.hashtable.Keys)
                    {
                        keys[i] = new KeyValuePairs(myhashtable.hashtable, key, myhashtable.hashtable[key]);
                        i++;
                    }
                    return keys;
                }
            }
        }
    }
}
