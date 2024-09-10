namespace WebhookIntegration.Models
{
    public class BillingDetails
    {
        public Address Address { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class Address
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
    }

    public class Outcome
    {
        public string NetworkStatus { get; set; }
        public string Reason { get; set; }
        public string RiskLevel { get; set; }
        public int RiskScore { get; set; }
        public string SellerMessage { get; set; }
        public string Type { get; set; }
    }

    public class Checks
    {
        public string AddressLine1Check { get; set; }
        public string AddressPostalCodeCheck { get; set; }
        public string CvcCheck { get; set; }
    }

    public class Card
    {
        public int AmountAuthorized { get; set; }
        public string AuthorizationCode { get; set; }
        public string Brand { get; set; }
        public Checks Checks { get; set; }
        public string Country { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Fingerprint { get; set; }
        public string Funding { get; set; }
        public string Last4 { get; set; }
        public string Network { get; set; }
    }

    public class PaymentMethodDetails
    {
        public Card Card { get; set; }
        public string Type { get; set; }
    }

    public class Refunds
    {
        public string Object { get; set; }
        public List<object> Data { get; set; }
        public bool HasMore { get; set; }
        public int TotalCount { get; set; }
        public string Url { get; set; }
    }

    public class StripeChargeCustom
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public int Amount { get; set; }
        public int AmountCaptured { get; set; }
        public int AmountRefunded { get; set; }
        public object Application { get; set; }
        public object ApplicationFee { get; set; }
        public object ApplicationFeeAmount { get; set; }
        public string BalanceTransaction { get; set; }
        public BillingDetails BillingDetails { get; set; }
        public string CalculatedStatementDescriptor { get; set; }
        public bool Captured { get; set; }
        public long Created { get; set; }
        public string Currency { get; set; }
        public object Customer { get; set; }
        public string Description { get; set; }
        public object Destination { get; set; }
        public object Dispute { get; set; }
        public bool Disputed { get; set; }
        public object FailureBalanceTransaction { get; set; }
        public object FailureCode { get; set; }
        public object FailureMessage { get; set; }
        public Dictionary<string, string> FraudDetails { get; set; }
        public object Invoice { get; set; }
        public bool Livemode { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public object OnBehalfOf { get; set; }
        public object Order { get; set; }
        public Outcome Outcome { get; set; }
        public bool Paid { get; set; }
        public string PaymentIntent { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentMethodDetails PaymentMethodDetails { get; set; }
        public object ReceiptEmail { get; set; }
        public object ReceiptNumber { get; set; }
        public string ReceiptUrl { get; set; }
        public bool Refunded { get; set; }
        public Refunds Refunds { get; set; }
        public object Review { get; set; }
        public object Shipping { get; set; }
        public object Source { get; set; }
        public object SourceTransfer { get; set; }
        public object StatementDescriptor { get; set; }
        public object StatementDescriptorSuffix { get; set; }
        public string Status { get; set; }
        public object TransferData { get; set; }
        public object TransferGroup { get; set; }
    }

}
