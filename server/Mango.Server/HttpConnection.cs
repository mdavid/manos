


using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using Mono.Unix.Native;


namespace Mango.Server {

	public class HttpConnection {

		public static void HandleConnection (IOStream stream, Socket socket, HttpRequestCallback callback)
		{
			HttpConnection connection = new HttpConnection (stream, socket, callback);
		}

		public HttpConnection (IOStream stream, Socket socket, HttpRequestCallback callback)
		{
			IOStream = stream;
			Socket = socket;
			HttpRequestCallback = callback;

			stream.ReadUntil ("\r\n\r\n", OnHeaders);
		}

		public IOStream IOStream {
			get;
			private set;
		}

		public Socket Socket {
			get;
			private set;
		}

		public  HttpRequestCallback HttpRequestCallback {
			get;
			private set;
		}

		private void OnHeaders (IOStream stream, byte [] data)
		{
			Console.WriteLine ("HEADERS:");
			Console.WriteLine ("=========");
			Console.WriteLine (Encoding.ASCII.GetString (data));
			Console.WriteLine ("=========");

			string h = Encoding.ASCII.GetString (data);
			StringReader reader = new StringReader (h);

			string verb;
			string path;
			string version;

			string line = reader.ReadLine ();
			ParseStartLine (line, out verb, out path, out version);
			
			HttpHeaders headers = new HttpHeaders ();
			headers.Parse (reader);

			if (headers.ContentLength != null) {
				stream.ReadBytes ((int) headers.ContentLength, OnBody);
			}
		}

		private void ParseStartLine (string line, out string verb, out string path, out string version)
		{
			int s = 0;
			int e = line.IndexOf (' ');

			verb = line.Substring (s, e);

			s = e + 1;
			e = line.IndexOf (' ', s);
			path = line.Substring (s, e - s);

			s = e + 1;
			version = line.Substring (s);

			Console.WriteLine ("VERB: '{0}'  PATH:  '{1}'   VERSION:  '{2}'", verb, path, version);
			if (!version.StartsWith ("HTTP/"))
				throw new Exception ("Malformed HTTP request, no version specified.");
		}

		private void OnBody (IOStream stream, byte [] data)
		{
		}
	}
}
