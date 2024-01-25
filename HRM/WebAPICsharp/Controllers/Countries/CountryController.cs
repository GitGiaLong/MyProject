using DALC.Catelogies.Countries;
using Entities.Application.Connect.Api;
using Entities.Catelogies.Countries;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPICsharp.Controllers.Countries
{
    [Route("countries")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        DALCountry Country = new DALCountry();
        public CountryController() { }

        [HttpGet]
        public IActionResult Get(string? value = null)
        {
            try
            {
                if (value != null && !string.IsNullOrEmpty(value))
                {
                    var detail = Country.GetCountry().FirstOrDefault(x => x.IsOnly == value);
                    if (detail == null)
                        return NotFound(new { Message = $"Không tìm thấy {value}", Success = false, status = HttpStatusCode.NotFound });
                    else
                        return Ok(new { Data = detail, Success = true, status = HttpStatusCode.OK });
                }
                else
                {
                    return Ok(new { Data = Country.GetCountry(), Success = true, status = HttpStatusCode.OK });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Data = value, Message = $"Lỗi", Success = false, status = HttpStatusCode.BadRequest });
            }
        }

        [HttpPost]
        public IActionResult Create(EntityCountry value)
        {
            try
            {
                if (value != null && !string.IsNullOrEmpty(value.IsOnly))
                {
                    Country.ActionCountry(EnumMethodApi.B, value);
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


        [HttpPut]
        public IActionResult Update(EntityCountry value)
        {
            try
            {
                if (value != null)
                {
                    var data = Country.GetCountry().FirstOrDefault(x => x.IsOnly == value.IsOnly);
                    if (data != null)
                    {
                        Country.ActionCountry(EnumMethodApi.C, value);
                        return Ok(new { Data = value, Message = $"Cập nhập thành công {value.NameVI}", Success = true, status = HttpStatusCode.OK });
                    }
                    else
                    {
                        return NotFound(new { Message = $"Không tìm thấy {value.IsOnly}", Success = false, status = HttpStatusCode.NotFound });
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Data = value, Message = $"Lỗi {ex.Message}", Success = false, status = HttpStatusCode.BadRequest });

            }
            return BadRequest(new { Data = value, Message = $"Lỗi", Success = false, status = HttpStatusCode.BadRequest });
        }

        [HttpDelete]
        public IActionResult Delete(EntityCountry value)
        {
            if (value != null && !string.IsNullOrEmpty(value.IsOnly))
            {

                Country.ActionCountry(EnumMethodApi.D, value);
                //countries.Remove(countries.Find(x => x.IsOnly == value.IsOnly));
                return Ok(new { Data = value, Message = $"Đã xoá ", Success = true, status = HttpStatusCode.OK });
            }

            return BadRequest(new { Data = value, });
        }
    }
}
