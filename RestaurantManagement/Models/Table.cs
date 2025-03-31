using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models
{
    public class Table
    {
        public int Id { get; set; } // Đảm bảo viết hoa đúng chữ "Id"
        public string TableName { get; set; } // Định nghĩa thuộc tính TableName
        public int Capacity { get; set; } // Định nghĩa thuộc tính Capacity
        public bool IsAvailable { get; set; } // Định nghĩa thuộc tính IsAvailable
    }


}