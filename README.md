# pdf-creator

*UPDATES 22/07*
---
*I created new feature branch (link [here](https://github.com/simons-work/pdf-creator/tree/improvements)) from pdf-creator master branch yesterday evening as i wanted to make a few improvements nut not intefere with the original version already submitted*
- Swapped XmlDocument for XDocument... apart from being a slightly newer way of doing things, it's saves 1 line of code when initialising and might save about approx 30% memory
- Wanted to move any knowledge of Xml elements out of the Command objects (into HtmlDocument) to improve Single Responsibility principle (have done this for 8 out of 10 commands so far - working on last two)
- Created Abstract base class for Commands to handle the common logic for at least 4 of the commands which is to request HtmlDocument to create a 'container' doc node
- Finally have done some amendments to the text below using striked font to correct some things and added new block of text to the end to clarify my approach
---

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
Responsible for building up an internal htmlDocument string as result of the Commands running. Wasn't sure if worth using 3rd party library like Aspose Html stuff so just went rightly or wrongly with ~~XmlDocument~~ XDocument as does seem to output suitable html or at least enough for this application. Appreciate there's difference between Html and XHtml and self closing tags etc'
Also keeps track of a pointer to Html container elements e.g. Para, Span as these are used as parent containers for the text content or other elements

#### Class: HtmlToPdfConverter #### 
Responsible for generating a pdf byte array from a html string

### Project: PdfCreator.Tests ###

I did write the tests for the HtmlToPdfConverter first before I had a console project and marked as Integration tests if someone wanted to exclude them from running as they are slower

The other class is just the tests for CommandManager as that hits a lot of the other classes

It does produce the output you asked for but not sure how brittle it is as I haven't finished writing all the tests for the edge cases. 
Have done the obvious ones like passing in null's, empty arrays, empty strings, etc 
I've gone over the 4 hour time limit suggested so might submit this as is but ...

### Other failings I can think of: ### 

- Not disposing of the ~~XmlDocument~~XDocument object correctly at moment.

- Have cloned the 2 setup methods from Console App into the one of the Unit tests TestSetup methods which isn't very DRY

- ~~Not sure if making most of the participating objects singletons is what you do with Command design pattern. Normally e.g. in bank account, the Withdrawal command would be instantied with the data it needed. So an instance would be constructed and at some point destroyed~~ Actually having thought about this, in my original version the Command Objects are not strictly singletons, as I registered them as Transient instances with MS DependencyInjection framework. However it's fair to say all 10 of them  are instantiated only once in the constructor of CommandManager by virtue of the collection of them being injected in and then me storing reference to them in the CommandLookup dictionary. Each one refers to the HtmlDocument as a singleton which i think makes sense as they should all be interacting with same instance of the 'receiver' object form the Command pattern. So maybe this is not the failing i thought it was?

- I think the ~~singleton~~ single-transient-instance-per-command approach for all the commands was to leverage functionality to have them all Register themselves automatically when the application starts up so that by time the application starts there is a map of command name -> command instance to execute the behaviour

# Finally some new thoughts about my approach (added 22/07) #

In the original text i submitted, the notes on my approach were mainly focused on what the classes did, etc... I forgot to say what my approach was with regard to the types of commands

If you accept my overal approach of not trying to output a PDF directly from the commands but to have an intermediate step of producing a 'styled' html document, then the problem is reduced to how to emit a suitable html document which implements the current command set.

I realised the commands need to emit html elements, but sometimes the commands are more about the content, sometimes they are about the content and the struture of how say the elements should be nested within each other.

So the 10 commands could be viewed as 6 types of command grouped like this:

| Command(s) | Behaviour(s) of the Command group |
| ---- | ---- |
| Bold, Italic, Large, Para | These commands should emit a 'container' element e.g. Span, H1, Para with optional content |
| Regular, Normal | These commands don't emit new content but rather should close a previously 'opened' container element |
| Content | This was a made up command to handle what to do with a line of input which is not a command, and in this case it should emit a 'text' element
| Fill | This command does something unique in that it should update the current paragraph style to be 'justified'... However if the fill command came after some existing text inside a paragraph, the fill command should probably in my opinion, close the previous paragraph element, and start a new one, and updates it's style |
| NoFill | If you were going from justified text to non justified text, then you really should start a new paragraph block so this command should essentially perform a close container, followed a open new container |
| Indent | You would think this should just apply a style e.g. a margin to the existing paragraph container, however again like NoFill, Fill, imagine you gave command to Indent after already including 1 or more lines of text in paragraph, so i thought it should close existing container element if necessary and start new one | 


