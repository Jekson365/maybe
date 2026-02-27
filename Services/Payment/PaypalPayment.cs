namespace TappApi.Interfaces;

public class PaypalPayment : IPaymentInterface {

    public string Pay(string payType)
    {
        return $"{payType} payed with!";
    }
}