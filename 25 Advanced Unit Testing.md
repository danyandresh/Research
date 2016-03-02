#Advanced Unit Testing
unit tests work as a _safety net_

##DRY vs DAMP
###DRY
code to reduce maintanability costs 

**Cohesion** can also be measured in terms of fields of a class being used withing methods.

###DAMP
- **d**escriptive
- **a**nd
- **m**eaningful
- **p**hrases

KISS:

- Arrange Act Assert (AAA) - four phase test; given/when/then - commonly used in BDD
- Small: 1-16 lines; DRY
- Clear: should be deterministic, a clear path through each test; cyclomatic complexity of 1; DAMP

seeing the tests failing is very important:
- TDD: red/green/refactor
- test after: follow procedure for characterization tests

trustworthy test suites are largely append only (should follow O/C principle)

Tests should DSL to be both DRY and DAMP.

##Test utility code
###Four-phase test
- fixture setup
- exercise SUT
- result verification
- fixture teardown

tests are not meant to test constructors, and constructors usually add _accidental complexity_

###test data builder
- mother object: a collection of helper methods that create test data methods according to various scenarios. it is difficult to create objects with combined characteristics, e.g. `CreateBasketWithSmallDiscountAndSpecialOffer()`
- fluent builder: makes it easier to deal with variations of test data (it is a query language for test data). e.g. `new BasketBuilder().WithSmallDiscount().WithSpecialOffer().Build()`

can simplify the data test builder by having an implicit coversion between data builder object and the test object
```c#
public static implicit operator UserProfileInput(UserProfileInputBuilder builder)
{
    return builder.Build();
}
```

test data builder can create coarse-grained higher order methods that can be used to from domain specific language (DSL)

AutoFixture: http://bit.ly/Zq5imH

###SUT Factory (system under test)
creates instances of a SUT and decouples from SUT constructor

use Moq as default for services

####SUT mother
can use a helper mother object, can mock object and pass that in to the mother object creator

####SUT (fluent)  Builder
use data builder and pass in mocked object to various methods

alter the services to use the real ones through dedicated methods

####Automation
- AutoFixture
- Castle Windsor
- Autofac

###auto-mocking container
creational patter used to create instances of a system under test

decouples test case from the SUT's constructor

uses autowiring to create instance of a SUT; repurposes a DI container

```c#
var container = new WindsorContainer()
                    .Install(new ShopFixture());
                    
var sut = container.Resolve<BasketController>();

var channelMock = container.Resolve<Mock<IChannel>>();
```

```c#
public class AutoMoqResolver : ISubDependencyResolver
{
    private readonly IKernel kernel;
    
    public AutoMoqResolver(IKernel kernel)
    {
        this.kernel = kernel;
    }
    
    public bool CanResolve(
                CreationContext context,
                ISubDependencyResolver contextHandlerResolver,
                ComponentModel model,
                DependencyModel dependency)
    {
        return dependency.TargetType.IsInterface;
    }
    
    public object Resolve(
                    CreationContext context,
                    ISubDependencyResolver contextHandlerResolver,
                    ComponentModel model,
                    DependencyModel dependency)
    {
        var mockType = typeof(Mock<>).MakeGenericType(dependency.TargetType);
        
        return ((Mock)this.gernel.Resolve(mockType)).Object;
    }
```

```c#
public class AutoMoqInstaller : IWindsorInstaller
{
    public void Install(
                    IWindsorContainer container,
                    IConfigurationStore store)
    {
        container.Kernel.Resolver.AddSubResolver(
                                    new AutoMoqResolver(container.Kernel));
                                    
        container.Register(Component.For(typeof(Mock<>)));
        
        container.Register(Classes
            .FromAssemblyContaining<Security>()
            .Pick()
            .WithServiceSelf()
            .LifestyleTransient());
    }
}
```

```c#
var container = new WindsorContainer().Install(new AutoMoqInstaller());

var sut = container.Resolve<SecurityController>();
```

###fixture object

a pattern (meta pattern) more conceptual in nature: encapsulate the entire fixture for a test case in a single entry point

```c#
var fixture = new BasketControllerFixture();
var sut - fixture.CreateSut();

var basket = fixture.Basket.WithSmallDiscount.Build();

var channelMock = fixture.channel.AsMock();
```

####feature envy
a method accesses the data of another object more than its own data

https://sourcemaking.com/refactoring/smells/feature-envy

test patterns:
1. test data builder
    - test data
    - creational pattern
2. SUT factory
    - creational pattern
    - template for test data builder
    - SUT instances
3. auto-mocking container
    - creational pattern
    - SUT instances
4. fixture object
    - creational pattern
    - SUT instances
    - structural pattern
    - template for test data builder

##structural inspection
complex: intrinsic, composed of parts; opposite of unity

complicated: extrinsic, complicated by external influences

- unit test each sub-component in isolation
- prove that all parts interact correctly
- inspect the structure of composed parts

- unit testing technique
- API design philosophy

1. triangulation: create test cases to enforce the system behave in the desired manner
2. behaviour verification: monitors the interaction between components in order to assert they behave as desired

structural inspection falls into behaviour verification category

###API design philosophy
what is composed can be exposed, e.g. an object composed using dependency inversion principle can expose as a property or field what has just been injected

```c#
public class Discount : IBasketElement
{
    public Discount(decimal amount)
    
    public decimal Amount { get; }
    
    public IBasketVisitor Accept(IBasketVisitor Accept(IBasketVisitor visitor))
}
```

> properties break encapsulation

it doesn't make much sense to say that exposing data that was provided through constructor really breaks encapsulation, as it is data already known to a third party

adding a property to a concrete class doesn't impact the interface; constructors are implementation details just like properties are

unit testing: test each part in isolation and prove that parts correctly interact

```c#
[Fact]
public void AcceptReturnsCorrectResult()
{
    //Fixture setup
    var v1 = new Mock<IBasketVisitor>().Object;
    var v2 = new Mock<IBasketVisitor>().Object;
    var v3 = new Mock<IBasketVisitor>().Object;
    
    var e1Stub = new Mock<IBasketElement>();
    var e2Stub = new Mock<IBasketElement>();
    
    e1Stub.Setup(e => e.Accept(v1)).Returns(v2);
    e2Stub.Setup(e => e.Accept(v2)).Returns(v3);
    
    var sut = new Basket(e1Stub.Object, e2Stub.Object);
    
    //excercise system
    var actual = sut.Accept(v1);
    
    //verify outcome
    Assert.Same(v3, actual);
}
```

Testing techniques:
- devil's advocate
- gollum style
- triangulation

structural inspection rather concerns itself with how fine-grained units are composed

###verifying a facade
- implement the structure
- sanity check integration tests

##itentity
- apply POLA (principle of least astonishment)

object types:

- entities: outlast the process lifetime (e.g. data persisted in a database); ID is the unique identification of such an entity
- value objects: value
- services: default reference equality

the more object can be modeled as value object, the easier is a system to test

**value object** is not a _value type_, it is a design pattern that explains how a calss can behave like a single value

###value equality testing
need to override and unit test `Equals`
```c#
public static IEnumerable<bool> BothEquals<T>(
    this T sut, T other)
    where T: IEquatable<T>
{
    yield return sut.Equals((object)other);
    yield return sut.Equals(other);
}
```
derived value: compute an (expected) value as the SUT is expected to, it is not DRY but the alternative is to provide magic numbers. http://xunitpatterns.com/Derived%20Value.html

###entity equality
is tested using the ID
###service equality
there is value in testing services as value objects, but if that is poossible raises the question whether the services are really services

####services with value object identity
treating services as value objects makes structural inspection more DAMP


##test specific identity

IEqualityComparer - xUnit has overloads that allow custom comparer to be provided

when assertions are closely related it might not be a case of _assertion roulette_ code smell http://xunitpatterns.com/Assertion%20Roulette.html

a method with a cyclomatic complexity of 1 is safe to be added to tests without being covered itself by other unit tests

`Assert.IsAssignableFrom<T>(t)` - will downcast an object to a particular type

###composite comparers
each comparer will downcast and wire down the comparison requests

###resemblance
is another technique for avoiding equality pollution

valuable when doing behaviour verification

make a derived class in test project, override `Equals` (and `GetHashCode`), use derived instance as expected value - invoke `Equals` on the derived class to compare against actual

###likeness
a semantic convention based comparison of two object, which may or may not be of the same type (types could be disparate)

`SemanticComparison` open source library on NuGet

`Likeness<TSource, TDestination>`
- Without
- OmitAutoComparison
- With

`CreateProxy()` dynamically emitted resemblance of TDestination, compiling it and injecting it into the current process

the dynamic class is a specialized mock that overrides the `Equals` method

```c#
[Theory, AutoWebData]
public void PostSendsOnChannel(
    [Frozen]Mock<IChannel<RequestReservationCommand>> channelMock,
    BookingController sut,
    BookingViewModel model)
{
    sut.POst(model);
    
    var expected = model.MakeReservation()
            .AsSource().OfLikeness<RequestReservationCommand>()
            .Without(d => d.Id)
            .CreateProxy();
            
    channelMock.Verify(c => c.Send(expected));
}
```

