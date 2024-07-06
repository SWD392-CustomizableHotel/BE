using AutoMapper;
using Google.Cloud.Vision.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SWD.SheritonHotel.Data.Base;
using SWD.SheritonHotel.Data.Context;
using SWD.SheritonHotel.Data.Repositories.Interfaces;
using SWD.SheritonHotel.Domain.DTO;
using SWD.SheritonHotel.Domain.Entities;
using SWD.SheritonHotel.Domain.OtherObjects;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SWD.SheritonHotel.Data.Repositories
{
    public class IdentityCardRepository : BaseRepository<IdentityCard>, IIdentityCardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public IdentityCardRepository(ApplicationDbContext context, IMapper mapper, IHttpClientFactory httpClientFactory, IConfiguration configuration, IOptions<FPTAIOptions> options) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _apiKey = options.Value.ApiKey;
        }

        public async Task<IdentityCardDto> UploadIdentityCardAsync(IFormFile frontFile, string userId, CancellationToken cancellationToken)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), "UploadIdentityCard");
            Directory.CreateDirectory(tempDirectory);

            var frontFilePath = Path.Combine(tempDirectory, $"{Guid.NewGuid()}_front.tmp");
            using (var stream = new FileStream(frontFilePath, FileMode.Create))
            {
                await frontFile.CopyToAsync(stream, cancellationToken);
            }

            var frontDetails = await ExtractIdentityCardDetailsAsync(frontFilePath, userId);

            if (frontDetails == null)
                throw new Exception("Unable to extract information from Identity ID card.");

            frontDetails.UserId = userId;

            await AddAsync(frontDetails, cancellationToken);

            return frontDetails;
        }

        private async Task AddAsync(IdentityCardDto identityCardDto, CancellationToken cancellationToken)
        {
            var identityCard = _mapper.Map<IdentityCard>(identityCardDto);
            identityCard.UserId = identityCardDto.UserId;

            identityCard.Code = GenerateRandomCode();
            identityCard.CreatedBy = identityCardDto.UserId;
            identityCard.CreatedDate = DateTime.UtcNow;
            identityCard.LastUpdatedBy = identityCardDto.UserId;
            identityCard.LastUpdatedDate = DateTime.UtcNow;

            Add(identityCard);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private string GenerateRandomCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }

        private async Task<IdentityCardDto> ExtractIdentityCardDetailsAsync(string filePath, string userId)
        {
            // Create HttpClient by using HttpClientFactory and add api key 
            using var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("api_key", _apiKey);

            // Create MultipartFormDataContent and add image file to it
            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(new FileStream(filePath, FileMode.Open));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            content.Add(fileContent, "image", Path.GetFileName(filePath));

            // Request POST method to API FPT AI with content provided by FPT AI
            var response = await client.PostAsync("https://api.fpt.ai/vision/idr/vnm", content);
            response.EnsureSuccessStatusCode();

            // Read the response body from the API as a string or JSON file
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<FPTResponse>(responseBody);

            if (result?.Data == null || !result.Data.Any())
            {
                throw new Exception("Unable to extract information from Identity Card.");
            }

            var cardData = result.Data.First();
            var identityCardDto = new IdentityCardDto
            {
                FullName = cardData.Name,
                DateOfBirth = DateTime.Parse(cardData.Dob),
                CardNumber = cardData.Id,
                Gender = cardData.Sex,
                Nationality = cardData.Nationality,
                Address = cardData.Address,
                UserId = userId
            };

            return identityCardDto;
        }
    }
}