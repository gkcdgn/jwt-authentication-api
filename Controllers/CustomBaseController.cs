using Microsoft.AspNetCore.Mvc;
using UdemySharedLibrary.Dtos;

namespace UdemyAuthServer.API.Controllers
{
    public class CustomBaseController : ControllerBase
    {/*
      Amaç: Tüm API cevaplarını tek tip (standart) hale getirmek.
      “Service’ten gelen Response’u düzgün bir HTTP cevabına çevirir”
      Bu sınıfın amacı:API cevaplarını tek tip yapmak,StatusCode yönetimini merkezi yapmak,Controller’lardaki kod tekrarını azaltmak
     : ControllerBase CustomBaseController, ASP.NET Core’un kendi sınıfı olan ControllerBase’den miras alıyor.“Ben ASP.NET Core’un bir API Controller’ıyım ve
tüm controller’lar için ortak olacak bir temel sınıf yazıyorum.”


        ActionResultInstance<T>()Bu metodu:AuthController’da,UserController’da,ProductController’da,hepsinde kullanıyorsun.
      
      */
        public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
        {
           return new ObjectResult(response)
           {
               StatusCode = response.StatusCode
           };

        }
    }
        
    
}
