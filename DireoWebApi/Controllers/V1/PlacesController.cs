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

        public PlacesController(DireoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public object Get()
        {
            var model = _context.Places.Include(p => p.Socials).Include(p => p.WorkHours).Include(p => p.PlaceSliderPhotos).Include(p => p.PlaceFaqs).Include(p => p.User).Include(p => p.City).ThenInclude(pc => pc.Country).Include(p => p.PlacesTags).ThenInclude(pa => pa.Tag);

            return Ok(_mapper.Map<List<PlaceGetDTO>>(model));

            //return Ok(model.Select(m => new
            //{
            //    m.Id,
            //    m.Name,
            //    m.Address,
            //    m.CreateAt,
            //    m.Desc,
            //    m.ShortDesc,
            //    m.Video,
            //    m.Website,
            //    m.Ranking,
            //    m.ReviewCount,
            //    m.Price,
            //    m.Phone,
            //    Photo=string.IsNullOrWhiteSpace(m.Photo)?null: $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/uploads/{m.Photo}",
            //    m.PhotoFileName,
            //    User = _mapper.Map<UserGetForPlaceDTO>(m.User),
            //    City = new
            //    {
            //        m.City.Id,
            //        m.City.Name,
            //        Country = _mapper.Map<CountryGetDTO>(m.City.Country)
            //    },
            //    Tags = _mapper.Map<List<TagGetDTO>>(m.PlacesTags.Select(pt => pt.Tag)),
            //    Sliders = _mapper.Map<List<SliderPhotoGetDTO>>(m.PlaceSliderPhotos),
            //    Faqs = _mapper.Map<List<FaqGetDTO>>(m.PlaceFaqs),
            //    Socials = _mapper.Map<List<SocialGetDTO>>(m.Socials),
            //    WorkHours = _mapper.Map<List<WorkHourGetDTO>>(m.WorkHours)
            //}));
        }

        [HttpGet("{id}")]
        public async Task<object> Get(string id)
        {
            Place place = await _context.Places.Include(p => p.Socials).Include(p => p.WorkHours).Include(p => p.PlaceSliderPhotos).Include(p => p.PlaceFaqs).Include(p => p.User).Include(p => p.City).ThenInclude(pc => pc.Country).Include(p => p.PlacesTags).ThenInclude(pa => pa.Tag).FirstOrDefaultAsync(p => p.Id == id);

            #region CheckIsNull
            if (place == null)
            {
                return NotFound();
            }
            #endregion

            return Ok(_mapper.Map<PlaceGetDTO>(place));

            //var model = new
            //{
            //    place.Id,
            //    place.Name,
            //    place.Address,
            //    place.CreateAt,
            //    place.Desc,
            //    place.ShortDesc,
            //    place.Video,
            //    place.Website,
            //    place.Ranking,
            //    place.ReviewCount,
            //    place.Price,
            //    place.Phone,
            //    Photo = string.IsNullOrWhiteSpace(place.Photo) ? null : $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/uploads/{place.Photo}",
            //    place.PhotoFileName,
            //    User = _mapper.Map<UserGetForPlaceDTO>(place.User),
            //    City = new
            //    {
            //        place.City.Id,
            //        place.City.Name,
            //        Country = _mapper.Map<CountryGetDTO>(place.City.Country)
            //    },
            //    Tags = _mapper.Map<List<TagGetDTO>>(place.PlacesTags.Select(pt => pt.Tag)),
            //    Sliders = _mapper.Map<List<SliderPhotoGetDTO>>(place.PlaceSliderPhotos),
            //    Faqs = _mapper.Map<List<FaqGetDTO>>(place.PlaceFaqs),
            //    Socials = _mapper.Map<List<SocialGetDTO>>(place.Socials),
            //    WorkHours = _mapper.Map<List<WorkHourGetDTO>>(place.WorkHours)
            //};
            //return Ok(model);
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
            if (!await _context.Users.AnyAsync(u => u.Id == User.GetUserId()))
            {
                return NotFound();
            }
            #endregion

            plc.Place.UserId = User.GetUserId();
            plc.Place.CreateAt = DateTime.UtcNow.AddHours(4);
            Place place = new Place();

            /*Try to photo upload,add to context and savecAhnges*/
            try
            {
                if (!string.IsNullOrWhiteSpace(plc.Place.Photo) && !string.IsNullOrWhiteSpace(plc.Place.PhotoFileName))
                {
                    plc.Place.Photo = FileManager.Upload(plc.Place.Photo, plc.Place.PhotoFileName);
                }

                place = _mapper.Map<Place>(plc.Place);
                await _context.Places.AddAsync(place);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(409);
            }

            /*Create PlaceTags*/
            if (plc.Tags != null && plc.Tags.Count > 0)
            {
                foreach (var tag in plc.Tags)
                {
                    #region CehckTagsNull
                    if (!await _context.Tags.AnyAsync(t => t.Id == tag.Id))
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
                    if (string.IsNullOrWhiteSpace(social.Link) || string.IsNullOrWhiteSpace(social.Name))
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

        [HttpPut]
        [Authorize]
        public async Task<object> Update(PlaceUpdateModel plc)
        {
            #region CheckModelIsInvalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            Place place = await _context.Places.FindAsync(plc.Place.Id);
            List<string> RelationIds = new List<string>();

            #region CheckIsNull
            if (place == null)
            {
                return NotFound();
            }
            #endregion

            place.Address = plc.Place.Address;
            place.CategoryId = plc.Place.CategoryId;
            place.CityId = plc.Place.CityId;
            place.Desc = plc.Place.Desc;
            place.Gender = plc.Place.Gender;
            place.HideContactInfo = plc.Place.HideContactInfo;
            place.HideMap = plc.Place.HideMap;
            place.Lat = plc.Place.Lat;
            place.Long = plc.Place.Long;
            place.Name = plc.Place.Name;
            place.Phone = plc.Place.Phone;
            place.ShortDesc = plc.Place.ShortDesc;
            place.Website = plc.Place.Website;
            place.Video = plc.Place.Video;
            place.Price = plc.Place.Price;

            /*File Upload*/
            try
            {
                /*Check photo is deleted*/
                if (string.IsNullOrWhiteSpace(plc.Place.PhotoFileName))
                {
                    place.PhotoFileName = null;
                    place.Photo = null;
                }
                /*Check photo is updated*/
                else if (!string.IsNullOrWhiteSpace(plc.Place.Photo))
                {
                    place.PhotoFileName = plc.Place.PhotoFileName;
                    place.Photo = FileManager.Upload(plc.Place.Photo, plc.Place.PhotoFileName);
                }
            }
            catch (Exception)
            {
            }

            /*Place Socials Update Start*/
            RelationIds = new List<string>();
            if (plc.Socials != null && plc.Socials.Count > 0)
            {
                foreach (var social in plc.Socials)
                {
                    Social scl = new Social();
                    /*Create new*/
                    if (string.IsNullOrWhiteSpace(social.Id))
                    {
                        #region CheckIsEmpty
                        if (string.IsNullOrWhiteSpace(social.Name) || string.IsNullOrWhiteSpace(social.Link))
                        {
                            continue;
                        }
                        #endregion

                        #region CheckSocialExist
                        if(await _context.Socials.AnyAsync(s => s.Name == social.Name && s.Link == s.Link && s.PlaceId == place.Id))
                        {
                            RelationIds.Add(_context.Socials.FirstOrDefault(s => s.Name == social.Name && s.Link == s.Link && s.PlaceId == place.Id).Id);
                            continue;
                        }
                        #endregion

                        scl.Name = social.Name;
                        scl.Link = social.Link;
                        scl.PlaceId = place.Id;
                        scl.Type = SocialType.Place;

                        /*Save Changes*/
                        try
                        {
                            await _context.Socials.AddAsync(scl);
                            await _context.SaveChangesAsync();
                            RelationIds.Add(scl.Id);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    /*Edit*/
                    else
                    {
                        scl = await _context.Socials.FirstOrDefaultAsync(s=>s.Id==social.Id&&s.PlaceId==place.Id);

                        #region CheckIsNull
                        if (scl == null)
                        {
                            continue;
                        }
                        #endregion

                        RelationIds.Add(scl.Id);

                        #region CheckSocialExist
                        if (await _context.Socials.AnyAsync(s => s.Name == social.Name && s.Link == s.Link && s.PlaceId == place.Id && s.Id!=social.Id))
                        {
                            continue;
                        }
                        #endregion

                        #region CheckIsEmpty
                        if (string.IsNullOrWhiteSpace(social.Name) || string.IsNullOrWhiteSpace(social.Link))
                        {
                            continue;
                        }
                        #endregion

                        scl.Name = social.Name;
                        scl.Link = social.Link;

                        /*Save Changes*/
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            /*Remove socials*/
            try
            {
                _context.Socials.RemoveRange(_context.Socials.Where(s => s.PlaceId == place.Id && !RelationIds.Contains(s.Id)));
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            /*Place Socials Update End*/

            /*Place Faqs Update Start*/
            RelationIds = new List<string>();
            if (plc.Faqs != null && plc.Faqs.Count > 0)
            {
                foreach (var faq in plc.Faqs)
                {
                    PlaceFaq placeFaq = new PlaceFaq();
                    /*Create new*/
                    if (string.IsNullOrWhiteSpace(faq.Id))
                    {
                        #region CheckIsEmpty
                        if (string.IsNullOrWhiteSpace(faq.Question) || string.IsNullOrWhiteSpace(faq.Answer))
                        {
                            continue;
                        }
                        #endregion

                        #region CheckFaqExist
                        if(await _context.PlaceFaqs.AnyAsync(f => f.Answer == faq.Answer && f.Question == faq.Question && f.PlaceId == place.Id))
                        {
                            RelationIds.Add(_context.PlaceFaqs.FirstOrDefault(f => f.Answer == faq.Answer && f.Question == faq.Question && f.PlaceId == place.Id).Id);
                            continue;
                        }
                        #endregion

                        placeFaq.Question = faq.Question;
                        placeFaq.Answer = faq.Answer;
                        placeFaq.PlaceId = place.Id;

                        /*Save Changes*/
                        try
                        {
                            await _context.PlaceFaqs.AddAsync(placeFaq);
                            await _context.SaveChangesAsync();
                            RelationIds.Add(placeFaq.Id);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    /*Edit*/
                    else
                    {
                        placeFaq = await _context.PlaceFaqs.FirstOrDefaultAsync(f=>f.Id==faq.Id &&f.PlaceId==place.Id);

                        #region CheckIsNull
                        if (placeFaq == null)
                        {
                            continue;
                        }
                        #endregion

                        RelationIds.Add(faq.Id);

                        #region CheckIsEmpty
                        if (string.IsNullOrWhiteSpace(faq.Question) || string.IsNullOrWhiteSpace(faq.Answer))
                        {
                            continue;
                        }
                        #endregion

                        #region CheckFaqExist
                        if(await _context.PlaceFaqs.AnyAsync(f => f.Id != faq.Id && f.PlaceId == place.Id && f.Answer == faq.Answer && f.Question == faq.Question))
                        {
                            continue;
                        }
                        #endregion

                        placeFaq.Question = faq.Question;
                        placeFaq.Answer = faq.Answer;

                        /*Save Changes*/
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            /*Remove faqs*/
            try
            {
                _context.PlaceFaqs.RemoveRange(_context.PlaceFaqs.Where(f => f.PlaceId == place.Id && !RelationIds.Contains(f.Id)));
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
            /*Place Faqs Update End*/

            /*Place Slider Update Start*/
            RelationIds = new List<string>();
            if (plc.SliderPhotos != null && plc.SliderPhotos.Count > 0)
            {
                foreach (var slider in plc.SliderPhotos)
                {
                    PlaceSliderPhotos photo = new PlaceSliderPhotos();
                    if (string.IsNullOrWhiteSpace(slider.Id))
                    {
                        #region CheckIsNull
                        if (string.IsNullOrWhiteSpace(slider.Photo) || string.IsNullOrWhiteSpace(slider.PhotoName))
                        {
                            continue;
                        }
                        #endregion

                        try
                        {
                            photo.Photo = FileManager.Upload(slider.Photo, slider.PhotoName);
                            photo.PhotoName = slider.PhotoName;
                            photo.PlaceId = place.Id;

                            await _context.PlaceSliderPhotos.AddAsync(photo);
                            await _context.SaveChangesAsync();
                            RelationIds.Add(photo.Id);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        photo =await _context.PlaceSliderPhotos.FirstOrDefaultAsync(s => s.Id == slider.Id && s.PlaceId == place.Id);

                        #region CheckIsNull
                        if (photo == null)
                        {
                            continue;
                        }
                        #endregion

                        RelationIds.Add(photo.Id);
                    }
                }
            }
            /*Remove sliders*/
            try
            {
                _context.PlaceSliderPhotos.RemoveRange(_context.PlaceSliderPhotos.Where(s => s.PlaceId == place.Id && !RelationIds.Contains(s.Id)));
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
            /*Place Slider Update End*/

            /*Place Tags Update Start*/
            RelationIds = new List<string>();
            if (plc.Tags != null && plc.Tags.Count > 0)
            {
                foreach (var tag in plc.Tags)
                {
                    #region CheckIsNull
                    if (!await _context.Tags.AnyAsync(t => t.Id == tag.Id))
                    {
                        continue;
                    }
                    #endregion

                    PlacesTags plcTag = await _context.PlacesTags.FirstOrDefaultAsync(pt => pt.TagId == tag.Id && pt.PlaceId == place.Id);

                    /*Create new*/
                    if (plcTag==null)
                    {
                        plcTag.PlaceId = place.Id;
                        plcTag.TagId = tag.Id;

                        /*SaveChanges*/
                        try
                        {
                            await _context.PlacesTags.AddAsync(plcTag);
                            await _context.SaveChangesAsync();
                            RelationIds.Add(plcTag.Id);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    RelationIds.Add(plcTag.Id);
                }
            }
            /*Remove PlaceTag*/
            try
            {
                _context.PlacesTags.RemoveRange(_context.PlacesTags.Where(t=> t.PlaceId == place.Id && !RelationIds.Contains(t.Id)));
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
            /*Place Tags Update End*/

            /*Place Work Hours Update Start*/
            RelationIds = new List<string>();
            if (plc.WorkHours != null && plc.WorkHours.Count > 0)
            {
                foreach (var hour in plc.WorkHours)
                {
                    WorkHour workHour = new WorkHour();
                    /*Create new*/
                    if (string.IsNullOrWhiteSpace(hour.Id))
                    {
                        #region CheckIsEmpty
                        if (hour.Open==null || hour.Close==null)
                        {
                            continue;
                        }
                        #endregion

                        #region CheckWorkHourExist
                        if (await _context.WorkHours.AnyAsync(wh=>wh.PlaceId==place.Id && wh.Day==hour.Day))
                        {
                            RelationIds.Add(_context.WorkHours.FirstOrDefault(wh => wh.PlaceId == place.Id && wh.Day == hour.Day).Id);
                            continue;
                        }
                        #endregion

                        workHour.Day = hour.Day;
                        workHour.Open = hour.Open;
                        workHour.Close = hour.Close;
                        workHour.PlaceId = place.Id;

                        /*Save Changes*/
                        try
                        {
                            await _context.WorkHours.AddAsync(workHour);
                            await _context.SaveChangesAsync();
                            RelationIds.Add(workHour.Id);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    /*Edit*/
                    else
                    {
                        workHour = await _context.WorkHours.FirstOrDefaultAsync(wh => wh.PlaceId == place.Id && wh.Id == hour.Id);

                        #region CheckIsNull
                        if (workHour == null)
                        {
                            continue;
                        }
                        #endregion

                        RelationIds.Add(workHour.Id);

                        #region CheckIsEmpty
                        if (hour.Open == null || hour.Close == null)
                        {
                            continue;
                        }
                        #endregion

                        #region CheckWorkHourExist
                        if(await _context.WorkHours.AnyAsync(wh=>wh.Id!=workHour.Id && wh.PlaceId==place.Id && wh.Day == hour.Day))
                        {
                            continue;
                        }
                        #endregion

                        workHour.Open = hour.Open;
                        workHour.Close = hour.Close;

                        /*Save Changes*/
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            /*Remove WorkHours*/
            try
            {
                _context.WorkHours.RemoveRange(_context.WorkHours.Where(wh => wh.PlaceId == place.Id && !RelationIds.Contains(wh.Id)));
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
            /*Place Faqs Update End*/

            return NoContent();
        }

    }
}