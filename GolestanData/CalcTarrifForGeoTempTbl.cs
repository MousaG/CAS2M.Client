//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GolestanData
{
    using System;
    using System.Collections.Generic;
    
    public partial class CalcTarrifForGeoTempTbl
    {
        public short CoCode { get; set; }
        public short RgnCode { get; set; }
        public short CityCode { get; set; }
        public short CalLawId { get; set; }
        public decimal TrfHCode { get; set; }
        public byte TrfType { get; set; }
        public int TRID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
    
        public virtual CalcTarrifHeaderTbl CalcTarrifHeaderTbl { get; set; }
        public virtual CalcTempratureRegionTbl CalcTempratureRegionTbl { get; set; }
    }
}