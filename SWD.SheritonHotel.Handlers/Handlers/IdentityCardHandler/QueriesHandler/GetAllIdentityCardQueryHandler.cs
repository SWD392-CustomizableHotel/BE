using MediatR;
using SWD.SheritonHotel.Domain.DTO.IdentityCard;
using SWD.SheritonHotel.Domain.DTO.Responses;
using SWD.SheritonHotel.Domain.Queries.IdentityCardQuery;
using SWD.SheritonHotel.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetAllIdentityCardsQueryHandler : IRequestHandler<GetAllIdentityCardsQuery, ResponseDto<List<IdentityCardDto>>>
{
    private readonly IIdentityCardService _identityCardService;

    public GetAllIdentityCardsQueryHandler(IIdentityCardService identityCardService)
    {
        _identityCardService = identityCardService;
    }

    public async Task<ResponseDto<List<IdentityCardDto>>> Handle(GetAllIdentityCardsQuery request, CancellationToken cancellationToken)
    {
        var identityCards = await _identityCardService.GetAllIdentityCardsAsync(cancellationToken);
        return new ResponseDto<List<IdentityCardDto>>
        {
            IsSucceeded = true,
            Data = identityCards,
            Message = "Identity cards retrieved successfully."
        };
    }
}