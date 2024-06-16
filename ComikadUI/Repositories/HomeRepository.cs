

using Microsoft.EntityFrameworkCore;

namespace ComikadUI.Repositories
{
    public class HomeRepository:IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db) 
        {
            _db = db;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genre.ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooks(string sTerm="", int genreId=0)
        {
            sTerm=sTerm.ToLower();
            IEnumerable<Book> books= await (from book in _db.Books
                       join genre in _db.Genre
                       on book.GenreId equals genre.Id
                       where string.IsNullOrWhiteSpace(sTerm) || (book!=null && book.BookName.ToLower().StartsWith(sTerm))
                       select new Book
                       {
                            Id=book.Id,
                            Image=book.Image,
                            AutorName=book.AutorName,
                            BookName=book.BookName,
                            GenreId=book.GenreId,
                            Price=book.Price,
                            GenreName=genre.GenreName   
                       }
                       ).ToListAsync();
            if(genreId>0)
            {

                books = books.Where(a => a.GenreId == genreId).ToList();
            }
            return books;

        }
    }
}
