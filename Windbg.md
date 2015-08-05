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
REM dump heap by object type
!DumpHeap -type Chapter_02_GarbageCollector.C
```

Note: I used `Windows SDK 8.1` as it has windbg versions for both `x86` and `x64` apps

###Useful commands
####!gcroot
####!findroots -gen 0


More information on `SOS` [on msdn](https://msdn.microsoft.com/en-us/library/windows/hardware/ff540665(v=vs.85).aspx)