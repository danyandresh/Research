###MVVM

Achieve [separation of concerns](https://en.wikipedia.org/wiki/Separation_of_concerns) in WPF/XAML applications

#####Benefits

- maintainability
- testability
- extensibility

#####Related to

- MVC
- MVP
- MVVM

#####Used for

- WPF
- Silverlight
- Windows 8/ WinRT
- HTML 5 (Knowckout/Angular)
- Xamarin/Mobile Apps
- Windows 10 (XAML + WinRT)

#####Responsibilities

- View
- ViewModel
- Model
- Client Services/Repositories

######Model responsibilities

- Contain data
- Relationships between model objects
- Computed properties
- Raise change notifications (INotifyPropertyChanged.PropertyChanged)
- Validation (INotifyDataErrorInfo/IDataErrorInfo)

######View responsibilities

- Structural definition of what the user sees
- **No code behind**
- Code used purely for UI transformations/interaction/reaction (with other UI elements)
 
######ViewModel responsibilities

- Expose data to the view for presentation
- Encapsulate interaction logic:
	* Calls to the business/data layer/service
	* Navigation logic
	* State transformation logic

- _Expose_ model objects
- _wrapped_ model properties (to apply transformation or simplify them for display)
- _client state_ (observable)

######Client Services/Repositories

- Shared functionality or data access
- Consumed by one or more `ViewModel`s
- Decouples `ViewModel`s from external dependencies
	* Data storage
	* Service access
	* Client environment
- Can act as data caching container

#####Fundamental to MVVM

```XAML
View.DataContext=ViewModel
```

#####View-ViewModel construction

1. View-First
2. ViewModel-First


