```
  ___ ___ ___ _     _   
 | __|_ _/ __| |___| |_ 
 | _| | | (_ | / -_)  _|
 |_| |___\___|_\___|\__|
 ```                                         

A simple to use (but extensible) FIGlet generator library for .NET framework.  
Available as a [![NuGet Status](http://img.shields.io/nuget/v/FIGlet-_-lib.svg?style=flat-square)](https://www.nuget.org/packages/FIGlet-_-lib) package.

# Features

If you want to know about FIGlet, you can visit the official web site at http://www.figlet.org.  
The library supports:
* Loading fonts from text or zip files
* Full size, fitting or smushing as layout modes
* Working canvas items are extensible to hold your own metadata (such as color)
* Has two integrated fonts ('big' and 'small'), for very lazy people.

# Use

The following is a very simple example:

```csharp
// loads a font...
var font = FIGfont.FromEmbeddedResource("small.flf", typeof(FontsRoot));
// ... or use a default font
var font2 = FIGdriver.DefaultFont;
// creates the FIGdriver, which renders the text
var figDriver = new FIGdriver { Font = font /* or font2 */ };
// add some text
figDriver.Write("Hi there!");
figDriver.Write("Great, isn't it?");
// and get the text
var text = figDriver.ToString();
```

# Want more?

[Ask](https://github.com/picrap/FIGlet/issues) or even better, contribute.
