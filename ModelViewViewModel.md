###Model View ViewModel

_Architecture_ pattern

> Variation of MVC pattern tailored for XAML based applications, leveraging Binding for abstracting and separating concerns

####Components

* **Model** Contract of the data container
* **View** Data presenter
* **ViewModel** Handles the `model`s and view actions on it (commands or events)
* **Binder** Leverages WPF binding capabilities to present data and route commands

####Similar patterns

* **Model View Presenter** Both derive from MVC, MVP communicate with interface through a set of contracts
* **Model View Controller** Inherited pattern, however the `controller` from MVC plays an _active_ role in coordinating and directing UI interaction, while presenter (`viewModel`) is much more _passive_
* **Presentation Model** Only difference is MVVM has _databinding_ capabilities

####Related

* **[Command](Command.md)** used to route events to the presenter
* **[Abstract Factory](AbstractFactory.md)** used to instantiate `view`s/`viewModel`s and inject the dependencies accordingly

