####Python

	- General purpose language
	- Interpreted language
	- Common code layout of all python programs
	- CPython (written in C)
	- Jython (Java)
	- IronPython (.NET)
	- pypy (ruby python, rpython)
	- _batteries included_ approach, can be used out of the box, directly
	
#####Code rules

######PEP8

1. Prefer **four spaces**
2. Never mix **spaces and tabs**
3. Code lines indentation *consistency**
4. Improve **readability** by breaking long lines

######Zen of `python`
```python
import this
```

#####Importing libraries
```python
import math
```
```python
math.sqrt(9)
help(math)
help(math.factorial)
n=5
k=3
math.factorial(n)/(math.factorial(k)*math.factorial(n-k))
```

Shortening imports

```python
from math import factorial
factorial(n)/(factorial(k)*factorial(n-k))
```

```python
from math import factorial as fac
fac(n)/(fac(k)*fac(n-k))
```

Integer division operator `//`

```python
len(str(fac(n)))

```

#####Python built in types

	- int
	- float
	- NoneType
	- bool
	
######int

- integer `10`
- binary `0b10`
- octal `0o10`
- hexadecimal `0x10`
- int(3.5)
- int("12112", 3)
	
######float

- `3e8`
- `1.616e-35`
- `float("nan")`
- `float("inf")`
- `float(-inf)`

######None

- absence of a value

```python
None
a = None
a is None
```

######bool

```python
True
False			
bool(0)			#Flase
bool(13)		#True
bool(-0.15)		#True
bool([])		#Flase
bool([1, 3, 5])	#True
bool("")		#Flase
bool("Test") 	#True
bool("False") 	#this returns True!
```

#####Relational operators

* `==` equivalence
* `!=`
* `<`
* `>`
* `<=`
* `>=`

#####Conditional statements

```python
if expr:
    print("expr is True")
	
if "expr":
    print("No bool contructor needed, as it is called implicitly")
	
if False:
    print("If branch")
else:
    print("Example of else branch")
	
```

`elif` - combined `else`+`if`

#####while loops

```python
while expr:
    print("loop while expr is True")
	
c=5
while c>0:
    print(c)
	c -= 1
	
while True:
    response = input()
    if int(response) % 2 == 0:
        break

```
