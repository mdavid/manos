//
// Copyright (C) 2010 Jackson Harper (jackson@manosdemono.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//


using System;
using Manos.IO;

namespace Manos.Http.Testing
{
    public class MockHttpTransaction : IHttpTransaction
    {
        private bool aborted;

        public MockHttpTransaction(IHttpRequest request, IHttpResponse response)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            Request = request;

            Response = response;
        }

        public String ResponseString
        {
            get { return (Response as MockHttpResponse).ResponseString(); }
        }

        public int AbortedStatusCode { get; private set; }

        public string AbortedMessage { get; private set; }

        public bool Finished { get; private set; }

        // TODO: I guess we need a mock server?

        #region IHttpTransaction Members

        public HttpServer Server
        {
            get { return null; }
        }

        public Context Context { get; private set; }

        public IHttpRequest Request { get; private set; }

        public IHttpResponse Response { get; private set; }

        public bool Aborted { get; private set; }

        public bool ResponseReady { get; private set; }

        public void Abort(int status, string message, params object[] p)
        {
            Aborted = true;
            AbortedStatusCode = status;
            AbortedMessage = String.Format(message, p);
        }

        public void OnRequestReady()
        {
        }

        public void OnResponseFinished()
        {
        }

        #endregion

        public void Finish()
        {
            Finished = true;
        }
    }
}