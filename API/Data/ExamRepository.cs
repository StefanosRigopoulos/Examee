using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ExamRepository(DataContext context, IMapper mapper) : IExamRepository
    {
        public void Update(Exam examDll)
        {
            context.Entry(examDll).State = EntityState.Modified;
        }

        public void Delete(Exam exam)
        {
            context.Remove(exam);
        }

        public async Task<IEnumerable<ExamDTO>> GetExamsAsync(string username)
        {
            return await context.Exams
                                 .Where(x => x.AppUser.UserName == username)
                                 .ProjectTo<ExamDTO>(mapper.ConfigurationProvider)
                                 .ToListAsync();
        }

        public async Task<Exam?> GetExamEntityAsync(string username, string examname)
        {
            return await context.Exams
                                 .Where(x => x.AppUser.UserName == username)
                                 .SingleOrDefaultAsync(x => x.ExamName == examname);
        }

        public async Task<ExamDTO?> GetExamAsync(string username, string examname)
        {
            return await context.Exams
                                 .Where(x => (x.ExamName == examname) && (x.AppUser.UserName == username))
                                 .ProjectTo<ExamDTO>(mapper.ConfigurationProvider)
                                 .FirstOrDefaultAsync();
        }
    }
}