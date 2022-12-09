using Microsoft.EntityFrameworkCore;
using MyDigitalCv.Data;
using MyDigitalCv.Interfaces;
using MyDigitalCv.Models;

namespace MyDigitalCv.Repositories
{
	public class BlogPostRepository : IBlogPostRepository
	{
		private readonly ApplicationDbContext _context;
		public BlogPostRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public bool Add(BlogPost post)
		{
			_context.Add(post);
			return Save();
		}

		public bool Delete(BlogPost post)
		{
			_context.Remove(post);
			return Save();
		}
		public bool Update(BlogPost post)
		{
			_context.Update(post);
			return Save();
		}

		public async Task<BlogPost> GetByIdAsync(int id)
		{
			return await _context.Posts.FirstOrDefaultAsync(n => n.Id == id);
		}
		public IEnumerable<BlogPost> GetAll()
		{
			return _context.Posts;
		}

		public bool Save()
		{
			var result = _context.SaveChanges();
			return (result>0)?true:false; 
		}
	}
}
