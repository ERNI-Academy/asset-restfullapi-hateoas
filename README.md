# About ERNI Api Hypertext as the Engine of Application State (Hateoas)

ERNI Academy Hateoas Api boilerplate to start a Hateoas Api.

<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-2-orange.svg?style=flat-square)](#contributors)
<!-- ALL-CONTRIBUTORS-BADGE:END -->
[![sonarcloud](https://github.com/ERNI-Academy/asset-restfullapi-hateoas/actions/workflows/sonarcloud.yml/badge.svg?branch=main)](https://github.com/ERNI-Academy/asset-restfullapi-hateoas/actions/workflows/sonarcloud.yml)
[![pages-build-deployment](https://github.com/ERNI-Academy/asset-restfullapi-hateoas/actions/workflows/pages/pages-build-deployment/badge.svg?branch=main)](https://github.com/ERNI-Academy/asset-restfullapi-hateoas/actions/workflows/pages/pages-build-deployment)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ERNI-Academy_asset-restfullapi-hateoas&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=ERNI-Academy_asset-restfullapi-hateoas)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=ERNI-Academy_asset-restfullapi-hateoas&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=ERNI-Academy_asset-restfullapi-hateoas)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ERNI-Academy_asset-restfullapi-hateoas&metric=coverage)](https://sonarcloud.io/summary/new_code?id=ERNI-Academy_asset-restfullapi-hateoas)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=ERNI-Academy_asset-restfullapi-hateoas&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=ERNI-Academy_asset-restfullapi-hateoas)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=ERNI-Academy_asset-restfullapi-hateoas&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=ERNI-Academy_asset-restfullapi-hateoas)

## Built With

This boilerplate is using the following technologies:

- [Net 6.0](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6)

## Getting Started

When we implement REST APIs, there are 4 levels of maturity to go from API till Fully Restful API:

- **Level 0: The Swamp of POX**: API designed at this level are not at all Rest APIs and this is where SOAP based web services takes place. 

- **Level 1: Resource Based Address/Uri**: In this Level, the Concept of Resource-based Address is introduced, which tells you there should be Individual URI for each Resource on the server. It's like reducing the burden from the single endpoint (LEVEL 0 end Point which handles all operations) into multiple Resource Based URIs like Divide and Conquer algorithm.

- **Level 2: Utilize Potential of HTTP**: REST developed under this level leverages the full potential of HTTP as an application Layer Protocol. REST API developed at this LEVEL uses Standard HTTP methods/verbs and different HTTP status codes to do different operations on Resource URI. So, the Request Body will no longer carry Operation information at this level. Hence, this API is much more mature than the API developed at LEVEL 0 and LEVEL 1.

- **Level 3> Use Hypermedia (OR HATEOAS)**: This level makes use of Hypermedia (also Called HATEOAS--Hypermedia as Engine of Application state in REST world), which drives the interaction for the API Client.

Typically, when we perform a REST request, we only get the data and not any actions around it. This is where HATEOAS comes in to fill the gap. A HATEOAS request allows you to not only send the data but also specify the related actions. For example:

![Json request](./docs/images/JsonResponse.PNG "Json request").

As it can be appreciated, it contains the **Links** property which represents all the resources available.

This boilerplate will help you to fullfill level 3 REST Api, so your API will be considered a RestFull API. 

## Prerequisites

To run and play with the boilerplates you need to install the following ide:

* Visual Studio 2022

It also uses Docker to deploy the Sample Api:

* [Docker](https://docs.docker.com/desktop/windows/install/)

## Installation

Installation instructions Erni Api Hateoas by running:

1. Clone the repo

   ```sh
   https://github.com/ERNI-Academy/asset-restfullapi-hateoas.git
   ```

2. Start docker

3. Restore packages

4. Build the application

## Project Structure

### The project contains the following projects

* **Erni.Api.Hateoas.Sample**: this project contains a Sample Api that uses the Core functionality.
* **Erni.Api.Hateoas.**: this project contains the main functionalities to get a functional Hateoas Api.

### The folder structure

. \
├── **Erni.Mobile.Hateoas** \
 &nbsp;&emsp;├── 📁 Dto: Contains all data transfer objects \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 Link.cs: Base class for Links generations. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 LinkCollectionWrapper.cs: Wrapper class for Links. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 LinkResourceBase.cs: Base class for LinkCollectionWrapper. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 PagedList.cs: Base class to implement the paged list functionality. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 PaginationFilter.cs: Class for filtering results. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 QueryStringParameters.cs: Class for query string parameters. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 ResponseDto.cs: Dynamic class to generate the Api responses. \
 &nbsp;&emsp;├── 📂 Extensions: Contains all custom extension \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 ServicesExtension.cs: Class that manages dependency injection. \
 &nbsp;&emsp;├── 📂 Formatter: Contains all custom formatters \
 &nbsp;&emsp;│&emsp;&emsp;├── 📄 JsonHateoasFormatter.cs: Class responsible to customize the json output format when Hateoas needs to be implemented. \
 &nbsp;&emsp;│&emsp;&emsp;├── 📄 XmlHateoasFormatter.cs: Class responsible to customize the xml output format when Hateoas needs to be implemented. \
 &nbsp;&emsp;├── 📂 Services: Contains all services that the application uses \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 DataShaper.cs: Shapes the data to fullfill the query filters. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 IDataShaper.cs: Interface for DataShaper class. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 ILinkGenerator.cs: Interface to be implemented for the Links generators. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 ISortHelper.cs: Interface for SortHelper. \
 &nbsp;&emsp;│&emsp;&emsp;└── 📄 SortHelper.cs: Sorts the data to fullfill the query filters.

## How to use it

1. Create your own API project.

2. Add reference to ERNI.Api.Hateoas project.

3. Call the AddHateoas extension method to register all the required services and formatters on your Program.cs.

This will get all required files from your project

```csharp
builder.Services.AddControllers().AddHateoas();
```
In case you have a multiprojects solution and you have the required files out of the main project then:
```csharp
var assemblies = new[]
{
   Assembley1, 
   Assembley2
   ...
};
builder.Services.AddControllers().AddHateoas(assemblies);
```

4. Implement the ILinkGenerator<> interface for all the Dtos that must implement the Links functionality in their responses. See the example attached:

![Link generator request](./docs/images/LinkGenerator.PNG "Link generator sample").

5. Implement the classes inheriting from QueryStringParameters needed for your endpoints and dtos logic. See the example attached:

![Sample query parameters](./docs/images/SampleQueryParameters.PNG "Query parameters sample class").

6. Your controllers endpoints need to receive the this QueryParameters class as they are going to be used automatically on the Formatters.

![Controller sample](./docs/images/ControllerSample.PNG "Controller sample").

7. Formatters will intercept and format the Responses automatically when a request is performed with the Header **Accept** - **application/json+hateoas** or **Accept** - **application/xml+hateoas**.
 
Once your sample app is up and running, an Api is listening on the configured port.
Then, a query can be perfomed. Let's see the following example:

![Sample request](./docs/images/SampleRequest.PNG "Sample request").

The Header **Accept** - **application/json+hateoas** could be also **Accept** - **application/xml+hateoas**.

Then responses would look like:

* For Json

![Json request](./docs/images/JsonResponse.PNG "Json request").

* For Xml

![Xml request](./docs/images/XmlResponse.PNG "Xml request").


## Contributing

Please see our [Contribution Guide](CONTRIBUTING.md) to learn how to contribute.

## License

![MIT](https://img.shields.io/badge/License-MIT-blue.svg)

(LICENSE) © 2022 [ERNI - Swiss Software Engineering](https://www.betterask.erni)

## Code of conduct

Please see our [Code of Conduct](CODE_OF_CONDUCT.md)

## Stats

![https://repobeats.axiom.co/api/embed/92f526f5dc3727059d6341530422d52c504def4d.svg](https://repobeats.axiom.co/api/embed/92f526f5dc3727059d6341530422d52c504def4d.svg)

## Follow us

[![Twitter Follow](https://img.shields.io/twitter/follow/ERNI?style=social)](https://www.twitter.com/ERNI)
[![Twitch Status](https://img.shields.io/twitch/status/erni_academy?label=Twitch%20Erni%20Academy&style=social)](https://www.twitch.tv/erni_academy)
[![YouTube Channel Views](https://img.shields.io/youtube/channel/views/UCkdDcxjml85-Ydn7Dc577WQ?label=Youtube%20Erni%20Academy&style=social)](https://www.youtube.com/channel/UCkdDcxjml85-Ydn7Dc577WQ)
[![Linkedin](https://img.shields.io/badge/linkedin-31k-green?style=social&logo=Linkedin)](https://www.linkedin.com/company/erni)

## Contact

📧 [esp-services@betterask.erni](mailto:esp-services@betterask.erni)

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://github.com/Robertcs8"><img src="https://avatars.githubusercontent.com/u/100421143?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Robertcs8</b></sub></a><br /><a href="https://github.com/ERNI-Academy/asset-restfullapi-hateoas/commits?author=Robertcs8" title="Code">💻</a> <a href="#content-Robertcs8" title="Content">🖋</a> <a href="https://github.com/ERNI-Academy/asset-restfullapi-hateoas/commits?author=Robertcs8" title="Documentation">📖</a> <a href="#design-Robertcs8" title="Design">🎨</a> <a href="#ideas-Robertcs8" title="Ideas, Planning, & Feedback">🤔</a> <a href="#maintenance-Robertcs8" title="Maintenance">🚧</a> <a href="https://github.com/ERNI-Academy/asset-restfullapi-hateoas/commits?author=Robertcs8" title="Tests">⚠️</a> <a href="#example-Robertcs8" title="Examples">💡</a> <a href="https://github.com/ERNI-Academy/asset-restfullapi-hateoas/pulls?q=is%3Apr+reviewed-by%3ARobertcs8" title="Reviewed Pull Requests">👀</a></td>
    <td align="center"><a href="https://github.com/Rabosa616"><img src="https://avatars.githubusercontent.com/u/12774781?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Rabosa616</b></sub></a><br /><a href="https://github.com/ERNI-Academy/asset-restfullapi-hateoas/commits?author=Rabosa616" title="Code">💻</a> <a href="#content-Rabosa616" title="Content">🖋</a> <a href="https://github.com/ERNI-Academy/asset-restfullapi-hateoas/commits?author=Rabosa616" title="Documentation">📖</a> <a href="#design-Rabosa616" title="Design">🎨</a> <a href="#ideas-Rabosa616" title="Ideas, Planning, & Feedback">🤔</a> <a href="#maintenance-Rabosa616" title="Maintenance">🚧</a> <a href="https://github.com/ERNI-Academy/asset-restfullapi-hateoas/commits?author=Rabosa616" title="Tests">⚠️</a> <a href="#example-Rabosa616" title="Examples">💡</a> <a href="https://github.com/ERNI-Academy/asset-restfullapi-hateoas/pulls?q=is%3Apr+reviewed-by%3ARabosa616" title="Reviewed Pull Requests">👀</a></td>
  </tr>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->
This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!
