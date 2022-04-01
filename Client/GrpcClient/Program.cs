using Google.Protobuf.WellKnownTypes;
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
            await AddAsync(client, new()
            {
                Sku = "SKU1",
                Title = "IPhone 14 100Gb",
                Price = 1_000_000,
                StatusProduct = StatusProduct.Actived,
                CreateAt = Timestamp.FromDateTime(DateTime.UtcNow),
            });

            await AddAsync(client, new()
            {
                Sku = "SKU2",
                Title = "IPhone 15 200Gb",
                Price = 2_000_000,
                StatusProduct = StatusProduct.Actived,
                CreateAt = Timestamp.FromDateTime(DateTime.UtcNow),
            });

            await AddAsync(client, new()
            {
                Sku = "SKU3",
                Title = "IPhone 16 300Gb",
                Price = 3_000_000,
                StatusProduct = StatusProduct.Inatived,
                CreateAt = Timestamp.FromDateTime(DateTime.UtcNow),
            });


            Console.WriteLine("Obtendo Produtos via Grpc...");
            await GetProducts(client);

            Console.WriteLine("Atualizando Produtos via Grpc...");
            await UpdateAsync(client, new()
            {
                Id = 1,
                Sku = "SKU3_POCO",
                Title = "Xiaomi POCO 600Gb",
                Price = new Random().Next(5000, 6000),
                StatusProduct = StatusProduct.Actived,
            });

            await GetProducts(client);

            Console.WriteLine("Inserindo Lote de Produtos via Grpc...");
            await InsertBulk(client).ConfigureAwait(true);



            Console.WriteLine("Obtendo Produtos via Grpc...");
            await GetProducts(client);


        }
        catch (RpcException ex)
        {

            Console.WriteLine(ex.Message);
        }

        Console.ReadKey();
    }





    private static async Task GetProducts(ProductProdtService.ProductProdtServiceClient client)
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



    private static async Task AddAsync(ProductProdtService.ProductProdtServiceClient client, ProductModel product)
    {
        var result = await client.AddAsync(new()
        {
            Product = new()
            {
                Sku = product.Sku,
                Title = product.Title,
                Price = product.Price,
                StatusProduct = product.StatusProduct,
                CreateAt = product.CreateAt,
            }
        });

        Console.WriteLine($"Product Add response:  { result.Title } => { result.Id.ToString()}");
    }


    private static async Task UpdateAsync(ProductProdtService.ProductProdtServiceClient client, ProductModel product)
    {
        Console.WriteLine($"Alterando o Produto {product.Title}");

        var resultado = await client.UpdateAsync(new UpdateRequest
        {
            Product = new ProductModel
            {
                Id = product.Id,
                Sku = product.Sku,
                Title = product.Title,
                Price = product.Price,
                StatusProduct = product.StatusProduct
            }
        });

    }





    private static async Task InsertBulk(ProductProdtService.ProductProdtServiceClient client)
    {

        using var clientBulk = client.InsertBulk();

        for (int i = 4; i < 15; i++)
        {
            var new_product = new ProductModel
            {
                Id= i,
                Sku = $"SKU_{i}",
                Title = $"Product {i} ",
                Price = 1_000_000,
                StatusProduct = StatusProduct.Actived,
            };

            await clientBulk.RequestStream.WriteAsync(new_product);
        }
        await clientBulk.RequestStream.CompleteAsync();

        var response =  clientBulk;

       //Console.WriteLine($"status: {response.ResponseStream.Current.Success} - Insert count {response.ResponseStream.Current.InsertCount}");


    }
}