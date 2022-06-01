using Domain.Entities;
using Domain.IRepositories;
using Domain.UnitOfWork;

namespace Infraestructure.Domain.Repositories
{
    public partial class CaoFaturaRepository : Repository<CaoFatura>, ICaoFaturaRepository
    {
        public CaoFaturaRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
