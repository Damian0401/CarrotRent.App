namespace Application.Interfaces
{
    public interface ISeedRepository
    {
        void SeedVehicleStatuses();
        void SeedFuels();
        void SeedRoles();
        void SeedRentStatuses();
        void SeedModelsAndBrands();
        void SeedDepartments();
    }
}