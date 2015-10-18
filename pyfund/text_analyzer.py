import unittest

def analyze_text(filename):

    lines = 0
    chars = 0
    with open(filename, 'rt') as f:
        for line in f:
            lines += 1
            chars += len(line)
        return (lines, chars)

class TestAnalysisTests(unittest.TestCase):
    '''Tests for the ``analyze_text()`` function.'''

    def setUp(self):
        self.filename = 'text_analysis_test_file.txt'
        with open(self.filename, 'wt') as f:
            f.write('Now we are engaged in a great civil war.\n'
                    'testing whether that nation,\n'
                    'or any nation so conceived and so dedicated,\n'
                    'can long endure')

    def tearDown(self):
        try:
            os.remove(self.filename)
        except:
            pass

    def test_function_runs(self):
        analyze_text(self.filename)

    def test_line_count(self):
        self.assertEqual(analyze_text(self.filename)[0], 4)

    def test_character_count(self):
        self.assertEqual(analyze_text(self.filename)[1], 130)

    def test_no_such_file(self):

        with self.assertRaises(IOError):
            analyze_text('foobar')

if __name__ == '__main__':
    unittest.main()


