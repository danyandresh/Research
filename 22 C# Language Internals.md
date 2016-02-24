#C# Language Internals
```cmd
> where csc.exe
```

Use ILDASM/ILASM and ILSpy for IL roundtripping

###windbg
Debugging Tools for windows include Windbg.exe and cdb.exe for debugging (the former is using a UI, the latter is console based)

Debugger extensions:
- SOS (son of strike)
- PSSCOR4 (Product support services)
- SOSEX
- CLRMD (can write queries over a different data structures)

`sxe ld clrjit` - set an exception enabled on loading a module with the name _clrjit_ (debugger will be notified when this module gets loaded)

`g` - to continue

`.loadby sos clr` - load the SOS debugger extension from the location where it finds clr.dll (it starts with . because it is a debugger instruction that controls the debugger engine)

`!help` - help about SOS debugger extension

`!procinfo` - dump info about the process (should be issued twice due to implementation of Windbg and SOS)

`.bpmd <executable> <method>` - enable breakpoint in metadescriptor

`g`

`k` - show the callstack

`!clrstack` - managed code stack (might need to run twice)

###IL
(Intermediate language) is the Virtual Machine language of the CLR.

emitted by managed language compilers (C#, VB, F#, etc.)

NEGEN or JIT compiled into native code

stack-based evaluation (no registers) (two pops and a push - pop operands and push the result)

type and memory safe

`pop` from the top of the stack; often used to discard stuff to rebalance the stack

`dup` duplicates the object on the top of the stack; used to eliminate loads and stores to locals (to avoid having to write to locals)

`nop` on non-optimized builds nops are emitted for curly braces lines (so breakpoints can be set there)

loading constants:
    1. Numerical
        - `ldc.i4` - load constant integer 4 bytes (int)
        - `ldc.r8` - load constant floating point 8 bytes (double)
        - value as a operand in the IL instruction stream
        - shorthand instructions, e.g. ldc.i4.1 for Int32 `1`, ldc.i4.m1 for Int32 `-1`
    2. Boolean
        - represented as `0` or `1`
    3. String
        - `ldstr` for load string
        - value as an operand that points to a string table entry
    4. Null
        - `ldnull`
        - null reference, used for initialization or cleanup of locations
        
arithmetic, relational, logical, conversions:
    1. Arithmetic instructions:
        - `neg`, `add`, `sub`, `mul`, `div`, `rem`, `shl`, `shr`
        - overflow variants with .ovf suffix (cf. checked in C#)
    2. Relational instructions
        - `clt`, `cle`, `cgt`, `cge`, `ceq`, `cne`
        - Push zero or one based on outcome of comparison
    3. Bitwise logical instructions
        - `and`, `or`, `xor`, `not`
        - Short-circuiting behaviour requires control flow
    4. Conversions
        - `conv.<type>`, e.g. `conv.i4` to convert to 4-byte integer
        - `isinst` for type checks
        - `castclass` for casts to a specified type
        
branch instructions
    1. unconditional branch instructions
        - `br` - method-local branch using the specified offset
    2. conditional branch instructions
        - `brtrue`, `brfalse` - check top of the stack for true and false
        - `beq`, `bne`, `blt`, `ble`, `bgt`, `bge` - relational operators with branching
    3. variants
        - `.s` suffix - short branch if offset fits in 1 byte
        - `.un` - unsigned variants
    4. switch tables
        - `switch` - jumps based on integer operand

call stacks
    1. different call instructions
        - `call` regular "direct" call
        - `callvirt` call with virtual dispatch
        - `calli` indirect call through a pointer (interop)
    2. arguments
        - push arguments to make a method call
            * arguments passed left -to-right on the stack
            * instance methods hold "this" in the 0th argument
        - call stack frames hold locals and arguments
        - `ldarg` to load an argument
    3. return to caller using `ret`
        - stack should only contain one object (or none if void)
    4. exceptions unwind the call stack
calls
    1. static methods use `call`
    2. instance methods use `callvirt`
        - even for non-virtuals
        - performs null check for v-table
special calls
    1. native implementation provided by the CLR
    2. `[MethodImpl(MethodImplOptions.InternalCall)]`
        - often used for reflection-related stuff
        - e.g. `System.Object::GetType`
    3. `[DllImport("QCall")]`
        - quick calls
            * from the mscorlib.dll assembly
            * to native helper methods in mscorwks.dll or clr.dll
        - e.g. `System.GC::_Collect`
        
throwing exception
    1. `throw`
        - used for `throw ex;` in C#
        - resets the stack trace
            * `ExceptionDispatchInfo` in .net 4.5 can rethrow using the original callstack https://msdn.microsoft.com/en-us/magazine/mt620018.aspx
    2. `rethrow`
        - used for `throw;` in C#
        
exception handling
    1. metadata describes protected regions, i.e. `try`
        - `catch` - type-based exception handling
        - `finally` - cleanup upon successful or exceptional exit
        - `fault` - not available in C#
        - `filter` - conditional exception handling available in Visual Basic
    2. control flow in and out of a potected region
        - `leave` instruction to exit a protected region
            * causes handlers to run
        - `endfinally` and `endfault` to exit handlers
working with objects
    1. `newobj` - creates a new object on the GC heap
        - causes memory allocation and can throw `OutOfMemoryException`
        - runs specified constructor on object with zeroed memory
        - returns reference to newly created object
    2. `ldfld` and `stfld`
        - loads and stores from/to fields
        - parameterized by metadata token of the fields
arrays (one-dimensional - known as vectors)
    1. `newarr` - creates a new array of the specified length
    2. `ldlen` - loads the array length
    3. `ldelem` and `stelem` - loads and stores array elements using an index

compilation model
    1. front-end
        * managed language compilers, C#, Visual Basic, F#
        * emit IL code
    2. back-end
        * Just In Time (JIT) compilation to x86, x64 or ARM
        * Native Image Generation (NGEN) ahead of time, e.g. during setup - to skip JIT costs
        
compiler tradeoffs
    1. front-end (C#)
        - developer productivity
        - global program knowledge
        - machine agnostic
        - more time to optimize
        - can defer to JIT
    2. back-end (JIT)
        - efficient execution
        - local program knowledge
        - machine knowledge
        - less time to optimize
        - last in line

SOS to analyze methods
    1. Method descriptor `md`
        - internal identifier of the method (internal to the CLR)
        - `!name2ee <module> <method>` convert md to execution engine
    2. display method info
        - class, method table, JIT status, etc.
        - `!dumpmd <method desc>`
    3. break on method
        - native instructions only appear after JIT
        - `!bpmd command`
            * `!bpmd -md <method desc>`
            * `!bpmd <module> <method>`
            * `!bpmd <source file>:<line>` (if PDBs are available)
show code
    1. `!dumpil <method desc>` - show the IL code like ILDASM
    2. `!U <method desc>` - JIT compiled compiled output
    
`thunk`s -   call sites to the JIT, it is patched after the first call

`~0s` switch to main thread

###JIT and inline optimizations
inlining
    1. quite aggressive by default
    2. using `MethodImplAttribute` and `MethodImplOptions`
        - `NoInlining` - prevents the method from getting inlined
        - `NoOptimization` - turns off optimizations in JIT/NGEN compilation
        - `AggressiveInlining` - inlines the method whenever possible

JIT instrinsics
    - methods provided by the CLR as native code; e.g. use the x86 and x64 provide `sin` instrunction equivalent to `Math.Sin`

NGEN - offline JIT compiler
    1. `ngen install <assembly>`
    2. creates _ni file in native image cache
        - %windir%\assembly\NativeImages_v4.0.30319_32
        - folder per assembly
        
JIT will replace contants directly with the result, e.g. `1+2` will be replaced directly with `3`

###Command line compiler
`csc.exe`
    - invoked by MSBuild and Visual Studio
    - supports respons (.rsp) file - compiler settings; csc.rsp in the framework installation folder

compiler options
    - specification of target (.dll, .exe, .winmd)
    - target platform specification ("Any CPU", x86, x64)
    - referencing of dependencies
    - code generation options (debug, optimize)
    - language options (version, checked arithmetic, unsafe code)
    - security settings (strong name signing)

####IL Code generation
    - default code generation for fragments
        * stores to local variable slots
        * branching for control flow
    - many NOP instructions
        * e.g. for curly braces
        * allows setting breakpoints
    
####Compiler `/o+` options
    - enables another compiler pass
        * branch optimizations
        * eliminates empty code blocks
        * gets rid of unused locals
    - only basic optimizations
        * JIT does a lot more at runtime
        * NGEN can take its time to optimize
    - should not change meaning of the code
####Excessive use of locals
    - compilation of return statement
        * evaluate expression into a local
        * emit `ret` instruction
        
####Branch optimization
    - compilation of conditional
        * evaluate conditional, e.g. using `c*` (compare) instructions into local
        * perform branch based on local

####target specified using /t switch
    - executable files
        * `exe` - console application (CUI)
            - console application projects
        * `winexe` - graphical UI application (GUI)
            - windows forms, WPF projects
        * `appcontainerexe` - WinRT application
            - windows XAML projects
        *see dumpbin.exe for the subsystem flag used by the Windows loader
    - library files
        * library - assembly in a .DLL file
            - class library projects
        * module - netmodule that can be linked using al.exe
            - no VS project support
        * winmdobj - Windows Metadata (WinMD) object file
            - Windows Runtime Module projects
            - consumed by WinMDExp.exe

####Windows Runtime (WinRT)
- Evolution of COM
    * Self describing modules using metadata
    * Classes besides interfaces
    * Application host model
    * support for multiple languages and runtimes
        - native code (CRT)
        - managed code (clr)
        - javascript (chakra)
- .winmd files
    * are not .NET assemblies
    * have the same metadata (ECMA 335)
    * can be ILDASM'ed
- `%WINDIR%\System32\WinMetadata`

####Platform architectures
1. architecture specified using `/platform` switch
    - C# compiler always generates IL code
    - platform influences CLR header flags
2. supported architectures
    - default:`anycpu`
        * JIT or NGEN compiles to _best_ target CPU architecture
        * e.g. x64 when running on AMD or INTEL 64 bit
    - specific architectures
        * x86 (will run in WOW64 on a 64-bit system)
        * x64
        * ARM, e.g. tables running windows RT
        * itanium
    - new in .NET 4.5
        * `anycpu32bitpreferred` will choose x86 even on a 64-bit or ARM system
        
`corflags /nologo <executable>` - to tell the IL headers (and whether it is a 32 or 64 bit)

####Language versions
1. restrict language syntax supported using /langversion
    - ISO-1 - C# 1.0 syntax, cf ISO 23270:2003 specification
    - ISO-2 - C# 2.0 syntax, cf ISO 23270:2006 specification
    - 3 - C# 3.0 syntax (LINQ, lambdas, auto properties, etc.)
    - 4 - C# 4.0 syntax (optional and named params, dynamic)
    - 5 - C# 5.0 syntax (async/await, caller info attributes)
2. advanced build settings
    - not "Target Framework" (which influences the BCL - base class library available - only)
3. used to restrict language
    - common baseline in team
    - optimize for reading code
    
####Assembly references
1. "add reference" dialog in Visual Studio
    - project references influence build order
        * no cycles allowed
    - `/r` compiler flag to reference binaries
        * full path or file name searched on /lib paths
        * can be used to specify aliases (cf. "extern alias")
2. new in .NET 4.5
    - framework API sets and Extension SDKs
        * referenced as a whole
        * a lot of `/r` flags emitted
        * used by Portable Library, Windows Runtime SDKs, etc.
3. C# compiler prunes out what's not used
    - make sure assemblies are copied if needed
    - e.g. for code relying on Assembly.LoadFrom
4. `/nostdlib` compiler switch
    - excludes mscorlib.dll default reference
    - often used by csc target in MSBuild
        * point to specific mscorlib.dll
        * one compiler, different frameworks
    - dependencies of C# on BCL
        * base classes such as Object, MulticastDelegate
        * APIs such as String.Concat, Interlocked.CompareExchange
        * interfaces such as IDisposable
####Portable library
1. design goals
    - "build once, run everywhere" libraries
    - targeting various environments
        * desktop CLR
        * windows XAML
        * windows phone
        * silverlight
2. new assembly structure
    - assemblies for sets of functionality
    - composition of assemblies into profiles
    - System.IO.dll, System.Reflection.dll, System.Collections.dll
3. mechanism:
    - `[assembly:TypeForwardedToAttribute]`
        * indicates a type has moved to another assembly
    - `[assembly:ReferenceAssemblyAttribute]`
        * empty reference assemblies targeted by build
        
##Constants and arrays initializers
`dumpbin.exe /all <dll>`

###switch statements
1. switch in C#
    - unordered set of cases
    - optional default case
    - cases have to be literals or compile time constants
    - no implicit fall-through
    - governing types:
        * integral types (int, long, enums)
        * bool, char, string
        * nullable variants of the above
        * user defined implicit conversion allowed
2. switch in IL
    - jump instruction
    - one unsigned integer operand on the stack
    - arguments consist of
        * the number of jump targets
        * an offset for each target
3. non-zero offsets
    - sort cases to find closest to zero
    - rebase to zero using add or sub
4. gaps between cases
    - fill gaps with jumps to default case
    - only if the gaps aren't too big
    - when gaps are too big it will compile to if-then-else
5. sparse labels
    - create clusters, build search tree
    - try to minimize the number of evaluations
    - will optimize and go directly to the corresponding rebased switch statement
6. switching on strings
    - start with if-else compilation strategy to cover _null_ case, for small number of cases
    - for more than 6 labels will lazily initialize a `Dictionary<string, int>` 
    
###Events
1. events in C#
    - more than delegate-typed properties
    - raising event requires private access
    - attaching and detaching event handlers can have non-private access
2. events in CLR
    - metadata citizens, just like properties
    - point to methods for add, remove, raise
3. compiler-generated code
    - default code pattern
        * delegate stored field
        * add and remove accessors
    - customization pattern
        * specify add and remove accessors
        * manage underlying delegate storage
3. raising events
    - delegate invocation syntax
    - typical On* method design pattern
    
4. adding and removing handlers
    - `+=` and `-=` are indirect calls to `add` and `remove` methods
    - thread safety is achieved using `Interlocked` methods
5. windows runtime (WinRT) support in .NET 4.5, C# 5.0
    - `System.Runtime.InteropServices.WindowsRuntime`
        * `EventRegistrationToken`
            - closer to Win32 handles
            - add accessor return a registration token
            - remove accessor accepts a registration token
        * `EventRegistrationTokenTable<T>`
            - stores mapping between delegates and tokens
            - `AddEventHandler` used by compiler
            - `RemoveEventhandler` used by compiler
            
##Performance of functional programming in C#
###lambda expression homoiconicity(homo - same, iconic - representation)
assignable to:
    - delegate types (shorthand for anonymous methods)
    - expression tree types (code as data representation)

###closures
capture local variable from outer scope

are accomplished using _display_ classes with backing fields, that hoist stack allocated values to fields of the heap allocated classes

####space leaks
    - locals hoisted to same display class
    - delegate keeping display class instance alive (u8sually when a delegate is created inside of another delegate - as this ensures access from one to the other)
    
###foreach loop closures
C# 5 has a breaking change for loops and creates itnernally a local variable to avoid closure related issues

##C# extensibility
###syntactic sugar
- language features defind in terms of other existing features
- expansion of shorthand syntax into patterns
    * sometimes using interfaces
    * often just a pattern
examples
    - **foreach statements** leverage _enumeration pattern_
        * get enumerator, MoveNext, Current, Dispose
    - **query expressions** leverage _query pattern_
        * Where, Select, SelectMany, GroupBy, OrderBy, ThenBy, Join, GroupJoin
    - **await expressions** leverage "awaiter pattern"
        * GetAwaiter, IsCompleted, OnCompleted
    - **list initialization expressions** leverage _collection pattern_
        * IEnuemrable, Add
###Foreach
only needs a type that exposes GerEnumerator method capable of producing an iterator

`foreach` typing can be defined as `var`, in which case the compiler will infer  the type of the enumerated elements; conversions are considered (for backward compatibility with C# 1.0) yet this introduces the conversion caveats (conversion failures or hidden boxings)

###Operator overloading
- arithmetic, relational, logical, conversions
- no _evil_ overloading like comma or dot
- just methods in disguise, e.g. op_Addition for decimal addition (there is no CLR `+` operator)

###Nullability for value types
- ternary logic:
    * return null if any operand is null
        - boolean for relational operators (false if any is null)
    * otherwise evaluate the operator
- lifting operators
    * when the value type is made nullable the compiler will _lift_ the overloaded operator and apply it on values of the nullable type
###Query expressions
consist of
    - one or more _from_ clauses
    - joins using _join_ clauses
    - filters using _where_ clauses
    - sorting using _orderby_ clauses
    - projections using _select_ and _let_ clauses
    - groupings using _group by_ clauses
    - termination with a _select_ or _group by_ clauses
        * continuation using _into_
        
###Query pattern
- each clause translates in a (fluent) method call pattern
- query expressions are syntactic sugar

###LINQ to *
- can implement the query pattern on any type
    * language has no dependencies on particular BCL types
- fluent method pattern using
    * instance methods
    * extension methods
- built-in query provider implementations using `IQueryable<T>`

###Query providers
- have a `GetEnumerator()` method
- can have `Where()`, `Select()` methods as instance methods
- need to use `Expression<Func...>` for sources not in memory, to capture the predicate for external use
- need to provide `Expression` getter to return an `Expression` type

query providers work by
- analysis of expression trees at runtime
- only lambda expression can be captured
    * queryable uses of a clever trick to capture the entire query expression
    
```c#
public static IQueryable<T> Where<T>(
    this IQueryable<T> source,
    Expression<Func<T, bool>> predicate)
{
    var whereOfT = (MethodInfo)MethodBase.GetCurrentMethod();
    var method = whereOfT.MakeGenericMethod(typeof(T));
    
    return source.Provider.CreateQuery<T>(
        Expression.Call(method, source.Expression, predicate));
}
```

###Transparent identifiers
keep track of scope in a query by flowing objects
    - anonymous type instances
    - no user-visible identifier (_transparent_)

###dynamic typing
operations on dynamically typed variables are late bound

the compiler emits _dynamic call sites_, parameterized with Microsoft.CSharp.dll language binders

####DLR
language binder performs late binding (and code to execute at runtime)
    - runtime type information (GetType)
    - overload resolution
    - emit expression trees
DLR compiles and caches call sites

leveraging the dynamic type:
    - interop with dynamic languages, e.g. IronPython, JavaScript
    - access to weakly typed data, e.g. XML, JSON
    
custom dynamic object: inherit from `DynamicObject` to intercept calls

method operating on dynamic types become late bound calls too

###awaitable types
await expressions
    - suspend the asynchronous method they're contained in until the awaitable object signals completion, evetually
    
mechanism
    - async method choppedup in state machine
        * each await expression introduces a state transition
        * return and throw wired up to resulting task
    - await expressions
        * `await` operand must be awaitable (method pattern for obtaining an awaiter)
        * continuation registered to trigger "move next" on state machine
async state machine
```C#
Task<double> SurfaceAsync(double r)
{
    var tcs = new TaskCompletionSource<double>();
    var state = 0;
    var awaiter1 = default(TaskAwaiter<double>);
    
    var moveNext = new Action(() =>
        {
            switch (state) {
                case 0: awaiter = ComputeAsync().GetAwaiter();
                        state = 1;
                        if(awaiter.IsCompleted)
                        {
                            goto case 1;
                        }
                        else
                        {
                            awaiter1.OnCompleted(moveNext);
                        }
                        
                        break;
                        
                case 1: tcs.SetResult(awaiter1.GetResult());
                
                        break;
            }
        });
    moveNext();
    
    return tcs.Task;
}
```

an object of type T is awaitable if
    - the compiler can emit a GetAwaiter() method call, with result type A
        * instance metho, extension method or dynamically typed
    - if the awaiter type A is dynamic or
        * has a boolean returning `IsCompleted` property
        * has a `GetResult()` method returning void or some type R
        * implements `INotifyCompletion` (or `ICriticalNotifyCompletion`)
        
##Generics
parameterization by type for
    - types: interfaces, classes, structs, delegates
    - methods
runtime feature added in CLR 2 (codename "Gyro")
    - specialized type and methods layouts created at runtime
        * unline erase-based approaches, cf. Java
        * full-fidelity type information in reflection
        
benefits:
    - static typing benefits:
        * compile time checking
    - performance improvements
        * reduces boxing of values
        * less runtime_checks
        * no compile time expansion
####no boxing cost
generic collections contain values
    - storage using specified type
    - specialized object layouts avoid boxing
    - static typing for the wind:
        * no casting that may fail
        
generics are represented in IL code

they are compiled at runtime by JIT (or NGEN)

`!T` - the type that IL works with (placeholder)

`System.__Canon` - canonical CLR representation for reference type parameters

###constrained virtual calls
- virtual calls on value types require boxing
    * access to method table
    * typical cost on interface calls
    * boxing is the price of calling interface methods on value types
- complexity for compilers
    * new `.constrained` prefix for call instructions
    * decide whether type parameter is value or not
###generic constraints
putting restrictions on generic type parameters
    - less flexibility for the caller
    - more power to the callee
    
####constraint types
- default constructor constrain `new()`
- `class`/`struct` constrain
- base class constraint
- interface implementation constraint

###covariance and contravariance
**co**variance (_companion_)

if `D -> B` and `D[]`, `B[]` would `D[] -> B[]`? yes. arrays are covariant; yet this covariance is broken
```C#
var d = new[]{new D(), new D()};
B[] b = d;
b[0]= new E();
d[0].CallMethodOnTypeD(); /*! CLR will throw a runtime exception: ArrayTypeMismatchException*/
```

for arrays
    - covariance is safe for reading
    - `ArrayTypeMismatchException` upon write
    
for generic interfaces and delegates
    - definition-site co - and contravariance
    - safety via input/output restrictions
    
**covariance** is indicated through `out` keyword and is used only on output positions; can assign `IEnumerable<string>` to `IEnumerable<object>` - `IEnumerable<out T>`

**contravariance** (`in`) restrict the type parameter to input only positions; 
```C#
IComparer<Fruit> fruitComparer= ...;
IComparer<Apple> appleComparer;

appleComparer = fruitComparer;
```

in IL the covariance is represented with:
    - `+` (`+T`) for the generic type parameter
    - `-` (`-T`) for the generic type parameter
