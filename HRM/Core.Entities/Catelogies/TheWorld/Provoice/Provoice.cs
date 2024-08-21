﻿using Core.Entities.Applications.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Catelogies.TheWorld.Provoice
{
    public class Provoice : BaseModel
    {
        private static string _IsOnly = string.Empty;
        /// <summary>
        /// Mã định danh
        /// </summary>
        [Display(Name = "Mã định danh")]
        public string IsOnly { get { return _IsOnly; } set { _IsOnly = value; OnPropertyChanged(); } }

        private static string _CodeCountry = string.Empty;
        /// <summary>
        /// Mã quốc gua
        /// </summary>
        [Display(Name = "Mã quốc gia")]
        public string CodeCountry { get { return _CodeCountry; } set { _CodeCountry = value; OnPropertyChanged(); } }

        private static string _DisplayName = string.Empty;
        /// <summary>
        /// Tên tỉnh thành
        /// </summary>
        [Display(Name = "Tên tỉnh thành")]
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }
    }
}