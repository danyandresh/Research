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

`String.public_methods.sort`

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

##standard types
###booleans
`true` - instance of `TrueClass` (singleton)

`false` - instance of `FalseClass` (singleton)

these classes inherit from `Object` (therefore they expose `to_s`, `nil?`)

###numbers
####integers
`Fixnum` and `Bignum` inherit from `Integer`

- `0x` - hexadecimal
- `0`, `0o` - octadecimal
- `0d` or no prexif - decimal
- `0b` - binary

`_` can be used to group digits, ruby ignores them
####floating point numbers
`Float`, 15 digits of precision

one digit required before point and one after (to avoid ambiguity with method calls)

numbers are readonly objects

###strings
default strings encoding is `UTF-8`

default encoding can be change by specifying `# encoding: <new encoding>` on the first line (or second, if the first is a _shebang_)

new instances of strings are created when passed as method arguments

can escape with `\` (backslash)

can be defined with `'`, `"` `%q<delimiter>string here<delimiter>` (where `<delimiter>` can be any non-alphanumeric character;if the character comes in pairs like `<>`, `{}`, `[]`,`()` it must be used in pairs)

```ruby
%q('test' text)
%q['test' text]
%q{'test' text}
%q<'test' text>

%q*'test' text*
%q/'test' text/
```

can use `\<number>`(octal) and `\x<number>` (hex) to insert chars by code

can use `\u<number>`, `\u{number, number, number, number, ...}` to insert unicode codepoints

double quotes allow _interpolation_
```ruby
result = 1000
puts "Box count: #{result}"
puts "Box count: #{result + 10}"
```

can use `%Q<delimiter>raw string<delimiter>` (as with `%q`) as well as `%<delimiter>raw string<delimiter>`

strings separated by a space a concatenated into a single one

multiple line string supported by _heredocs_: starting with `<<-EOS`(on new line) and ended with `EOS` (on new line)

###string operators and methods
####`[]` indexer
`[index]` - returns element at _index_

`[index, char_count]` - returns a _char_count_ sequence from specified _index_

`["string sequence"]` - return the "string sequence" sequence if found in the string; `nil` otherwise

`["string sequence"] = "new string sequence"` will replace the **first** string sequence with new string sequence

####formatters
`"%05d" %123` - padding of string '123' with to more zeros (to get 5 digits)

`"%.8g" % 123.456567678789` - limits the total number of digits to 8

`.chars` - returns array of chars in the string

`.codepoints` - array character codes

`.bytes` - array of bytes

`.upcase` - returns uppercase (has the interpolating version too)

`.downcase` - returns lowercase (has the interpolating version too)

`.gsub("character to replace", "character to replace with")`

`.split` - splits string (by default by space)

###regular expressions
`Regexp` - standard builtin type

define regexp using `/<regular expression here>/` or `%r(<regular expression here>)`

`=~` - test match on a string; returns the position (if matched) or `nil` (if not)

`!~` - test if the regex _doesn't_ match; it only returns true or false

`.match` - method on string and regex to get a matching object

variables set on match:
- `$` - pre-match
- `$'` - post-match
- `$&` - the match
- `$1`, `$2` - submatches (groups)

`scan` - an array of match substrings

###symbols
a special class of objects similar to enums

are created using `:` and a string literal (supports interpolation too)
```ruby
direction = "west"
:"turn_#{direction}" # :turn_west
```

`.to_s` - converts to symbol

`.to_sym` - converst a string to symbol

usage of symbols is more efficient than plain strings from memory usage perspective

###arrays
`Array`

`[]`, `[1, 2, 3]` - will create a new array

`Array.new(element_count, default_value)` - new array with the same instance of default value on all positions

`Array.new(element_count) { default_value }` - new array with **different instances** of default value as elements

`%w(<string>)` - creates an array of words by splitting the string by space; can escape a space to exempt it from splitting

`%W(<string>)` - get the behaviour of double string quotes (including interpolation)

`%i(<symbols text>)` - produce array of symbols

`[<index>]` - provides indexing (including _negative_ indexing)

`[start_position..end_position]` - slicing capability; values can be assigned too

`<< new value` - will push new value at the end of array

`+` - merge arrays

`*` - duplicate arrays; if used with string as second operator will join elements into a single string

`-` - removes elements in the second array from the first array

###enumerable

`map` - apply transformation to each element of the collection

`reduce` - aggregates elements

`sort`

`select` - filter elements

`each_cons` - iterate over subsets of elements

```ruby
a = [1, 2, 3]

a.map { |v| v * 10 }
=> [10, 20, 30]

a.reduce(0) { |sum, v| sum + v }
=> 6

a.select { |n| n.even? }

a.each_cons(2) { |v| p v }
```

###hashes
`Hash` class

_ordered_ (in the order they were added to the hash) key/value pairs, unique keys

```ruby
> h = { a: "a", b: "b" }

> h.each { |k, v| puts "#{k}: #{v}"}
a: a
b: b
```

###ranges
`a..b` - `[a, b]`

`a...b` - `[a, b)`

`begin` - a

`end` 

`include?` - test if an object is in the range

can have ranges of integers, strings, floats(iteration is not allowed on these)

can be used in a `case` statement
```ruby
case number
    when 0..100 then "below 100"
    when 101..150 then "between 101 and 150"
    else "above 150"
end
```

###parallel assignment and the splat operator
parallel assignment unwraps the values to variables so can have `a, b = 1, 2`

combines multiple variables into an array so `a = 1, 2, 3, 4, 5` turns a into an array: `a = [1, 2, 3, 4, 5]`

discard values so `a, b = 1, 2, 3, 4` will discard 3 and 4

can use `_` as _dummy_ variable: `a, _, _, b = 1, 2, 3, 4` will discard 2 and 3

`*` - splat operator, accumulates elements (greedy)
```ruby
> a, *b = 1, 2, 3, 4
> b
=> [2, 3, 4]

> a, *b, c = 1, 2, 3, 4
> b
=> [2, 3]

> a, b, c = *1..3

> a
=> 1
> c
=> 3

> first, _, _, _, last = 1, 2, *[3, 4, 5]
> last
=> 5

> first, *, last = 1, 2, *[3, 4, 5]
> last
=> 5
```

splat operator also converts a range to an array

```ruby
> r = (3..5)
> [1, 2, *r]
=> [1, 2, 3, 4, 5]
```

splat operator works on any class that has `to_a` method

```ruby
> h = { a: "a", b: "b" }
> [*h]
=> [[:a, "a"], [:b, "b"]]
```

##methods
- methods with the same name and different formal parameters are not allowed

###default param values (optional params)
- are calculated at the point of method call (not definition)

- can call other methods to calculate default value

- optional params doesn't have to come last in the definition

- cannot mix optional and required parameters

###variable length param list
use _splat_ operator in method definition to declare an _array parameter_ (and works like the parallel assignment)

```ruby
def m(p1, *p2)
end

m(v1, v2, v3, v4) # p2 == [v2, v3, v4]
```

optional parameters have to come before array parameter

optional parameters can't be omitted anymore when array parameters are used 

can use splat operator when calling a method (to call `to_a` method on the object)

###keyword arguments
are defined using `:` instead of `=`; they have to have a default value

can coexist with regular params, but regular params come first

```ruby
def keyword_args(p3= 1, p1: :a, p2: :b)
    puts "Keyword args " + p3.to_s
end

keyword_args(5, p2: 2)
```

`**` - _double splat_ will collect any keyword arguments that aren't specified in the definition

```ruby
def keyword_double_splat_args(p3= 1, p1: :a, **p2)
    puts "Double splat: "
    p2.each { |k, v| puts "k: #{k} v: #{v}" }
end

keyword_double_splat_args(5, p2: 2, p4: 4, p5: 5, p6: 6)

h = {a: :a, b: :b}
keyword_double_splat_args(6, h)
```

###method aliasing
`alias_method` creates copy of a method under a different name so it can be accessed under a different name when overridden

```ruby
class String

    alias_method "original_size" "size"
    
    def size
        original_size * 5
    end
end
```

###operators
all operators are methods except:

* _logical operators_: `&&`, `||`, `not`, `and`, `or`, `?:`
* _assignment operators_: `=`, `+=`, `-=`, `*=`, `/=`, `%=`, `**=`, `&=`, `|=`, `^=`, `>>=`, `<<=`, `&&=`, `||=`

```ruby
class Operators
    attr_reader :capacity
    
    def initialize()
        @h = {}
        @capacity = 0
    end
    
    def [](index)
        @h[index]
    end
    
    def []=(index, value)
        @h[index] = value
    end
    
    def <=>(other)
        h <=> other.h
    end
    
    def +(capacity)
        @capacity += capacity
    end
    
    def +@
        @capacity += 1
    end
    
    def !
        puts "Hash destructed"
    end
end

op1 = Operators.new
op1[:d] = :c
puts op1[:d]

puts "Capacity #{op1.capacity}"
op1 + 10
puts "Capacity #{op1.capacity}"

+op1
puts "Capacity #{op1.capacity}"

if !op1
    puts "Operators class destructed"
end
```

###method calls as messages

can use `send` (or its alias `__send__`) to call a method

```ruby
class ReceiverObject
    def m1
        puts "Method 1 called"
    end
    
    def m2
        puts "Method 2 called"
    end
end

h = {method1: :m1, method2: :m2}
method = :method1
receiver = ReceiverObject.new
receiver.__send__(h[method])
method = :method2
receiver.__send__(h[method])
```

outside of any context `self` refers to main

no _free_ methods (no methods that do not belong to an object)

###method_missing
when a an invoked method is not found on the object a `method_missing` method is called, that raises `NoMethodError`

`responds_to?` - determine if the object has a particular method

`const_missing` - equivalent to `method_missing` but for constants

can add/remove method at runtime

`inherited` - method executed when a subclass of this class is created

##blocks, constants, modules
###blocks
code between `do` - `end` (multiline blocks) or curly brackets (singleline blocks)

can only pass blocks as an argument to a method

block arguments are defined by `|<arguments>|` at the start of the block

`yield` executes the block passed as parameter to the method

block arguments can have default values, can be keyword arguments, can have array arguments (using splat)

```ruby
def execute_block
    return nil unless block_given?
    
    puts "Yielding to block"
    
    yield
end    

def execute_block_with_args
    return nil unless block_given?
    
    puts "Yielding to block with arguments"
    
    yield :def, :splat1, :splat2, :splat3, p_keyed: :keyed, k1: :v1, k2: :v2, k3: :v3
end    


puts "Calling execute_block with no block to execute"
execute_block
puts "Call complete"

puts "Calling execute_block with block to execute"
execute_block do
    puts "Running block"
end
puts "Call complete"

puts "Calling execute_block with block and args to execute"
execute_block_with_args do |p_default = :a, *p_splat, p_keyed: :b, **p_double_splat_hash|
    puts "Running block"
    puts "Args p_default= #{p_default}"
    puts "Args p_splat= #{p_splat}"
    puts "Args p_keyed= #{p_keyed}"
    puts "Args p_double_splat_hash= #{p_double_splat_hash}"
end
puts "Call complete"
```

###block local variables
block variables shadow the variables defined in outscope

can avoid fix the problem using `;` in block parameters

```ruby
exec_block do |h;h2|
    h2 = {}
    h2[:k5] = :e
end
```

###using blocks
if the block contains a return statement will execute in the context it was defined in, which could no longer exist at the time of block execution

block closures cannot prevent GC collecting the objects while still referred by the block

timing execution

```ruby
def measure_time
    start = Time.now
    if block_given?
        yield
        
        puts "method took #{Time.now - start} seconds"
    end
end
```

transactional blocks
```ruby
def transactionally
    start_transaction
    begin
        yield
    rescue DBError => e
        rollback_transaction
        log_error e.message
        return
    end
    commit_transaction
end
```

block limitations:

- can only pass one block to a method
- blocks can't be passed around between methods
- passing the same block to several methods isn't DRY
- blocks are not objects

###from block to proc
prefixing a parameter with a `&` indicates that parameter is a block, not a regular parameter (this converts the block to a proc - which is an object)

parameter on the callsite must be prefixed as well with `&`

can use `Proc.new {<block>}` or `proc {<block>}` to create a proc

invoking a proc:
- `p.call`
- `p.yield`
- `p.()`
- `p[]`

###lambdas
`lambda` is an instance of the `Proc` class, but behaves differently

`lambda {(params) <block>}` - create a lambda

`->(params) {<block>}` - create a _stabby_ lambda

lambdas are like anonymous methods

are strict about their arguments: too many or too few cause exceptions (unlike blocks and procs, which simply discard if too many or fill with nil if too few)

`return` and `break` behave differently in procs and lambdas
- `return` is executed in the scope defined for a proc
- `break` is not allowed outside a loop (unlike with blocks)
- should not use either in procs, and should avoid `return` in blocks
- `return` and `break` in lambda simply returns control to the calling method

###using lambdas and procs
`arity` - returns the parameter count:
 - the actual count if none of the params are optional
 - negative (the number of non-optional params + 1) if any optional params

procs implement `===` so can be used in `case` statements

```ruby
weekend = proc {|time| time.saturday? || time.sunday?}
weekday = proc {|time| time.wday < 6}

case Time.now
    when weekend then puts "Weekend!"
    when weekday puts puts "Weekday!"
end
```

symbols can be converted into procs
```ruby
n = ["a", "b" , "c"]
upper_n = n.map {|v| v.upcase}
upper_n = n.map {&:upcase}
```

###constants
constant names are all uppercase letters

class names are constants too

```irb
irb(main):001:0> A = 1
=> 1
irb(main):002:0> A = 2
(irb):2: warning: already initialized constant A
(irb):1: warning: previous definition of A was here
=> 2
```

`freeze` - to prevent runtime object changes (produces `RuntimeError`)

can't un-freeze object

`frozen?` - check whether the object is frozen

freeze is shallow

can access constants defined in a class by typing class name and `::` and then the constant name

can add constants to a class or module the same way as accessing them

method can't contain constants
###modules
modules are similar to classes, but can't be instantiated

```ruby
module <ModuleName>
end
```

provide namespacing

method defined a module level must be prefixed with `self.`

classes from modules must use `::` same as constants

```ruby
SomeModule::SomeClass
```

modules can be nested

_mixins_ help defining reusable code that can be mixed into classes later

they contain instance methods (not prefixed with `self.`)
```ruby
module ReuseCodeMixin
    def MethodToReuse
    end
end

class ReusingClass
    include ReuseCodeMixin
end

c = ReusingClass.new
c.MethodToReuse
```

modules are part of the inheritance hierarchy

```ruby
class Class1
    include Module1
    include Module2
end
```
has the following hierarchy: `Class1` inherits `Module2` inherits `Module1` inherits `Object`

can incorporate (instance) module methods as (static) class methods by using `extend` instead of include

can group methods intended to be class methods into a submodule and _extend_ the class with it while _including_ its parent module

alternative to the above
```ruby
module Module1
    module Module2
        def Method2
        end
    end
    
    def self.included(base)
        base.extend(Module2)
    end
    
    def Method1
    end
end    

class Class1
    include Module1 #will call included() from above
end
```

modules support attribute accessor definitions but no initialization (except the cases where methods are assigned to)






