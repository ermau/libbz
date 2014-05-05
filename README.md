What Is It
===========

A Bugzilla client library, written in C#.

What It's Not
=============

* A generic JSON-RPC client (though the bits are all there, more or less).

How
===

* CookieClient (a modified WebClient that exposes and stores cookies).
* Bugzilla's JSON-RPC interface (http://bugzilla.yourcompany.com/jsonrpc.cgi).
* JSON serialization and deserialization by the simple, hackable Hyena.Json (http://git.gnome.org/browse/Hyena).
* Everything is async and task-driven. No synchronous API here.

Requirements
============

A .NET 4.0/C# 4.0 runtime/compiler. Mono 2.8.x or later will do.

Why
===

Why not! Bugzilla API clients are hard to come by.

Using It
========

```csharp
using CodeRinseRepeat.Bugzilla;

var client = new BugzillaClient ("http://bugzilla.yourcompany.com/jsonrpc.cgi");
await client.LoginAsync ("username", "password");
var bug = await client.GetBugAsync (1234);
```

Future
======

* Add more BZ API coverage.
* Make the code better.

License
=======

MIT.
