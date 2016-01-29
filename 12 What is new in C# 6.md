#What is new in C# 6
##Intro
###History
C# 1.0, 2002
C# 2.0, 2005 - generics
C# 3.0, 2007 - `var`, lambda expressions
C# 4.0, 2010 - dynamic
C# 5.0, 2012 - async

###C# 6 features

- auto property initializers
```c#
public Guid Id { get; } = Guid.NewGuid();
```

- primary constructors
```c#
public struct Money(string currency, decimal amount)
{
    public string Currency { get; } = currency;
    public string Amount { get; } = amount;
}
```

- static using (using static type name)
```c#
using System.Console;

class Program
{
    public static void Main()
    {
        WriteLine("Calling method on Console static type");
    }
}
```

- collection initializers on dictionaries
```c#
Dictionary<string, User> _defaultUsers = new Dictionary<string, User>()
    {
        ["admin"] = new User("admin"),
        ["guest"] = new User("guest")
    };
```

- new syntax for numeric literals

`0b` - binary literal
`_` - to break numbers and make them more readable

- declaration expressions
```c#
int result = 0;

foreach(var n in var odd = numbers.Where(n => n % 2 ==1).ToList())
{
    result += n + odd.Count();
}
```

- conditional access operator (dereference valid pointers)
```C#
operation ?.Method ?.Name ?? "no name"
```

- can use await inside of a catch block

###Roslyn

.NET compiler platform

- open source
- compiler as a service

http://roslyn.codeplex.com

###scriptcs
http://scriptcs.net/

scriptcs makes it easy to write and execute C# with a simple text editor.

Roslyn tools

- cssdk
- Roslyn SDK Project templates
- Roslyn Syntax Visualizer

##Language Features
###Property initializers
- set no longer needed when a property is initialized

###Primary constructors
- work for constructors and classes
- compiler uses an initialization scope to capture and assign values
- compiler will provide the body of the primary constructor

###Explicit constructors
- have to call the primary constructor

###Dictionary initializers
effectively new initializers call the indexer as opposed to `Add()` method

extension method `Add` enables collection initializers on dictionary

can use `+=` in object initializer to assign a delegate to an event handler

`params` can be used with `IEnumerable`
```c#
var x = Sum(45, 54, 14);

public int Sum(params IEnumerable<int> numbers)
{
    numbers.Sum(n=>n);
}
```

###Literals and separators

`0b` - for binary

`ox` - for hexadecimal

`_` as separator for long numbers

###Declaration expressions

```c#
int result = 0;

foreach(var n in var odd = numbers.Where(n => n % 2 ==1).ToList())
{
    result += n + odd.Count();
}
```

- more control over the scope of variables

can declare variables directly inline

###static usings
- if type is static

instance methods win

static methods on non-static types can be exposed through static proxy methods defined in a dedicated type

###conditional access
`?.` - only drills down into following property if the instance is not null; does not work with void methods

###await and catch

can await from `catch` block

####exception filter
```c#
catch (Exception ex) if (ex.InnerException == null)
```

###nameof
name of a field, param, method name

```c#
new ArgumentNullException(nameof(parameter1));
```

###expression bodies members
can define expressions as body members with deferred execution

```c#
public class Point(int x, int y)
{
    public int X => x;
    
    public int Y => y;
    
    public double Distance => Math.Sqrt(x*x + y*y);
    
    public Point Move(int dx, int dy) => new Point(x + dx, y + dy);
}
```

##Roslyn
`Microsoft.CodeAnalisys.CSharp` - Roslyn code analysis package

```c#
var code = File.ReadAllText("files.cs");
var tree = SyntaxFactory.ParseSyntaxTree(code);

var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

var reference = new MetadataFileReference(typeof(object).Assembly.Location);

var compilation = CSharpCompilation.Create("test")
                                   .WithOptions(options)
                                   .AddSyntaxTrees(tree)
                                   .AddReferences(reference);
                                   
var diagnostics = compilation.GetDiagnostics();

foreach (var diagnostic in diagnostics)
{
    Console.WriteLine(diagnostic.ToString());
}

using(var stram = new MemoryStream())
{
    compilation.Emit(stream)
    
    var assembly = Assembly.Load(stream.GetBuffer());    
    var type = assembly.GetType("Greeter");
    var greeter = Activator.CreateInstance(type);
    var method = type.GetMethod("Test");
    
    method.Invoke(greeter, null);    
}
```

###Syntax
With Roslyn SDK the Roslyn syntax can be visualized in VS in Roslyn Syntax Visualizer window

###Semantics
```c#
var model = compilation.GetSemanticModel(tree);
var locals = tree.GetRoot()
                 .DescendantNodes()
                 .OfType<LocalDeclarationStatementSyntax>();
                 
foreach (var node in locals)
{
    var type = model.GetTypeInfo(node.Declaration.Type);
    WriteLine("{0}, {1}", type.Type, node.Declaration);
}
```

can analyze the exit points in a method, etc.

###Extensions
####custom diagnostics
build VS extensions

`ISymbolAnalyzer` defines a rule (that is checked on code); it can be linked to a `ICodeFixprovider` through an Id that uses a `CodeAction` to fix the problem. `Renamer` is a utility provided by Roslyn intended to rename and update references accordingly.



 
