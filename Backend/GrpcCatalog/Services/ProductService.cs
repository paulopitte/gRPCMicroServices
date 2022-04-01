﻿using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcCatalog.Data;
using GrpcCatalog.Domain;
using GrpcCatalog.Protos;

namespace GrpcCatalog.Services
{
    public class ProductService : ProductProdtService.ProductProdtServiceBase
    {

        private readonly ILogger<ProductService> _logger;
        private readonly ICatalogRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(ILogger<ProductService> logger, ICatalogRepository repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }







        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Obtendo Produto ID: {request.Id} ...");


            var product = await _repository.GetProduct(request.Id);

            if (product is null)
            {
                //grpc excpetion
                throw new RpcException(status: Grpc.Core.Status.DefaultCancelled);
            }

            var productModel = _mapper.Map<ProductModel>(product);
            return productModel;
        }


        public override async Task GetProducts(GetAllProductRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Obtendo lista de Produtos...");

            var products = await _repository.GetProducts();
            if (products.Count == 0)
            {
                //grpc excpetion
                throw new RpcException(status: Grpc.Core.Status.DefaultCancelled, message: "Product not found...");
            }

            products.ForEach(async product =>
            {
                var productModel = _mapper.Map<ProductModel>(product);
                await responseStream.WriteAsync(productModel);
            });
        }





        public override async Task<ProductModel> Add(AddRequest request, ServerCallContext context)
        {
            var domain = _mapper.Map<Product>(request.Product);
            var result = await _repository.Add(domain).ConfigureAwait(false);
            return _mapper.Map<ProductModel>(result);
        }



        public override Task<ProductModel> Update(UpdateRequest request, ServerCallContext context)
        {
            return base.Update(request, context);
        }
    }
}
