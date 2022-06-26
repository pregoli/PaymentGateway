# PaymentGateway

PaymentGateway is a payment API application built on top of [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) letting the consumer to submit payments and then read the executed transactions filtering by `transactionId` or `MerchantId`.

## Project structure

The project structure follows the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) design, separating layers into ring levels as shown below:

<table>
  <tr>
    <td><img src="https://github.com/pregoli/PaymentGateway/blob/master/Docs/CleanArchitecture.png" alt="Clean Architecture" width="280"/></td>
    <td><img src="https://github.com/pregoli/PaymentGateway/blob/master/Docs/SolutionStructure.png" alt="Clean Architecture"/></td>
 </tr>
</table>

## Implementation

The PaymentGateway has been implemented following the [CQRS](https://martinfowler.com/bliki/CQRS.html) pattern:

![PaymentGateway Api_Flow](https://github.com/pregoli/PaymentGateway/blob/master/Docs/appdiagram.png)

The main benefit is in handling high performance applications. The above design allows you to separate the load from reads and writes allowing you to deploy and scale each application independently based on their needs.


## Pre-requirements

In order to build and run the application the following are some pre-requirements:

- [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed in the hosting machine in order to be able to build and run tyhe application from your favourite IDE.
- [Docker](https://docs.docker.com/get-docker/) installed in order to be able to run the application from container.

## Run the Api application

Both you run from IDE or from Docker you will be able to play with the application endpoints by using the Swagger UI as below
![Run from Rider](https://github.com/pregoli/PaymentGateway/blob/master/Docs/Swagger.png)

1. ### Run from IDE

In order to run the application from your favourite IDE, the [Checkout.Api](https://github.com/pregoli/PaymentGateway/tree/master/App/Checkout.Api) needs to be set as default starting project, then `F5` or `click start from UI` as shown below for Visual Studio od Rider:

  - Rider: 
 
  ![Run from Visual Studio](https://github.com/pregoli/PaymentGateway/blob/master/Docs/RunFromVS.png)
  
  - Visual Studio: 
  
  ![Run from Rider](https://github.com/pregoli/PaymentGateway/blob/master/Docs/RunFromRider.png)
  
 The application should startup at page `https://localhost:5001/swagger/index.html`
 
 
2. ### Run from Docker

In order to run the application from Docker, these are the steps to follow:
- Move to the main root where is placed the `Solution` and the `Dockerfile`.
- Run the command:
 
 ```console
docker build -t checkout-api -f Dockerfile .
```
It should take some seconds to pull all the dependencies and then:

 ```console
docker run -ti --rm -p 8080:80 checkout-api
```

The above command will run your application in interactive mode, mapping it to port 8080 so you should be able to navigate to the swagger page at `http://localhost:8080/swagger/index.html` and start playing with the endpoints.


Every swagger endpoints will present a pre-binded request sample you can use to play straight away with the api.

## Metrics

The application metrics are being recorded during the Api utilization by [Prometheus](https://github.com/prometheus-net/prometheus-net) and available at the endpoint `/api/beta/metrics` as shown below:

 ![Run from Rider](https://github.com/pregoli/PaymentGateway/blob/master/Docs/SwaggerMetricsResponse.png)

## Project dependencies
