namespace Course.Basket.Dtos;

public class BasketDto
{
    public string UserId { get; set; }
    public string DiscountCode { get; set; }
    public List<BasketItemDto> basketItemDto { get; set; }
    public decimal TotalPrice { get => basketItemDto.Sum(x => x.Price * x.Qunatitiy); }
}
