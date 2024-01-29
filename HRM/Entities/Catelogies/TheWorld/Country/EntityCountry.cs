﻿using Entities.Application.Base;
using Entities.Catelogies.TheWorld.Provoice;
using System.ComponentModel.DataAnnotations;

namespace Entities.Catelogies.TheWorld.Country
{
    public class EntityCountry : BaseModel
    {
        /// <summary>
        /// Mã quốc gia (3 ký tự)
        /// </summary>
        [Display(Name = "Mã quốc gia (3 ký tự)")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Tên Tiếng Anh
        /// </summary>
        [Display(Name = "Tên tiếng Anh")]
        public string NameEN { get; set; } = string.Empty;

        /// <summary>
        /// Tên tiếng Việt
        /// </summary>
        [Display(Name = "Tên tiếng Việt")]
        public string NameVI { get; set; } = string.Empty;
    }
}