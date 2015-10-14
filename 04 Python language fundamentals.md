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

> Zen: Practicality beats purity

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

##bytes

Immutable sequences of bytes that support almost the same operations as strings do

```python
>>> b'data'
b'data'
>>> b"data"
b'data'

>>> m = b'data'
>>> m[0]
100
>>> print(m[0])
100
>>> print(m[1])
97
>>> m.split()
[b'data']
```
Conversion between `str` and `bytes` requires the encoding of `bytes` to be known; [more on python encoding](http://docs.python.org/3/library/codecs.html#standard-encodings)

```python
>>> string = "String with hexadecimal \u00e5"
>>> data = string.encode('utf-8')
>>> convString = data.decode('utf-8')
>>> string == convString
True
>>> convString
'String with hexadecimal \xe5'
>>> string
'String with hexadecimal \xe5'
```

##list

_Mutable_ sequences of objects

```python
>>> [1,'a', 3]
[1, 'a', 3]
>>> l = ['string 1', 'string 2', 'string 3']
>>> lol = [l, l, l]
>>> print(lol)
[['string 1', 'string 2', 'string 3'], ['string 1', 'string 2', 'string 3'], ['string 1', 'string 2', 'string 3']]
>>> l[0]
'string 1'
>>> l[0] = l
>>> print(l)
[[...], 'string 2', 'string 3']
>>> print(l[0])
[[...], 'string 2', 'string 3']
>>> print(l[0][0])
[[...], 'string 2', 'string 3']

>>> [1,'a', 3]
[1, 'a', 3]
>>> l = ['string 1', 'string 2', 'string 3']
>>> lol = [l, l, l]
>>> print(lol)
[['string 1', 'string 2', 'string 3'], ['string 1', 'string 2', 'string 3'], ['string 1', 'string 2', 'string 3']]
>>> l[0]
'string 1'
>>> l[0] = l
>>> print(l)
[[...], 'string 2', 'string 3']
>>> print(l[0])
[[...], 'string 2', 'string 3']
>>> print(l[0][0])
[[...], 'string 2', 'string 3']
>>> l.append(4.55)
>>> l.append(l)
>>> print(l)
[[...], 'string 2', 'string 3', 4.55, [...]]
>>> list('characters')
['c', 'h', 'a', 'r', 'a', 'c', 't', 'e', 'r', 's']
>>> l = ['bear',
...      'bär',
...      'mishka',
...      'urs',]
>>> l
['bear', 'bär', 'mishka', 'urs']
```

##dictionaries

mutable mappings of keys to values

```python
>>> bearTranslations = {'ro': 'urs',
...                     'de': 'bär',
...                     'en': 'bear',}
>>> bearTranslations['ro']
'urs'
>>> bearTranslations['ro'] = 'ursul'
>>> bearTranslations
{'ro': 'ursul', 'en': 'bear', 'de': 'bär'}
>>> bearTranslations['ru'] = 'mishka'
>>> bearTranslations
{'ro': 'ursul', 'en': 'bear', 'ru': 'mishka', 'de': 'bär'}
>>> bearTranslations = {}
>>> bearTranslations
{}
```

entries order cannot be relied upon

##for-loops

visit each item in an iterable series

```python
#with lists iterates elements
>>> l = ['item 1', 'item 2', 'item 3', 'item 4']
>>> for i in l:
...     print(i)
...
item 1
item 2
item 3
item 4

#with dictionaries, iterates keys
>>> d = {'1':'one', '3': 'three', '2': 'two'}
>>> for k in d:
...     print(k, d[k])
...
3 three
2 two
1 one
```

##urlopen sample

- `urlopen` library to deal with web requests
- `with`- statement taking care of resource allocation and deallocation 
- `list`
- `for`-loop
- `bytes.split()`
- `bytes.decode()`
- `str.split()`

```python
>>> from urllib.request import urlopen
>>> with urlopen('http://sixty-north.com/c/t.txt') as story:
...     story_words = []
...     for line in story:
...         line_words = line.split()
...         for word in line_words:
...             story_words.append(word)
...
>>> story_words
[b'It', b'was', b'the', b'best', b'of', b'times', b'it', b'was', b'the', b'worst', b'of', b'times', b'it', b'was', b'the
', b'age', b'of', b'wisdom', b'it', b'was', b'the', b'age', b'of', b'foolishness', b'it', b'was', b'the', b'epoch', b'of
', b'belief', b'it', b'was', b'the', b'epoch', b'of', b'incredulity', b'it', b'was', b'the', b'season', b'of', b'Light',
 b'it', b'was', b'the', b'season', b'of', b'Darkness', b'it', b'was', b'the', b'spring', b'of', b'hope', b'it', b'was',
b'the', b'winter', b'of', b'despair', b'we', b'had', b'everything', b'before', b'us', b'we', b'had', b'nothing', b'befor
e', b'us', b'we', b'were', b'all', b'going', b'direct', b'to', b'Heaven', b'we', b'were', b'all', b'going', b'direct', b
'the', b'other', b'way', b'in', b'short', b'the', b'period', b'was', b'so', b'far', b'like', b'the', b'present', b'perio
d', b'that', b'some', b'of', b'its', b'noisiest', b'authorities', b'insisted', b'on', b'its', b'being', b'received', b'f
or', b'good', b'or', b'for', b'evil', b'in', b'the', b'superlative', b'degree', b'of', b'comparison', b'only']

#this is a list of bytes instead of strings;

#to get strings decode bytes
>>> with urlopen('http://sixty-north.com/c/t.txt') as story:
...     story_words = []
...     for line in story:
...         line_words = line.decode('utf-8').split()
...         for word in line_words:
...             story_words.append(word)
...
>>> story_words
['It', 'was', 'the', 'best', 'of', 'times', 'it', 'was', 'the', 'worst', 'of', 'times', 'it', 'was', 'the', 'age', 'of',
 'wisdom', 'it', 'was', 'the', 'age', 'of', 'foolishness', 'it', 'was', 'the', 'epoch', 'of', 'belief', 'it', 'was', 'th
e', 'epoch', 'of', 'incredulity', 'it', 'was', 'the', 'season', 'of', 'Light', 'it', 'was', 'the', 'season', 'of', 'Dark
ness', 'it', 'was', 'the', 'spring', 'of', 'hope', 'it', 'was', 'the', 'winter', 'of', 'despair', 'we', 'had', 'everythi
ng', 'before', 'us', 'we', 'had', 'nothing', 'before', 'us', 'we', 'were', 'all', 'going', 'direct', 'to', 'Heaven', 'we
', 'were', 'all', 'going', 'direct', 'the', 'other', 'way', 'in', 'short', 'the', 'period', 'was', 'so', 'far', 'like',
'the', 'present', 'period', 'that', 'some', 'of', 'its', 'noisiest', 'authorities', 'insisted', 'on', 'its', 'being', 'r
eceived', 'for', 'good', 'or', 'for', 'evil', 'in', 'the', 'superlative', 'degree', 'of', 'comparison', 'only']
```

##functions

- are defined with `def`,
- are required to return a value
- return early by using `return` with no parameter (the value returned is `None`)
- implicit return (that is not typed) at the end of the function return `None`

```python
>>> def even_or_odd(n):
...     if n % 2 ==0:
...         print('even')
...         return
...     print('odd')
...
>>> even_or_odd(4)
even
>>> even_or_odd(5)
odd
>>> v = even_or_odd(5)
odd
>>> v is None
True
```

importing a module and executing a function
```python
>>> import words
>>> words.fetch_words()
```

importing a specific function
```python
>>> from words import fetch_words
>>> fetch_words()
```

##special attributes

are delimited in Python by double underscores

`__name__` evaluates to `__main__` or the actual module name (when imported)

`__name__` is only executed once on import

```python
#module words ends with line
print(__name__)

#REPL says
>>> import words
words
```
```cmd
PS C:\Users\Daniel\Documents\GitHub\Research\pyfund> python words.py
__main__
```

###determine when module is used from script

```python
#module ends with 
if __name__ == '__main__':
    fetch_words()

#now the module calls the function when executed from script or simply imports when imported into other modules
```

##functions with command line arguments

```python
#words module has three functions now: fetch_words, print_words and main

#import multiple functions from module; parenthesis can break on multiple lines if list of imports is long 
>>> from words import (fetch_words, print_words)

#execute functions
>>> print_words(fetch_words())

#importing all functions from a module is not recommended as import would be beyond control
>>> from words import *

```

###accessing command line arguments

`sys.argv[0]`

```python
#module must import
import sys
#and access the command line arguments
sys.argv[1]
```

module functions should have two blank lines in between

> Zen: Sparse is better than dense

##docstrings

to document the code

module documentation is written on the first line
function documentation is written on the first line after function header

docstring conventions in PEP 257

tools for docstrings: reStructuredText/Sphinx

```python
#module words has documentation

#on module words.py this function has documentation
def fetch_words(url):
    """ Fetch a list of words from a URL
    
    Args:
        url: The URL of a UTF-8 text document
    
    Returns:
        A list of strings containing the words from the document
    
    """
    with urlopen(url) as story:
        story_words = []
        for line in story:
            line_words = line.decode('utf-8').split()
            for word in line_words:
                story_words.append(word)
    return story_words
```

###accessing the documentation

through `help`

```python
>>> help(fetch_words)
Help on function fetch_words in module words:

fetch_words(url)
    Fetch a list of words from a URL

    Args:
        url: The URL of a UTF-8 text document

    Returns:
        A list of strings containing the words from the document
```

```python                
>>> import words
>>> help(words)
Help on module words:

NAME
    words - Retrieve and print words from a URL

DESCRIPTION
    Usage:

        python words.py <URL>

FUNCTIONS
    fetch_words(url)
        Fetch a list of words from a URL

        Args:
            url: The URL of a UTF-8 text document

        Returns:
            A list of strings containing the words from the document

    main(url)
        Print each word from a text document from a URL

        Args:
            url: The URL of a UTF-8 text document.

    print_items(items)
        Print items one per line

        Args:
            An iterable series of printable items

FILE
    c:\users\daniel\documents\github\research\pyfund\words.py

```

##code comments

code comments are marked with `#` and continue to the end of the line

###`shebang`

the module starts with a `#!/usr/bin/env python3`

* allows the program loaded to determine which interpreter should be used to run the program
* documents for what version of python is the code
* `shebang` works for windows too

```cmd
#the module can then be ran as a script
words.py http://sixty-north.com/c/t.txt
```

More on PEP 397

 ##Objects
 
 All objects are reference objects in Python
 
 `id()`
 
- returns integer constant that is _unique_ and _constant_ for the lifetime of the object
- debugging tool
 
 ```python
 >>> a = 123
>>> id(a)
1713233456
 ```

 `is`
 
 - reference equality checker
 
 ```python
 >>> b = a
>>> a is b
True
>>> a is None
False
 ```
 
 ###Equality
 
 * **Value** equality: equivalent contents - can be controller programatically
 * **Identity** equality: same object reference
 
 ```python
 >>> a = [1, 2, 3]
>>> b = [1, 2, 3]
>>> a == b
True
>>> a is b
False
 ```

##function arguments

are passed by _object reference_

###default function arguments

```python
>>> def banner(message, border='-'):
...     line = border * len(message)
...     print(line)
...     print(message)
...     print(line)
...

>>> banner("Function default argument '-' should be used")
--------------------------------------------
Function default argument '-' should be used
--------------------------------------------

>>> banner("Function custom argument '#' should be used", '#')
###########################################
Function custom argument '#' should be used
###########################################

>>> banner(border='*', message="Function custom argument '*' should be used")
*******************************************
Function custom argument '*' should be used
*******************************************
```

####default arguments evaluation

default arguments are evaluated only once, when the `def` statement is executed

```python
>>> import time
>>> time.ctime()
'Tue Oct 13 17:05:19 2015'

>>> def show_default(arg=time.ctime()):
...     print(arg)
...
>>> show_default()
Tue Oct 13 17:06:13 2015
>>> show_default()
Tue Oct 13 17:06:13 2015

>>> def add_spam(menu=[]):
...     menu.append("spam")
...     return menu
...
>>> breakfast = ['bacon', 'eggs']
>>> add_spam(breakfast)
['bacon', 'eggs', 'spam']
>>> lunch = ['baked beans']
>>> add_spam(lunch)
['baked beans', 'spam']
>>> add_spam()
['spam']
>>> add_spam()
['spam', 'spam']
```

always assign _immutable_ objects as default argument values

```python
>>> def add_spam(menu=None):
...     if menu is None:
...         menu = []
...     menu.append('spam')
...     return menu
...
>>> add_spam()
['spam']
>>> add_spam()
['spam']
```

##type system

- dynamic: object types are only resolved at runtime
```python
>>> def add(a,b):
...     return a + b
...
>>> add(1, 2)
3
>>> add('a', 'b')
'ab'
>>> add([1, 2, 3], [4, 5, 6])
[1, 2, 3, 4, 5, 6]
```
- strong: the is no implicit type conversion
```python
>>> add(23, 'abc')
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
  File "<stdin>", line 2, in add
TypeError: unsupported operand type(s) for +: 'int' and 'str'
```

##variable scoping

_scopes_ are _contexts_ in which named references can be looked up

- `Local` scope: names inside the current function
- `Enclosing` scope: any and all enclosing functions
- `Global` scope: names in top-level module
- `Build-in` scope: provided by the `builtins` module

###change variable scoping

use `global` to change scoping

```python
>>> count = 0
>>> def show_count():
...     print("count = ", count)
...
>>> def set_count(c):
...     count = c
...
>>> show_count()
count =  0
>>> set_count(5)
>>> show_count()
count =  0

#use global to resolve the count to the global scope
>>> def set_count(c):
...     global count
...     count = c
...
>>> set_count(5)
>>> show_count()
count =  5
```

> Zen: Special cases aren't special enough to break the rules

> Patterns are followed not to kill complexity but to master it

in Python everything is an object

- use `type()` to get the type of an object
- use `dir()` to get the attributes of an object

```python
>>> import words
>>> type(words)
<class 'module'>
>>> dir(words)
['__builtins__', '__cached__', '__doc__', '__file__', '__loader__', '__name__', '__package__', '__spec__', 'fetch_words'
, 'main', 'print_items', 'sys', 'urlopen']

>>> type(words.fetch_words)
<class 'function'>
>>> dir(words.fetch_words)
['__annotations__', '__call__', '__class__', '__closure__', '__code__', '__defaults__', '__delattr__', '__dict__', '__di
r__', '__doc__', '__eq__', '__format__', '__ge__', '__get__', '__getattribute__', '__globals__', '__gt__', '__hash__', '
__init__', '__kwdefaults__', '__le__', '__lt__', '__module__', '__name__', '__ne__', '__new__', '__qualname__', '__reduc
e__', '__reduce_ex__', '__repr__', '__setattr__', '__sizeof__', '__str__', '__subclasshook__']
>>> words.fetch_words.__name__
'fetch_words'
>>> words.fetch_words.__doc__
' Fetch a list of words from a URL\n    \n    Args:\n        url: The URL of a UTF-8 text document\n    \n    Returns:\n
        A list of strings containing the words from the document\n    \n    '
```

##tuple

heterogeneous immutable sequence, defined using parenthesis (which in some case are omitted)
```python
>>> t = ("Bear", 5, 2.31)
>>> t
('Bear', 5, 2.31)
>>> t[1]
5
>>> len(t)
3
>>> for i in t:
...     print(i)
...
Bear
5
2.31
>>> t + ('fox', 'frog', '12')
('Bear', 5, 2.31, 'fox', 'frog', '12')
>>> t * 2
('Bear', 5, 2.31, 'Bear', 5, 2.31)
>>> t * 2.2
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
TypeError: can't multiply sequence by non-int of type 'float'
>>> c = ((45, 44.1), (47, 46.4), 'latitude', 'longitude')
>>> c
((45, 44.1), (47, 46.4), 'latitude', 'longitude')
>>> c[0][1]
44.1
>>> t = (123)
>>> type(t)
<class 'int'>
>>> t = (123,)
>>> type(t)
<class 'tuple'>
>>> t = ()
>>> t
()
>>> type(t)
<class 'tuple'>
>>> t = 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
>>> type(t)
<class 'tuple'>
>>> def minmax(items):
...     return min(items), max(items)
...
>>> minmax(t)
(0, 9)
```

###tuple unpacking

can destructure directly into named references

```python
>>> lower, upper = minmax(t)
>>> lower
0
>>> upper
9

>>> (a, (b, (c, d))) = (4, (3, (2, 1)))
>>> print(a, b, c, d)
4 3 2 1

>>> a, b = b, a
>>> print(a, b, c, d)
3 4 2 1
```

###constructor
```python
>>> tuple([0, 1, 2, 3, 4, 5, 6, 7, 8, 9])
(0, 1, 2, 3, 4, 5, 6, 7, 8, 9)
```

###in/not in operators
```python
>>> 5 in (4, 5, 6,)
True
>>> 5 not in (4, 5, 6,)
False
```

##str
homogeneous immutable sequence of Unicode codepoints

###`len()`
```python
>>> len("llanfairpwllgwyngyllgogerychwyrndrobwllllantysiliogogogoch")
58
```

###concatenation
```python
>>> 'New' + 'found' + 'land'
'Newfoundland'
>>> s = 'New'
>>> s += 'found'
>>> s
'Newfound'
>>> s += 'land'
>>> s
'Newfoundland'
```

###`join()`
```python
>>> s = ' '.join(['New', 'found', 'land'])
>>> s
'New found land'
>>> s.split(' ')
['New', 'found', 'land']
```

> Zen: The way may not be obvious at first
> To concatenate invoke join on empty text
> Something for nothing

###`partition()`

`_` is for unused or dummy values

```python
>>> 'unforgetable'.partition('forget')
('un', 'forget', 'able')

>>> departure, separator, arrival = 'London:Edinburgh'.partition(':')
>>> departure
'London'
>>> arrival
'Edinburgh'

>>> departure, _, arrival = 'London to Oxford'.partition(' to ')
>>> departure
'London'
>>> arrival
'Oxford'
```

###`format()`

- replaces `{<number>}` fields with arguments according to their index
- replaces empty `{}` fields in the order they are specified
- replaces keyword `{<name>}` fields with the  named arguments
```python
>>> '{0} ist {1} Jahre alt; Geburtstag von {1} ist auf {2}'.format('Thomas', 30, 'September')
'Thomas ist 30 Jahre alt; Geburtstag von 30 ist auf September'

>>> 'Position is {} {}'.format(44.5, 32.5)
'Position is 44.5 32.5'
>>> 'Position is {longitude} {latitude}'.format(longitude = 44.5,
...                                             latitude = 32.4)
'Position is 44.5 32.4'
```

- access values through keys or indexes with square brackets in the replacement field
```python
>>> positions = ((43.8, 52.1), (43.4, 35.2))
>>> 'Moving target at {p[0][1]} {p[1][0]}'.format(p = positions)
'Moving target at 52.1 43.4'
```

- access object attributes using `.` in the replacement fields
```python
>>> import math
>>> 'Math constants: pi={m.pi}, e={m.e}'.format(m=math)
'Math constants: pi=3.141592653589793, e=2.718281828459045'
```

####python formatting mini-language
```python
>>> 'Math constants: pi={m.pi:.3f}, e={m.e:.3f}'.format(m=math)
'Math constants: pi=3.142, e=2.718'
```

##range

- it is a collection rather than a container
- a type of sequence for representing an arithmetic progression of integers

```python
>>> range(5)
range(0, 5)

>>> for i in range(5):
...     print(i)
...
0
1
2
3
4

>>> range(5, 10)
range(5, 10)

>>> list(range(5, 10))
[5, 6, 7, 8, 9]

>>> list(range(10, 15))
[10, 11, 12, 13, 14]

>>> list(range(0, 10, 2))
[0, 2, 4, 6, 8]
```

determines what arguments means by counting them

1. _one_ argument - stop
2. _two_ args - start, stop
3. _three _ args - start, stop, step

doesn't support keyword arguments

it is un-pythonic to use range to iterate over lists

```python
#this is un-pythonic
>>> s = [0, 10, 100, 1000, 10000, ]
>>> for i in range(len(s)):
...     print(s[i])
...
0
10
100
1000
10000

#this is pythonic way of doing it
>>> for i in s:
...     print(i)
...
0
10
100
1000
10000
```

###`enumerate()`
preferred for counters
yields `(index, value)` tuples
often combined with tuple unpacking
```python
>>> for i, v in enumerate(s):
...     print("i = {}, v = {}".format(i, v))
...
i = 0, v = 0
i = 1, v = 10
i = 2, v = 100
i = 3, v = 1000
i = 4, v = 10000
```

##list
heterogeneous mutable sequence

```python
>>> s = "show how to index into sequences".split()
>>> s
['show', 'how', 'to', 'index', 'into', 'sequences']
>>> s[4]
'into'
```

###negative indexing

- starts from `-len()`
- the last element is at `-1`
- no difference between `-0` and `0`

```python
>>> s[-2]
'into'
```

###list slices
half-open ranges give complementary slices

`s[:x] + s[x:] == s`

```python
>>> s
['show', 'how', 'to', 'index', 'into', 'sequences']

>>> s[1:4]
['how', 'to', 'index']

>>> s[1:-1]
['how', 'to', 'index', 'into']

>>> s[3:]
['index', 'into', 'sequences']
>>> s[:3]
['show', 'how', 'to']
```

###copy a list
shallow copy

####slicing with `[:]`
```python
>>> l = s[:]
>>> l is s
False
>>> l = s[:]
>>> s
['show', 'how', 'to', 'index', 'into', 'sequences']
>>> l
['show', 'how', 'to', 'index', 'into', 'sequences']

>>> l is s
False
>>> l == s
True
```
####`copy()`
```python
>>> l = s.copy()
>>> l is s
False
>>> l == s
True
>>> l
['show', 'how', 'to', 'index', 'into', 'sequences']
>>> s
['show', 'how', 'to', 'index', 'into', 'sequences']
```

####list constructor
this is the preferred way as it works with any iterable series of the source
```python
>>> l = list(s)
>>> l
['show', 'how', 'to', 'index', 'into', 'sequences']
>>> s
['show', 'how', 'to', 'index', 'into', 'sequences']
>>> l is s
False
>>> l == s
```

###repetition
is shallow
```python
>>> [0] * 5
[0, 0, 0, 0, 0]

>>> s = [ [-1, +1] ] * 5
>>> s
[[-1, 1], [-1, 1], [-1, 1], [-1, 1], [-1, 1]]

>>> s[3].append(7)
>>> s
[[-1, 1, 7], [-1, 1, 7], [-1, 1, 7], [-1, 1, 7], [-1, 1, 7]]
```

###find an element with `index()`

- `index` returns the integer index of the first equivalent element
- raises `ValueError` if not found
```python
>>> w = 'the quick brown fox jumps over the lazy dog'.split()
>>> w
['the', 'quick', 'brown', 'fox', 'jumps', 'over', 'the', 'lazy', 'dog']

>>> i = w.index('fox')
>>> i
3
>>> w[i]
'fox'

>>> w.index('unicorn')
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
ValueError: 'unicorn' is not in list
```
###count elements matching with `count()`
```python
>>> w.count('the')
2
>>> w.count('unicorn')
0
```

###test for membership with `in` and `not in`
```python
>>> 'dog' in w
True
>>> 'fox' not in w
False
```

###remove elements
####`del`
```python
>>> d = list(w)
>>> d
['the', 'quick', 'brown', 'fox', 'jumps', 'over', 'the', 'lazy', 'dog']
>>> del d[3]
>>> d
['the', 'quick', 'brown', 'jumps', 'over', 'the', 'lazy', 'dog']

>>> d = list(w)
>>> del d[d.index('jumps')]
>>> d
['the', 'quick', 'brown', 'fox', 'over', 'the', 'lazy', 'dog']
>>> del d[d.index('jumps')]
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
ValueError: 'jumps' is not in list
```

####`remove()`
```python
>>> d = list(w)
>>> d.remove('jumps')
>>> d
['the', 'quick', 'brown', 'fox', 'over', 'the', 'lazy', 'dog']

>>> d.remove('unicorn')
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
ValueError: list.remove(x): x not in list
```

###`insert()`
```python
>>> d
['the', 'quick', 'brown', 'fox', 'over', 'the', 'lazy', 'dog']
>>> d.insert(4, 'jumps')
>>> d
['the', 'quick', 'brown', 'fox', 'jumps', 'over', 'the', 'lazy', 'dog']
```

###extending a list
these techniques (should) work with any iterable series as right operand
####`+`
creates a new list without altering the original
```python
>>> a = [1, 2, 3,]
>>> b = [4, 5, 6,]
>>> a + b
[1, 2, 3, 4, 5, 6]
>>> a
[1, 2, 3]
>>> b
[4, 5, 6]
```

####`+=`
modifies the first operand
```python
>>> a += b
>>> a
[1, 2, 3, 4, 5, 6]
>>> b
[4, 5, 6]
```

####`extend()`
```python
>>> a.extend('456')
>>> a
[1, 2, 3, '4', '5', '6']
```

#!Get back to this problem
```python
>>> a = ['a', 'b', 'c',]
>>> a += 'def'
>>> a
['a', 'b', 'c', 'd', 'e', 'f']
>>> a + 'ghi'
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
TypeError: can only concatenate list (not "str") to list
```

###`reverse()`
reverses in place
```python
>>> a
['a', 'b', 'c', 'd', 'e', 'f']
>>> a.reverse()
>>> a
['f', 'e', 'd', 'c', 'b', 'a']
```

###`sort()`
```python
>>> a
['f', 'e', 'd', 'c', 'b', 'a']

>>> a.sort()
>>> a
['a', 'b', 'c', 'd', 'e', 'f']

>>> a.sort(reverse=True)
>>> a
['f', 'e', 'd', 'c', 'b', 'a']
```

####sorting with `key` to callable object
```python
>>> a = 'not perplexing do handwriting family where I illegibly know doctors'.split()
>>> a
['not', 'perplexing', 'do', 'handwriting', 'family', 'where', 'I', 'illegibly', 'know', 'doctors']
>>> a.sort(key=len)
>>> a
['I', 'do', 'not', 'know', 'where', 'family', 'doctors', 'illegibly', 'perplexing', 'handwriting']
```

####`sorted()`
```python
>>> a = ['d', 'f', 'e', 'a', 'b', 'c',]
>>> b = sorted(a)
>>> b
['a', 'b', 'c', 'd', 'e', 'f']
>>> a
['d', 'f', 'e', 'a', 'b', 'c']
```

####`reversed()`

```python
>>> b = reversed(a)
>>> b
<list_reverseiterator object at 0x00000000008AF5F8>
>>> list(b)
['c', 'b', 'a', 'e', 'f', 'd']
>>> a
['d', 'f', 'e', 'a', 'b', 'c']
```

the last two functions work only on finite length, iterable sources

##dict
unordered mapping from unique, immutable keys to mutable values

- `keys` must be immutable types
- `value` objects can be mutable
- never rely on order of items in the dictionary

###`dict()`
```python
>>> d = [ ('A', '0041'), ('B', '0042'), ('C', '0043'), ('D', '0044'), ('F', '0046')]
>>> chars = [ ('A', '0041'), ('B', '0042'), ('C', '0043'), ('D', '0044'), ('F', '0046')]
>>> d = dict(chars)
>>> d
{'B': '0042', 'F': '0046', 'C': '0043', 'D': '0044', 'A': '0041'}

>>> phonetic = dict(a='alfa', b='bravo', c='charlie', d='delta', e='echo', f='foxtrot')
>>> phonetic
{'d': 'delta', 'e': 'echo', 'a': 'alfa', 'f': 'foxtrot', 'b': 'bravo', 'c': 'charlie'}

>>> d1 = dict(d)
>>> d1
{'B': '0042', 'A': '0041', 'C': '0043', 'D': '0044', 'F': '0046'}
```

###`update()`
```python
>>> d1.update(phonetic)
>>> d1
{'B': '0042', 'e': 'echo', 'C': '0043', 'b': 'bravo', 'c': 'charlie', 'd': 'delta', 'a': 'alfa', 'D': '0044', '
F': '0046', 'f': 'foxtrot', 'A': '0041'}

>>> e = dict(B='Bear')
>>> d1.update(e)
>>> d1
{'B': 'Bear', 'e': 'echo', 'C': '0043', 'b': 'bravo', 'c': 'charlie', 'd': 'delta', 'a': 'alfa', 'D': '0044', '
F': '0046', 'f': 'foxtrot', 'A': '0041'}
```

###`value()`
gets the iterable sequence of values
```python
>>> d
{'B': '0042', 'F': '0046', 'C': '0043', 'D': '0044', 'A': '0041'}
>>> values = d.values()
>>> values
dict_values(['0042', '0046', '0043', '0044', '0041'])
>>> d
{'B': '0042', 'F': '0046', 'C': '0043', 'D': '0044', 'A': '0041'}
>>> d['B'] = '0049'
>>> values
dict_values(['0049', '0046', '0043', '0044', '0041'])

>>> for v in d.values():
...     print(v)
...
0049
0046
0043
0044
0041
```

###`keys()`
```python
>>> for k in d.keys():
...     print(k)
...
B
F
C
D
A
```

###`items()`
tuples of keys and values
```python
>>> for k, v in d.items():
...     print('{key} => {value}'.format(key=k, value=v))
...
B => 0049
F => 0046
C => 0043
D => 0044
A => 0041
```

###membership testing

- works only with the keys
- `in` and `not in`
```python
>>> 'C' in d
True

>>> 'E' not in d
True
```

###`del`
```python
>>> d
{'F': '0046', 'C': '0043', 'D': '0044', 'A': '0041'}
>>> del d['F']
>>> d
{'C': '0043', 'D': '0044', 'A': '0041'}
```

###pretty print with `pprint`
```python
#it is important to import pprint as pp so the function won't override the module name
>>> from pprint import pprint as pp

>>> pp(d)
{'A': '0041', 'C': '0043', 'D': '0044'}
>>> d
{'C': '0043', 'D': '0044', 'A': '0041'}
```

##set
mutable unordered collection of unique, immutable objects

- delimited by `{` and `}`
- single comma separated items
- `{}` creates an empty dictionary so for empty set use `set()` constructor
- the order is arbitrary
```python
>>> p = {1,
...      2,
...      3,
...      5,
...      7,
...      11,
...      13,
...      17,}

>>> p
{1, 2, 3, 5, 7, 11, 13, 17}

>>> l = [1, 2, 4, 3, 5, 5, 2, ]
>>> set(l)
{1, 2, 3, 4, 5}

>>> s = set()
>>> s.add(3)
>>> s.add(2)
>>> s.add(1)
>>> s.add(4)
>>> s.add(32)
>>> s
{32, 1, 2, 3, 4}
>>> for x in s:
...     print(x)
...
32
1
2
3
4
```

###membership checking with `in` and `not in`
```python
>>> 3 in s
True
>>> 5 in s
False
>>> 7 not in s
True
```

###`add()`
doesn't produce side effect if the item already in the set

###'update()'
adds elements from another iterable sequence of immutable objects
```python
>>> s.update([4, 5, 6, 7, 8, 9, 10, ])
>>> s
{32, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}
```

###`remove()`
requires element to be in the set
```python
>>> s
{32, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10}
>>> s.remove(10)
>>> s
{32, 1, 2, 3, 4, 5, 6, 7, 8, 9}
>>> s.remove(100)
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
KeyError: 100
```

###`discard()`
discards the element if it is in the set

###copying
shallow copies
```python
>>> s1 = s.copy()
>>> s1 == s
True
>>> s1 is s
False

>>> s2 = set(s)
>>> s2 == s
True
>>> s2 is s
False
```

###set algebra
####`union()`

- collects together elements that are in either or both sets
- commutative

####`intersection()`

- collects together only the elements contained in both sets
- commutative

####`difference()`

- all elements that are in the first set but not in the second
- non-commutative

####`symmetric_difference()`

- all elements that are in the first or in the second, but not in both
- commutative

####`issubset()`

- check whether elements from the first set are found in the second
- non-commutative

####`issuperset()`

- check whether elements from the second set is are found in the first
- non-commutative

####`isdisjoint()`

- test whether two sets have no members in common
- commutative

##collection protocols
to implement a protocol , objects must support certain operations

protocol          | implementing collections
------------------|------------------------------------------
`container`       | str, list, range, tuple, bytes, set, dict 
`sized`           | str, list, range, tuple, bytes, set, dict 
`iterable`        | str, list, range, tuple, bytes, set, dict 
`sequence`        | str, list, range, tuple, bytes
`mutable sequence`| list
`mutable set`     | set
`mutable mapping` | dict

- `container`

    * `in` and `not in` to be supported
    
- `sized`

    * determine the number of elements with `len()`
    
- `iterable`

    * can produce an iterator with `iter()`
    * they can be used with `for` loops
    
- `sequence`

    * `seq[index]`: retrieve elements by index
    * `seq.index(item)`: find items by value
    * `seq.count(item)`: count items
    * `reversed(seq)`: produce a reversed sequence

##exception handling
is a mechanism for stopping normal program flow and continuing at some surrounding context or code block

these exception types should not normally be catched:

- `IndentationError`
- `SyntaxError`
- `NameError`

`pass` - causes empty exception catch blocks to pass
`as exception` - captures at exception catching level the thrown exception
`str(exception)` - converts exception to string
exceptions can't be ignored, but error codes can
`raise`:
    - without a parameter simply re-raises the exception currently being handled
    - use with a new exception object, e.g. `ValueError`

callers need to know what exceptions to expect and when
avoid too tight scope for exception handling 
use exceptions that users will anticipate
standard exceptions are often the best choice
exceptions are parts of families of related functions referred to as protocols
use common or existing exception types when possible

- `IndexError` integer index is out of range
- `ValueError` object is of the right type, but contains an inappropriate value
- `KeyError` look-up in a mapping fails

avoid protecting against `TypeErrors`
it's usually not worth checking types, this can limit functions unnecessarily

LBYL (look before you leap) vs EAFP (it's easier to ask forgiveness than permission)

Python is in favour of EAFP

error codes require interspersed, local handling
exceptions allow centralized non-local handling

exceptions require explicit handling
error codes are silent by default

`finally` - is executed no matter how the try-block exits

> Zen: Errors should never pass silently, unless explicitly silenced
> Errors are like bells and if we make them silent they are of no use

##iterables
comprehensions are a concise syntax for describing lists, sets or dictionaries in a _declarative_ or _functional_ style
they are _readable_, _expressive_ and _effective_
###list comprehensions
general form of list comprehensions
```python
[ expr(item) for item in iterable]
```

```python
>>> words = 'Why sometimes I have believed as many as six impossible things before breakfast'.split()
>>> words
['Why', 'sometimes', 'I', 'have', 'believed', 'as', 'many', 'as', 'six', 'impossible', 'things', 'before', 'bre
akfast']
>>> [len(word) for word in words]
[3, 9, 1, 4, 8, 2, 4, 2, 3, 10, 6, 6, 9]

>>> from math import factorial
>>> f = [len(str(factorial(x))) for x in range(20)]
>>> for i, v in enumerate(f):
...     print('{} {}'.format(str(i).zfill(2),str(v).zfill(2)))
...
00 01
01 01
02 01
03 01
04 02
05 03
06 03
07 04
08 05
09 06
10 07
11 08
12 09
13 10
14 11
15 13
16 14
17 15
18 16
19 18
```

###set comprehensions
general form
```python
{ expr(item) for item in iterable}
```

```python
>>> s = {len(str(factorial(x))) for x in range(20)}
>>> for i, v in enumerate(s):
...     print('{} {}'.format(str(i).zfill(2),str(v).zfill(2)))
...
00 01
01 02
02 03
03 04
04 05
05 06
06 07
07 08
08 09
09 10
10 11
11 13
12 14
13 15
14 16
15 18
```

###dictionary comprehensions
general form
```python
{key: value for item in items}
```

```python
>>> from pprint import pprint as pp
>>>
>>> country_to_capital = {'United Kingdom': 'London',
...                      'Brazil': 'Brazilia',
...                      'Morocco': 'Rabat',
...                      'Sweden': 'Stockholm'}
>>> capital_to_country = {capital: country for country, capital in country_to_capital.items()}
>>> pp(capital_to_country)
{'Brazilia': 'Brazil',
 'London': 'United Kingdom',
 'Rabat': 'Morocco',
 'Stockholm': 'Sweden'}
 
 >>> words = ['hi', 'hello', 'foxtrot', 'hotel']
>>> {x[0]: x for x in words}
{'h': 'hotel', 'f': 'foxtrot'}
```

not to cram too much complexity into comprehensions
```python
>>> import os
>>> import glob
>>> file_sizes = {os.path.realpath(p): os.stat(p).st_size for p in glob.glob('*.py')}
>>> pp(file_sizes)
{'C:\\Users\\Daniel\\Documents\\GitHub\\Research\\pyfund\\exceptional.py': 436,
 'C:\\Users\\Daniel\\Documents\\GitHub\\Research\\pyfund\\keypress.py': 983,
 'C:\\Users\\Daniel\\Documents\\GitHub\\Research\\pyfund\\roots.py': 1027,
 'C:\\Users\\Daniel\\Documents\\GitHub\\Research\\pyfund\\words.py': 1122}
```

###filtering predicates
appends an `if` at the end of comprehension

```python
>>> from math import sqrt

>>> def is_prime(x):
...     if(x<2):
...             return False
...     for i in range(2, int(sqrt(x)) + 1):
...             if x % i ==0:
...                     return False
...     return True
...
>>> [x for x in range(101) if is_prime(x)]
[2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97]

#numbers with only three divisors mapped to those divisors
>>> prime_square_divisors = {x*x:(1, x, x*x) for x in range(101) if is_prime(x)}
>>> pp(prime_square_divisors)
{4: (1, 2, 4),
 9: (1, 3, 9),
 25: (1, 5, 25),
 49: (1, 7, 49),
 121: (1, 11, 121),
 169: (1, 13, 169),
 289: (1, 17, 289),
 361: (1, 19, 361),
 529: (1, 23, 529),
 841: (1, 29, 841),
 961: (1, 31, 961),
 1369: (1, 37, 1369),
 1681: (1, 41, 1681),
 1849: (1, 43, 1849),
 2209: (1, 47, 2209),
 2809: (1, 53, 2809),
 3481: (1, 59, 3481),
 3721: (1, 61, 3721),
 4489: (1, 67, 4489),
 5041: (1, 71, 5041),
 5329: (1, 73, 5329),
 6241: (1, 79, 6241),
 6889: (1, 83, 6889),
 7921: (1, 89, 7921),
 9409: (1, 97, 9409)}
```

> Zen: Simple is better than complex
> Code is written once but read over and over
> Fewer is clearer

###iteration protocols
- _iterable_ protocol
    * iterable objects can be passed to the built-in `iter()` function to get an iterator
```python
iterator = iter(iterable)
```
- _iterator_ protocol
    * iterator objects can be passed to the built-in `next()` function to fetch the next item
    * throws exception when the end of the iteration is encountered
```python
item = next(iterator)
```

```python
>>> iterable = ['Spring', 'Summer', 'Autumn', 'Winter']
>>> iterator = iter(iterable)
>>> next(iterator)
'Spring'
>>> next(iterator)
'Summer'
>>> next(iterator)
'Autumn'
>>> next(iterator)
'Winter'
>>> next(iterator)
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
StopIteration

>>> def first(iterable):
...     iterator = iter(iterable)
...     try:
...             return next(iterator)
...     except StopIteration:
...             raise ValueError('iterable is empty')
...
>>> first(['first', 'second', 'third'])
'first'
>>> first({'first', 'second', 'third'})
'first'
>>> first(set())
Traceback (most recent call last):
  File "<stdin>", line 4, in first
StopIteration

During handling of the above exception, another exception occurred:

Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
  File "<stdin>", line 6, in first
ValueError: iterable is empty
```

###generators
- specify iterable sequences
    * all generators are iterators
- are lazily evaluated
    * the next value in the sequence is computed on demand
- can model infinite sequences
    * such as data streams with no definite end
- are composable into pipelines
    * for natural stream processing
    
functions that use `yield` are _generators_

```python
>>> def gen123():
...     yield 1
...     yield 2
...     yield 3
...
>>> g = gen123()
>>> g
<generator object gen123 at 0x0000000000716A98>
>>> next(g)
1
>>> next(g)
2
>>> next(g)
3
>>> next(g)
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
StopIteration
>>>
>>> for v in gen123():
...     print(v)
...
1
2
3
>>> h = gen123()
>>> i = gen123()
>>> next(h)
1
>>> next(h)
2
>>> next(i)
1
>>> h is i
False
>>> h
<generator object gen123 at 0x0000000000716B48>
>>> i
<generator object gen123 at 0x00000000011AC938>
```

```python
>>> def gen246():
...     print('About to yield 2')
...     yield 2
...     print('About to yield 4')
...     yield 4
...     print('About to yield 6')
...     yield 6
...     print('About to return')
...
>>> g = gen246()
>>> next(g)
About to yield 2
2
>>> next(g)
About to yield 4
4
>>> next(g)
About to yield 6
6
>>> next(g)
About to return
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
StopIteration
>>> next(gen246())
About to yield 2
2
>>> next(gen246())
About to yield 2
2
```

###stateful generator functions
- generators resume execution
- can maintain state in local variables
- complex control flow
- lazy evaluation

`continue` - continues the `for`/`while` iteration loops to the next item

- just in time computation
- infinite (or large) sequences
    * sensor readings
    * mathematical series
    * massive files

```python
>>> def lucas():
...     yield 2
...     a = 2
...     b = 1
...     while b < 50: #use True to get a never ending sequence
...             yield b
...             a, b = b, a + b
...
>>> for x in lucas():
...     print(x)
...
2
1
3
4
7
11
18
29
47
```

###generator comprehensions
- similar syntax to list comprehensions
- create a generator object
- concise
- lazy evaluation

syntax
```python
( expr(item) for item in iterable)
```

```python
>>> squares = (x*x for x in range(1, 11))
>>> squares
<generator object <genexpr> at 0x00000000011AC990>
>>> list(squares)
[1, 4, 9, 16, 25, 36, 49, 64, 81, 100]
>>> list(squares)
[]

#memory footprint is insignificant here
>>> sum(x*x for x in range(1, 10000001))
333333383333335000000
```

can have predicates too

###iteration tools
https://docs.python.org/3.5/library/itertools.html

```python
>>> from itertools import islice, count
>>> islice(all_primes, 1000)
Traceback (most recent call last):
  File "<stdin>", line 1, in <module>
NameError: name 'all_primes' is not defined

>>> thousand_primes = islice((x for x in count() if is_prime(x)), 50)
>>> list(thousand_primes)
[2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107,
 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229]
 
>>> sum(islice((x for x in count() if is_prime(x)), 50))
5117
```

```python
>>> any([False, False, True])
True
>>> all([False, False, True])
False

>>> any(is_prime(x) for x in range(1328, 1361))
False
>>> all(name == name.title() for name in ['London', 'New York', 'Sydney'])
True
```

```python
>>> a = [1, 2, 3, 4, 5, 6, 7, 8, ]
>>> b = [1, 2, 3, 4, 5, 6, 7, 8, ]
>>> for x in zip(a,b):
...     print(x)
...
(1, 1)
(2, 2)
(3, 3)
(4, 4)
(5, 5)
(6, 6)
(7, 7)
(8, 8)

>>> b = [2, 3, 4, 5, 6, 7, 8, 9, ]
>>> for ax, bx in zip(a,b):
...     print('average = ', (ax + bx) /2)
...
average =  1.5
average =  2.5
average =  3.5
average =  4.5
average =  5.5
average =  6.5
average =  7.5
average =  8.5
```

```python
>>> from itertools import chain
>>> l = chain(a, b)
>>> all(x>0 for x in l)
True

>>> for x in (p for p in lucas() if is_prime(p)):
...     print(x)
...
2
3
7
11
29
47
```
