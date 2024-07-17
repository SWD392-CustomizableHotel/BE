using AutoMapper;
using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PaymentRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreatePaymentAsync(Payment payment)
        {
            Add(payment);
            await _context.SaveChangesAsync();
            return payment.Id;
        }

        public async Task<PaymentDto> GetPaymentByBookingIdAsync(int bookingId)
        {
            var payment = await _context.Payment
                .Include(p => p.IdentityCard)
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);

            if (payment == null) return null;

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<Payment> GetPaymentByPaymentIntentIdAsync(string paymentIntentId)
        {
            return await _context.Payment.FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payment.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
