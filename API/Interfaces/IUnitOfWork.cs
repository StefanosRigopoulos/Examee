namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IExamRepository ExamRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}