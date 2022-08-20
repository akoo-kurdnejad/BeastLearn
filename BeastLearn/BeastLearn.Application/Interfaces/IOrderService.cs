using System;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Application.ViewModels.Orders;
using BeastLearn.Domain.Models.Order;

namespace BeastLearn.Application.Interfaces
{
    public interface IOrderService: IDisposable
    {
        #region Orders

        int AddOrder(string userName, int courseId);
        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string userNam, int orderId);
        bool FinalyOrder(string userName, int orderId);
        List<Order> GetUserOrder(string userName);
        List<Order> GetUserOrder(int userId);
        bool IsUserInCourse(string userName, int courseId);
        #endregion

        #region Discount

        DiscountUseType UseDiscount(int orderId, string code);
        List<Discount> GetDiscount();
        void AddDiscount(Discount discount);
        void UpdateDiscount(Discount discount);
        bool IsExistCode(string code);
        Discount GetDiscountById(int discountId);
        void DeleteDiscount(int discountId);
        List<Discount> GetListDeleteCode();

        #endregion
    }
}
