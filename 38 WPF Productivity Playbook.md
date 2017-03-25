#WPF Productivity Playbook
https://github.com/briannoyes/wpfproductivityplaybook

"PART_" - a convention in most WPF controls used to identify elements that the control logic will get a reference to and use to drive the behaviour of the control

template bindings pull down whatever property value is set on the control itself and use that value to drive the corresponding property on the element within the visual tree of the control

##object tree
is a tree structure created (when loading XAML) based on the nesting relationships of the elements in the markup, or based on property values for properties that implement the content model for objects (when defined in code)

###logical tree
elements of a WPF user interface are logically related; it describes the relations between elements of the user interface

- inherit `DependencyProperty` values
- resolve `DynamicResource`s references
- look up element names for bindings
- forward routed events
###visual tree
the template of an element consists of multiple visual elements; contains all logical tree elements plus the visual elements of the template of each element

- rendering visual elements
- propagate element opacity
- propagate layout/render transforms
- propagate the `IsEnabled` property
- do hit-testing
- `RelativeSource` (FindAncestor)

```programatically find and ancestor  in the visual tree
public static class VisualTreeHelperExtensions
{
    public static T FindAncestor<T>(DependencyObject dependencyObject) where T: class
    {
        DependencyObject target = dependencyObject;
        do
        {
            target = VisualTreeHelper.GetParent(target);
        }
        while(target != null && !(target is T));
        
        return target as T;
    }
}
```

##dependency object
base class for all of the layout controls; 
##dependency properties
provide additional functionality over standard CLR properties; they can be defined on any control that subclasses `DependencyObject` (as they are using `GetValue()`, `SetValue()` methods defined by it)

in contrast to normal CLR properties (which access the value directly), dependency properties **resolve dynamically** their value (using `GetValue()`). Values of dependency properties are stored in a dictionary of keys and values in Dependency Object.

* reduced memory footprint: the object instance only store non-default values for properties; the default values are stored once within the dependency property

* value inheritance; the value resolution strategy consists of: 
    1. animation
    2. binding expression
    3. local value
    4. custom style trigger
    5. custom template trigger
    6. custom style setter
    7. default style trigger
    8. default style setter
    9. inherited value
    10. default value
    
if no logical value is set it will search the logical tree for an inherited value
    
* change notification; dps have a build in change notification mechanism. can register a callback in the property metadata - used by the data binding.

###attached properties
allow to attach a value to an object that knows nothing about this value

are defined by the control that needs the data from another control in a specific context.

```xaml
<Canvas>
    <Button Canvas.Top="20" Canvas.Left="20" Content="Click me!"/>
</Canvas>
```

```c#
public static readonly DependencyProperty TopProperty =
    DependencyProperty.RegisterAttached("Top", 
    typeof(double), typeof(Canvas),
    new FrameworkPropertyMetadata(0d,
        FrameworkPropertyMetadataOptions.Inherits));
 
public static void SetTop(UIElement element, double value)
{
    element.SetValue(TopProperty, value);
}
 
public static double GetTop(UIElement element)
{
    return (double)element.GetValue(TopProperty);
}
```

###listen to dependency property changes
use `DependencyPropertyDescriptor`
```c#
DependencyPropertyDescriptor textDescr = DependencyPropertyDescriptor.
    FromProperty(TextBox.TextProperty, typeof(TextBox));
 
if (textDescr!= null)
{
    textDescr.AddValueChanged(myTextBox, delegate
    {
        // Add your propery changed logic here...
    });
} 
``` 

###delete local value of a dependency property

```
button1.ClearValue(Button.ContentProperty)
```

or use `DependencyProperty.UnsetValue`

## routed events
events that navigate up or down the visual tree according to their `RoutingStrategy`; it can bubble, tunnel or direct; can hook up event handlers to an element's event or other elements by using attached event syntax `Button.Click="Button_Click"`

routed event normally come as pairs: `PreviewMouseDown` and `MouseDown`. to stop the propagation of a routed event must call `e.Handled`;

* tunneling - the event is raised on the root element and navigates down the visual tree unti it reaches the source element or until the tunneling is stopped by marking the event as handled. naming convention `Preview...`

* bubbling - the event is raised on the source element and navigates up to the visual tree until it reaches the root element or until the bubbling is stopped by marking the event as handled. this event is raised afther the tunnling event.

* direct - the event is raised on the source element and must be handled on the source element itself (normal .net event behaviour)

```c#
// Register the routed event
public static readonly RoutedEvent SelectedEvent = 
    EventManager.RegisterRoutedEvent( "Selected", RoutingStrategy.Bubble, 
    typeof(RoutedEventHandler), typeof(MyCustomControl));
 
// .NET wrapper
public event RoutedEventHandler Selected
{
    add { AddHandler(SelectedEvent, value); } 
    remove { RemoveHandler(SelectedEvent, value); }
}
 
// Raise the routed event "selected"
RaiseEvent(new RoutedEventArgs(MyCustomControl.SelectedEvent));
```

##XAML
language based on XML to create and initialize .NET objects with hierarchical relations.

markup extensions:
###binding
to bind the values of two properties together

###`Static Resource`
one time lookup of a resource entry

will be resolved and assigned to the property during the loading of the XAML which occurs before the application is actually run. It will only be assigned once and any changes to resource dictionary ignored.

###`Dynamic Resource`
auto updating lookup of a resource entry

 assigns an Expression object to the property during loading but does not actually lookup the resource until runtime when the Expression object is asked for the value. This defers looking up the resource until it is needed at runtime. A good example would be a forward reference to a resource defined later on in the XAML. Another example is a resource that will not even exist until runtime. It will update the target if the source resource dictionary is changed

###template binding
to bind a property of a control template to a dependency property of the control

###x:Static
resolve the value of a static property

###x:Null
return null

##namespaces
`xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"` - it is mapped to all wpf controls in System.Windows.Controls

`xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"` it is mapped to `System.Windows.Markup` that defines the XAML keywords

## `IsEnabled` property

## hit testing


