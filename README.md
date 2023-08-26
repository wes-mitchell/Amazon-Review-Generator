
# Amazon Review Generator

Welcome to the Amazon Review Generator API. This API has one endpoint that when called returns a random review json object with the properties of _starRating_ and _reviewText_. The reviews have a star rating between 1 and 5 and are are built using a Markov text chain string generator. This can be briefly described as a computer program that uses patterns in language to create new pieces of text that sound like something it has seen / learned before. You can get a more in depth analysis on the Markov Chain Process at the wikipedia page here: [Markov Chain Wiki.](https://en.wikipedia.org/wiki/Markov_chain)

## Running The API

If you would like to locally run the API and see what kind of reviews the API is returning you may follow these instructions. 

1. Clone the repository onto your local machine using the following command ```git@github.com:wes-mitchell/Loyal-Health-Assessment.git```
2. Once the repository has been cloned you may open the solution from the file explorer or from Visual Studio. 
3. After you have opened the solution in Visual Studio ensure your startup project is set correctly to _LoyalHealthAPI_. If it is not currently set, you may do so by right clicking the project and selecting "Set as Startup Project" from the menu. 
4. You now may launch / start the project in either the "Dev" or "Local" environment. Each environment has it's own unique set of training data. If you'd like to reference the files used for training the markov chain generorator see the _ReviewDataFileName_ value in the corresponding _appsettings.Development.json_ and _appsettings.Local.json_ files.
5. Once you have selected your preferred environment, start the program and a Swagger web browser will open. From here, click the blue "GET" button and select "Try it out" to the right.
6. Click on Execute and reference the response body to see the random Review json object returned from the API.

## References
This API was built with .NET Core 5 and uses the Newtonsoft.Json nuget package to aid in deseralizing the zipped gz file. All unit tests in the **LoyalHealthAPI.Tests** project were written using both _xunit_ and _xunit.runner.visualstudio_ nuget packages. 
