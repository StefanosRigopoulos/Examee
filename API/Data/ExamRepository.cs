using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ExamRepository : IExamRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ExamRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public void Update(Exam examDll)
        {
            _context.Entry(examDll).State = EntityState.Modified;
        }

        public void Delete(Exam exam)
        {
            _context.Remove(exam);
        }

        public async Task<IEnumerable<ExamDTO>> GetExamsAsync(string username)
        {
            return await _context.Exams
                                 .Where(x => x.AppUser.UserName == username)
                                 .ProjectTo<ExamDTO>(_mapper.ConfigurationProvider)
                                 .ToListAsync();
        }

        public async Task<Exam> GetExamEntityAsync(string username, string examname)
        {
            return await _context.Exams
                                 .Where(x => x.AppUser.UserName == username)
                                 .SingleOrDefaultAsync(x => x.ExamName == examname);
        }

        public async Task<ExamDTO> GetExamAsync(string username, string examname)
        {
            return await _context.Exams
                                 .Where(x => (x.ExamName == examname) && (x.AppUser.UserName == username))
                                 .ProjectTo<ExamDTO>(_mapper.ConfigurationProvider)
                                 .FirstOrDefaultAsync();
        }
    }
}