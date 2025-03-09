using RepositoryPatternWithEFCore.EF4.Dtos.DtoWareHouses;

namespace RepositoryPatternWithUOW.Core.MappeingModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, dtoDetails>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreatedDate))
                  .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products != null ? src.Products.ToList() : new List<Product>()))
                .ReverseMap();

            CreateMap<Category, dtoCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            CreateMap<Product, DtoDetailsProduct>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.pro_Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.pro_Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Purchaseprice, opt => opt.MapFrom(src => src.PurchasePrice))
                .ForMember(dest => dest.sellingprice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.Pro_DateAdded, opt => opt.MapFrom(src => src.DateAdded))
                .ForMember(dest => dest.Pro_Expirydate, opt => opt.MapFrom(src => src.ExpiryDate))
                .ForMember(dest => dest.InternationalCode, opt => opt.MapFrom(src => src.InternationalCode))
                .ForMember(dest => dest.Pro_Shortcode, opt => opt.MapFrom(src => src.ShortCode))
                .ForMember(dest => dest.pro_CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.productExpiry, opt => opt.MapFrom(src => src.ExpiryDate <= DateTime.Now))
                .ForMember(dest => dest.Category_Name, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name ?? "Unknown" : "Unknown")) 
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore()); 
                


            CreateMap<Product, dtoProduct>()
                .ForMember(dest => dest.Pro_Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Pro_Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PurchasePrice, opt => opt.MapFrom(src => src.PurchasePrice))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.Pro_DateAdded, opt => opt.MapFrom(src => src.DateAdded))
                .ForMember(dest => dest.Pro_ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
                .ForMember(dest => dest.InternationalCode, opt => opt.MapFrom(src => src.InternationalCode))
                .ForMember(dest => dest.Pro_Shortcode, opt => opt.MapFrom(src => src.ShortCode))
                .ForMember(dest => dest.Pro_CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ReverseMap(); // Expired check

            // Mapping ProductStock to DtoProductStock
            CreateMap<Stock, DtoProductStock>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ReverseMap();

            // Mapping ProductStock to DtoProductStockDetails
            CreateMap<Stock, DtoProductStockDetails>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : default))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product != null ? src.Product.SellingPrice : default))
                .ForMember(dest => dest.Expiry, opt => opt.MapFrom(src => src.Product != null ? src.Product.ExpiryDate : default))

                .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : default))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ReverseMap();

            // Mapping WareHouses to DtoWareHouses
            CreateMap<Warehouse, DtoWareHouses>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ReverseMap();
            // Mapping WareHouses to DtoWareHousesDetails
            CreateMap<Warehouse, DtoWareHousesDetials>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ReverseMap();

            // Mapping StockTransaction to StockTransactionDto
            CreateMap<StockTransaction, StockTransactionDto>()
                .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                .ForMember(dest => dest.DestinationWarehouseId, opt => opt.MapFrom(src => src.DestinationWarehouseId))
                .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.OldValue, opt => opt.MapFrom(src => src.OldValue))
                .ForMember(dest => dest.ValueChanged, opt => opt.MapFrom(src => src.ChangeValue))
                .ForMember(dest => dest.NewValue, opt => opt.MapFrom(src => src.NewValue))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));
        }
    }
    
}
