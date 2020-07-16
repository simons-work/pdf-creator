# pdf-creator

It's coded in VS 2019 community edition. Published to git hub repo so hopefully can git clone if you prefer although will send ZIP 

Hopefully you can just F5 it, the appSettings.json has the input / output paths and they are relative so hopefully should read/write to the top level console app folder

Notes about the approach:

As I imagine you did not want us to reinvent the wheel with regard to the actual generation of a pdf, I choose one of the existing libraries for this.

I chose https://github.com/vilppu/OpenHtmlToPdf which only has 99 stars but says it's a web kit based Html to PDF converter

This meant the program mainly had to focus on generating html form the mixture of commands and data being supplied to it

I have tried to follow at least the S and the D of SOLID and also implement the Command Design Pattern as seemed to sort of fit the requirements (maybe!)

I have split the classes up so they better follow Single Responsibilty Principle... so a brief tour of them

Project: PdfCreator.Console

Only exists so that different front ends could be used to call the logical layer

Class: Program - only exists as starting point to setup Dependency injection container and call the logical layer

Project PdfCreator.Library

Contains the logical layer as described below

Class: PdfCreator - Responsible for reading/writing the input file / pdf output file to disk and has no other logic except to call other classes as described below

Class CommandManager - Responsible for implementing the 'Host/Controller' behaviour in the Command Design pattern. Basically manages all the Command objects and also the 'Receiver' telling it when to initialise, or return it's output'. Essentially it's just a loop over the supplied lines of data'

Classes: Command(s) implement ICommand - basically one of these for each command in the requirements plus a made up one called ContentCommand to handle a line of data. As name implies they implement
the 'command' of the Command Design pattern... so they know what to do to the 'Receiver' HtmlDocument to implement the behaviour required from each command

Class: HtmlDocument - Responsible for building up an internal htmlDocument string as result of the Commands running. Wasn't sure if worth using 3rd party library like Aspose Html stuff so just went rightly or wrongly with XmlDocument as does seem to output suitable html or at least enough for this application. Appreciate there's difference between Html and XHtml and self closing tags etc'
Also keeps track of a pointer to Html container elements e.g. <p>, <span> as these are used as parent containers for the text content or other elements

Class: HtmlToPdfConverter - Responsible for generating a pdf byte array from a html string

Project: PdfCreator.Tests

I did write the tests for the HtmlToPdfConverter first before I had a console project and these marked as Integration tests if someone wanted to exclude them from running as they are slower

The other class is just the tests for CommandManager as that's hits a lot of the other classes


It does produce the output you asked for but not sure how brittle it is. I think there's small bug in that the paragraph containers get embedded inside each other which doesn't seem to affect the output but is next on my list to fix but I've gone over the 4 hour time limit suggested so '

