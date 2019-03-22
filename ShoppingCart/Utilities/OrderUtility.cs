using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Utilities
{
    public static class OrderUtility
    {
        public const string StatusSubmitted = "Submitted";
        public const string StatusInProcess = "Being Prepared";
        public const string StatusReady = "Ready for Pickup";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled= "Cancelled";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusRejected = "Rejected";
    }
}
