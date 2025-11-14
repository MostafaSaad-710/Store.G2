namespace Store.G02.Domain.Entities.orders
{
    public enum OrderStatus
    {
        pending= 0, 
        PaymentSuccess = 1, 
        PaymentFailed = 2,
    }
}