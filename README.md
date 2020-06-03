# MoneyMaker
**What is Money Maker?**
----------------

Money Maker is an application supporting* the investor in making investment decisions. The long-term and the speculative ones on the forex market. \
It is divided into several projects. In addition to the standard **Domain-driven Design** infrastructure \
[**API**](https://github.com/Filip555/Money-Maker/tree/master/src/Core/Api), [**Domain**](https://github.com/Filip555/Money-Maker/tree/master/src/Core/Domain), [**Infrastructure**](https://github.com/Filip555/Money-Maker/tree/master/src/Core/Infrastructure), you will also find here \
[**BackgroundTask**](https://github.com/Filip555/Money-Maker/tree/master/src/Core/BackgroundTasks) - which is a hosted service, operating in real time. Playing on the forex market using various strategies. \
[**NeuralNetwork**](https://github.com/Filip555/Money-Maker/tree/master/src/Core/NeuralNetwork) - prototype project to support decisions using RNN neural networks. \
Using Domain-driven Design architecture written in .NET Core 3.1, CQRS pattern along with logging in to Elasticsearch and Docker.

*The public version does not have repositories implemented*.

**But the main reason for creating the application was self-education.*

**Technological stack**
----------------

![MoneyMaker](https://github.com/Filip555/Money-Maker/blob/master/assets/stackTech.png)

**How to start the application?**
----------------
Prerequisites - Docker - Docker composer
The application runs through a docker compose with all dependencies and networks. At the /src path, enter *docker-compose up --build*. 

*I currently do not share repository files, so please implement your own sources.*