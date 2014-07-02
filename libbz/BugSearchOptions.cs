//
// BugSearchOptions.cs
//
// Author:
//       pjbeaman <>
//
// Copyright (c) 2014 pjbeaman
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

namespace CodeRinseRepeat.Bugzilla
{
	public class BugSearchOptions
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeRinseRepeat.Bugzilla.BugSearchOptions"/> class.
		/// Set properties to add parameters to the search.
		/// </summary>
		public BugSearchOptions ()
		{
		}

		public string Alias { get; set; }

		public string Assigned_To { get; set; }

		public string Component { get; set; }

		public DateTime? Creation_Time  { get; set; }

		public string Id { get; set; }

		public DateTime? Last_Change_Time { get; set; }

		public int? Limit { get; set; }

		public int? Offset { get; set; }

		public string OperatingSystem { get; set; }

		public string Platform { get; set; }

		public string Priority { get; set; }

		public string Product { get; set; }

		public string Reporter { get; set; }

		public string Resolution { get; set; }

		public List<string> ResolutionList { get; set; }

		public string Severity { get; set; }

		public string Status { get; set; }

		public List<string> StatusList { get; set; }

		public string Summary { get; set; }

		public string Target_Milestone { get; set; }

		public string QA_Contact { get; set; }

		public string URL { get; set; }

		public string Version { get; set; }

		public int? Votes { get; set; }

		public string Whiteboard { get; set; }

		public Dictionary<string, object> ToDictionary ()
		{
			var optionDict = new Dictionary<string, object> ();
			if (!string.IsNullOrEmpty (Alias))
				optionDict.Add ("alias", Alias);
			if (!string.IsNullOrEmpty (Assigned_To))
				optionDict.Add ("assigned_to", Assigned_To);

			if (!string.IsNullOrEmpty (Component))
				optionDict.Add ("component", Component);

			if (Creation_Time != null)
				optionDict.Add ("creation_time", Creation_Time.ToString ());

			if (!string.IsNullOrEmpty (Id))
				optionDict.Add ("id", Id);

			if (Last_Change_Time != null)
				optionDict.Add ("last_change_time", Last_Change_Time.ToString ());

			if (Limit != null)
				optionDict.Add ("limit", Limit.ToString ());

			if (Offset != null)
				optionDict.Add ("offset", Offset.ToString ());

			if (!string.IsNullOrEmpty (OperatingSystem))
				optionDict.Add ("op_sys", OperatingSystem);

			if (!string.IsNullOrEmpty (Platform))
				optionDict.Add ("platform", Platform);

			if (!string.IsNullOrEmpty (Priority))
				optionDict.Add ("priority", Priority);

			if (!string.IsNullOrEmpty (Product))
				optionDict.Add ("product", Product);

			if (!string.IsNullOrEmpty (Reporter))
				optionDict.Add ("reporter", Reporter);

			if (!string.IsNullOrEmpty (Resolution))
				optionDict.Add ("resolution", Resolution);

			if (ResolutionList != null) {
				optionDict.Add ("resolution", ResolutionList);
			}

			if (!string.IsNullOrEmpty (Status))
				optionDict.Add ("bug_status", Status);

			if (StatusList != null) {
				optionDict.Add ("bug_status", StatusList);
			}

			if (!string.IsNullOrEmpty (Severity))
				optionDict.Add ("severity", Severity);

			if (!string.IsNullOrEmpty (Summary))
				optionDict.Add ("summary", Summary);

			if (!string.IsNullOrEmpty (Target_Milestone))
				optionDict.Add ("target_milestone", Target_Milestone);

			if (!string.IsNullOrEmpty (QA_Contact))
				optionDict.Add ("qa_contact", QA_Contact);

			if (!string.IsNullOrEmpty (URL))
				optionDict.Add ("url", URL);

			if (!string.IsNullOrEmpty (Version))
				optionDict.Add ("version", Version);

			if (Votes != null)
				optionDict.Add ("votes", Votes.ToString ());

			if (!string.IsNullOrEmpty (Id))
				optionDict.Add ("whiteboard", Whiteboard);

			if (optionDict.Count > 0)
				return optionDict;

			return null;
		}


	}
		
}

