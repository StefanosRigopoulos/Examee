using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IExamRepository
    {
        void Update(Exam examDll);
        public void Delete(Exam exam);
        Task<IEnumerable<ExamDTO>> GetExamsAsync(string username);
        Task<Exam> GetExamEntityAsync(string username, string examname);
        Task<ExamDTO> GetExamAsync(string username, string examname);
    }
}