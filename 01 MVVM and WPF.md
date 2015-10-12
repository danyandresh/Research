#MVVM

Achieve [separation of concerns](https://en.wikipedia.org/wiki/Separation_of_concerns) in WPF/XAML applications

###Benefits

- maintainability
- testability
- extensibility

###Related to

- MVC
- MVP
- MVVM

###Used for

- WPF
- Silverlight
- Windows 8/ WinRT
- HTML 5 (Knowckout/Angular)
- Xamarin/Mobile Apps
- Windows 10 (XAML + WinRT)

###Responsibilities

- View
- ViewModel
- Model
- Client Services/Repositories

####Model responsibilities

- Contain data
- Relationships between model objects
- Computed properties
- Raise change notifications (INotifyPropertyChanged.PropertyChanged)
- Validation (INotifyDataErrorInfo/IDataErrorInfo)

####View responsibilities

- Structural definition of what the user sees
- **No code behind**
- Code used purely for UI transformations/interaction/reaction (with other UI elements)
 
####ViewModel responsibilities

- Expose data to the view for presentation
- Encapsulate interaction logic:
	* Calls to the business/data layer/service
	* Navigation logic
	* State transformation logic

- _Expose_ model objects
- _wrapped_ model properties (to apply transformation or simplify them for display)
- _client state_ (observable)

####Client Services/Repositories

- Shared functionality or data access
- Consumed by one or more `ViewModel`s
- Decouples `ViewModel`s from external dependencies
	* Data storage
	* Service access
	* Client environment
- Can act as data caching container

###Fundamental to MVVM

```XAML
View.DataContext=ViewModel
```

###View-ViewModel construction

1. View-First
2. ViewModel-First

##Async in MVVM

- `Task` based async and `async`/`await` used in .NET
- `ViewModel`s need to wait for some async operations to complete
- Client Services/Repositories expose such async operations

See [Asynchronous Programming - Pause and Play with Await](03 Asynchronous Programming - Pause and Play with Await.md) for a detailed set of notes on `async`/`await`

##Data binding

- Bind the `DataContext` on the `View`

```XAML
<UserControl.DataContext>
    <local:UserControlViewModel />
</UserControl.DataContext>
```

- Bind `View` controls to `ViewModel` (after the `DataContext` of the view has been set)

```XAML
<TextBox Text="{Binding Customer.FirstName}" />
```

- Bind _commands_ to `ViewModel` defined properties that return `ICommand`

```XAML
<Button Command="{Binding SaveCommand}"/>
```

###Blend SDK

```XAML
<i:Interaction.Triggers>
    <i:EventTrigger EventName="Loaded">
	    <ei:CallMethodAction TargetObject="{Binding}" MethodName="LoadCustomer"/>
	</i:EventTrigger>
</i:Interaction.Triggers>
```

###Hooking up `View`s and `ViewModel`s
####View-First construction
Set the `DataContext` property of the `View` to the `ViewModel` instance:

- _declaratively_, through XAML
- in codebehind, before `InitializeComponent` method is called (if the constructor needs parameters or when using DI)

Determine whether the current `viewModel` is actually used in the _design_ mode:
```csharp
if(DesignerProperties.GetIsInDesignMode(new System,Windows.DependencyObject()))
```

Import an namespace or type in `XAML`:
```XAML
xmlns:namespaceOrType="clr-namespace:MyNamespace.Type"
```
#####View-First: ViewModelLocator

`ViewModelLocator` is a meta-pattern for locating and hooking up the `viewModel`s. It's responsibility is:

- Determine and construct the `viewModel` according to the `view`
- Set the binding between the `view` and the `viewModel`

Example using an _attached property_

```csharp
public class ViewModelLocator
{
    public static bool GetCustomAttached(DependencyObject obj)
	{
	    return (bool)obj.GetValue(CustomAttachedProperty)
	}
	
	public static void SetCustomAttached(DependencyObject obj, bool value)
	{
	    obj.SetValue(CustomAttachedProperty, value);
	}
	
	public static readonly DependencyProperty CustomAttachedProperty =
	    DependencyProperty.RegisterAttached("CustomAttached", typeof(bool), typeof(ViewModelLocator), new PropertyMetadata(0, AutoWireViewModelChanged));
		
	private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
	    //determine if it runs in designer and do things differently
		/*...*/
		
		//determine view type name		
		//determine viewModel (if it can be inferred from view type name)
		//create instance of the viewModel
		//set the DataContext of the view to the viewModel
	}
}
```

Use the above constructed `viewModelLocator`:

```XAML
<UserControl local:ViewModelLocator.AutoWireViewModel="True">
...
```

`Prism` framework has a `ViewModelLocator` built-in.

####DataContext

A `DataContext` bound at a root level element become the `DataContext` for the child elements too

[Binding _mode_](https://msdn.microsoft.com/en-us/library/system.windows.data.bindingmode(v=vs.110).aspx) can be:

- `OneWay` - when data from the `ViewModel` is pushed to the `View`
- `TwoWay` - in addition to what `OneWay` offers, this mode allows the changes in `View` to be reflected in the `ViewModel`
- `OneTime` - the data from `ViewModel` is pushed to the `View` **once** only
- `OneWayToSource` - opposite to `OneWay`

For `OneWay` or `TwoWay` bindings `INotifyPropertyChanged` interface needs be implemented; for `TwoWay` or `OneWayToSource` the [`UpdateSourceTrigger`](https://msdn.microsoft.com/en-us/library/system.windows.data.binding.updatesourcetrigger(v=vs.110).aspx) must be set. 

#####Data Binding for ItemsSource

`ItemsSource` binds the control to a collection of items, and the _data template_ is bound to each of the items contained in that collection. Certain controls support data template and `DataGrid` is one of them.

######DataTemplates

`ContentControl`
    
```XAML
<ContentControl Content="{Binding CurrentViewModel}"/>
```

* If the content bound to is a UI element, `ContentControl` will just render the element, based on its layout scheme for children
* If the element is not a UI one, the resources are being looked up to discover any `DataTemplate` defined for this type of object
    ```XAML
	<DataTemplate DataType="{x:Type <type>}">
	    <!-- instance of the root element to be created automatically-->
	    <<view type>/>
	</DataTemplate>
	```
* If there is no resource available for a non-UI element, it is simply `ToString`ed

`DataTemplate` types:
* **Explicit** DataTemplate is defined as a keyed resource and `ItemTemplate` property is set to that key
    ```XAML
	ItemTemplate="{StaticResource ExplicitDataTemplateKey}"
	```
 * **Implicit** DataTemplate is defined on the _type_ using  `DataType` property on the `DataTemplate`: the type must be referred with `{x:Type <wanted type>}`

One example of binding `View`s to `ViewModel`s is to define a `ViewModel` property on a window that exposes the `ViewModel` (and binds `DataContext` to that property), present it with a `ContentControl` like this:
```XAML
<ContentControl Content="{Binding CurrentViewModel}" />
```
The last thing needed is to define an _implicit_ `DataTemplate` for the `ViewModel` types as described above.

##View/ViewModel communication

###`Command` pattern 

1. `Invoker` - a UI control (from the `View`) or another piece of code
2. `Receiver` - Action defined by `ViewModel`

The above two are decoupled using an abstraction layer using a delegating command implementation (`ICommand`).
The commands in WPF can enable/disable the `invoker` control (`ICommand.CanExecute()`); after binding, the `invoker` is automatically enabled/disabled through the `DelegateCommandBase.CanExecuteChanged` event

####Input bindings

```xaml
<UserControl.InputBindings>
    <KeyBinding Key="D" Modifiers="Control" Command="{binding DelegateCommand}"/>
</UserControl.InputBindings>
```

The above command is called when `Ctrl`+`D` is pressed.

###Attached Properties

Can be defined in any class and applied to any object inheriting `DependencyObject`

###Behaviours

Can change properties, attach to event handlers; can be defined with `Attached Properties` or with `Blend SDK`

####Blend SDK

Uses attached properties to attach behaviours to elements.
The `behaviour`s can communicate between `View` and `ViewModel` by wiring events/property changes from `View` into `ViewModel` and the other way around

To turn an `Attached Property` into a `Behaviour` the `PropertyMetadata` must use a change event (as shown in property definition from [View-First: ViewModelLocator](#View-First: ViewModelLocator))
The resulted attached property will be used like
```xaml
local:CustomBehaviours.CustomBehaviourProperty="Initialize"
```
```csharp
public async void Initialize()
{
    if(DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
	{
	    return;
	}
	
	//do expensive async initialization stuff
}
```

To achieve the same through Blend SDK behaviours:
```XAML
<i:Interaction.Triggers>
    <i:EventTrigger EventName="Loaded">
	    <ei:CallMethodAction TargetObject="{Binding}" MethodName="Initialize"/>
	</i:EventTrigger>
</i:Interaction.Triggers>
```

`{Binding}` is binding the `CallMethodAction` directly to the current `DataContext`

###Property Change notifications

A mechanism to trigger bindings to refresh

1. `DependencyProperty`s have their own internal mechanism for change notifications
2. `INotifypropertyChanged`, by defining an event and change code so property changes call that event (this is commonly used in MVVM)

```c#
public class CustomViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged = delegate { /* to avoid calling for null each time*/ };
}
```

###Blend SDK Behaviour

```c# 
public class CustomBehaviour : Behaviour<ContentControl>
{
    
    public static string Message
	{
	    get
	    {
    	    return (string)GetValue(MessageProperty)
	    }
	
	    set
	    {
	        SetValue(MessageProperty, value);
	    }
	
	public static readonly DependencyProperty MessageProperty =
	    DependencyProperty.Register("Message", typeof(string), typeof(CustomBehaviour), new PropertyMetadata(0, OnMessageChanged));
		
	private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
	    // cast d to CustomBehaviour
		behaviour.AssociatedObject.Content = e.NewValue;
		behaviour.AssociatedObject.Visibility = Visibility.Visible;
	}
	
	protected override void OnAttached()
	{
	    //attach to various events from the AssociatedObject
	} 
}
```

Usage of this behaviour:
```xaml
<ContentControl>
    <i:Interaction.Behaviours>
	    <local:CustomBehaviour Message="{Binding <view model property>}"
    </i:Interaction.Behaviours>
</ContentControl>
```

##Naming conventions

- "<Name>View"
    * Only name it like this if there is a corresponding `ViewModel`
    * Can suffix with _Window_, _Page_, _Dialog_ etc.
- `ViewModels`:
    - if `View` name ends in "_View_", suffix with "_Model_"
	- if `View` name doesn't end in "_ViewModel_" suffix with "_ViewModel_"
- `Model`s are mostly POCOs (entities) and should be named to reflect the domain objects

Pair `View`s and `ViewModel`s in folders by _feature_.

##Implementation guidelines

- Have a base class for `ViewModel`s, to implement `INotifypropertyChanges`
    * use `CallerMemberName` attribute to determine changed property name
	* compare previous value with new value before triggering change notification
- Navigation using _Event Aggregator_, _Strategy_  design patterns
    1. Source `ViewModel` raises a NavigationPreparationEvent which is a _Strategy_ (strategy is decided by this `ViewModel`)
	2. A **single** _event subscriber_ `ViewModel` should intercept this event (and read the data and store it for later use)
	3. Latter `ViewModel` should raise a NavitagionRequestEvent
	4. Main `ViewModel` (or the navigation root) should intercept this event and make the transition
	
 - Scaffold from Data Sources window in Visual Studio for add/edit templates
 - `CollectionView` represents a view for grouping, sorting, filtering, and navigating a data collection.
 - `ColelctionViewSource` is the (design time) XAML proxy of a `CollectionView`
 - Converter instances can be defined as resources (and properties be added to them for more flexibility)
     ```xaml
	 <converters:NegatableCustomConverter x:Key="CustomConverter"/>
	 <converters:NegatableCustomConverter x:Key="NegatedCustomConverter" Negate="True"/>	 
	 ```
 - Validation with:
     * IDataErrorInfo
	 * ValidationRules
	 * **INotifyDataErrorInfo** introduced in Framework 4.5: Can query the object for validation errors, async validation, more than one error associated on a property
	 ```C#
	 //Method to be called on each property change (after NotifyPropertyChanged)
	 void ValidateProperty<T>(string propertyname, T value)
	 {
	     var results = new List<ValidationResult>();
		 var context = new ValidationContext(this);
		 context.MemberName = propertyName;
		 
		 //Validate based on DataAnnotation attributes defined in `Model`
		 Validator.TryValidateProperty(value, context, results);
		 
		 if(results.Any())
		 {
		     _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
		 }
		 else
		 {
		     _errors.Remove(propertyName);
		 }
		 
		 ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
	 }
	 
	 //define DataAnnotation attrinutes inside model
	 [Required]
	 [EmailAddress]
	 [Phone]
	 ```
	 ```XAML
	 <TextBox Text="Binding FirstName, ValidatesOnNotifyDataErrors=True}"/>
	 ```
	     - To disable the command implement the CanExecute() method of the command to look at validation errors if any (on model property changed, ideally)
		 - To define a style for errors:
		 ```XAML
		 <Style TargetType="<Validated control type, don't forget x:Type if it is custom>"
		     <Style.Triggers>
			     <Trigger Property="Validation.HasError" Value="true">
				     <Setter Property="ToolTip" Value="{Binding RelativeSource={x:Static RelativeSource.Self}, Path=(Validation.Errors).CurrentItem.ErrorContent}"/>
			     </Trigger>
		     </Style.Triggers>
		 </Style>
		 ```

- Dependency Injection/IoC Containers
    * Responsible for:
        - Constructing an object 
		- Determine and constructing its dependencies
	* WindsorCastle, Unity, AutoFac, Ninject, StructureMap
- Automapper for copying object data

##MVVM Toolkits

- Prism
    * MVVM pattern support
	* Bindable base class
	* ViewModel locator
	* Modularity
	* UI Composition/Regions
	* Navigation + Navigation Stack
	* Commands
	* Publisher/Subscriber events (loosely coupled, much in the spirit of Event Aggregator design pattern)
	
- MVVM Light
- Caliburn Micro

		