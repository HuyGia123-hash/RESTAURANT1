using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using RestaurantManagement.DAL;
using RestaurantManagement.Models;

namespace RestaurantManagement.Controllers
{
    public class ReservationController : Controller
    {
        private static readonly string connectionString = "Data Source=LUKEY\\MSSQLLUKEYY;Initial Catalog=QL_NhaHang;Integrated Security=True";
        private readonly ReservationDAL reservationDAL = new ReservationDAL();
        private readonly TableDAL tableDAL = new TableDAL();

        public ActionResult Create()
        {
            ViewBag.AvailableTables = tableDAL.GetAvailableTables(); 
            return View();
        }

        [HttpPost]
        public ActionResult Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                bool isAdded = reservationDAL.AddReservation(reservation);
                if (isAdded)
                {
                    return RedirectToAction("Success");
                }
                else
                {
                    ModelState.AddModelError("", "Đặt bàn thất bại! Vui lòng thử lại.");
                }
            }
            return View(reservation);
        }


        public ActionResult ConfirmArrival()
        {
            List<Reservation> reservations = reservationDAL.GetPendingReservations();
            return View(reservations);
        }

        [HttpPost]
        public ActionResult Confirm(int id)
        {
            bool isConfirmed = reservationDAL.ConfirmReservation(id);
            if (isConfirmed)
            {
                return RedirectToAction("ConfirmArrival");
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xác nhận!";
                return RedirectToAction("ConfirmArrival");
            }
        }


        public ActionResult Success()
        {
            return View();
        }
    }
}
