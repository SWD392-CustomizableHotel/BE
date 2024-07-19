using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SWD.SheritonHotel.Domain.DTO.Responses
{
    public class BaseResponse<T>
    {
        public bool IsSucceed { get; set; }
        public T? Result { get; set; }
        public List<T> Results { get; set; }
        public string? Message { get; set; }
    }
}
