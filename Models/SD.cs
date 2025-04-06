namespace Reina.MacCredy.Models
{
    public static class SD
    {
        // Role Constants
        public const string Role_Customer = "Customer";
        public const string Role_Admin = "Admin";
        public const string Role_Company = "Company";
        public const string Role_Employee = "Employee";
        
        // Payment Method Constants
        public const int Payment_CashOnDelivery = 1;
        public const int Payment_CreditCard = 2;
        public const int Payment_BankTransfer = 3;
        public const int Payment_MoMo = 4;
        public const int Payment_VNPay = 5;
        
        // Payment Status Constants
        public const int PaymentStatus_Pending = 0;
        public const int PaymentStatus_Approved = 1;
        public const int PaymentStatus_Rejected = 2;
        public const int PaymentStatus_Refunded = 3;
    }
}
