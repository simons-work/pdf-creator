# pdf-creator

It's coded in VS 2019 community edition. Published to git hub repo so hopefully can git clone if you prefer although will send ZIP 

Hopefully you can just F5 it, the appSettings.json has the input / output paths and they are relative so hopefully should read/write to the top level console app folder

## Notes about the approach: ##

As I imagine you did not want us to reinvent the wheel with regard to the actual generation of a pdf, I choose one of the existing libraries for this.

I chose https://github.com/vilppu/OpenHtmlToPdf which only has 99 stars but says it's a web kit based Html to PDF converter

This meant the program mainly had to focus on generating html form the mixture of commands and data being supplied to it

I have tried to follow at least the S and the D of SOLID and also implement the Command Design Pattern as seemed to sort of fit the requirements (maybe!)

I have split the classes up so they better follow Single Responsibilty Principle... so a brief tour of them

### Project: PdfCreator.Console ###
Only exists so that different front ends could be used to call the logical layer

#### Class: Program ####
The starting point and also sets up  Dependency injection container and call the logical layer

### Project PdfCreator.Library ###
Contains the logical layer as described below

#### Class: PdfCreator #### 
Responsible for reading/writing the input file / pdf output file to disk and has no other logic except to call other classes as described below

#### Class CommandManager ####
Responsible for implementing the 'Host/Controller' behaviour in the Command Design pattern. Basically manages all the Command objects and also the 'Receiver' object (HtmlDocument) telling it when to initialise, or return it's output'. Essentially it's just a loop over the supplied lines of data'

#### Classes: Command(s) implement ICommand #### 
Basically one of these for each command in the requirements plus a made up one called ContentCommand to handle a line of data. As name implies they implement
the 'command' of the Command Design pattern... so they know what to do to the 'Receiver' HtmlDocument to implement the behaviour required from each command

#### Class: HtmlDocument #### 
Responsible for building up an internal htmlDocument string as result of the Commands running. Wasn't sure if worth using 3rd party library like Aspose Html stuff so just went rightly or wrongly with XmlDocument as does seem to output suitable html or at least enough for this application. Appreciate there's difference between Html and XHtml and self closing tags etc'
Also keeps track of a pointer to Html container elements e.g. Para, Span as these are used as parent containers for the text content or other elements

#### Class: HtmlToPdfConverter #### 
Responsible for generating a pdf byte array from a html string

### Project: PdfCreator.Tests ###

I did write the tests for the HtmlToPdfConverter first before I had a console project and marked as Integration tests if someone wanted to exclude them from running as they are slower

The other class is just the tests for CommandManager as that hits a lot of the other classes

It does produce the output you asked for but not sure how brittle it is as I haven't finished writing all the tests for the edge cases. 
Have done the obvious ones like passing in null's, empty arrays, empty strings, etc 
I've gone over the 4 hour time limit suggested so might submit this as is but ...

Other failings I can think of:

Not disposing of the XmlDocument object correctly at moment.

Have cloned the 2 setup methods from Console App into the one of the Unit tests TestSetup methods which isn't very DRY

Not sure if making most of the participating objects singletons is what you do with Command design pattern. Normally e.g. in bank account, the Withdrawal command would be instantied with the data it needed. So an instance would be constructed and at some point destroyed

I think the singleton approach for all the commands was to leverage functionality to have them all Register themselves automatically when the application starts up so that by time the application starts there is a map of command name -> command instance to execute the behaviour