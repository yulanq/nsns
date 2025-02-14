using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment> GetByIdAsync(int id);

        Task<IEnumerable<Payment>> GetByChildAsync(int childId);

        Task<Child> GetChildByIdAsync(int childId);

        Task<IEnumerable<Parent>> GetParentsByChildAsync(int childId);

        Task<IEnumerable<PaymentPackage>> GetAllActivePackagesAsync();
        Task<bool> AddAsync(Payment payment);
        Task<bool> UpdateAsync(Payment payment);
        Task<bool> RemoveAsync(int id);
    }
}
