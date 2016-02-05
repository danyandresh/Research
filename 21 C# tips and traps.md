#C# Tips and traps
###Debugger display values
```c#
[DebuggerDisplay("Class has members {Member1} and {Member2}")]
```
- classes
- structs
- delegates
- enums
- fields
- properties
- assemblies

```c#
[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
```

###Caller information attributes
- `CallerMemberName` - useful with property changed
- `CallerFilePath`
- `CallerLineNumber`

###Partial declaration
used to split declaration of a class across multiple files

methods must be `void`
```c#
partial void APartialMethod();
```

###Compiler errors and warnings
```c#
# warning This is a compilation warning
# error This is a compliation error message
```

###Unicode category
```c#
char.GetUnicodeCategory(character);
```

###Random
`System.Security.Cryptography.RandomNumberGenerator`

```c#
var r = System.Security.Cryptography.RandomNumberGenerator.Create();

var rb = new byte[4];

r.GetBytes(rb);

int rndInt = BitConverter.ToInt32(rb, 0);
```

c# compiler optimization would assign the same reference for two identical strings (give they are immutable)

###Enums
`Flags`

###Conditional formatting for positive negative and zero numbers
```c#
var threePartFormat = "(pos)#.##;(neg)#.##;(value is zero)";
threePartFormat = "(+)#.##;(-)#.##;(value is zero)";
```

###Arrays
`Array.ConstrainedCopy` - atomically copies the array, reverts the destination array to the default state if any of the elements fails to copy

###Exception callstack
is preserved when `throw;` but not when `throw ex;`

###Numbers
```c#
int.Parse("(24)", NumberStyles.AllowParentheses); // returns -24
```

###DateTime
`'o'` to convert to and from **ISO8601**

###BitConverter
`.ToSingle`

###Optional parameters
are compiled into the calling site with the default value, so updating the defining library has no effect

###Library reference
can define namespace alias to avoid conflicts (in the properties window) then use `external alias` to import namespace

###Condition executing method
mark them with `Conditional` attribute; it is used at compile time

###Path
`Combine` method requires the drive path to end with a railing backslash; 

strings starting with a backslash are treated as absolute paths

`..` refers to the parent directory

`GetFullPath` would navigate to the parent

`ChangeExtension`

`GetDirectoryName`

`GetExtension`

`GetFileName`

`GetFilenameWithoutExtension`

`HasExtension`

`GetInvalidFileNameChars`

`GetRandomFileName`

`GetTempFileName`

`GetTempPath`

`DirectorySeparatorChar`

###Formatting
`{0, -20}` second parameter specifies the minimum width to apply; negative = left alignment, positive = right alignment

###Environment info
`Environment`:
    - `GetEnvironmentVariables()`
    - `Is64BitOperatingSystem`
    - `Is64BitProcess`
    - `ProcessorCount`
    - `SystemPageSize`
    - `Version`
    - `NewLine`
    - `SpecialFolder`
    - `CurrentDirectory`
    - `GetLogicalDrives()`
    - `SystemDirectory`
    
###Multicast delegates
`delegate void Name(params)`

non void delegates only return the result of the last delegate being invoked

can use `+=` and `-=` to add or remove from multicast delegates

delegates are not structurally compatible

###Using reserved keywords as class names and members
```c#
var a = new @abstract();
a.@foreach = 1;
```

###Static constructor exceptions
make the type unusable for the remainder of the program

###Structs equality performance
structs with value members only are a lot faster to check equality than structs containing reference members (due to reflection being used to check equality of the structs)

can fix performance by overriding `Equals` and `GetHashCode`

###Short circuiting conditional operators
use `&&` and the second operand won't execute if the first is false

use `&` and both will execute both operands regardless

###Sequence aggregation
starts from second element using the first as seed, when no aggregation seed is used

###Task delaying
`Task.Delay(time).GetAwaiter().OnCompleted(()=>{});`

###Change the display in the debugger window
use `DebuggerTypeProxy`

###File zipping
`ZipFile`, `ZipArchive`, `DefalteStream`, `GZipStream`

###Caracters conversion to Numeric values
can convert unicode character like `⅔` to numeric (double) using `char.GetNumericValue`

###Lazy objects
are chaching the exception if the constructor that uses lambda expression is used

###DebuggerStepThrough

can be applied to property getters or setters individually

can be controlled from VS setting: "Enable just my code" (it will step through the method regardless method is marked as "debugger step through" or not when disabled)





