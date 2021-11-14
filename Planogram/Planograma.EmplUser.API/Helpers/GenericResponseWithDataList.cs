using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planograma.EmplUser.API.Helpers
{
    public class GenericResponseWithDataList<T>:GenericResponse
    {
        private List<T> _data;
        

        
        public List<T> data
        {
            get { return _data; }
            set { _data = value; }
        }
        public GenericResponseWithDataList(List<T> data, bool success, string message)
        {
            this.success = success;
            this.data = data;
            this.message = message;
        }
        public GenericResponseWithDataList(List<T> data, bool success = true)
        {
            this.success = success;
            this.data = data;
            this.message = message;
        }

    }
}
