using Microsoft.EntityFrameworkCore;
using MyDigitalCv.Data;
using MyDigitalCv.Interfaces;
using MyDigitalCv.Models;

namespace MyDigitalCv.Repositories
{
	public class CommentsRepository : ICommentsRepository
	{
		private readonly ApplicationDbContext _context;
		public CommentsRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public bool Add(Comment comment)
		{
			_context.Add(comment);
			return Save();
		}

		public bool Delete(Comment comment)
		{
			_context.Remove(comment);
			return Save();
		}
		public bool Update(Comment comment)
		{
			_context.Update(comment);
			return Save();
		}

		public async Task<Comment> GetByIdAsync(int id)
		{
			return await _context.Comments.FirstOrDefaultAsync(n => n.Id == id);
		}
		public async Task<IEnumerable<Comment>> GetAllByPostId(int id)
		{
			return await _context.Comments.Where(c => c.PostId.Equals(id)).ToListAsync();
		}

		public bool Save()
		{
			var result = _context.SaveChanges();
			return (result > 0) ? true : false;
		}
	}
}
