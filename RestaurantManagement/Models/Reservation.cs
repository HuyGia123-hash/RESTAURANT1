using System;

public class Reservation
{
    public int Id { get; set; }
    public string CustomerName { get; set; } // Tên khách hàng
    public string PhoneNumber { get; set; } // Số điện thoại
    public DateTime ReservationDate { get; set; } // Ngày đặt
    public DateTime ArrivalDate { get; set; } // Ngày nhận bàn
    public int NumberOfGuests { get; set; } // Số lượng khách
    public int TableId { get; set; } // Mã bàn
}
