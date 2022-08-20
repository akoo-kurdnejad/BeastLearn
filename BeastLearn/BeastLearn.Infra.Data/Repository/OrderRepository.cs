using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.Course;
using BeastLearn.Domain.Models.Order;
using BeastLearn.Domain.Models.User;
using BeastLearn.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BeastLearn.Infra.Data.Repository
{
    public class OrderRepository:IOrderRepository
    {
        private BestLearnContext _context;

        public OrderRepository(BestLearnContext context)
        {
            _context = context;
        }
        public IEnumerable<Order> GetOrder()
        {
            return _context.Orders;
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            Save();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            Save();
        }

        public void AddOrderDetail(OrderDetails orderDetails)
        {
            _context.OrderDetailses.Add(orderDetails);
            Save();
        }

        public void UpdateOrderDetail(OrderDetails orderDetails)
        {
            _context.OrderDetailses.Update(orderDetails);
            Save();
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public IEnumerable<OrderDetails> GetOrderDetaile()
        {
            return _context.OrderDetailses;
        }

        public IEnumerable<Order> GetOrderForUserPanel()
        {
            return _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od=>od.Course);
        }

        public void AddUserCourse(UserCourse userCourse)
        {
            _context.UserCourses.Add(userCourse);
            Save();
        }

        public IQueryable<UserCourse> GetUserCourse()
        {
            return _context.UserCourses;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<Discount> GetDiscounts()
        {
            return _context.Discounts;
        }

        public void AddDiscount(Discount discount)
        {
            _context.Discounts.Add(discount);
            Save();
        }

        public void UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            Save();
        }

        public Discount GetDiscountById(int discountId)
        {
            return _context.Discounts.Find(discountId);
        }

        public void AddUserDiscount(UserDiscountCode userDiscount)
        {
            _context.UserDiscountCodes.Add(userDiscount);
            Save();
        }

        public void UpdateUserDiscount(UserDiscountCode userDiscount)
        {
            _context.UserDiscountCodes.Update(userDiscount);
            Save();
        }

        public IEnumerable<UserDiscountCode> GetUserDiscount()
        {
            return _context.UserDiscountCodes;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
