## What are Microservices?
### Microservices are the architectural approach to building applications from small to large-scale applications.
### With this architectural approach, an application is broken down into the smallest components, independent of each other.

## Advantages of Microservices
### Microservices give development teams and testers a faster approach through distributed development.
### It provides the ability to develop multiple Microservices simultaneously.
### This means more developers working on the same app but different functional modules simultaneously,
### which results in less deliverable time of application to the client.

##  API Gateway (Ocelot Gateway)
### API Gateway is a middleware layer that directs incoming HTTP request calls from Client applications to specific Microservice
### without directly exposing the Microservice details to the Client and returning the responses generated from the respective Microservice.
## <img width="601" alt="Microservices" src="https://github.com/AhmedSaberKhalil/MicrocervicesUsingOcelot-RabbitMq/assets/89740052/44c5b5ea-5e77-4a43-abdf-90fd47a9fe1b">

## What is RabbitMQ?
### RabbitMQ is one of the most popular Message-Broker Service. It supports various messaging protocols. It basically gives your applications a common platform for sending and receiving messages. This ensures that your messages (data) is never lost and is successfully received by each intended consumer. RabbitMQ makes the entire process seemless.
## What is MassTransit?
### MassTransit is a .NET Friendly abstraction over message-broker technologies like RabbitMQ. It makes it easier to work with RabbitMQ by providing a lots of dev-friendly configurations. It essentially helps developers to route messages over Messaging Service Buses, with native support for RabbitMQ.
### MassTransit does not have a specific implementation. It basically works like an interface, an abstraction over the whole message bus concept. Remember how Entity Framework Core provides an abstraction over the data access layers? Similary MassTransit supports all the Major Messaging Bus Implementations like RabbitMQ, Kafka and more.

