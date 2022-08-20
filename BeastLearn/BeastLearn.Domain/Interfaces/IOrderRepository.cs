using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Domain.Models.Course;
using BeastLearn.Domain.Models.Order;
using BeastLearn.Domain.Models.User;

namespace BeastLearn.Domain.Interfaces
{
   public interface IOrderRepository: IDisposable
    {
        #region Orders

        IEnumerable<Order> GetOrder();
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void AddOrderDetail(OrderDetails orderDetails);
        void UpdateOrderDetail(OrderDetails orderDetails);
        Order GetOrderById(int orderId);
        IEnumerable<OrderDetails> GetOrderDetaile();
        IEnumerable<Order> GetOrderForUserPanel();
        void AddUserCourse(UserCourse userCourse);
        IQueryable<UserCourse> GetUserCourse();
        void Save();

        #endregion

        #region Discount

        IQueryable<Discount> GetDiscounts();
        void AddDiscount(Discount discount);
        void UpdateDiscount(Discount discount);
        Discount GetDiscountById(int discountId);
        void AddUserDiscount(UserDiscountCode userDiscount);
        void UpdateUserDiscount(UserDiscountCode userDiscount);
        IEnumerable<UserDiscountCode> GetUserDiscount();
     




        #endregion

   }
}
