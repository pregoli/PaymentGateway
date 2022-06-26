# PaymentGateway

PaymentGateway is a payment API application built on top of [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) letting the consumer able to submit payments and then read the transactions status through different endpoints, one by applying the `transactionId` filter and another by applying the `MerchantId` filter.

## Project structure

The project structure follows the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) design, separating layers into ring levels as shown below:

<table>
  <tr>
    <td><img src="https://github.com/pregoli/PaymentGateway/blob/master/Docs/CleanArchitecture.png" alt="Clean Architecture" width="280"/></td>
    <td><img src="https://github.com/pregoli/PaymentGateway/blob/master/Docs/SolutionStructure.png" alt="Clean Architecture"/></td>
 </tr>
</table>

  - `Domain`: It is the center of the application which is not referencing any other project.
  - `Command.Application`: It's responsible to submit the payment through commands & events into the database. It is also exposing interfaces implemented by the infrastructure. It's referencing the Domain layer and referenced by the Api & Infrastructure layers.
  - `Query.Application`: It's responsible to query the transactions from database through the query handler. It is also exposing interfaces implemented by the infrastructure. It's referencing the Domain layer and referenced by the Api & Infrastructure layers.
  - `Infrastructure`: It's exposing concrete infrastructure implementations as write & read repositories, telemetries...
  - `Api`: It's exposing the Payments and Transactions Api, the first for write operations and the second for the read ones.

## Implementation

The PaymentGateway has been implemented following the [CQRS](https://martinfowler.com/bliki/CQRS.html) pattern:

![PaymentGateway Api_Flow](https://github.com/pregoli/PaymentGateway/blob/master/Docs/appdiagram.png)

The main benefit is in handling high performance applications. The above design allows you to separate the load from reads and writes allowing you to deploy and scale each application independently based on their needs.


## Pre-requirements

In order to build and run the application the following are some pre-requirements:

- [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed in the hosting machine in order to be able to build and run the application from your favourite IDE.
- [Docker](https://docs.docker.com/get-docker/) installed in order to be able to run the application from container.

## Run the Api application

Both you run from IDE or from Docker you will be able to play with the application endpoints by using the Swagger UI as below
![Run from Rider](https://github.com/pregoli/PaymentGateway/blob/master/Docs/Swagger.png)

1. ### Run from IDE

In order to run the application from your favourite IDE, the [Checkout.Api](https://github.com/pregoli/PaymentGateway/tree/master/App/Checkout.Api) needs to be set as default starting project, then `F5` or `click start from UI` as shown below for Visual Studio or Rider:

  - Rider: 
 
  ![Run from Visual Studio](https://github.com/pregoli/PaymentGateway/blob/master/Docs/RunFromVS.png)
  
  - Visual Studio: 
  
  ![Run from Rider](https://github.com/pregoli/PaymentGateway/blob/master/Docs/RunFromRider.png)
  
 The application should launch the browser at page `https://localhost:5001/swagger/index.html`
 
 
2. ### Run from Docker

In order to run the application from Docker, these are the steps to follow:
- Move to the main root where the `Solution` and the `Dockerfile` are placed.
- Run the command:
 
 ```console
docker build -t checkout-api -f Dockerfile .
```
It should take some seconds to pull all the dependencies and then:

 ```console
docker run -ti --rm -p 8080:80 checkout-api
```

The above command will run your application in interactive mode, mapping it to port 8080 so you should be able to navigate to the swagger page at `http://localhost:8080/swagger/index.html` and start playing with the endpoints.


Each endpoint exposes its request/response schema.

## Playground

1. Submit one or more payments through the `/api/beta/transactions` endpoint.
2. Use the transactionId from previous response to invoke the `/api/beta/transactions/{id}/` and fetch the transaction detail.
3. Use the merchantId from the previous response to invoke the `/api/beta/merchants/{id}/transactions` and fetch all the transactions against the given merchant.
4. After playng around with above endpoints, you can try out and have a look at metrics recorder by `Prometheus` at the endpoint `/api/beta/metrics`

## Metrics

The application metrics have being recorded during the Api utilization by [Prometheus](https://github.com/prometheus-net/prometheus-net) and available at the endpoint `/api/beta/metrics` as shown below:

 ![Run from Rider](https://github.com/pregoli/PaymentGateway/blob/master/Docs/SwaggerMetricsResponse.png)

## Bank Authorization Provider

The fake validation has been implemented by running checks against the submitted amounts as the following table. Inspiration taken from [Checkout documentation - Soft decline](https://www.checkout.com/docs/resources/codes/response-codes#Soft_decline_(20X))

<table>
  <tr>
    <td><b>Amount ends with</b></td>
    <td><b>Code</b></td>
    <td><b>Description</b></td>
 </tr>
  <tr>
    <td>05</td>
    <td>20005</td>
    <td>Declined - Do not honour</td>
 </tr>
  <tr>
    <td>12</td>
    <td>20012</td>
    <td>Invalid transaction</td>
 </tr>
  <tr>
    <td>14</td>
    <td>20014</td>
    <td>Invalid card number</td>
 </tr>
  <tr>
    <td>51</td>
    <td>20051</td>
    <td>Insufficient funds</td>
 </tr>
  <tr>
    <td>54</td>
    <td>20087</td>
    <td>Bad track data</td>
 </tr>
  <tr>
    <td>62</td>
    <td>20062</td>
    <td>Restricted card</td>
 </tr>
  <tr>
    <td>63</td>
    <td>20063</td>
    <td>Security violation</td>
 </tr>
  <tr>
    <td>9998</td>
    <td>20068</td>
    <td>Response received too late / timeout</td>
 </tr>
  <tr>
    <td>150</td>
    <td>20150</td>
    <td>Card not 3D Secure enabled</td>
 </tr>
  <tr>
    <td>6900</td>
    <td>20150</td>
    <td>Unable to specify if card is 3D Secure enabled</td>
 </tr>
  <tr>
    <td>5000</td>
    <td>20153</td>
    <td>3D Secure system malfunction</td>
 </tr>
  <tr>
    <td>5029</td>
    <td>20153</td>
    <td>3D Secure system malfunction</td>
 </tr>
  <tr>
    <td>6510</td>
    <td>20154</td>
    <td>3D Secure Authentication Required</td>
 </tr>
  <tr>
    <td>6520</td>
    <td>20154</td>
    <td>3D Secure Authentication Required</td>
 </tr>
  <tr>
    <td>6530</td>
    <td>20154</td>
    <td>3D Secure Authentication Required</td>
 </tr>
  <tr>
    <td>6540</td>
    <td>20154</td>
    <td>3D Secure Authentication Required</td>
 </tr>
  <tr>
    <td>33</td>
    <td>30033</td>
    <td>Expired card - Pick up</td>
 </tr>
  <tr>
    <td>4001</td>
    <td>40101</td>
    <td>Payment blocked due to risk</td>
 </tr>
  <tr>
    <td>4008</td>
    <td>40108</td>
    <td>Gateway Reject – Post code failed</td>
 </tr>
  <tr>
    <td>2011</td>
    <td>200R1</td>
    <td>Issuer initiated a stop payment (revocation order) for this authorization</td>
 </tr>
  <tr>
    <td>2013</td>
    <td>200R3</td>
    <td>Issuer initiated a stop payment (revocation order) for all payments</td>
 </tr>
</table>

## Bonus points
- Metrics recording through `Prometheus'.
- Dockerization


## Improvements
- Add and manage Idempotency-key on payments post requests
- Using 2 storages or collection/tables, one for the append-only domain events and one for the current state of the domain or aggregate root
