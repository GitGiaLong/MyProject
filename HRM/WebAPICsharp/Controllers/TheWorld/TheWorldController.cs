using DALC.Catelogies.TheWorld;
using Entities.Application.Connect.Api;
using Entities.Application.Convert;
using Entities.Catelogies.TheWorld;
using Entities.Catelogies.TheWorld.Country;
using Entities.Catelogies.TheWorld.Distrist;
using Entities.Catelogies.TheWorld.Provoice;
using Entities.Catelogies.TheWorld.Town;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Net;
using WebAPICsharp.Extensions;

namespace WebAPICsharp.Controllers.TheWorld
{
    [Route("theWorld")]
    [ApiController]
    public class TheWorldController : ControllerBase
    {
        DALTheWorld theWorld = new DALTheWorld();
        ResponseResult result;
        public TheWorldController() { result = new ResponseResult(); }

        [HttpGet]
        public IActionResult Get([FromQuery] Filter query, EnumTheWorld typeTheWorld)
        {
            try
            {
                int totalRecords = 0;
                if (typeTheWorld == EnumTheWorld.Provoice)
                {
                    totalRecords = theWorld.GetTotalRecords(typeTheWorld);
                    return totalRecords > 0 ? Ok(result.ResultOk<ObservableCollection<EntityProvoice>>(theWorld.GetTheWorld<EntityProvoice>(typeTheWorld, query), query.Page, query.PageSize, totalRecords)) : NotFound(result.ResultNotFound());
                }
                else if (typeTheWorld == EnumTheWorld.Distrist)
                {
                    totalRecords = theWorld.GetTotalRecords(typeTheWorld);
                    return totalRecords > 0 ? Ok(result.ResultOk<ObservableCollection<EntityDistrist>>(theWorld.GetTheWorld<EntityDistrist>(typeTheWorld, query), query.Page, query.PageSize, totalRecords)) : NotFound(result.ResultNotFound());
                }
                else if (typeTheWorld == EnumTheWorld.Town)
                {
                    totalRecords = theWorld.GetTotalRecords(typeTheWorld);
                    return totalRecords > 0 ? Ok(result.ResultOk<ObservableCollection<EntityTown>>(theWorld.GetTheWorld<EntityTown>(typeTheWorld, query), query.Page, query.PageSize, totalRecords)) : NotFound(result.ResultNotFound());
                }
                else
                {
                    totalRecords = theWorld.GetTotalRecords(typeTheWorld);
                    return totalRecords > 0 ? Ok(result.ResultOk<ObservableCollection<EntityCountry>>(theWorld.GetTheWorld<EntityCountry>(typeTheWorld, query), query.Page, query.PageSize, totalRecords)) : NotFound(result.ResultNotFound());
                }
            }
            catch (Exception ex)
            {
                return BadRequest(result.ResultBad(ex.Message));
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
