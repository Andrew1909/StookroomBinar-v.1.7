//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockroomBinar.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlasticStor
    {
        public int ID { get; set; }
        public string ColorName { get; set; }
        public string PlasticType { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public Nullable<int> NumberСoils { get; set; }
        public string Manufacturer { get; set; }
        public byte[] ProfileCura { get; set; }
        public string PlasticName { get; set; }
        public string Notes { get; set; }
        public Nullable<int> IDInsaid { get; set; }
        public byte[] Image { get; set; }
    }
}
