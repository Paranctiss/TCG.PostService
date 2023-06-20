using TCG.PostService.Domain;

namespace TCG.PostService.Application.Consumer.DTO;

public class MerchPostResponse
{
    public Guid MerchPostId { get; set; }
    public string MerchPostName { get; set; }
    public IEnumerable<string> MerchPostNamePhotos { get; set; }
}