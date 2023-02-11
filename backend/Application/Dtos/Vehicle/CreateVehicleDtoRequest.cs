namespace Application.Dtos.Vehicle
{
    public class CreateVehicleDtoRequest
    {
        public string Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string Registration { get; set; } = default!;
        public string Vin { get; set; } = default!;
        public int Seats { get; set; }
        public int YearOfProduction { get; set; }
        public double Price { get; set; }
        public Guid ModelId { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid FuelId { get; set; } 
    }
}