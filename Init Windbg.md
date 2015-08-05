###Prerequisites
On Windows 7 get Windows 7 SDK and .NET Framework 4

###Start `WinDbg`
Use these instructions to get Windbg initialized (and `SOS` loaded)

```
.cordll -ve -u -l
.load c:\Windows\Microsoft.NET\Framework64\v4.0.30319\SOS.dll
.load c:\Windows\Microsoft.NET\Framework64\v4.0.30319\clr.dll
sxe ld sos
.loadby sos clr
lmv mclr
REM dump heap by object type
!DumpHeap -type Chapter_02_GarbageCollector.C
```

More information on `SOS` [on msdn](https://msdn.microsoft.com/en-us/library/windows/hardware/ff540665(v=vs.85).aspx)