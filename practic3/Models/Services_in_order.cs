//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace practic3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Services_in_order
    {
        public long ID { get; set; }
        public long Order { get; set; }
        public long Service { get; set; }
    
        public virtual Order Order1 { get; set; }
        public virtual Service Service1 { get; set; }
    }
}
