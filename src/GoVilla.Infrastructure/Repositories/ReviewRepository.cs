using GoVilla.Domain.Reviews;

namespace GoVilla.Infrastructure.Repositories;

internal sealed class ReviewRepository : Repository<Review, ReviewId>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}