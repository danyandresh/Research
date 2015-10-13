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




