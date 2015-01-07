
// 
// Bug.cs
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
using Hyena.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeRinseRepeat.Bugzilla
{
	public class Bug
	{
		#region Properties
		public string Priority { get; private set; }
		public long[] BlocksOn { get; private set; }
		public string Creator { get; private set; }
		public long? DuplicateOf { get; private set; }
		public DateTime LastChanged { get; private set; }
		public string[] Keywords { get; private set; }
		public string[] Subscribers { get; private set; }
		public string AssignedTo { get; private set; }
		public string[] Groups { get; private set; }
		public string[] SeeAlso { get; private set; }
		public long[] DependsOn { get; private set; }
		public long Id { get; private set; }
		public string Resolution { get; private set; }
		public string Classification { get; private set; }
		public string Status { get; private set; }
		public string Summary { get; private set; }
		public string Severity { get; private set; }
		public string Version { get; private set; }
		public string Component { get; private set; }
		public string Product { get; private set; }
		public string Milestone { get; private set; }
		public string Url { get; private set; }
		#endregion

		public IDictionary<string, object> Attributes { get; private set; }

		public override string ToString ()
		{
			return string.Format ("[Bug: Priority={0}, BlocksOn={1}, Creator={2}, DuplicateOf={3}, LastChanged={4}, " +
				"Keywords={5}, Subscribers={6}, AssignedTo={7}, Groups={8}, SeeAlso={9}, DependsOn={10}, Id={11}, " +
				"Resolution={12}, Classification={13}, Status={14}, Summary={15}, Severity={16}, Version={17}, " +
				"Component={18}, Product={19}, Milestone={20}, Url={21}]", Priority, BlocksOn, Creator, DuplicateOf, LastChanged,
				Keywords, Subscribers, AssignedTo, Groups, SeeAlso, DependsOn, Id, Resolution, Classification, Status,
				Summary, Severity, Version, Component, Product, Milestone, Url
			);
		}
		

		public static Bug FromJsonObject (IDictionary<string, object> jsonObject)
		{
			jsonObject = jsonObject.WrapInMissingKeySafeDictionary ();
			var bug = new Bug ();

			try {
				bug.AssignedTo = (string) jsonObject["assigned_to"];
				bug.BlocksOn = ((JsonArray) jsonObject["blocks"]).Cast<long> ().ToArray ();
				bug.Classification = (string) jsonObject["classification"];
				bug.Component = (string) jsonObject["component"];
				bug.Creator = (string) jsonObject["creator"];
				bug.DependsOn = ((JsonArray) jsonObject["depends_on"]).Cast<long> ().ToArray ();
				bug.DuplicateOf = (long?) jsonObject["duplicate_of"];
				bug.Groups = ((JsonArray) jsonObject["groups"]).Cast<string> ().ToArray ();
				bug.Id = (long) jsonObject["id"];
				bug.Keywords = ((JsonArray) jsonObject["keywords"]).Cast<string> ().ToArray ();
				bug.LastChanged = DateTime.SpecifyKind (DateTime.Parse ((string) jsonObject["last_change_time"]), DateTimeKind.Utc);
				bug.Milestone = (string) jsonObject["target_milestone"];
				bug.Priority = (string) jsonObject["priority"];
				bug.Product = (string) jsonObject["product"];
				bug.Resolution = (string) jsonObject["resolution"];
				bug.SeeAlso = ((JsonArray) jsonObject["see_also"]).Cast<string> ().ToArray ();
				bug.Severity = (string) jsonObject["severity"];
				bug.Status = (string) jsonObject["status"];
				bug.Subscribers = ((JsonArray) jsonObject["cc"]).Cast<string> ().ToArray ();
				bug.Summary = (string) jsonObject["summary"];
				bug.Version = (string) jsonObject["version"];
				bug.Attributes = jsonObject.AsReadOnly ();
				bug.Url = (string) jsonObject["url"];
			} catch (Exception e) {
				#if DEBUG
				Console.Error.WriteLine(jsonObject.ToString());
				#endif
				Console.WriteLine("Failed to parse bug from JSON: {0}", e.Message);
		}
			return bug;
		}
	}
}

