// 
// BugzillaClient.cs
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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hyena.Json;

namespace CodeRinseRepeat.Bugzilla
{
	public partial class BugzillaClient : IDisposable
	{
		public Uri ServiceUri { get; private set; }
		long callId;
		CookieContainer cookies;
		bool disposed;

		public BugzillaClient (Uri serviceUri)
		{
			ServiceUri = serviceUri;
		}

		const string Login = "User.login";
		const string BugGet = "Bug.get";
		const string BugComment = "Bug.add_comment";
		const string BugUpdate = "Bug.update";
		const string Logout = "User.logout";
		const string BugSearch = "Bug.search";

		string loginForCurrentUser;

		public Task LoginAsync (string login, string password)
		{
			loginForCurrentUser = login;
			return DoServiceCallAsync (Login, new Dictionary<string, object> {
				{"login", login},
				{"password", password},
			});
		}

		public Task<Bug> GetBugAsync (int bugId)
		{
			return DoServiceCallAsync (BugGet, new Dictionary<string, object> {
				{"ids", new [] { bugId }}
			}).ContinueWith (t => {
				var result = (JsonObject) t.Result["result"];
				return Bug.FromJsonObject (((JsonObject) ((JsonArray) result["bugs"])[0]));
			});
		}

		public Task<long> AddCommentAsync (Bug bug, string comment, bool isPrivate = false, double workTime = 0.0)
		{
			return DoServiceCallAsync (BugComment, new Dictionary<string, object> {
				{"id", bug.Id},
				{"comment", comment},
				{"is_private", isPrivate},
				{"work_time", workTime},
			}).ContinueWith (t => {
				var result = (JsonObject) t.Result["result"];
				return (long) result["id"];
			});
		}

		public Task<bool> UpdateBugAsync (Bug bug, object fields) {
			return DoServiceCallAsync (BugUpdate, new Dictionary<string, object> {
				{"ids", new [] { bug.Id }}
			}.Concat (fields.ToDictionary ()).ToDictionary (t => t.Key, t => t.Value)).ContinueWith (t => {
				return true;
			});
		}

		public Task<IEnumerable<Bug>> GetBugsAsync (params long[] bugIds)
		{
			return DoServiceCallAsync (BugGet, new Dictionary<string, object> {
				{"ids", bugIds}
			}).ContinueWith (t => {
				var result = (JsonObject) t.Result["result"];
				return ((JsonArray) result["bugs"]).Select (bo => Bug.FromJsonObject ((JsonObject) bo));
			});
		}

		public Task<IEnumerable<Bug>> GetBugsAssignedToMeAsync ()
		{
			if (string.IsNullOrWhiteSpace (loginForCurrentUser))
				throw new InvalidOperationException ("You must be logged in to perform that action.");

			return DoServiceCallAsync (BugSearch, new Dictionary<string, object> {
				{"assigned_to", loginForCurrentUser}
			}).ContinueWith (t => {
				var result = (JsonObject)t.Result["result"];
				return ((JsonArray)result["bugs"]).Select (bo => Bug.FromJsonObject ((JsonObject)bo));
			});
		}		
		public Task<IEnumerable<Bug>> GetBugSearchResultsAsync (BugSearchOptions options)
		{
			if (string.IsNullOrWhiteSpace (loginForCurrentUser))
				Console.WriteLine ("WARNING: Search result attempted without login. You will not see private search results.");
		
			return DoServiceCallAsync (BugSearch, options.ToDictionary()
			).ContinueWith (t => {
				var result = (JsonObject)t.Result["result"];
				return ((JsonArray)result["bugs"]).Select (bo => Bug.FromJsonObject ((JsonObject)bo));
			});
		}		

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		~BugzillaClient ()
		{
			Dispose (false);
		}

		private void Dispose (bool disposing)
		{
			if (disposing)
				DoServiceCallAsync (Logout);
		}
	}
}

