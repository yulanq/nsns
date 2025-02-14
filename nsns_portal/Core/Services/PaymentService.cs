using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Core.Models;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Core.Repositories;
using System.Numerics;
using System.Xml.Linq;

namespace Core.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // 🔹 Get all payments
        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Payment>> GetByChildAsync(int childId)
        {
            return await _paymentRepository.GetByChildAsync(childId);
        }

        // 🔹 Get payment by ID
        public async Task<Payment> GetByIdAsync(int id)
        {
            return await _paymentRepository.GetByIdAsync(id);
        }


        public async Task<Child> GetChildByIdAsync(int childId)
        {
            return await _paymentRepository.GetChildByIdAsync(childId);
        }


        public async Task<IEnumerable<Parent>> GetParentsByChildAsync(int childId)
        {
            return await _paymentRepository.GetParentsByChildAsync(childId);
        }

        public async Task<IEnumerable<PaymentPackage>> GetAllActivePackagesAsync()
        {
            return await _paymentRepository.GetAllActivePackagesAsync();
        }

        // 🔹 Add a new payment
        public async Task<bool> AddAsync(Payment payment)
        {
            if (payment.ParentID == null || payment.PaymentPackageID == null)
                throw new ArgumentException("Parent and Payment Package must be selected.");

            if (payment.Amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            return await _paymentRepository.AddAsync(payment);
        }

        // 🔹 Update an existing payment
        public async Task<bool> UpdateAsync(Payment payment)
        {
            var existingPayment = await GetByIdAsync(payment.PaymentID);
            if (existingPayment == null)
                throw new Exception("Payment not found.");

            return await _paymentRepository.UpdateAsync(payment);
        }

        // 🔹 Delete a payment
        public async Task<bool> RemoveAsync(int id)
        {
            var payment = await GetByIdAsync(id);
            if (payment == null)
                throw new Exception("Payment not found.");

            return await _paymentRepository.RemoveAsync(id);
        }
    }
}





