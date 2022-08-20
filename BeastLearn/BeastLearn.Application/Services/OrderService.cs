using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.ViewModels.Orders;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.Course;
using BeastLearn.Domain.Models.Order;
using BeastLearn.Domain.Models.User;
using BeastLearn.Domain.Models.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeastLearn.Application.Services
{
   public class OrderService:IOrderService
   {
       private IOrderRepository _orderRepository;
       private IUserService _userService;
       private ICourseService _courseService;
       private IWalletService _walletService;

       public OrderService(IOrderRepository orderRepository, IUserService userService, ICourseService courseService, IWalletService walletService)
       {
           _orderRepository = orderRepository;
           _userService = userService;
           _courseService = courseService;
           _walletService = walletService;
       }

        public int AddOrder(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            var course = _courseService.GetCourseById(courseId);
            Order order = _orderRepository.GetOrder()
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);

            // No OrderOpen and Create new Order
            if (order == null)
            {
                order = new Order()
                {
                    UserId = userId,
                    IsFinaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = course.CoursePrice,

                    OrderDetails = new List<OrderDetails>()
                    {
                        new OrderDetails()
                        {
                            Count = 1,
                            Price = course.CoursePrice,
                            CourseId = courseId
                        }
                    }
                    
                };
                _orderRepository.AddOrder(order);
            }

            //Yes OrderOpen and Append To OldOrder
            else
            {
                OrderDetails detail = _orderRepository.GetOrderDetaile()
                    .FirstOrDefault(d => d.OrderId == order.OrderId && d.CourseId == courseId);

                //old Course and Append To Count
                if (detail != null)
                {
                    detail.Count += 1;
                    _orderRepository.UpdateOrderDetail(detail);
                }

                //New Course and Create New orderdetail
                else
                {
                    detail = new OrderDetails()
                    {
                         OrderId = order.OrderId,
                         CourseId = courseId,
                         Count = 1,
                         Price = course.CoursePrice
                    };
                    _orderRepository.AddOrderDetail(detail);
                }

                UpdatePriceOrder(order.OrderId);
            }

            return order.OrderId;
        }

        public void UpdatePriceOrder(int orderId)
        {
            var order = _orderRepository.GetOrderById(orderId);
            var orderDetail = _orderRepository.GetOrderDetaile();

            order.OrderSum = orderDetail.Where(d => d.OrderId == orderId).Sum(d => d.Price);
            _orderRepository.UpdateOrder(order);
        }

        public Order GetOrderForUserPanel(string userNam, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userNam);
            return _orderRepository.GetOrderForUserPanel()
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
        }

        public bool FinalyOrder(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            var order = _orderRepository.GetOrderForUserPanel()
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);

            if (order == null || order.IsFinaly)
            {
                return false;
            }

            //Wallet is More Price Course
            if (_walletService.BalanceWalletUser(userName) >= order.OrderSum)
            {
                order.IsFinaly = true;

                _walletService.AddWallet(new Wallet()
                {
                    Amount = order.OrderSum,
                    CreateDate = DateTime.Now,
                    Description = " فاکتور شماره #" + order.OrderId,
                    IsPay = true,
                    TypeId = 2,
                    UserId = userId
                });

                _orderRepository.UpdateOrder(order);

                foreach (var detail in order.OrderDetails)
                {
                    _orderRepository.AddUserCourse(new UserCourse()
                    {
                        UserId = userId,
                        CourseId = detail.CourseId
                    });
                }
                return true;
            }

            return false;
        }

        public List<Order> GetUserOrder(string userName)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            var order = _orderRepository.GetOrder();

            return order.Where(o => o.UserId == userId).ToList();
        }

        public List<Order> GetUserOrder(int userId)
        {
            var order = _orderRepository.GetOrder();

            return order.Where(o => o.UserId == userId).ToList();
        }

        public bool IsUserInCourse(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            return _orderRepository.GetUserCourse()
                .Any(u => u.UserId == userId && u.CourseId == courseId);
        }

        public DiscountUseType UseDiscount(int orderId, string code)
        {
            var discount = _orderRepository.GetDiscounts()
                .SingleOrDefault(d => d.DiscountCode == code);

            var order = _orderRepository.GetOrderById(orderId);

            if (discount == null)
            {
                return DiscountUseType.NotFound;
            }

            if (discount.StartDate != null && discount.StartDate >= DateTime.Now)
            {
                return DiscountUseType.ExpierDate;
            }

            if (discount.EndDate != null && discount.EndDate < DateTime.Now)
            {
                return DiscountUseType.ExpierDate;
            }

            if (discount.UsableCount != null && discount.UsableCount < 1)
            {
                return DiscountUseType.Finished;
            }

            if (_orderRepository.GetUserDiscount()
                .Any(ud => ud.UserId == order.UserId && ud.DiscountId == discount.DiscountId))
            {
                return DiscountUseType.UserUsed;
            }

            int percent = (order.OrderSum * discount.DiscountPercent) / 100;

            order.OrderSum = order.OrderSum - percent;
            _orderRepository.UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }
            _orderRepository.UpdateDiscount(discount);

            _orderRepository.AddUserDiscount(new UserDiscountCode()
            {
                DiscountId = discount.DiscountId,
                UserId = order.UserId
            });
         
            return DiscountUseType.Success;
        }

        public List<Discount> GetDiscount()
        {
            return _orderRepository.GetDiscounts().ToList();
        }

        public void AddDiscount(Discount discount)
        {
            _orderRepository.AddDiscount(discount);
        }

        public void UpdateDiscount(Discount discount)
        {
            _orderRepository.UpdateDiscount(discount);
        }

        public bool IsExistCode(string code)
        {
            var discount = _orderRepository.GetDiscounts();

            return discount.Any(d => d.DiscountCode == code);
        }

        public Discount GetDiscountById(int discountId)
        {
            return _orderRepository.GetDiscountById(discountId);
        }

        public void DeleteDiscount(int discountId)
        {
            var discount = GetDiscountById(discountId);
            discount.IsDelete = true;
            _orderRepository.UpdateDiscount(discount);
        }

        public List<Discount> GetListDeleteCode()
        {
            var discount = _orderRepository.GetDiscounts().IgnoreQueryFilters().Where(u => u.IsDelete);
            return discount.ToList();
        }

        public void Dispose()
        {
            _orderRepository?.Dispose();
        }
   }
}
