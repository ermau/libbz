//
// BugzillaClient-ServiceCall.cs
//
// Authors:
//       Bojan Rajkovic <brajkovic@coderinserepeat.com>
//       Eric Maupin <me@ermau.com>
//
// Copyright (c) 2011-2013 Bojan Rajkovic
// Copyright (c) 2015 Xamarin Inc.
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
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CodeRinseRepeat.Bugzilla;
using Hyena.Json;

namespace CodeRinseRepeat.Bugzilla
{
	public partial class BugzillaClient
	{
		async Task<Dictionary<string, object>> DoServiceCallAsync (string method, params object[] parameters)
		{
			var callId = Interlocked.Increment (ref this.callId);

			var callObject = new Dictionary<string, object> {
				{"id", callId},
				{"method", method},
				{"params", parameters}
			};
			
			#if TRACE
			Debug.WriteLine ("Serializing JSON-RPC call object...");
			Stopwatch s = new Stopwatch ();
			s.Start ();
			#endif
			string jsonPayload = await Task.Run (() => new Serializer (callObject).Serialize()).ConfigureAwait (false);
			#if TRACE
			s.Stop ();
			Debug.WriteLine ("...done in {0}.", s.Elapsed);
			Debug.WriteLine ("Serialized payload: {0}", jsonPayload);
			#endif

			if (cookies == null)
				cookies = new CookieContainer();

			HttpClient serviceClient = new HttpClient (new HttpClientHandler {
				CookieContainer = cookies,
			});

			#if TRACE
			Debug.WriteLine ("Making request...");
			s.Reset ();
			s.Start ();
			#endif

			var r = await serviceClient.SendAsync (new HttpRequestMessage (HttpMethod.Post, ServiceUri) {
				Content = new StringContent (jsonPayload, Encoding.UTF8, "application/json")
			}).ConfigureAwait (false);

			var responseJson = await r.Content.ReadAsStringAsync().ConfigureAwait (false);
			
			#if TRACE
			s.Stop ();
			Debug.WriteLine ("...done in {0}.", s.Elapsed);
			Debug.WriteLine ("Response: {0}", responseJson);
			#endif
			
			#if TRACE
			s.Reset ();
			Debug.WriteLine ("Deserializing result...");
			s.Start ();
			#endif
			var response = await Task.Run (() => new Deserializer (responseJson).Deserialize () as Dictionary<string, object>).ConfigureAwait (false);
			#if TRACE
			s.Stop ();
			Debug.WriteLine ("done in {0}.", s.Elapsed);
			#endif
			
			if ((long)response ["id"] != callId)
				throw new Exception (string.Format ("Response ID and original call ID don't match. Expected {0}, received {1}.", callId, response ["id"]));
			
			if (response ["error"] != null)
				throw new Exception (string.Format ("Received error message from Bugzilla. Message: {0}", ((JsonObject)response ["error"]) ["message"]));
			
			return response;
		}
	}
}

