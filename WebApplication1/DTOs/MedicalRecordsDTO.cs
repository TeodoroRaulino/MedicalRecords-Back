namespace WebApplication1.DTOs
{
    public class MedicalRecordsDTO
    {
        public IFormFile Photo { get; set; }
        public string PhotoPath { get; set; }
        public string FullName { get; set; }
        public string CPF { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDTO? Address { get; set; }
        public int UserId { get; set; }
    }

    public class AddressDTO
    {
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
