using E_Com_Monolithic.Models;

namespace E_Com_Monolithic.Helper
{
    public class ProductListPaginationModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

    }
}
