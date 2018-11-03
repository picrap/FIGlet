```
 _______ _______ _______ __         __   
|    ___|_     _|     __|  |.-----.|  |_ 
|    ___|_|   |_|    |  |  ||  -__||   _|
|___|   |_______|_______|__||_____||____|
```                                         

A FIGlet library (which should kick asses, or not)

# This is work in progress!

It does not work. Wait a bit until it's finished.  
Here are the things to do before we have something to release:  
- [x] Loading font
- [x] Full size rendering
- [x] Fitting rendering
- [x] Smushing rendering
- [x] Rendering on buffer
- [ ] Meta character rendering (such as marking with colors)
- [x] CI
- [x] Nuget package
- [x] Zip support
- [ ] Multi-line

# Basic use

The following is a very simple example:

```csharp
// loads a font
var font = FIGfont.FromEmbeddedResource("small.flf", typeof(FontsRoot));
// creates the FIGdriver, which renders the text
var figDriver = new FIGdriver { Font = font };
// add some text
figDriver.Write("Hi there!");
figDriver.Write("Great, isn't it?");
// and get the text
var text = figDriver.ToString();
```
