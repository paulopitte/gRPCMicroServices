using Grpc.Core;
using Grpc.Net.Client;
using GrpcCatalog.Protos;
using GrpcCatalog.Services;

class Program
{

    private const string SERVER_GRPC = "https://localhost:7069";


    static async Task Main()
    {

        try
        {


            if (!SERVER_GRPC.StartsWith("https"))
            {
                AppContext.SetSwitch(
                    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            }

            var channel = GrpcChannel.ForAddress(SERVER_GRPC);
            var client = new ProductProdtService.ProductProdtServiceClient(channel);




            Console.WriteLine("Incluindo Produtos inicial via Grpc...");
            await AddAsync(client);


            Console.WriteLine("Obtendo Produtos via Grpc...");
            await GetProducts(client);
        }
        catch (RpcException ex)
        {

            Console.WriteLine(ex.Message);
        }

        Console.ReadKey();
    }





    private static async Task GetProducts(
    ProductProdtService.ProductProdtServiceClient client)
    {
        Console.WriteLine("Produtos cadastrados:");
        using (var call = client.GetProducts(new()))
        {
            var responseStream = call.ResponseStream;

            CancellationTokenSource cts = new();
            var token = cts.Token;

            while (await responseStream.MoveNext(token))
            {
                var product = responseStream.Current;
                Console.WriteLine(
                    product.Id + " | " +
                    product.Sku + " | " +
                    product.Title + " | " +
                    product.Price + " | " +
                    product.CreateAt + " | " +
                    product.StatusProduct);
            }
        }

        Console.WriteLine();
    }



    private static async Task AddAsync(ProductProdtService.ProductProdtServiceClient client)
    {

        var result = await client.AddAsync(new()
        {
            Product = new()
            {
                Sku = "SKU1",
                Title = "IPhone 14 100Gb",
                Price = 1_000_000,
                StatusProduct = StatusProduct.Actived
            }
        });

        Console.WriteLine($"Product Add response:  { result.Title }");
    }




    // var resultado = await client.InsertBulk();





    //private static async Task InsertBulk(ProductProdtService.ProductProdtServiceClient client)
    //{
    //    var products = new List<ProductModel>
    //                {
    //                    new ProductModel
    //                    {
    //                        Sku = "SKU1",
    //                        Title = "IPhone 14 100Gb",
    //                        Price = 1_000_000,
    //                        StatusProduct =    StatusProduct.Actived,
    //                    },
    //                    new ProductModel
    //                    {
    //                        Sku = "SKU2",
    //                        Title = "IPhone 15 200Gb",
    //                        Price = 2_000_000,
    //                        StatusProduct =    StatusProduct.Actived,
    //                    },
    //                    new ProductModel
    //                    {
    //                        Sku = "SKU3",
    //                        Title = "IPhone 16 300Gb",
    //                        Price = 3_000_000,
    //                        StatusProduct =    StatusProduct.Actived,
    //                    }
    //                };


    //    // var resultado = await client.InsertBulk();




}