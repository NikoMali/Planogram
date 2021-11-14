using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planograma.EmplUser.API.Helpers
{
    public class GenericResponse
    {
      
        private bool _success = true;
        private string _message;

        
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }

        
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }


        public GenericResponse() { }
        public GenericResponse(bool success, string message) 
        {
            this.success = success;
            this.message = message;
        }
        public GenericResponse(bool success)
        {
            this.success = success;
        }





    }
}
