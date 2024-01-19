namespace generalStore.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();

        public PagingInfo PagingInfo { get; set; } = new PagingInfo();

        public IEnumerable<Size> Sizes { get; set; } = Enumerable.Empty<Size>();
        public IEnumerable<Color> Colors { get; set; } = Enumerable.Empty<Color>();
    }
}
