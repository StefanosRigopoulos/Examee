using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork(DataContext context, IMapper mapper) : IUnitOfWork
    {
        public IUserRepository UserRepository => new UserRepository(context, mapper);
        public IExamRepository ExamRepository => new ExamRepository(context, mapper);
        
        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }
        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }
    }
}