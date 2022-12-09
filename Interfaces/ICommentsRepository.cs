using MyDigitalCv.Models;

namespace MyDigitalCv.Interfaces
{
	public interface ICommentsRepository
	{
		Task<Comment> GetByIdAsync(int id);
		Task<IEnumerable<Comment>> GetAllByPostId(int id);
		bool Add(Comment comment);
		bool Delete(Comment comment);
		bool Update(Comment comment);
		bool Save();
	}
}
