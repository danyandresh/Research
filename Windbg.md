This material is a shortlist of operations needed to inspect and understand your application's memory.

Please refer to [Ben Watson's book `Writing High Performance .NET code`](http://www.writinghighperf.net/) for a more detailed discussion on this topic.

####Prerequisites

- On Windows 7 get `Windows 7 SDK and .NET Framework 4` or `Windows SDK 8.1`

#### Getting started with `WinDbg` and SOS
Use these instructions to get Windbg initialized (and `SOS` loaded)

```
REM execute application to a breakpoint (so the clr load is triggered)
.cordll -ve -u -l

REM for x64 app launch x64 version of Windbg
REM for x86 app launch x86 version of Windbg

sxe ld sos
.loadby sos clr
lmv mclr
```

Note: I used `Windows SDK 8.1` as it has windbg versions for both `x86` and `x64` apps

#### Determine objects not garbage collected

* `gcroot` - find the root of an object using it's address
* `dumpheap` - dump the heap for a particular type of objects
* `findroots` - set breakpoints before next GC collection; find information about particular objects

```
!DumpHeap -type CodeSandbox.C
REm use gcroot to navigate to the root handle
!gcroot 036a33a8
```

```
REM set breakpoint before next gen 0 collection
!findroots -gen 0
g
REM wait for the breakpoint to be hit
REM use the address of an object dumped from the heap above
!findroots 265ce2b0
REM output:
REM Object 265ce2b0 will survive this collection:
REM 	gen(0x265ce2b0) = 2 > 0 = condemned generation.
```

#### Determine pinned objects

* `gchandles` - display handles and their type (use for pinned objects too)
* `do` - explore objects hierarchy and discover source of a pinned object

```
REM Find handles and their type; this would display pinned object too
!gchandles
REM use !do to go through objects hierarchy and discover the source of a pinned object
!do 0256521c
```

##### Complementary/Additional tools:

* `PerfView`

#### Determine heap fragmentation

* `dumpheap`
* `eeheap` - inspect the structure of heap segments
* `address` - insepct addresses of the objects from the heap

```
REM Get the list of free blocks of memory
!dumpheap -type Free
REM Get the list of heap segments
!eeheap -gc
REM observe the ephemeral segment allocation, dump it
!dumpheap 02731000  02c74df
```

```
!address -summary
!address -f:Free
```

##### Complementary/Additional tools:

* `VMMap`
* `CLRProfiler`

#### Determine the generation of an object

* `gcwhere` - find objects generation
* `dso` - equivalent to `DumpStackObjects`

Find and objects' generation
```
REM Make sure to call this on the right thread
!dso
!gcwhere 027335fc
```

#### Determine objects that survived gen 0

* `FindRoots`
* `DumpHeap`
* `eeheaps`
* `bp`

Generation 0 heap is at the end of the `ephemeral segment`, therefore discovering objects in that range requires discovering the heap segments addresses (end) and generations start addresses
```
REM Set breakpoint for first gen 0 collection
!FindRoots -gen 0
g
!DumpHeap -stat
REM discover address of the ephemeral segment (that is gen 1 and gen 0) as well as gen 0 start
!eeheap -gc
REM given gen 0 is at the end of the heap, dump heap for gen0 start and ephemeral segment end (to get the heap range for gen 0)
!DumpHeap 0x027e56d8 029e56e8
bp clr!WKS::GCHeap::RestartEE
REM for server GC call bp clr!SRV::GCHeap::RestartEE
REM also for server GC eeheap would print info for each heap
g
!eeheap -gc
REM notice the addresses of heaps changed
!DumpHeap 0x027e7018 027e7024
REM
```

#### Discover explicit GC.Collect calls

* `bpmd` - set a _managed_ breakpoint

```
REM 
!bpmd mscorlib.dll System.GC.Collect
g
!DumpStack
```

#### Weak references

* `gchandles` - weak references are GC handles, discover using this method

```
!gchandles
```

#### Inspect JIT Compilation

* `sxe` set a breakpoint when the clrjit is loaded
* `bpmd` set a breakpoint in managed code

```
REM set a breakpoint when clrjit is loaded
sxe ld clrjit
g
.loadby sos clr
REM set a breakpoint at the beginning of the main function
!bpmd CodeSandbox JitCall.Main
g
REM Alt + 7 for disassembly
REM Alt + 4 for registers

```

Additional materials:

* [`SOS`](https://msdn.microsoft.com/en-us/library/windows/hardware/ff540665(v=vs.85).aspx)
* [`Windbg` common commands](http://windbg.info/doc/1-common-cmds.html)
