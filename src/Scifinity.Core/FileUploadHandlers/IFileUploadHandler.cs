using Scifinity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scifinity.Core.FileUploadHandlers
{
    public interface IFileUploadHandler
    {
        public void Upload(CodeUpload codeUpload);
        public IFileUploadHandler SetNext(IFileUploadHandler handler);
    }
}
