###attaching aspects
- interceptors
    * intercept calls to class methods/properties
    * usually involves an IoC container
    * no postcompilation changes to assemblies
- IL code weaving
    * run after application compilation
    * post process assemblies
###AOP using interceptors
- IoC containers : the request for object instance is intercepted and wrapped within a dynamic object (interceptor) that implements the interface and proxies the call the the actual instance
    * castle windsor: implement `IInterceptor`
    * ninject
    * unity
- dynamic proxies
    * in memory decorators
    
###AOP using IL code weaving
- post compilation
- modifies code execution

* PostSharp
* LOOM.NET
* Wicca

- implement a base class
- attaching 