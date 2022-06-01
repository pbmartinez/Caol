using Domain.Entities;
using Domain.IRepositories;
using Domain.UnitOfWork;

namespace Infraestructure.Domain.Repositories
{
    public partial class CaoORepository : Repository<CaoO>, ICaoORepository
    {
        public CaoORepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
