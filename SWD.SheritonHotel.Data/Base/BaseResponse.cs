using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Base
{
    public class BaseResponse<T>
    {
        public bool IsSucceed { get; set; }
        public T Result { get; set; }
        public T[] Results { get; set; }
        public string Message { get; set; }

        public BaseResponse() { }

        public BaseResponse(bool isSucceed, T result, T[] results, string message )
        {
            IsSucceed = isSucceed;
            Result = result;
            Results = results;
            Message = message;
        }
    }
}
