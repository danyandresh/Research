#Python

	- General purpose language
	- Interpreted language
	- Common code layout of all python programs
	- CPython (written in C)
	- Jython (Java)
	- IronPython (.NET)
	- pypy (ruby python, rpython)
	- _batteries included_ approach, can be used out of the box, directly
	
##Code rules

###PEP8

1. Prefer **four spaces**
2. Never mix **spaces and tabs**
3. Code lines indentation *consistency**
4. Improve **readability** by breaking long lines

###Zen of `python`
```python
import this
```

##Importing libraries
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

##Python built in types

	- int
	- float
	- NoneType
	- bool
	
###int

- integer `10`
- binary `0b10`
- octal `0o10`
- hexadecimal `0x10`
- int(3.5)
- int("12112", 3)
	
###float

- `3e8`
- `1.616e-35`
- `float("nan")`
- `float("inf")`
- `float(-inf)`

###None

- absence of a value

```python
None
a = None
a is None
```

###bool

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

##Relational operators

* `==` equivalence
* `!=`
* `<`
* `>`
* `<=`
* `>=`

##Conditional statements

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

##while loops

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

##strings

`str` - immutable sequences of Unicode codepoints

delimited by `'` or `"` and can be interchanged to avoid complicated escaping techniques

!Zen: Practicality beats purity

###adjacent literal strings are **concatenated**
```python
"abc" "cde"
'abccde'
```
###Newlines

1. **Multiline** strings; python 3 has universal newline support for `\n` [PEP-0278](http://www/python.org/dev/peps/pep-0278)
```python
>>> """This
... is
... a
... string
... on
... multiple
... lines
... """
'This\nis\na\nstring\non\nmultiple\nlines\n'

>>> '''Multiple
... lines'''
'Multiple\nlines'

>>> m = 'Multi\nline\example'
>>> m
'Multi\nline\\example'
>>> print(m)
Multi
line\example
```

2. **Escape** sequences; `\` escapes sequences. More on http://docs.python.org/3/reference/lexical_analysis.html#strings
```python
>>> "This line contains a \" (double quote)"
'This line contains a " (double quote)'

>>> 'This line contains a \' (single quote)'
"This line contains a ' (single quote)"

>>> 'This line contains a \' (single quote) and a \" (double quote)'
'This line contains a \' (single quote) and a " (double quote)'

>>> m = 'This line contain a \\ (backslash)'
>>> m
'This line contain a \\ (backslash)'
>>> print(m)
This line contain a \ (backslash)
```

###Raw strings

Start with `r` and don't support any escape sequences

```python
>>> path = r'C:\Users\Daniel\Downloads'
>>> path
'C:\\Users\\Daniel\\Downloads'
>>> print(path)
C:\Users\Daniel\Downloads
```

###string representations of other types

use `str()`

```python
>>> str(123)
'123'
>>> str(3.14e39)
'3.14e+39'
```

###string operations
####query
_Characters_ are _one element_ strings

```python
>>> m = 'string to query'
>>> m[7]
't'
>>> type(m[7])
<class 'str'>
```
####capitalize
```python
>>> o = 'monthy'
>>> o.capitalize()
'Monthy'
>>> o
'monthy'
```

###Encoding

All _code points_ of Unicode can be encoded in python strings, as its default **encoding is UTF-8**
```python
>>> m = 'Can use hexadecimal representation like \u00e6 '
>>> m
'Can use hexadecimal representation like \xe6 '
>>> print(m)
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
  File "C:\Users\Daniel\AppData\Local\Programs\Python\Python35\lib\encodings\cp852.py", line 19, in encode
    return codecs.charmap_encode(input,self.errors,encoding_map)[0]
UnicodeEncodeError: 'charmap' codec can't encode character '\xe6' in position 40: character maps to <undefined>

>>> m = 'Can use escaped octal like \345 '
>>> m
'Can use escaped octal like \xe5 '
>>> print(m)
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
  File "C:\Users\Daniel\AppData\Local\Programs\Python\Python35\lib\encodings\cp852.py", line 19, in encode
    return codecs.charmap_encode(input,self.errors,encoding_map)[0]
UnicodeEncodeError: 'charmap' codec can't encode character '\xe5' in position 27: character maps to <undefined>
```
#!To get back to this printing issue



