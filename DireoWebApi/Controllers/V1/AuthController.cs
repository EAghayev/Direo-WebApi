using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DireoWebApi.Data;
using DireoWebApi.Models;
using DireoWebApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DireoWebApi.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace DireoWebApi.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private DireoContext _context;
        private IConfiguration _configuration;
        private IMapper _mapper;

        public AuthController(DireoContext context,IConfiguration configuration,IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<object> Register(UserSignUpDTO usr)
        {
            
            #region CheckEmailExist
            if (await _context.Users.AnyAsync(u=>u.Email==usr.Email))
                {
                   ModelState.AddModelError("Exist email","This email had been registered");
                }
            #endregion

            #region CheckPasswordIsInvalid
            if (!PasswordValid.Valid(usr.Password))
            {
                ModelState.AddModelError("Password", PasswordValid.Message);
            }
            #endregion

            #region CheckModelIsInvalid
                if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            usr.Password = CryptoHelper.Crypto.HashPassword(usr.Password);
            User user = new User();
            try
            {
                user = _mapper.Map<User>(usr);
                user.CreatedAt = DateTime.UtcNow.AddHours(4);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(409);
            }
            return Ok(_mapper.Map<UserGetDTO>(user));

        }

        [HttpPost("login")]
        public async Task<object> Login([FromBody]UserSignInDTO usr)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == usr.Email);

            #region CheckIsNull
            if (user == null)
            {
                return NotFound();
            }
            #endregion

            #region CheckPasswordIsIncorrect
            if (!CryptoHelper.Crypto.VerifyHashedPassword(user.Password, usr.Password))
            {
                return StatusCode(401, "Password is incorrect");
            }
            #endregion

            #region CheckModelIsInvalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id)
                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {
                id=user.Id,
                token=tokenString
            });
        }
    }
}