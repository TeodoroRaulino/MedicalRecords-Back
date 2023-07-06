namespace WebApplication1.DTOs
{
    public class GetMedicalRecordsDTO
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; }
        public string FullName { get; set; }
        public string CPF { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDTO? Address { get; set; }
        public int UserId { get; set; }
    }
    public class GetAddressDTO
    {
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
