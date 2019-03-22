using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DireoWebApi.Data;
using DireoWebApi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DireoWebApi.Helpers;
using DireoWebApi.Models;
using DireoWebApi.Models.CustomModels;

namespace DireoWebApi.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private DireoContext _context;
        private IMapper _mapper;

        public PlacesController(DireoContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            var model = _context.Places.Include(p => p.Socials).Include(p => p.WorkHours).Include(p => p.PlaceSliderPhotos).Include(p => p.PlaceFaqs).Include(p => p.User).Include(p => p.City).ThenInclude(pc=>pc.Country).Include(p => p.PlacesTags).ThenInclude(pa => pa.Tag);

            return Ok(model.Select(m => new
            {
                m.Id,
                m.Name,
                m.Address,
                m.CreateAt,
                m.Desc,
                m.ShortDesc,
                m.Video,
                m.Website,
                m.Ranking,
                m.ReviewCount,
                m.Price,
                m.Phone,
                m.Photo,
                m.PhotoFileName,
                User=_mapper.Map<UserGetForPlaceDTO>(m.User),
                City=new
                {
                    m.City.Id,
                    m.City.Name,
                    Country=_mapper.Map<CountryGetDTO>(m.City.Country)
                },
                Tags=_mapper.Map<List<TagGetDTO>>(m.PlacesTags.Select(pt=>pt.Tag)),
                Sliders=_mapper.Map<List<SliderPhotoGetDTO>>(m.PlaceSliderPhotos),
                Faqs=_mapper.Map<List<FaqGetDTO>>(m.PlaceFaqs),
                Socials=_mapper.Map<List<SocialGetDTO>>(m.Socials),
                WorkHours=_mapper.Map<List<WorkHourGetDTO>>(m.WorkHours)
            }));
        }

        [HttpGet("{id}")]
        public async Task<object> Get(string id)
        {
            return Ok();
        }

       [HttpPost]
       [Authorize]
       public async Task<object> Create(PlaceCreateModel plc)
       {
            #region CheckModelIsInvalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            #region CheckUserIsNull
            if(! await _context.Users.AnyAsync(u => u.Id == User.GetUserId()))
            {
                return NotFound();
            }
            #endregion

            plc.Place.UserId = User.GetUserId();
            plc.Place.CreateAt = DateTime.UtcNow.AddHours(4);
            Place place = new Place();

            try
            {
                if(!string.IsNullOrWhiteSpace(plc.Place.Photo) && !string.IsNullOrWhiteSpace(plc.Place.PhotoFileName))
                {
                    plc.Place.Photo = FileManager.Upload(plc.Place.Photo,plc.Place.PhotoFileName);
                }

                place =_mapper.Map<Place>(plc.Place);
                await _context.Places.AddAsync(place);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(409);
            }

            /*Create PlaceTags*/
            if(plc.Tags!=null && plc.Tags.Count > 0)
            {
                foreach (var tag in plc.Tags)
                {
                    #region CehckTagsNull
                    if(!await _context.Tags.AnyAsync(t => t.Id == tag.Id))
                    {
                        continue;
                    }
                    #endregion

                    PlacesTags placeTag = new PlacesTags
                    {
                        TagId = tag.Id,
                        PlaceId = place.Id
                    };

                    //PlaceTags add to context
                    try
                    {
                        await _context.PlacesTags.AddAsync(placeTag);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            /*Create PlaceTags*/
            if (plc.Socials != null && plc.Socials.Count > 0)
            {
                foreach (var social in plc.Socials)
                {
                    #region CehckSocialIsEmpty
                    if (string.IsNullOrWhiteSpace(social.Link)||string.IsNullOrWhiteSpace(social.Name))
                    {
                        continue;
                    }
                    #endregion

                    Social soc = new Social
                    {
                        Link = social.Link,
                        Name = social.Name,
                        PlaceId = place.Id,
                        Type = SocialType.Place
                    };

                    //Social add to context
                    try
                    {
                        await _context.Socials.AddAsync(soc);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            /*Create Slider Photos*/
            if (plc.SliderPhotos != null && plc.SliderPhotos.Count > 0)
            {
                foreach (var slider in plc.SliderPhotos)
                {
                    #region CehckSliderIsEmpty
                    if (string.IsNullOrWhiteSpace(slider.Photo) || string.IsNullOrWhiteSpace(slider.PhotoName))
                    {
                        continue;
                    }
                    #endregion

                    PlaceSliderPhotos photo = new PlaceSliderPhotos();
                    photo.PlaceId = place.Id;
                    try
                    {
                        photo.Photo = FileManager.Upload(slider.Photo, slider.PhotoName);
                        photo.PhotoName = slider.PhotoName;

                        await _context.PlaceSliderPhotos.AddAsync(photo);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            /*Create PlaceFaqs*/
            if (plc.Faqs != null && plc.Faqs.Count > 0)
            {
                foreach (var faq in plc.Faqs)
                {
                    #region CehckFaqIsEmpty
                    if (string.IsNullOrWhiteSpace(faq.Answer) || string.IsNullOrWhiteSpace(faq.Question))
                    {
                        continue;
                    }
                    #endregion

                    PlaceFaq newFaq = new PlaceFaq
                    {
                        Answer = faq.Answer,
                        Question = faq.Question,
                        PlaceId = place.Id
                    };

                    //Faq add to context
                    try
                    {
                        await _context.PlaceFaqs.AddAsync(newFaq);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            /*Create WorkHours*/
            if (plc.WorkHours != null && plc.WorkHours.Count > 0)
            {
                foreach (var hour in plc.WorkHours)
                {
                    WorkHour workHour = new WorkHour
                    {
                        Day = hour.Day,
                        Open = hour.Open,
                        Close = hour.Close,
                        PlaceId = place.Id
                    };

                    //WorkHour add to context
                    try
                    {
                        await _context.WorkHours.AddAsync(workHour);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return StatusCode(201);
        }


    }
}