# Using gRPC in Microservices for Building a high-performance Interservice Communication with .Net 6


### Overall Picture
See the overall picture of **implementations on gRPC in Microservices for Building a high-performance Interservice Communication with .Net 6** on real-world **e-commerce microservices** project. You can see that we will have 6 microservices which we are going to develop.
We will use Worker Services and Asp.Net 6 Grpc applications to build client and server gRPC components defining proto service definition contracts.

![Overall Picture of Repository](https://user-images.githubusercontent.com/1147445/98652230-5f66ee80-234c-11eb-9201-8b291b331c9f.png)

Basically we will implement e-commerce logic with only gRPC communication. We will have 3 gRPC server applications which are Product — ShoppingCart and Discount gRPC services. And we will have 2 worker services which are Product and ShoppingCart Worker Service. Worker services will be client and perform operations over the gRPC server applications. And we will secure the gRPC services with standalone Identity Server microservices with OAuth 2.0 and JWT token.

### CatalogGrpc Server Application
First of all, we are going to develop ProductGrpc project. This will be Asp.Net gRPC Server Web Application and expose apis for Product CRUD operations.

### Catalog Worker Service
After that, we are going to develop Product Worker Service project for consuming ProductGrpc services. This product worker service project will be the client of ProductGrpc application and generate products and insert bulk product records into Product database by using client streaming gRPC proto services of ProductGrpc application. This operation will be in a time interval and looping as a service application.

### ShoppingCartGrpc Server Application
After that, we are going to develop ShoppingCartGrpc project. This will be asp.net gRPC server web application and expose apis for SC and SC items operations. The grpc services will be create sc and add or remove item into sc.

### ShoppingCart Worker Service
After that, I implemented a ShoppingCart Worker Service project to consume the ShoppingCartGrpc services. This ShoppingCart worker service project will be the application client of ProductGrpc and ShoppingCartGrpc. This job service will read products from ProductGrpc and create sc and add product items to sc using the proto gRPC services of the ProductGrpc and ShoppingCartGrpc application. This operation will be on a time slot and loop like a service application.

### DiscountGrpc Server Application
When adding product item into SC, it will retrieve the discount value and calculate the final price of product. This communication also will be gRPC call with SCGrpc and DiscountGrpc application.

### Identity Server
Also, I implemented the centralized and standalone Authentication Server with the implementation of the IdentityServer4 package and the name of the microservice is Identity Server.
Identity Server4 is an open source framework that implements the OpenId Connect and OAuth2 protocols for .Net Core.
With IdentityServer, we can secure our SC gRPC services with OAuth 2.0 and JWT tokens. SC Worker will get the token before sending the request to the SC Grpc server application.

## Installation
Follow these steps to get your development environment set up:
1. Check All projects run profiles. One by one Right Click the project file, open Properties window and check the debug section. Launch Profile should be the "Project" and App URLs should be the same as big picture.
2. For all projects, one by one, Set a Startup project and see the Run profile on the Run button. Change the default running profile to IIS Express to Project name.
3. Multiple startup projects. Right click the solution, open Properties, and set Multiple startup project and Start **only gRPC server applications** click apply and ok.
4. Start Worker Service applications one by one. You can go to location of ProductServiceWorker and ShoppingCartServiceWorker project location and run cmd on that location. After that you can write **dotnet run** command and see the logs for all project in console windows.

