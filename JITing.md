
####Prerequisites

- `Visual Studio Command prompt`

#### Native Image

> The Native Image Generator (Ngen.exe) is a tool that improves the performance of managed applications. Ngen.exe creates native images, which are files containing compiled processor-specific machine code, and installs them into the native image cache on the local computer. The runtime can use native images from the cache instead of using the just-in-time (JIT) compiler to compile the original assembly.

Find more on [msdn](https://msdn.microsoft.com/en-us/library/6t9t5wcf(v=vs.110).aspx)

##### Install image in the machine native image cache

```
ngen install CodeSandbox.exe
```

##### Uninstall image from the machine native image cache

```
ngen uninstall CodeSandbox.exe
```

##### Check native image

```
ngen display CodeSandbox.exe
```

#### Optimized NGen images

Before installing a native image use [`Managed Profile Guided Optimization (MPGO)`](https://msdn.microsoft.com/en-us/library/hh873180.aspx) to keep _reference locality_ (that is otherwise lost when using solely NGen). This is creating a profile for NGen to use later on when the native image is generated

```
mpgo -scenario CodeSnadbox.exe -assemblyList *.* -OutDir c:\CodeSandboxProfile\
```

Sure, now ngen has to be used with the new profile stored in `CodeSnadboxProfile` folder
```
ngen install c:\CodeSandboxProfile\CodeSandbox.exe
```
