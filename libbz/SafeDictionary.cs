//
// SafeDictionary.cs
//
// Author:
//       Bojan Rajkovic <bojan.rajkovic@xamarin.com>
//
// Copyright (c) 2013 Xamarin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Collections;

namespace CodeRinseRepeat.Bugzilla
{
	public static partial class DictionaryExtensions
	{
		public static IDictionary<TKey, TValue> WrapInMissingKeySafeDictionary<TKey, TValue> (this IDictionary<TKey, TValue> self) 
		{
			return new SafeDictionary<TKey, TValue> (self);
		}
	}
	public class SafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
	{
		IDictionary<TKey, TValue> dictionary;
		
		internal SafeDictionary ()
		{
			dictionary = new Dictionary<TKey, TValue> ();
		}
		
		internal SafeDictionary (IDictionary<TKey, TValue> dictionary)
		{
			this.dictionary = dictionary;
		}
		
		void IDictionary<TKey, TValue>.Add (TKey key, TValue value)
		{
			dictionary.Add (key, value);
		}
		
		public bool ContainsKey (TKey key)
		{
			return dictionary.ContainsKey (key);
		}
		
		public ICollection<TKey> Keys {
			get { return dictionary.Keys; }
		}
		
		bool IDictionary<TKey, TValue>.Remove (TKey key)
		{
			return dictionary.Remove (key);
		}
		
		public bool TryGetValue (TKey key, out TValue value)
		{
			return dictionary.TryGetValue (key, out value);
		}
		
		public ICollection<TValue> Values {
			get { return dictionary.Values; }
		}
		
		public TValue this [TKey key] {
			get {
				if (dictionary.ContainsKey (key))
					return dictionary [key];
				return default(TValue);
			}
		}
		
		TValue IDictionary<TKey, TValue>.this [TKey key] {
			get {
				if (dictionary.ContainsKey (key))
					return dictionary [key];
				return default(TValue);
			}
			set {
				dictionary [key] = value;
			}
		}
		
		void ICollection<KeyValuePair<TKey, TValue>>.Add (KeyValuePair<TKey, TValue> item)
		{
			dictionary.Add (item);
		}
		
		void ICollection<KeyValuePair<TKey, TValue>>.Clear ()
		{
			dictionary.Clear ();
		}
		
		public bool Contains (KeyValuePair<TKey, TValue> item)
		{
			return dictionary.Contains (item);
		}
		
		public void CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			dictionary.CopyTo (array, arrayIndex);
		}
		
		public int Count {
			get { return dictionary.Count; }
		}
		
		public bool IsReadOnly {
			get { return true; }
		}
		
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove (KeyValuePair<TKey, TValue> item)
		{
			return dictionary.Remove (item);
		}
		
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
		{
			return dictionary.GetEnumerator ();
		}
		
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}
}

