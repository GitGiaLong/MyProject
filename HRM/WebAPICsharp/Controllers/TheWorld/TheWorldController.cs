using DALC.Catelogies.TheWorld;
using Entities.Application.Connect.Api;
using Entities.Catelogies.TheWorld;
using Entities.Catelogies.TheWorld.Country;
using Entities.Catelogies.TheWorld.Distrist;
using Entities.Catelogies.TheWorld.Provoice;
using Entities.Catelogies.TheWorld.Town;
using GSMF.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPICsharp.Controllers.TheWorld
{
    [Route("theWorld")]
    [ApiController]
    public class TheWorldController : ControllerBase
    {
        DALTheWorld theWorld = new DALTheWorld();
        ConvertClass convertEvent = new ConvertClass();
        public TheWorldController() { }

        [HttpGet]
        public IActionResult Get([FromQuery] EnumTheWorld value, string? code = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    if (value == EnumTheWorld.Country)
                    {
                        var detail = theWorld.GetCountry<EntityCountry>(value).FirstOrDefault(x => x.IsOnly == code);
                        return detail == null ? NotFound(new { Message = $"Không tìm thấy {code}", Success = false, status = HttpStatusCode.NotFound }) : Ok(new { Data = detail, Success = true, status = HttpStatusCode.OK });
                    }
                    else if (value == EnumTheWorld.Provoice)
                    {
                        var detail = theWorld.GetCountry<EntityProvoice>(value).FirstOrDefault(x => x.IsOnly == code);
                        return detail == null ? NotFound(new { Message = $"Không tìm thấy {code}", Success = false, status = HttpStatusCode.NotFound }) : Ok(new { Data = detail, Success = true, status = HttpStatusCode.OK });
                    }
                    else if (value == EnumTheWorld.Distrist)
                    {
                        var detail = theWorld.GetCountry<EntityDistrist>(value).FirstOrDefault(x => x.IsOnly == code);
                        return detail == null ? NotFound(new { Message = $"Không tìm thấy {code}", Success = false, status = HttpStatusCode.NotFound }) : Ok(new { Data = detail, Success = true, status = HttpStatusCode.OK });
                    }
                    else if (value == EnumTheWorld.Town)
                    {
                        var detail = theWorld.GetCountry<EntityTown>(value).FirstOrDefault(x => x.IsOnly == code);
                        return detail == null ? NotFound(new { Message = $"Không tìm thấy {code}", Success = false, status = HttpStatusCode.NotFound }) : Ok(new { Data = detail, Success = true, status = HttpStatusCode.OK });
                    }

                    return BadRequest(new { Message = $"???", Success = false, status = HttpStatusCode.BadRequest });
                }
                else
                {
                    return Ok(
                        new
                        {
                            Data = theWorld.GetCountry<object>(value),
                            Success = true,
                            status = HttpStatusCode.OK
                        });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Lỗi {ex.Message}", Success = false, status = HttpStatusCode.BadRequest });
            }
        }

        [HttpPost]
        public IActionResult Create(EnumTheWorld type,object value)
        {
            try
            {
                if (value != null)
                {
                    theWorld.ActionCountry(EnumMethodApi.B, type, value);
                    //countries.Add(value);
                    return Ok(new { Data = value, Message = $"Thêm thành công", Success = true, status = HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Data = value, Message = $"Lỗi {ex.Message}", Success = false, status = HttpStatusCode.BadRequest });
            }
            return BadRequest(new { Data = value, Message = $"Lỗi", Success = false, status = HttpStatusCode.BadRequest });
        }


        //[HttpPut]
        //public IActionResult Update(EntityCountry value)
        //{
        //    try
        //    {
        //        if (value != null)
        //        {
        //            var data = Country.GetCountry().FirstOrDefault(x => x.IsOnly == value.IsOnly);
        //            if (data != null)
        //            {
        //                Country.ActionCountry(EnumMethodApi.C, value);
        //                return Ok(new { Data = value, Message = $"Cập nhập thành công {value.NameVI}", Success = true, status = HttpStatusCode.OK });
        //            }
        //            else
        //            {
        //                return NotFound(new { Message = $"Không tìm thấy {value.IsOnly}", Success = false, status = HttpStatusCode.NotFound });
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Data = value, Message = $"Lỗi {ex.Message}", Success = false, status = HttpStatusCode.BadRequest });

        //    }
        //    return BadRequest(new { Data = value, Message = $"Lỗi", Success = false, status = HttpStatusCode.BadRequest });
        //}

        //[HttpDelete]
        //public IActionResult Delete(EntityCountry value)
        //{
        //    if (value != null && !string.IsNullOrEmpty(value.IsOnly))
        //    {

        //        Country.ActionCountry(EnumMethodApi.D, value);
        //        //countries.Remove(countries.Find(x => x.IsOnly == value.IsOnly));
        //        return Ok(new { Data = value, Message = $"Đã xoá ", Success = true, status = HttpStatusCode.OK });
        //    }

        //    return BadRequest(new { Data = value, });
        //}
    }
}
