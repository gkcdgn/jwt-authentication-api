
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyCoreLayer.Dtos;
using UdemyCoreLayer.Services;

namespace UdemyAuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {/*
      
      Burası: Dış dünyadan gelen HTTP isteklerini karşılayan kapı
      CreateToken – Kullanıcı giriş yapınca token üretir
      Client → email & password gönderir;Controller alır ve Service’e yollar:Kullanıcıyı bulur,Şifreyi kontrol eder,JWT üretir,Refresh token üretir
     DB’ye kaydeder..Sonuç geri gelir ,ActionResultInstance ile:StatusCode ayarlanır ve Token JSON olarak döner
      
        
        
        CreateTokenByClient – Sistemler (client) için token:Bu insan değil, başka bir uygulama için Örnek:MiniApp,MobileApp,Başka bir API
     Burada: User yok,Refresh token yok,Sadece access token var
      
      RevokeRefreshToken – Logout işlemi :Kullanıcı çıkış yaptıysa (logout) Refresh token’ı iptal et.Client refresh token gönderir.Service:DB’de bulur ve Siler.
     Artık o refresh token ile: Yeni token alınamaz..


        CreateTokenByRefreshToken – Refresh token ile yeni token alma:Kullanıcı access token süresi dolduğunda,Refresh token ile yeni access token alabilir.
        Client refresh token gönderir..Service:Token DB’de var mı? Süresi dolmuş mu? Kullanıcıyı bulur,Yeni JWT + yeni refresh token üretir,Eskisini günceller,Yeni tokenlar döner
        
        
        
        
        Client
         ↓
      AuthController   (sadece yönlendirir)
          ↓
      AuthenticationService   (tüm iş mantığı burada)
           ↓
      TokenService / UserManager / Repository
           ↓
       Database

      
      */
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);
            return ActionResultInstance(result);
        }


        [HttpPost]
        public  IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result =  _authenticationService.CreateTokenByClient(clientLoginDto);
            return ActionResultInstance(result);

        }
        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token);
            return ActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);
            return ActionResultInstance(result);
        }

















    }
}

