using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using GrpcCatalog.Protos;

namespace GrpcCatalog.Mapper
{
    public class ProductProfile : Profile
    {

        public ProductProfile()
        {

            CreateMap<Domain.Product, ProductModel>()
                .ForMember(dest => dest.CreateAt,
                opt => opt.MapFrom(src => Timestamp.FromDateTime(src.CreateAt)));


            CreateMap<ProductModel, Domain.Product>()
             .ForMember(dest => dest.CreateAt,
             opt => opt.MapFrom(src => src.CreateAt.ToDateTime()));

        }
    }
}
