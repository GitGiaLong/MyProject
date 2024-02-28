using DALC.Catelogies.TheWorld;
using Entities.Application.Connect.Api;
using Entities.Catelogies.TheWorld;
using Entities.Catelogies.TheWorld.Country;
using Entities.Catelogies.TheWorld.Distrist;
using Entities.Catelogies.TheWorld.Provoice;
using Entities.Catelogies.TheWorld.Town;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPICsharp.Controllers.TheWorld
{
    [Route("theWorld")]
    [ApiController]
    public class TheWorldController : ControllerBase
    {
        DALTheWorld theWorld = new DALTheWorld();
        public TheWorldController() { }

        [HttpGet]
        public IActionResult Get([FromQuery] EntityTheWorld query, EnumTheWorld typeTheWorld)
        {
            try
            {
                if (typeTheWorld == EnumTheWorld.Provoice)
                {
                    return Ok(new { Data = theWorld.GetTheWorld<EntityProvoice>(typeTheWorld, query), Success = true, status = HttpStatusCode.OK });
                }
                else if (typeTheWorld == EnumTheWorld.Distrist)
                {
                    return Ok(new { Data = theWorld.GetTheWorld<EntityDistrist>(typeTheWorld, query), Success = true, status = HttpStatusCode.OK });

                }
                else if (typeTheWorld == EnumTheWorld.Town)
                {
                    return Ok(new { Data = theWorld.GetTheWorld<EntityTown>(typeTheWorld, query), Success = true, status = HttpStatusCode.OK });
                }
                else
                {
                    return Ok(new { Data = theWorld.GetTheWorld<EntityCountry>(typeTheWorld, query), Success = true, status = HttpStatusCode.OK });
                }
                //    return detail == null || detail.Count() <= 0 ? NotFound(new { Message = $"Không tìm thấy {query}", Success = false, status = HttpStatusCode.NotFound }) : Ok(new { Data = detail, Success = true, status = HttpStatusCode.OK });

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Lỗi {ex.Message}", Success = false, status = HttpStatusCode.BadRequest });
            }
        }

        [HttpPost]
        public IActionResult Create(EnumTheWorld type, object value)
        {
            try
            {
                if (value != null)
                {
                    theWorld.ActionCountry(EnumMethodApi.B, type, value);
                    return Ok(new { Data = value, Message = $"Thêm thành công", Success = true, status = HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Data = value, Message = $"Lỗi {ex.Message}", Success = false, status = HttpStatusCode.BadRequest });
            }
            return BadRequest(new { Data = value, Message = $"Lỗi", Success = false, status = HttpStatusCode.BadRequest });
        }

        [HttpPut]
        public IActionResult Update(EnumTheWorld type, object value)
        {
            try
            {
                if (value != null)
                {
                    theWorld.ActionCountry(EnumMethodApi.C, type, value);
                    return Ok(new { Data = value, Message = $"Cập nhập thành công", Success = true, status = HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Lỗi {ex.Message}", Success = false, status = HttpStatusCode.BadRequest });
            }
            return BadRequest(new { Message = $"Lỗi", Success = false, status = HttpStatusCode.BadRequest });
        }

        [HttpDelete]
        public IActionResult Delete(EnumTheWorld type, object value)
        {
            if (value != null)
            {
                theWorld.ActionCountry(EnumMethodApi.D, type, value);
                return Ok(new { Data = value, Message = $"Đã xoá ", Success = true, status = HttpStatusCode.OK });
            }
            return BadRequest(new { Data = value, });
        }
    }
}
