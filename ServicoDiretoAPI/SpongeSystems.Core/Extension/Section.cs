using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpongeSolutions.Core.Extension
{
    public class Section : IDisposable
    {
        private readonly ViewContext _viewContext;
        private bool _disposed;
        private TagBuilder _tag = null;

        public Section(ViewContext viewContext, TagBuilder tag)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            viewContext.Writer.Write(tag.ToString(TagRenderMode.StartTag));
            this._viewContext = viewContext;
            this._tag = tag;
        }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        
        {
            if (!this._disposed)
            {
                this._disposed = true;
                this._viewContext.Writer.Write(String.Format("</{0}>", this._tag.TagName));
            }
        }

        public void EndSection()
        {
            this.Dispose(true);
        }
    }
}