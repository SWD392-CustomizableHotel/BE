namespace SWD.SheritonHotel.Domain.DTO
{
    public class ResponseDto<T>
    {
        public ResponseDto()
        {
        }
        public ResponseDto(T data)
        {
            IsSucceeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public T Data { get; set; }
        public bool IsSucceeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}
