namespace TappApi.Factories;
using TappApi.Interfaces;


public static class PaymentFactory {

    public static IPaymentInterface CreatePayment(string paymentType) {
        switch (paymentType) {
            case "credit_card":
                return new CreditCardPayment();
            case "paypal":
                return new PaypalPayment();
            default:
                throw new Exception("Invalid payment type");
        }
    }
}