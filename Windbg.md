###Prerequisites
On Windows 7 get Windows 7 SDK and .NET Framework 4

###Start `WinDbg`
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

###Useful commands

####!DumpHeap
```
!DumpHeap -type Chapter_02_GarbageCollector.C
```

####!findroots
```
REM dump the heap
!DumpHeap
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

####!gchandles
```
REM Find handles and their type; this would display pinned object too
!gchandles
REM use !do to go through objects hierarchy and discover the source of a pinned object
!do 0256521c
```

####!eeheap
```
REM Get the list of free blocks of memory
!dumpheap -type Free
REM Get the list of heap segments
!eeheap -gc
REM observe the ephemeral segment allocation, dump it
!dumpheap 02731000  02c74df
```

####!address
```
!address -summary
!address -f:Free
```

####!gcwhere
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

Find more information about `SOS` [on msdn](https://msdn.microsoft.com/en-us/library/windows/hardware/ff540665(v=vs.85).aspx)