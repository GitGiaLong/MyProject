﻿using Core.Entities.Applications.Base;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Catelogies.TheWorld.Distrist
{
    public class Distrist : BaseModel
    {
        private static string _IsOnly = string.Empty;
        /// <summary>
        /// Mã định danh
        /// </summary>
        [Display(Name = "Mã định danh")]
        public string IsOnly { get { return _IsOnly; } set { _IsOnly = value; OnPropertyChanged(); } }

        private static string _CodeProvoice = string.Empty;
        /// <summary>
        /// Mã quốc tỉnh
        /// </summary>
        [Display(Name = "Mã Tỉnh")]
        public string CodeProvoice { get { return _CodeProvoice; } set { _CodeProvoice = value; OnPropertyChanged(); } }

        private static string _DisplayName = string.Empty;
        /// <summary>
        /// Tên tỉnh thành
        /// </summary>
        [Display(Name = "Tên tỉnh thành")]
        public string DisplayName { get { return _DisplayName; } set { _DisplayName = value; OnPropertyChanged(); } }

    }
}