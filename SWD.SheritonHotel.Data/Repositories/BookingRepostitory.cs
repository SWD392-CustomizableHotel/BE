using System.Linq.Dynamic.Core;
using AutoMapper;
using Entities;
using Microsoft.EntityFrameworkCore;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;

namespace SWD.SheritonHotel.Data.Repositories;

public class BookingRepostitory : BaseRepository<Booking>, IBookingRepository
{
    
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BookingRepostitory (ApplicationDbContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<(List<BookingHistoryDto>, int)> GetBookingsByUserIdAsync(string userId, PaginationFilter paginationFilter)
    {
        throw new Exception();
    }
}