using TappApi.Interfaces;

public class CreditCardPayment : IPaymentInterface {
    public string Pay(string payType)
    {
        return $"{payType} payed! with";
    }
}