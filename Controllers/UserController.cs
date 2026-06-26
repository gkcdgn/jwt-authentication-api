using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UdemyCoreLayer.Dtos;
using UdemyCoreLayer.Services;
using UdemyServiceLayer;
using UdemySharedLibrary.Dtos;

namespace UdemyAuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {/*
      UserController kullanıcı oluşturma ve giriş yapmış kullanıcının bilgilerini alma işini yapar.
      
      AuthController = kimlik doğrulama / token işleri
      UserController = kullanıcı oluşturma ve kullanıcı bilgisi işleri
      UserController’ın amacı:Yeni kullanıcı oluşturmak (register / signup),Giriş yapmış kullanıcının kendi bilgilerini almak Yani bu sınıf kullanıcı yönetimi API’si.
      Controller → Service → Repository → Database
      CerateUser:Yeni kullanıcı oluşturur (kayıt olma işlemi) Frontend’ten gelen:Email,Username,Passwordgibi bilgileri alır ve kullanıcıyı veritabanına kaydeder.
      
        İstek gelir,CreateUserDto ile bilgiler alınır ,_userService.CreateUserAsync(...) çağrılır,Sonuç Response<T> olarak gelir,ActionResultInstance ile düzgün HTTP cevabı döner

        GetUsers :Giriş yapmış kullanıcının bilgilerini alır.Burada Authorization attribute’u var, yani bu metoda erişmek için kullanıcı giriş yapmış (authenticated) olmalı.
       Token içinden: HttpContext.User.Identity.Name   Buradan:Kullanıcının username’i alınır ,O username ile veritabanından kullanıcı bilgisi çekilir

        AuthController → Token üretir / yeniler / iptal eder

       UserController → Kullanıcı oluşturur, kullanıcı bilgisi verir

       CustomBaseController → Ortak response yapısı sağlar

      Service katmanı → Asıl iş mantığı burada

        Repository → Veritabanı işleri


      */
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;
           
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var result = await _userService.CreateUserAsync(createUserDto);
            return ActionResultInstance(result);
        }


        //[Authorize]
        //[HttpGet("getuser")]
        //public async Task<IActionResult> GetUsers()
        //{
        //    var result = await _userService.GetUserByNameAsync("bebis");
        //    return ActionResultInstance(result);
        //}


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUsers()
        {
          return  ActionResultInstance( await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }

        [HttpPost("CreateUserRoles/{userName}")]
        public async Task<IActionResult> CreateUserRoles(string userName)
        {
            return ActionResultInstance(await _userService.CreateUserRoles(userName));
        }





        }
}
