Code created using Visual Studio 2017 Pro.

There are two solution files:
CroweCodingTest.Web.sln
CroweCodingTest.API.sln

To run the unit tests, use the CroweCodingTest.API solution. 
To run the web, api and console apps use the CroweCodingTest.Web solution.

Notes: 
When the web app starts up, it makes an API call to CroweCodingTest.API and will (should) display "Hello World".
There is also link on the main web page to start the console application. "Run Console Application: Run Console".
When this link is clicked, it should load the console app which will make an API call to display "Hello World" as well.
Other considerations: 
I have an attached .mdf database which allows for displaying a "Get(1)" with parameters. 
If you're having trouble running the code, it could be the port assigned to you Visual Studio instance. The API app uses 57748 and the Web app uses 57727. 
The unit test app.config setting "<add key="webApiClientUri" value="http://localhost:56627/" />" may need to be modified. 


