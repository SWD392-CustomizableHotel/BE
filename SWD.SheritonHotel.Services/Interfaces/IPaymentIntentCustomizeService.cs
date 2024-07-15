using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Services.Interfaces
{
    public interface IPaymentIntentCustomizeService
    {
        Task<List<string>> CreatePaymentIntent(CreatePaymentIntentDTO paymentIntentDto);
    }
}
