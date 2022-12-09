using MyDigitalCv.Models;

namespace MyDigitalCv.Interfaces
{
	public interface IBlogPostRepository
	{
		Task<BlogPost> GetByIdAsync(int id);
		IEnumerable<BlogPost> GetAll();
		bool Add(BlogPost blogPost);
		bool Delete(BlogPost blogPost);
		bool Update(BlogPost blogPost);
		bool Save();
	}
}
