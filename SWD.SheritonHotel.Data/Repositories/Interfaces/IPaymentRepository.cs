using Entities;
using SWD.SheritonHotel.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<int> CreatePaymentAsync(Payment payment);
        Task UpdatePayment(Payment payment);
    }
}
