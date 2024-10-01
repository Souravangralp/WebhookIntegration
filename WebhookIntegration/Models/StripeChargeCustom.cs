namespace WebhookIntegration.Models
{
    public class Address
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
    }

    public class BillingDetails
    {
        public Address Address { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class Refunds
    {
        public string Object { get; set; }
        public List<object> Data { get; set; }
        public bool HasMore { get; set; }
        public int TotalCount { get; set; }
        public string Url { get; set; }
    }

    public class UsBankAccount
    {
        public string AccountHolderType { get; set; }
        public string AccountType { get; set; }
        public string BankName { get; set; }
        public string Fingerprint { get; set; }
        public string Last4 { get; set; }
        public string Mandate { get; set; }
        public string PaymentReference { get; set; }
        public string RoutingNumber { get; set; }
    }

    public class PaymentMethodDetails
    {
        public string Type { get; set; }
        public UsBankAccount UsBankAccount { get; set; }
    }

    public class Charge
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Amount { get; set; }
        public int AmountCaptured { get; set; }
        public int AmountRefunded { get; set; }
        public string BalanceTransaction { get; set; }
        public BillingDetails BillingDetails { get; set; }
        public string CalculatedStatementDescriptor { get; set; }
        public bool Captured { get; set; }
        public long Created { get; set; }
        public string Currency { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public bool Disputed { get; set; }
        public bool Paid { get; set; }
        public string PaymentIntent { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentMethodDetails PaymentMethodDetails { get; set; }
        public Refunds Refunds { get; set; }
        public string Status { get; set; }
        public bool Refunded { get; set; }
    }

    public class Charges
    {
        public string Object { get; set; }
        public List<Charge> Data { get; set; }
        public bool HasMore { get; set; }
        public int TotalCount { get; set; }
        public string Url { get; set; }
    }

    public class UsBankAccountOptions
    {
        public string VerificationMethod { get; set; }
    }

    public class PaymentMethodOptions
    {
        public UsBankAccountOptions UsBankAccount { get; set; }
    }

    public class StripePaymentIntent
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Amount { get; set; }
        public int AmountCapturable { get; set; }
        public int AmountReceived { get; set; }
        public string CaptureMethod { get; set; }
        public Charges Charges { get; set; }
        public string ClientSecret { get; set; }
        public string ConfirmationMethod { get; set; }
        public long Created { get; set; }
        public string Currency { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentMethodOptions PaymentMethodOptions { get; set; }
        public List<string> PaymentMethodTypes { get; set; }
        public string ReceiptEmail { get; set; }
        public string Status { get; set; }
    }
}
