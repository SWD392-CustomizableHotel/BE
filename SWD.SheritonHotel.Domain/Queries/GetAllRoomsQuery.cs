using MediatR;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.OtherObjects;


namespace SWD.SheritonHotel.Domain.Queries
{
    public class GetAllRoomsQuery : IRequest<PagedResponse<List<RoomDto>>>
    {
        public PaginationFilter PaginationFilter { get; set; }

        public RoomFilter RoomFilter { get; set; }

        public string SearchTerm {  get; set; }
        public GetAllRoomsQuery(PaginationFilter paginationFilter, RoomFilter roomFilter, string searchTerm)
        {
            PaginationFilter = paginationFilter ?? new PaginationFilter();
            RoomFilter = roomFilter ?? new RoomFilter();
            SearchTerm = searchTerm;
        }
    }
}
