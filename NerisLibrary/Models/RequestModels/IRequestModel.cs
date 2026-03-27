using System;
using System.Collections.Generic;
using System.Text;

namespace NerisLibrary.Models.RequestModels
{
    public interface IRequestModel
    {
        public string CreateQueryURI(string baseUrl);
        public bool Validate();
    }
}
