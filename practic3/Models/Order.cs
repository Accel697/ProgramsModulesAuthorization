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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Services_in_order = new HashSet<Services_in_order>();
            this.Workers_on_order = new HashSet<Workers_on_order>();
        }
    
        public long ID { get; set; }
        public long Customer { get; set; }
        public long Product { get; set; }
        public long Services { get; set; }
        public decimal Estimate { get; set; }
        public bool Estimate_paid { get; set; }
        public decimal Price { get; set; }
        public long Status { get; set; }
        public Nullable<System.DateTime> Date_of_completion { get; set; }
    
        public virtual Client Client { get; set; }
        public virtual Furniture Furniture { get; set; }
        public virtual Status_of_order Status_of_order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Services_in_order> Services_in_order { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Workers_on_order> Workers_on_order { get; set; }
    }
}
