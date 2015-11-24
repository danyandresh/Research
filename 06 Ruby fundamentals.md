#Ruby fundamentals

- thoroughly object oriented
- dynamic typing and duck typing
- multi-paradigm
- reflection
- metaprogramming
- bytecode interpreted:
    * MRI - Methods Ruby Interpreter
    * jRuby - built on top of jvm
    * MacRuby - jit compilation and ahead of time compilation

##Installing ruby
rubyinstaller.org/downloads
```cmd
ruby -v
```
`rvm` on linux

##Interactive Shell

`irb` - starts Interactive Ruby Shell, REPL

`x.class` -> type of the object

`String.public_method.sort`

```ruby
irb(main):007:0> def double(val); val * 2; end
=> :double
irb(main):008:0> double(10)
=> 20
irb(main):009:0>  double("abc")
=> "abcabc"
irb(main):010:0> double([1,2,3])
=> [1, 2, 3, 1, 2, 3]
irb(main):011:0> double(_)
=> [1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3]
irb(main):012:0>
```

##IDE Options
- RubyMine
- Aptana Studio

##variables, nil, methods and scope

variables start with lower case letter and use underscores to make them more readable

variables in inner scope will shadow variables in outer scope

`nil` is a special class that signifies no value
```ruby
result = nil

puts nil.class

puts result.nil?
```

methods can end with a question mark (to indicate a true/false test) or and exclamation mark (to indicate execution can do something slightly unexpected or dangerous)

```ruby
a = " xyz "

puts a.strip

puts a

puts a.strip!

puts a
```

methods
- are defined with each statement on a single line
- are ended with keyword `end`
- can return values of different types
- implicitly return the evaluation of last expression
- can use `return` explicitly for flow control
- typically code is indented with two spaces
- methods arguments are enclosed in parenthesis but that is optional
- methods provide scope for variables


```ruby
def double(val)
  val * 2
end



puts double("abc")
``` 

by default variables scope is _local_

_global_ variables can be defined with `$` prefix

ruby evaluates the last statement in each `if` branch

```ruby
message = if count > 10
  "bigger than ten"
else
  "smaller than ten"
end

puts message
```

`**` - exponentiation operator

`a+=b` -> `a= a + b`

`+=`, `-=`, `*=`, `/=`, `%=`, `**=`, `&=`, `|=`, `^=`, `>>=`, `<<=`

`#` starts a comment on line

###useful methods
`print`

`gets`

`puts` - adds a new line to the string

`` `time /t` `` - backticks delimits a command to execute; equivalent to `%x(time /t)`

`system` - returns false if the command result is zero, false otherwise and `nil` if the command failed

```ruby
res = system "time /t"
puts res
```

##Classes and objects
```ruby
class Spaceship
end
```

classes start with upper case letter

camelcase

acronyms with uppercase letters

call constructor through `new` method
```ruby
ship = Spaceship.new

```

`object_id` - unique object identifier

`clone` - shallow copy

instance variables are denoted with `@`

can inspect the state of an object using `inspect` method or using `p <object>`

instance variables of an object are private

methods are public by default

accessors to provide access to an instance variable

- `attr_accessor` (read and write)
- `attr_reader`
- `attr_writer`
```ruby
class Spaceship
    attr_accessor :destination, :name
    attr_reader :name
    attr_writer :name
end
```

`:destination` is a _symbol_

inside method `self.destination` must be used to refer to the accessor

definition of a class that uses accessors defines behind the scenes _getter_ and _setter_ methods

```ruby
class Spaceship
    def destination
        @destination
    end
    
    def destination=(new_destination)
        @destination = new_destination
    end 
end
```

the variable `@destination` can be changed to store the destination in a different variable (e.g. in an instance variable of another object) without needing to change the calling code (careful here with the Law of Demeter, or principle of least knowledge); this becomes a _virtual attribute_

###initializing the class
requires the defining of `initialize` method along with the needed arguments
```ruby
def initialize(name, cargo_count)
    @name = name
    @cargo_size = CargoSize.new(cargo_count)
    @autonomy = 500
end


ship = Spaceship.new("Falcon 9", 13150)
```

Ruby's garbage collector takes care of destroying objects

###inheritance

multiple inheritance is not allowed

`<` specifies which class the current inherits from

method can be overridden (even changing the arguments)

an overriding method can call super method through `super`

```ruby
class Probe
    def deploy(deploy_time, return_time)
        # deploy the probe
    end
    
    def take_sample
        # do generic sampling
    end
end

class MineralProbe < Probe
    def deploy(deploy_time)
        puts "Preparing diamond drill"
        super(deploy_time, deploy_time + 2 * 3600)
    end
    
    def take_sample
        # take a mineral sample
    end
end
```

role of inheritance is to reuse functionality not to impose interfaces

###class methods

```ruby
def self.class_method
    #constant
    23
end

puts Spaceship.class_method
```

###class variables
defined with double `@@`

defined inside the body of the class

need accessors to expose it publicly

shared between class definitions (!!)

###instance class variables
```ruby
class Spaceship
    @thrust_count = 2
    
    def self.thrust_count
        @thrust_count
    end
end

class LargerSpaceship
    @thrust_count = 4    
end

puts Spaceship.thrust_count
=> 2

puts LargerSpaceship.thrust_count
=> 4
```

###method visibility
- _public_, by default
- _private_: 
    * state `private :method_name` _after_ defining the method
    * type private before a group of methods definition, _all_ methods following that keyword will be private
    * rule is that a private method can't be called with an explicit object receiver
    * class methods (define with `self.`) cannot be made private with `private :<method_name>` (no error, but it does nothing); `private_class_method :<method_name>` must be used instead
- _protected_ method 
    * can be called from another object of the same class, or from a descendant
    * marked with `protected :<method_name>`
    
###executable class bodies

```ruby
result = class Spaceship
    answer = 7 * 6
    puts answer.to_s
    answer
end
```
###self
refers to the current execution context

###open classes
classes with multiple definitions do not override each other, but extend existing definition

this allows methods to be overridden (along with access modifiers, which can be changed) - _Monkey Patching_

can extend/change classes from the library

to be used judiciously, as it can introduce surprising behaviour

###equality
`==` - equality of content
`.equals?` - reference equality

```ruby
def ==(other)
    name == other.name
end
```
##Flow control
###branching
- `if` can be written on a single line
- `elsif` 
- only _false_ and _nil_ evaluate to false
_ everything else is true: _true_, 0, empty string, empty array etc.


####`unless condition` equivalent to `if not condition`
```ruby
unless fuel_level < 25
    launch
end

launch unless fuel_level < 25
```

####ternary operator
```ruby
can_launch? ? launch : wait
```

###conditional initialization
`||=` - only executes if the first operand doesn't exist or evaluated to false

###flow of control
- `and` and `or` have a much lower precedence than `&&` and `||`
- `&&` has higher precedence than `||`; `and` and `or` have the _same_ precedence

- `and` executes a chain until one operand returns nil or false
- `or` used to construct a series of fallback operations
```ruby
lander = Lander.locate(lander_id) and lander.recall
#equivalend to
lander = Lander.locate(lander_id)
lander.recall if lander

if engine.cut_out?
    engine.restart or enable_emergency_power
end

#equivalent to
if engine.cut_out?
    enable_emergency_power unless engine.restart
end
```

###case
```ruby
case distance
when "far" then 10
when "close" then 5
when "closer" then 1
else 0
end
```
- fallthrough
- can be used within an assignment statement
- uses triple equal operator to determine if an object is an instance of a class (when )
- can be used without specifying an object to the `case`; then each `when` has to specify its own clause

###looping
####`while`
```ruby
while condition?
    execute_operation
end

while condition? do execute_operation end

execute_operation while condition?
```
####`until`
```ruby
until condition_is_fulfilled?
    execute_operation
end

until condition_is_fulfilled? do execute_operation end

execute_operation until condition_is_fulfilled?
```

####`begin`/`end` - with `while` and `until`
```ruby
begin
    execute_operation1
    execute_operation2
end while condition?

begin
    execute_operation1
    execute_operation2
end until condition_is_fulfilled?
```

####`for`
```ruby
for variable in [3, 2, 1]
    execute_operation_with(variable)
end
```
`(1..10)` range from 1 to 10

####iterators and blocks
```ruby
[1, 2, 3].each do
    operation
end

[1, 2, 3].each { |variable| operation_with(variable) }

loop do
    operation
end

10.upto(20) { |i| puts i }
20.downto(10) { |i| puts i }
3.times { puts "message" }
1.step(10, 2) { |i| puts i }
```

###controlling loop flow
####`next`
starts the next iteration of the loop without finishing the current iteration
```ruby
while condition
    next if condition1
    operation
end
```
####`break`
exits the loop terminating it
```ruby
while condition
    break if condition1
    operation
end

result = while condition
            break value_to_return if condition1
            operation
        end
```
####`redo`
repeats the iteration without re-evaluating loop condition
```ruby
while condition
    operation
    redo if condition1 #will move execution to the start of the loop without re-evaluating the loop condition    
end
```

###handling exceptions
```ruby
def launch
    begin
        operation
    rescue
        handle_exception
        return false
    end
    other_operation
end

def launch
    operation
    true
rescue
    handle_exception
    false
end

def launch
    operation
    true
rescue LightError => e
    puts e.message
    puts e.backtrace
    false
rescue StandardError => e
    puts e.message
    puts e.backtrace
    false
end
```
- exception clauses are evaluated in order
- subclasses match to superclasses

###raise exceptions
```ruby
# ...
raise "Error raised" # RuntimeError
# ...
raise CustomError, "Error raised" # CustomError
# ...


rescue CustomError => err
    puts $!.message # $! refers to the same object as err does
    raise #raises err further
```
###ensure and else clauses
`ensure` - will execute any final statements before return (cleaning up); come after all `rescue` - must not return explicitly, as that will swallow any exception rethrown

`else` - code executed if the codeblock didn't throw any exceptions; must come before `ensure` and after all `rescues`

###`retry` and `rescue` modifier
`retry` - moves the execution to the beginning of the `begin`/`end` or the method block

`rescue` modifier works like if and while

```ruby
operation_likely_to_throw_exception rescue value_to_return_on_exception_caught
```

###`throw`/`catch`
`catch` takes a codeblock as an argument

`throw` can move the execution out of the catch block, having the same _symbol_ as catch defined

`catch` returns the second parameter of `throw` if provided, `nil` otherwise

a mechanism for breaking loops

```ruby
result = catch :symbol do
    iterable.each do |item|
        while condition
            throw :symbol, "Condition2 met" if condition2
        end
    end
    
    value_to_return_normally_no_catch
end
```

`throw`/`catch` works across method calls

```ruby
def handle()
    throw :symbol, "Condition2 met" if condition2
end
result = catch :symbol do
    iterable.each do |item|
        while condition
            handle
        end
    end
    
    value_to_return_normally_no_catch
end
```

similar to `goto`

###scope
- classes and methods introduce scopes
- if, begine/end, loops don't (variables defined within these constructs will still be in scope afterwards)
- `{}` blocks do introduce scopes (variables defined inside these scopes won't be visible outside)

