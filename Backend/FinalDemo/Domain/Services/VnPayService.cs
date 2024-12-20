﻿using AutoMapper;
using Domain.Helper;
using Domain.Models;
using Domain.Models.Dto.Request;
using Domain.Models.Dto.Response;
using Domain.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SWP391.KCSAH.Repository;
using System.Web;

namespace Domain.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VnPayService(UnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
        }

        public async Task<string> CreatePaymentUrl(HttpContext context, VNPayRequestDTO model)
        {
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            Order order = await _unitOfWork.OrderRepository.GetByIdAsync(model.OrderId);

            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (order.TotalPrice * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", model.OrderId.ToString());
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);

            vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);

            return await Task.FromResult(paymentUrl);
        }

        //public async Task<VNPayResponseDTO> PaymentCallback(IQueryCollection collections)
        //{
        //    var vnpay = new VnPayLibrary();
        //    foreach (var (key, value) in collections)
        //    {
        //        if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
        //        {
        //            vnpay.AddResponseData(key, value.ToString());
        //        }
        //    }

        //    var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
        //    var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
        //    var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
        //    var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        //    var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

        //    bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
        //    if (!checkSignature)
        //    {
        //        return await Task.FromResult(new VNPayResponseDTO
        //        {
        //            Success = false
        //        });
        //    }

        //    return await Task.FromResult(new VNPayResponseDTO
        //    {
        //        Success = true,
        //        PaymentMethod = "VnPay",
        //        OrderDescription = vnp_OrderInfo,
        //        OrderId = vnp_orderId.ToString(),
        //        TransactionId = vnp_TransactionId.ToString(),
        //        Token = vnp_SecureHash,
        //        VnPayResponseCode = vnp_ResponseCode
        //    });
        //}

        //public async Task<(bool success, string message)> ProcessVnPayCallback(IQueryCollection queryCollection)
        //{
        //    try
        //    {
        //        var vnPayData = ParseVnPayData(queryCollection);

        //        if (!ValidateSignature(queryCollection))
        //        {
        //            return (false, "Invalid signature");
        //        }

        //        var transaction = await SaveTransaction(vnPayData);

        //        if (IsSuccessfulTransaction(vnPayData))
        //        {
        //            await UpdateOrderStatus(vnPayData.VnpTxnRef);
        //            return (true, "Payment processed successfully");
        //        }

        //        return (false, "Payment processing failed");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //private VnPayCallbackDTO ParseVnPayData(IQueryCollection queryCollection)
        //{
        //    return new VnPayCallbackDTO
        //    {
        //        VnpTxnRef = queryCollection["vnp_TxnRef"].ToString(),
        //        VnpAmount = queryCollection["vnp_Amount"].ToString(),
        //        VnpBankCode = queryCollection["vnp_BankCode"].ToString(),
        //        VnpBankTranNo = queryCollection["vnp_BankTranNo"].ToString(),
        //        VnpCardType = queryCollection["vnp_CardType"].ToString(),
        //        VnpOrderInfo = queryCollection["vnp_OrderInfo"].ToString(),
        //        VnpPayDate = queryCollection["vnp_PayDate"].ToString(),
        //        VnpResponseCode = queryCollection["vnp_ResponseCode"].ToString(),
        //        VnpTransactionNo = queryCollection["vnp_TransactionNo"].ToString(),
        //        VnpTransactionStatus = queryCollection["vnp_TransactionStatus"].ToString(),
        //        VnpSecureHash = queryCollection["vnp_SecureHash"].ToString()
        //    };
        //}

        //private bool ValidateSignature(Dictionary<string, string> queryParams)
        //{
        //    try
        //    {
        //        // Lấy secure hash từ response
        //        if (!queryParams.ContainsKey("vnp_SecureHash"))
        //            return false;

        //        string vnp_SecureHash = queryParams["vnp_SecureHash"];

        //        // Tạo sorted dictionary để sắp xếp các tham số
        //        var sortedParams = new SortedDictionary<string, string>();

        //        // Thêm tất cả các tham số ngoại trừ vnp_SecureHash vào dictionary
        //        foreach (var param in queryParams.Where(p => p.Key.StartsWith("vnp_") && p.Key != "vnp_SecureHash"))
        //        {
        //            sortedParams.Add(param.Key, param.Value);
        //        }

        //        // Tạo chuỗi hash
        //        var signData = string.Join("&", sortedParams
        //            .Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}")
        //            .ToArray());

        //        string hashSecret = _config["VnPay:HashSecret"];

        //        // Tạo chữ ký
        //        // var secureHash = HmacSHA512(hashSecret, signData);
        //        string computedHash;
        //        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(hashSecret)))
        //        {
        //            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signData));
        //            computedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        //        }

        //        // So sánh chữ ký được tạo với chữ ký từ VNPay
        //        return string.Equals(computedHash, vnp_SecureHash, StringComparison.OrdinalIgnoreCase);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error nếu cần
        //        return false;
        //    }
        //}

        //private async Task<PaymentTransaction> SaveTransaction(VnPayCallbackDTO vnPayData)
        //{
        //    var transaction = new PaymentTransaction
        //    {
        //        VnpTxnRef = vnPayData.VnpTxnRef,
        //        Amount = decimal.Parse(vnPayData.VnpAmount) / 100,
        //        BankCode = vnPayData.VnpBankCode,
        //        BankTranNo = vnPayData.VnpBankTranNo,
        //        CardType = vnPayData.VnpCardType,
        //        OrderInfo = vnPayData.VnpOrderInfo,
        //        PayDate = DateTime.ParseExact(vnPayData.VnpPayDate, "yyyyMMddHHmmss", null),
        //        ResponseCode = vnPayData.VnpResponseCode,
        //        TransactionNo = vnPayData.VnpTransactionNo,
        //        TransactionStatus = vnPayData.VnpTransactionStatus,
        //        CreatedDate = DateTime.UtcNow
        //    };

        //    await _unitOfWork.PaymentTransactionRepository.CreateAsync(transaction);
        //    return transaction;
        //}

        private bool IsSuccessfulTransaction(Dictionary<string, string> paramsQuery)
        {
            return paramsQuery["vnp_ResponseCode"] == "00" &&
                   paramsQuery["vnp_TransactionStatus"] == "00";
        }

        //private bool IsEqualAmount(Dictionary<string, string> paramsQuery, Order order)
        //{
        //    return double.Parse(paramsQuery["vnp_Amount"]) == order.TotalPrice;
        //}

        private async Task<int> UpdateOrderStatus(Order order, string status)
        {
            order.OrderStatus = status;
            var updateOrderStatusStatus = await _unitOfWork.OrderRepository.UpdateAsync(order);
            return updateOrderStatusStatus;
        }
        //public async Task<(bool Success, string Message)> ProcessVnPayCallback2(IQueryCollection collection)
        //{
        //    try
        //    {

        //        if (collection == null || !collection.Any())
        //        {
        //            return (false, "Invalid parameters");
        //        }

        //        var vnPay = new VnPayLibrary();
        //        foreach (var (key, value) in collection)
        //        {
        //            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
        //            {
        //                vnPay.AddResponseData(key, value.ToString());
        //            }
        //        }

        //        var orderId = collection["vnp_TxnRef"].ToString();
        //        var vnPayTranId = Convert.ToInt64(collection["vnp_TransactionNo"]);
        //        var vnpResponseCode = collection["vnp_ResponseCode"].ToString();
        //        var vnpSecureHash = collection["vnp_SecureHash"].ToString();

        //        bool checkSignature = vnPay.ValidateSignature(vnpSecureHash, _config["VnPay:HashSecret"]);

        //        if (!checkSignature)
        //        {
        //            return (false, "Invalid signature");
        //        }

        //        if (vnpResponseCode == "00")
        //        {
        //            // Payment successful
        //            return (true, "Payment processed successfully");
        //        }

        //        return (false, "Payment processing failed");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        private Dictionary<string, string> ParseVnPayUrl(string url)
        {
            var queryParams = new Dictionary<string, string>();

            // Tách phần query string từ URL
            var queryString = url.Split('?').LastOrDefault();
            if (string.IsNullOrEmpty(queryString)) return queryParams;

            // Parse các tham số
            var parameters = queryString.Split('&');
            foreach (var param in parameters)
            {
                var keyValue = param.Split('=');
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0];
                    var value = HttpUtility.UrlDecode(keyValue[1]);
                    queryParams[key] = value;
                }
            }

            return queryParams;
        }

        // Phương thức xử lý và lưu response
        public async Task<(bool success, string message)> ProcessVnPayReturn(string returnUrl)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync()) // Bắt đầu giao dịch
            {
                try
                {
                    // 1. Validate input
                    if (string.IsNullOrEmpty(returnUrl))
                    {
                        return (false, "Return URL is empty or null");
                    }

                    var queryParams = ParseVnPayUrl(returnUrl);
                    if (!queryParams.Any())
                    {
                        return (false, "No parameters found in return URL");
                    }

                    // 2. Get and validate order information
                    if (!queryParams.ContainsKey("vnp_OrderInfo"))
                    {
                        return (false, "Order information not found");
                    }

                    string orderGet = queryParams["vnp_OrderInfo"];
                    if (string.IsNullOrEmpty(orderGet) || orderGet.Length < 1)
                    {
                        return (false, "Invalid order information format");
                    }

                    // 3. Parse order ID and get order
                    // string orderIdString = orderGet.Substring(orderGet.Length - 1);
                    if (!int.TryParse(orderGet, out int orderId))
                    {
                        return (false, "Invalid order ID format");
                    }

                    Order order = await _unitOfWork.OrderRepository.GetByOrderIdAsync(orderId);
                    if (order == null)
                    {
                        return (false, "Order not found");
                    }

                    string userId = order.UserId;
                    VnPayLibrary vnpay = new VnPayLibrary();
                    if (!queryParams.ContainsKey("vnp_SecureHash"))
                    {
                        return (false, "SecureHash Not found");
                    }

                    string vnp_SecureHash = queryParams["vnp_SecureHash"];
                    string hashSecret = _config["VnPay:HashSecret"];

                    // 4. Validate signature
                    AddResponseData(queryParams, vnpay);
                    if (!vnpay.ValidateSignature(vnp_SecureHash, hashSecret))
                    {
                        await UpdateOrderStatus(order, "Failed");
                        return (false, "Error processing payment due to Invalid Signature");
                    }

                    // 5. Validate amount and process transaction
                    //if (IsEqualAmount(queryParams, order))
                    //{
                    if (IsSuccessfulTransaction(queryParams))
                    {
                        // Create payment response
                        var paymentResponse = new PaymentTransactionDTO
                        {
                            VnpAmount = queryParams.GetValueOrDefault("vnp_Amount"),
                            VnpBankCode = queryParams.GetValueOrDefault("vnp_BankCode"),
                            VnpBankTranNo = queryParams.GetValueOrDefault("vnp_BankTranNo"),
                            VnpCardType = queryParams.GetValueOrDefault("vnp_CardType"),
                            VnpOrderInfo = orderId,
                            VnpPayDate = queryParams.GetValueOrDefault("vnp_PayDate"),
                            VnpResponseCode = queryParams.GetValueOrDefault("vnp_ResponseCode"),
                            VnpTmnCode = queryParams.GetValueOrDefault("vnp_TmnCode"),
                            VnpTransactionNo = queryParams.GetValueOrDefault("vnp_TransactionNo"),
                            VnpTransactionStatus = queryParams.GetValueOrDefault("vnp_TransactionStatus"),
                            VnpTxnRef = queryParams.GetValueOrDefault("vnp_TxnRef"),
                            VnpSecureHash = queryParams.GetValueOrDefault("vnp_SecureHash"),
                            PaymentStatus = queryParams.GetValueOrDefault("vnp_ResponseCode") == "00",
                            userId = userId
                        };


                        var result = _mapper.Map<PaymentTransaction>(paymentResponse);

                        //Nếu không phải đơn hàng vip
                        if (order.isVipUpgrade != true)
                        {
                            //Nếu không phải dạng buy now
                            if (order.isBuyNow != true)
                            {
                                //xóa cart items
                                var cart = await _unitOfWork.CartRepository.GetByIdAsync(order.UserId);
                                if (cart != null)
                                {
                                    await _unitOfWork.CartItemRepository.RemoveAllItemsInCart(cart.CartId);
                                    cart.TotalAmount = 0;
                                    var cartUpdateStatus = await _unitOfWork.CartRepository.UpdateAsync(cart);
                                    if (cartUpdateStatus != 1)
                                    {
                                        await transaction.RollbackAsync();
                                        return (false, "Update Cart failed");
                                    }
                                }


                            }
                            //if (order.isBuyNow == true)
                            //{
                            //    foreach (var item in order.OrderDetails)
                            //    {
                            //        var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                            //        order.ShopId = product.ShopId;
                            //        await _unitOfWork.OrderRepository.UpdateAsync(order);
                            //    }

                            //}
                            await CreateOrderShop(order);
                            //Cập nhật lại số lượng product
                            var UpdateProductStatus = await UpdateProductQuantity(order);
                            if (UpdateProductStatus != 1)
                            {
                                await transaction.RollbackAsync();
                                return (false, "Update Product Quantity failed");
                            }
                            //Cập nhật doanh thu
                            var updateRevenueStatus = await UpdateRevenue(order);
                            if (updateRevenueStatus != 1)
                            {
                                await transaction.RollbackAsync();
                                return (false, "Update Revenue failed");
                            }

                            var updateShopRevenueStatus = await UpdateRevenueShop(order);
                            if (updateRevenueStatus != 1)
                            {
                                await transaction.RollbackAsync();
                                return (false, "Update Shop Revenue failed");
                            }

                            var updateOrderStatusStatus = await UpdateOrderStatus(order, "Pending");
                            if (updateOrderStatusStatus != 1)
                            {
                                await transaction.RollbackAsync();
                                return (false, "Update Order Status Failed");
                            }
                        }
                        else
                        {
                            var updateVipRevenueStatus = await UpdateVipRevenue(order);
                            if (updateVipRevenueStatus != 1)
                            {
                                await transaction.RollbackAsync();
                                return (false, "Update Vip Revenue failed");

                            }
                            var updateVipOrderStatusStatus = await UpdateOrderStatus(order, "Successful");
                            if (updateVipOrderStatusStatus != 1)
                            {
                                await transaction.RollbackAsync();
                                return (false, "Update Order Vip Status Failed");
                            }
                        }
                        var addPaymentInfoStatus = await _unitOfWork.PaymentTransactionRepository.CreateAsync(result);
                        if (addPaymentInfoStatus != 1)
                        {
                            await transaction.RollbackAsync();
                            return (false, "Add Payment failed");
                        }



                        await transaction.CommitAsync();
                        return (true, "Payment processed successfully");

                    }
                    else
                    {
                        // Handle unsuccessful transaction
                        var updateOrderStatusResult = await UpdateOrderStatus(order, "Failed");
                        if (updateOrderStatusResult != 1)
                        {
                            await transaction.RollbackAsync();
                            return (false, "Failed to update order status for unsuccessful transaction");
                        }

                        await transaction.CommitAsync();
                        return (false, "Transaction was not successful");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"Error processing payment: {ex.Message}");
                }
            }
        }

        private async Task<int> UpdateProductQuantity(Order order)
        {
            try
            {
                var flag = 1;
                foreach (var item in order.OrderDetails)
                {

                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                    product.Quantity -= item.Quantity;
                    if (product.Quantity <= 0)
                    {
                        product.Quantity = 0;
                        product.Status = "Out Of Stock";
                    }

                    var updateProductResult = await _unitOfWork.ProductRepository.UpdateAsync(product);
                    if (updateProductResult != 1)
                    {
                        flag = 0;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<int> UpdateRevenue(Order order)
        {

            var totalRevenue = 0;
            foreach (var item in order.OrderDetails)
            {
                totalRevenue += (item.UnitPrice * item.Quantity * 8 / 100);
            }
            var revenueDto = new RevenueRequestDTO
            {
                OrderId = order.OrderId,
                // Income = (order.TotalPrice * 8 / 100)
                Income = totalRevenue
            };
            var revenue = _mapper.Map<Revenue>(revenueDto);

            var createResultRevenue = await _unitOfWork.RevenueRepository.CreateAsync(revenue);
            return createResultRevenue;
        }

        private async Task<int> UpdateRevenueShop(Order orderUser)
        {
            var orderList = await _unitOfWork.OrderRepository.GetShopListOrder(orderUser);
            var flag = 1;
            foreach (var order in orderList)
            {
                var totalRevenue = 0;
                foreach (var item in order.OrderDetails)
                {
                    totalRevenue += (item.UnitPrice * item.Quantity * 92 / 100);
                }
                var revenueDto = new RevenueRequestDTO
                {
                    OrderId = order.OrderId,
                    isShopRevenue = true,
                    Income = totalRevenue
                };
                var revenue = _mapper.Map<Revenue>(revenueDto);
                var result = await _unitOfWork.RevenueRepository.CreateAsync(revenue);
                if (result == 0)
                {
                    flag = 0;
                }
            }
            return flag;
        }

        private async Task<int> UpdateVipRevenue(Order order)
        {
            var revenueDto = new RevenueRequestDTO
            {
                OrderId = order.OrderId,
                isVip = true,
                Income = order.TotalPrice
            };

            var revenue = _mapper.Map<Revenue>(revenueDto);
            var createResultRevenue = await _unitOfWork.RevenueRepository.CreateAsync(revenue);
            return createResultRevenue;
        }

        public async Task CreateOrderShop(Order order)
        {
            Dictionary<int, List<Product>> allListProductMap = new Dictionary<int, List<Product>>();
            foreach (var item in order.OrderDetails)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                //var shop = _unitOfWork.ShopRepository.GetById(product.ShopId);
                if (!allListProductMap.ContainsKey(product.ShopId))
                {
                    allListProductMap[product.ShopId] = new List<Product>();
                }
                allListProductMap[product.ShopId].Add(product);
            }

            foreach (var shopIdInList in allListProductMap.Keys)
            {
                var orderShop = new Order
                {
                    ShopId = shopIdInList,
                    UserId = order.UserId,
                    Street = order.Street,
                    District = order.District,
                    City = order.City,
                    Country = order.Country,
                    FullName = order.FullName,
                    Phone = order.Phone,
                    CreateDate = order.CreateDate,
                    Email = order.Email,
                    isBuyNow = order.isBuyNow,
                    isVipUpgrade = order.isVipUpgrade,
                    OrderStatus = order.OrderStatus,
                    OrderDetails = new List<OrderDetail>()
                };
                //  await _unitOfWork.OrderRepository.CreateAsync(orderShop);
                //List<OrderDetail> orderDetailList = new List<OrderDetail>();

                foreach (var product in allListProductMap[shopIdInList])
                {
                    // Tìm OrderDetail gốc từ Order ban đầu dựa trên ProductId
                    var originalOrderDetail = order.OrderDetails.FirstOrDefault(od => od.ProductId == product.ProductId);

                    if (originalOrderDetail != null)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderId = orderShop.OrderId,
                            ProductId = product.ProductId,
                            Quantity = originalOrderDetail.Quantity,
                            UnitPrice = originalOrderDetail.UnitPrice,
                            OrderDetailStatus = originalOrderDetail.OrderDetailStatus
                        };

                        orderShop.OrderDetails.Add(orderDetail);
                    }
                }

                //orderShop.OrderDetails = orderDetailList;
                orderShop.TotalPrice = orderShop.OrderDetails.Sum(od => od.Quantity * od.UnitPrice);

                await _unitOfWork.OrderRepository.CreateAsync(orderShop);
                //await _unitOfWork.OrderRepository.UpdateAsync(orderShop);

            }

        }
        //public async Task<(bool success, string message)> ProcessVnPayReturnBuyNow(string returnUrl)
        //{
        //    try
        //    {
        //        // 1. Validate input
        //        if (string.IsNullOrEmpty(returnUrl))
        //        {
        //            return (false, "Return URL is empty or null");
        //        }

        //        var queryParams = ParseVnPayUrl(returnUrl);
        //        if (!queryParams.Any())
        //        {
        //            return (false, "No parameters found in return URL");
        //        }

        //        // 2. Get and validate order information
        //        if (!queryParams.ContainsKey("vnp_OrderInfo"))
        //        {
        //            return (false, "Order information not found");
        //        }

        //        string orderGet = queryParams["vnp_OrderInfo"];
        //        if (string.IsNullOrEmpty(orderGet) || orderGet.Length < 1)
        //        {
        //            return (false, "Invalid order information format");
        //        }

        //        // 3. Parse order ID and get order
        //        string orderIdString = orderGet.Substring(orderGet.Length - 1);
        //        if (!int.TryParse(orderIdString, out int orderId))
        //        {
        //            return (false, "Invalid order ID format");
        //        }

        //        Order order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        //        if (order == null)
        //        {
        //            return (false, "Order not found");
        //        }

        //        string userId = order.UserId;
        //        VnPayLibrary vnpay = new VnPayLibrary();
        //        if (!queryParams.ContainsKey("vnp_SecureHash"))
        //        {
        //            return (false, "SecureHash Not found");
        //        }

        //        string vnp_SecureHash = queryParams["vnp_SecureHash"];
        //        string hashSecret = _config["VnPay:HashSecret"];

        //        // 4. Validate signature
        //        AddResponseData(queryParams, vnpay);
        //        if (!vnpay.ValidateSignature(vnp_SecureHash, hashSecret))
        //        {
        //            await UpdateOrderStatus(order, "Giao dịch thất bại");
        //            return (false, "Error processing payment due to Invalid Signature");
        //        }

        //        // 5. Validate amount and process transaction
        //        //if (IsEqualAmount(queryParams, order))
        //        //{
        //        if (IsSuccessfulTransaction(queryParams))
        //        {
        //            // Create payment response
        //            var paymentResponse = new PaymentTransactionDTO
        //            {
        //                VnpAmount = queryParams.GetValueOrDefault("vnp_Amount"),
        //                VnpBankCode = queryParams.GetValueOrDefault("vnp_BankCode"),
        //                VnpBankTranNo = queryParams.GetValueOrDefault("vnp_BankTranNo"),
        //                VnpCardType = queryParams.GetValueOrDefault("vnp_CardType"),
        //                VnpOrderInfo = orderId,
        //                VnpPayDate = queryParams.GetValueOrDefault("vnp_PayDate"),
        //                VnpResponseCode = queryParams.GetValueOrDefault("vnp_ResponseCode"),
        //                VnpTmnCode = queryParams.GetValueOrDefault("vnp_TmnCode"),
        //                VnpTransactionNo = queryParams.GetValueOrDefault("vnp_TransactionNo"),
        //                VnpTransactionStatus = queryParams.GetValueOrDefault("vnp_TransactionStatus"),
        //                VnpTxnRef = queryParams.GetValueOrDefault("vnp_TxnRef"),
        //                VnpSecureHash = queryParams.GetValueOrDefault("vnp_SecureHash"),
        //                PaymentStatus = queryParams.GetValueOrDefault("vnp_ResponseCode") == "00",
        //                userId = userId
        //            };

        //            // Save transaction and update order
        //            var result = _mapper.Map<PaymentTransaction>(paymentResponse);
        //            await _unitOfWork.PaymentTransactionRepository.CreateAsync(result);
        //            await UpdateOrderStatus(order, "Giao dịch thành công");
        //            return (true, "Payment processed successfully");
        //        }
        //        await UpdateOrderStatus(order, "Giao dịch thất bại");
        //        return (false, "Error processing payment");
        //        //}

        //        //await UpdateOrderStatus(order, "Giao dịch thất bại");
        //        //return (false, "Error processing payment due to Invalid Amount");
        //    }
        //    catch (Exception ex)
        //    {
        //        return (false, $"Error processing payment: {ex.Message}");
        //    }
        //}

        private void AddResponseData(Dictionary<string, string> queryParams, VnPayLibrary vnpay)
        {
            vnpay.AddResponseData("vnp_Amount", queryParams.GetValueOrDefault("vnp_Amount"));
            vnpay.AddResponseData("vnp_BankCode", queryParams.GetValueOrDefault("vnp_BankCode"));
            vnpay.AddResponseData("vnp_BankTranNo", queryParams.GetValueOrDefault("vnp_BankTranNo"));
            vnpay.AddResponseData("vnp_CardType", queryParams.GetValueOrDefault("vnp_CardType"));
            vnpay.AddResponseData("vnp_OrderInfo", queryParams.GetValueOrDefault("vnp_OrderInfo"));
            vnpay.AddResponseData("vnp_PayDate", queryParams.GetValueOrDefault("vnp_PayDate"));
            vnpay.AddResponseData("vnp_ResponseCode", queryParams.GetValueOrDefault("vnp_ResponseCode"));
            vnpay.AddResponseData("vnp_TmnCode", queryParams.GetValueOrDefault("vnp_TmnCode"));
            vnpay.AddResponseData("vnp_TransactionNo", queryParams.GetValueOrDefault("vnp_TransactionNo"));
            vnpay.AddResponseData("vnp_TransactionStatus", queryParams.GetValueOrDefault("vnp_TransactionStatus"));
            vnpay.AddResponseData("vnp_TxnRef", queryParams.GetValueOrDefault("vnp_TxnRef"));
            vnpay.AddResponseData("vnp_SecureHash", queryParams.GetValueOrDefault("vnp_SecureHash"));
        }


    }
}
