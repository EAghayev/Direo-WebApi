using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DireoWebApi.Data;
using DireoWebApi.Helpers;
using DireoWebApi.Models;
using DireoWebApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DireoWebApi.Controllers.V1
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private DireoContext _context;
        private IMapper _mapper;

        public UsersController(DireoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        /*Get All Users*/
        [HttpGet]
        public async Task<object> Get()
        {
            var model = await _context.Users.Include("Socials").ToListAsync();
            return Ok(_mapper.Map<List<UserGetDTO>>(model));
        }

        /*Get User By Id*/
        [HttpGet("{id}")]
        public async Task<object> Get(string id)
        {
            User user = await _context.Users.Include("Socials").FirstOrDefaultAsync(u=>u.Id==id);

            #region CheckIsNull
            if (user == null)
            {
                return NotFound();
            }
            #endregion

            return Ok(_mapper.Map<UserGetDTO>(user));
        }

        /*Update User Info*/
        [HttpPut]
        [Authorize]
        public async Task<object> Put(UserUpdateDTO usr)
        {
            #region CheckIdsIsNotSame
            if (User.GetUserId() != usr.Id)
            {
                return StatusCode(401, "User cannot update another user information");
            }
            #endregion

            User user = _context.Users.Find(User.GetUserId());

            #region CheckIsNull
            if (user == null)
            {
                return NotFound();
            }
            #endregion

            #region CheckEmailIsExisting
            if (_context.Users.Any(u=>u.Email==usr.Email && u.Id != usr.Id))
            {
                ModelState.AddModelError("EmailExisting", "This email had been registered");
            }
            #endregion

            #region CheckPasswordsAreNotSame
            if (usr.Password.Trim() != usr.NewPassword.Trim())
            {
                ModelState.AddModelError("Password", "New Passwords are different");
            }
            #endregion

            #region CheckModelIsInvalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            user.Address = usr.Address;
            user.Email = usr.Email;
            user.FullName = usr.FullName;
            user.Gender = usr.Gender;
            user.Phone = usr.Phone;
            user.Website = usr.Phone;

            if (!string.IsNullOrWhiteSpace(usr.Password) && !string.IsNullOrWhiteSpace(usr.NewPassword))
            {
                user.Password = CryptoHelper.Crypto.HashPassword(usr.Password);
            }

            /*Crete or Update User's socials*/
            if (usr.Socials!=null)
            {
                foreach (var scl in usr.Socials)
                {
                    if (string.IsNullOrWhiteSpace(scl.Id))
                    {
                        if(! await _context.Socials.AnyAsync(s=>s.UserId==user.Id && s.Name==scl.Name && s.Link == scl.Link))
                        {
                            Social social = new Social
                            {
                                Name = scl.Name,
                                Link = scl.Link,
                                Type = SocialType.User,
                                UserId = usr.Id
                            };
                            await _context.Socials.AddAsync(social);
                        }
                        continue;
                    }
                    else
                    {
                        Social social = _context.Socials.Find(scl.Id);
                        if (social != null && social.UserId == usr.Id && social.Type == SocialType.User)
                        {
                            social.Name = scl.Name;
                            social.Link = scl.Link;
                        }
                    }
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return StatusCode(409);
                    }
                }
            }
           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(409);
            }
            return NoContent();
        }

        [HttpPost("ProfileUpload")]
        [Authorize]
        public async Task<object> ProfileUpload(UploadedFile photo)
        {
            #region CheckIsEmpty
            if(string.IsNullOrWhiteSpace(photo.FileRaw) || string.IsNullOrWhiteSpace(photo.FileName))
            {
                return NotFound("Filename and Filerow cannot be empty");
            }
            #endregion

            User user = await _context.Users.FindAsync(User.GetUserId());

            #region CheckIsNull
            if (user == null)
            {
                return StatusCode(201);
            }
            #endregion


            string filename = " ";
            try
            {
                filename = FileManager.Upload(photo.FileRaw,photo.FileName);
            }
            catch (Exception)
            {
            }

            #region CheckFileTypeInvalid
            if (filename == null)
            {
                ModelState.AddModelError("FilType", "Photo type must be png,jpeg or ico");
            }
            #endregion

            #region CheckModelIsInvalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            user.Profile = filename;
            user.ProfileFileName = photo.FileName;
            try
            {
               await  _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(409);
            }
            return Ok(filename);
        }

        [HttpDelete("ProfileDelete")]
        public async Task<object> DeleteProfile()
        {
            User user =await  _context.Users.FindAsync(User.GetUserId());

            #region CheckIsNull
            if (user == null)
            {
                return NotFound();
            }
            #endregion

            user.Profile = null;
            user.ProfileFileName = null;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(409);
            }
            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        public async Task<object> Delete()
        {
            User user =await _context.Users.FindAsync(User.GetUserId());

            #region CheckIsNull
            if (user == null)
            {
                return NotFound();
            }
            #endregion

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(409);
            }
            return NoContent();
        }
    }
    public class UploadedFile
    {
        public string FileRaw { get; set; }
        public string FileName { get; set; }
    }
}