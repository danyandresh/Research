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


