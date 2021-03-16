using Scifinity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scifinity.Core.FileUploadHandlers
{
    public abstract class AbstractFileUploadHandler : IFileUploadHandler
    {
        private IFileUploadHandler nextHandler;
        public virtual async Task UploadAsync(CodeUpload codeUpload)
        {
            if (nextHandler != null)
            {
                await nextHandler.UploadAsync(codeUpload);

                return;
            }
        }

        public IFileUploadHandler SetNext(IFileUploadHandler handler)
        {
            nextHandler = handler;

            return nextHandler;
        }
    }
}
