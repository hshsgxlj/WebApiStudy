namespace WebApp.Models.Repository
{
    public static class ShirtRepository
    {
        private static List<Shirt> shirts = new()
        {
                new Shirt { ShirtId = 1, Brand = "Nike", Color = "Red", Gender = "men", Size = 9, Price = 29.99 },
                new Shirt { ShirtId = 2, Brand = "Adidas", Color = "Blue", Gender = "women", Size = 6, Price = 25.99 },
                new Shirt { ShirtId = 3, Brand = "Puma", Color = "Black", Gender = "men", Size = 10, Price = 19.99 },
                new Shirt { ShirtId = 4, Brand = "Uniqlo", Color = "White", Gender = "women", Size = 7, Price = 15.99 }
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
        public static void AddShirt(Shirt shirt)
        {
            shirt.ShirtId = shirts.Max(x => x.ShirtId) + 1;
            shirts.Add(shirt);
        }
        public static Shirt? GetShirtByProperties(Shirt shirt)
        {
            return shirts.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.Brand) &&
                                            !string.IsNullOrWhiteSpace(shirt.Brand) &&
                                            x.Brand.Equals(shirt.Brand) &&
                                            !string.IsNullOrWhiteSpace(x.Color) &&
                                            !string.IsNullOrWhiteSpace(shirt.Color) &&
                                            x.Color.Equals(shirt.Color) &&
                                            !string.IsNullOrWhiteSpace(x.Gender) &&
                                            !string.IsNullOrWhiteSpace(shirt.Gender) &&
                                            x.Gender.Equals(shirt.Gender) &&
                                            x.Size.HasValue &&
                                            shirt.Size.HasValue &&
                                            shirt.Size.Value == x.Size.Value);
        }
        public static void UpdateShirt(Shirt shirt)
        {
            var ShirtToUpdate = shirts.First(x => x.ShirtId == shirt.ShirtId);
            ShirtToUpdate.Brand = shirt.Brand;
            ShirtToUpdate.Color = shirt.Color;
            ShirtToUpdate.Gender = shirt.Gender;
            ShirtToUpdate.Size = shirt.Size;
            ShirtToUpdate.Price = shirt.Price;
        }
        public static void DeleteShirt(int shirtId)
        {
            var shirt = GetShirtById(shirtId);
            if (shirt != null)
            {
                shirts.Remove(shirt);
            }
        }
    }
}
