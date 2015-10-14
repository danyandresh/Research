'''A module for demonstrating exceptions.'''

import sys
from math import log

def convert(s):
    '''Convert to an integer'''
    x = -1;
    try:
        x = int(s)
        print('Conversion succeeded! x=', x)
    except (ValueError, TypeError) as e:
        print('Conversion error: {}'.format(str(e)), file=sys.stderr)
        raise
    return x


def string_log(s):
    v = convert(s)
    return log(v)

    
