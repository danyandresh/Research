# MDX

multi dimensional expression language

### selecting
```sql
SELECT
  [Order Quantity] on Columns,
  [Product].[Products].[Product].Members on Rows
  -- product dimension. hierarchy in that dimension. level in that dimension
FROM
  [Sales]

```

`NON EMPTY` - to get rid of the _Unknown_ row

_UDM_ - unified dimension model contains logic to automatically sort data

### sorting
```sql
SELECT
  [Order Quantity] on Columns,
  NON EMPTY Order([Products].[Product].[Product].Members, [Order Quantity], asc) on Rows
FROM
  [Sales]

WHERE
  [Date].[Year].&[2007]

```

### children
```sql
SELECT
  [Order Quantity] on Columns,
  NON EMPTY [Products].[Products].[Bikes].Children on Rows
FROM
  [Sales]
```

### filter
```sql
SELECT
  [Order Quantity] on Columns,
  Filter([Products].[Subcategory].Members,
      Left([Product].[Subcategory].CurrentMember.Name, 1)= "M") on Rows
FROM
  [Sales]
```

### MDX explicit members in row set

```sql
SELECT
  [Order Quantity] on Columns,
  {
    [Product].[Subcategory].[Mountain Bikes],
    [Product].[Subcategory].[Road Bikes]
  } on Rows
FROM
  [Sales];
```

### top counting

```sql
SELECT
  [Order Quantity] on Columns,
  TopCount([Product].[Product].[Product].Members, 2, [Order Quantity]) on Rows
FROM
  [Sales];
```

## MDX usage

- query cubes for reporting and analysis
- SSAS, Oracle Essbase, SAP Netweaver BI, SAS Olap Server

### cube objects
- _calculated member_: an expression stored in a cube that is evaluated when a query executes and includes that calculated member
- _named set_: an expression that returns a set of members based on some rules specified in an expression
- _key performance indicators (KPIs)_: a collection of expressions used to show how well a value compares to a goal at a point in time
- _MDX script statement_: is a more advanced type of expression that overrides values stored in cube cells
- _Action_: can associate the opening of a URL in browser based on an MDX expression or pass parameters to a Reporting Services report
- _Security_: can define security  on dimensions or cell data by using MDX expressions
- create or modify cube components

### tools
- SSMS
- Excel pivot tables
- Reporting Services
- PerformancePoint Analytics Reports

## structure

- _measures_ - the numeric values
- _calculated member_ - an expression stored in the cube, calculates the value at query runtime
- _dimension_ - a collection of hierarchies
- _attribute hierarchies_ - similar to columns from a sql table
- _user-defined hierarchies_ - arrange attributes into a multilevel structure used to navigate from summary to detailed data
- _levels_ - substructures of user-defined hierarchies

when expanding an hierarchy:

- members folder: all members regardless of hierarchy level
- dimensions level: members that exist there

members are the values (typically strings) to be placed on rows and columns in a pivot table to get aggregated values for measures at each intersection

## tuples and sets
### location

a location, a coordinate into the cube used to retrieve data, an intersection of multiple members

### sets

a set is a collection of members from the same dimension and hierarchy

## identifiers

`[Dimension].[Hierarchy].[Level].[Member]`
`[Dimension].[Hierarchy].[Parent].[Member]`

### key reference

`[Dimension].[Hierarchy].&[Member Key]`

brackets requirements:
- embedded space
- reserved word
- first character is not a letter or underscore
- contains characters other than letters

## axes

single axis query:
```sql
SELECT
  <set> ON COLUMNS
FROM <cube>
WHERE
  (<Member1>, <Member2>)
```


two axis query:
```sql
SELECT
  <set> ON COLUMNS,
  <set> ON ROWS
FROM <cube>
WHERE
  (<Member1>, <Member2>)
```

## sets

* multiple members from the same dimension and hierarchy
```sql
{[Mountain-200 Black, 38], [Road-250 Black, 44]}
```

* single member
```sql
[Mountain-200 Black, 44]
```

* empty set
```sql
{}
```

* MDX functions
```sql
Product.Product.Members
Product.Products.Bikes.Children
```

```sql
SELECT
  [Measures].[Sales Amount] on 0,
  [Product].[Product].[Product].Members on 1,
  [Date].[Year].[Year].Members on 2
FROM sales;
```

## query resolution

1. rows and columns are evaluated independently
2. tuples constructed from current row member plus current column member plus WHERE clause
3. empty members removed if NON EMPTY on axis

### cell properties
- include the lcause to retrieve formatted value, colors
- add the clause at the end of the SELECT statement


Cell Property  | Description
--|--
ACTION_TYPE  | Bitmask for type of actions assigned to cell  
BACK_COLOR  | Bitask for background color  
CELL_ORDINAL  | Ordinal number of cell in cell set  
FONT_FLAGS  | Bitmask for display for italic, bold, underline, or strikeout  
FONT_NAME  | Font name for display of value or formatted value  
FORE_COLOR  | Bitmask for foreground color
FORMAT  | Equivalent to FORMAT_STRING  
FORMAT_STRING  |  String defining the format value
FORMATTED_VALUE  |  String display of value with formatting applied
LANGUAGE  |  Locale to which format string applies
UPDATEABLE  |  Indicator whether the cell can be updated
VALUE  |  Unformatted value of cell

### dimension properties

include the clause to retrieve member data

Dimension Property  |  Description
--|--
CATALOG_NAME  |  Name of cube
CHILDREN_CARDINALITY  |  number of children (potential estimate)
CUSTOM_ROLLUP  |  expression for custom member
CUSTOM_ROLLUP_PROPERTIES  |  properties of custom member
DESCRIPTION |  description of member
DIMENSION_UNIQUE_NAME  |  name of dimension
HIERARCHY_UNIQUE_NAME  |  name of hierarchy
IS_DATA_MEMBER  |  boolean indicator to identify data members
IS_PLACEHOLDERMEMBER  |  boolean indicator to identify placeholder members
KEYx  |  member key (use Key0, Key1,...)
LCIDx  |  translated member caption based on x (locale ID decimal value)
LEVEL_NUMBER  |  distance from root of hierarchy
LEVEL_UNIQUE_NAME  |  name of level

## set functions

`MEMBERS` - non-calculated members

`ALLMEMBERS` - non-calculated and calculated members

`set` usage in a query:
- on an axis (ON ROWS or ON COLUMNS)
- in a slicer (WHERE)
- as a calculation in query (WITH MySet AS <SetExpression>)

`ASC`, `DESC`, `BASC`, `BDESC`

### set manipulation

* head: returns specified number of members from beginning of the set
* tail: returns specified number of members from end of set
* topCount: sorts a set in descending oerder and returns specified number of members from beginning of set
* bottomCount: sorts a se in ascending order and returns specified number of members from beginning of set
* topPercent: sorts a set in descending order and returns a set of members for which cumulative total >= specified percentage
* buttomPercent: sorts a set in ascending order and returns a set of members for which cumulative total >= specified percentage
* topSum: sorts a set in descending order and returns a set of members for which cumulative total >= specified value
* bottomSum: sorts a set in ascending order and returns a set of members for which cumulative total >= specified value
* filter: returns a set of members that satisfy specified criteria in boolean expression
* crossJoin: returns a tuple set of members that combines multiple hierarchies
### polymorphic operators

* `*` an operator that creates a tuple set of members by combining multiple hierarchies like a crossjoin
* `+` union of sets
* `-` except with

* generate: returns a tuple set of members by iterating through a set
* nonempty: returns only members that are non-empty
* extract: reduces a tuple set to specified dimension hierarchies

https://docs.microsoft.com/en-us/sql/mdx/mdx-function-reference-mdx

### upward navigation

* parent: returns the member above the current member. `[July 2009].Parent`
* ancestor: returns the member above the current member at specified level
* ascendants: returns a set of members from the current member to the dimension root at all levels

### downward navigation

* children: returns a set of members one level below the current member
* firstChild: returns the first member one level below current member
* lastChild: returns the last member one level below current member
* descendants: returns a set of members from the current member to specified lower level

### horizontal navigation

* siblings: returns a set of members on same level as current member and sharing common parent with current member, includes member
* firstSibling: returns the first member of current members siblings
* lastSibling: returns the last member of current members sibling
* nextMember: returns the member following the current member
* prevMember: returns the member preceding the current member
* lead: returns nth member following the current member
* lag: returns nth member preceding the current member
* cousin: returns the member in same position as current member in ancestor's sibling's set of children
* parallelPeriod: returns the member in same position as current member in ancestor's set of children
