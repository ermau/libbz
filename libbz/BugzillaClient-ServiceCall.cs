//
// BugzillaClient-ServiceCall.cs
//
// Author:
//       Bojan Rajkovic <brajkovic@coderinserepeat.com>
//
// Copyright (c) 2011-2013 Bojan Rajkovic
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
using System.Threading;
using System.Threading.Tasks;
using CodeRinseRepeat.Bugzilla;
using Hyena.Json;

namespace CodeRinseRepeat.Bugzilla
{
	public partial class BugzillaClient
	{
		Dictionary<string, object> DoServiceCall (string method, params object[] parameters)
		{
			var callId = Interlocked.Increment (ref this.callId);

			var callObject = new Dictionary<string, object> {
				{"id", callId},
				{"method", method},
				{"params", parameters}
			};
			
			#if DEBUG
			Console.Error.Write ("Serializing JSON-RPC call object...");
			Stopwatch s = new Stopwatch ();
			s.Start ();
			#endif
			string jsonPayload = new Serializer (callObject).Serialize ();
			#if DEBUG
			s.Stop ();
			Console.Error.WriteLine ("done in {0}.", s.Elapsed);
			Console.Error.WriteLine ("Serialized payload: {0}", jsonPayload);
			#endif
			
			var serviceClient = new CookieClient ();
			
			if (cookies != null)
				serviceClient.Cookies = cookies;
			
			serviceClient.Headers.Add (HttpRequestHeader.ContentType, "application/json");
			
			#if DEBUG
			Console.Error.Write ("Making request...");
			s.Reset ();
			s.Start ();
			#endif
			
			string responseJson = serviceClient.UploadString (ServiceUri, jsonPayload);
			
			#if DEBUG
			s.Stop ();
			Console.Error.WriteLine ("done in {0}.", s.Elapsed);
			Console.Error.WriteLine ("Response: {0}", responseJson);
			#endif
			
			cookies = serviceClient.Cookies;
			
			#if DEBUG
			s.Reset ();
			Console.Error.Write ("Deserializing result...");
			s.Start ();
			#endif
			var response = new Deserializer (responseJson).Deserialize () as Dictionary<string, object>;
			#if DEBUG
			s.Stop ();
			Console.Error.WriteLine ("done in {0}.", s.Elapsed);
			#endif
			
			if ((long)response ["id"] != callId)
				throw new Exception (string.Format ("Response ID and original call ID don't match. Expected {0}, received {1}.", callId, response ["id"]));
			
			if (response ["error"] != null)
				throw new Exception (string.Format ("Received error message from Bugzilla. Message: {0}", ((JsonObject)response ["error"]) ["message"]));
			
			return response;
		}
		
		Task<Dictionary<string, object>> DoServiceCallAsync (string method, params object[] parameters)
		{
			return Task.Factory.StartNew (() => DoServiceCall (method, parameters), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
		}
	}
}

