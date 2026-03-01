
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TappApi.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using TappApi.ViewModels;
using TappApi.Factories;
using TappApi.Interfaces;


namespace TappApi.Controllers;
[ApiController]
[Route("api/paymentss")]
public class PaymentController : ControllerBase
{
    public AppDbContext _db;
    public PaymentController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public ActionResult<IPaymentInterface> Pay([FromBody] PaymentViewModel payment)
    {
        IPaymentInterface paymentInterface = PaymentFactory.CreatePayment(payment.PaymentType); 
        return Ok(paymentInterface.Pay(payment.PaymentType));

    }
}