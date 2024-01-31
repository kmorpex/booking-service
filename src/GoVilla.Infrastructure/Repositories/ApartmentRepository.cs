using GoVilla.Domain.Apartments;

namespace GoVilla.Infrastructure.Repositories;

internal sealed class ApartmentRepository : Repository<Apartment, ApartmentId>, IApartmentRepository
{
    public ApartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}