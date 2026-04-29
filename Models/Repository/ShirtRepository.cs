namespace WebApiStudy.Models.Repository
{
    public static class ShirtRepository
    {
        private static List<Shirt> shirts = new List<Shirt>()
        {
            new Shirt { ShirtId = 1, Brand = "Nike", color = "Red", Gender = "men", Size = 9, Price = 29.99 },
            new Shirt { ShirtId = 2, Brand = "Adidas", color = "Blue", Gender = "women", Size = 6, Price = 25.99 },
            new Shirt { ShirtId = 3, Brand = "Puma", color = "Black", Gender = "men", Size = 10, Price = 19.99 },
            new Shirt { ShirtId = 4, Brand = "Uniqlo", color = "White", Gender = "women", Size = 7, Price = 15.99 }
        };
        public static List<Shirt> GetShirts()
        {
            return shirts;
        }
        public static bool ShirtExists(int id)
        {
            return shirts.Any(x => x.ShirtId == id);
        }
        public static Shirt? GetShirtById(int id)
        {
            return shirts.FirstOrDefault(x => x.ShirtId == id);
        }
    }
}
