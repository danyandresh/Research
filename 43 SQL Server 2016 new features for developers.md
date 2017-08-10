# SQL Server 2016 new features for developers

* DIE - drop if exists
* SESSION_CONTEXT()
* Dynamic Data Masking
* Row-level security
* Always encrypted
* Stretch database
* Temporal data
* Built-in JSON support
* Query store
* PolyBase
* R integration

Tools:

- SSMS (SQL Server Management Studio)
- SSDT (SQL Server Data Tools)

## DIE

```SQL
DROP TABLE IF EXISTS dbo.Product
DROP TRIGGER IF EXISTS trProductInsert
```

- AGGREGATE
- ASSEMBLY
- DATABASE
- DEFAULT
- INDEX
- PROCEDURE
- ROLE
- RULE
- SCHEMA
- SECURITY POLICY
- SEQUENCE
- SYNONYM
- TABLE
- TRIGGER
- TYPE
- VIEW

## Session Context

- stateful dictionary
- retains data for the lifetime of the connection

`sp_set_session_context` `@key` `@value` `@read_only`

## Dynamic Data Masking (DDM)

limit the exposure to sensitive data by masking

- full - entire column is masked
- partial - show starting  and/or ending characters  of the column data, mask the rest  with a custom string
- email - similar to partial, except all values will look like email addresses
- random - entire column  is replaced with random values

reveals data to queries

```SQL
CREATE TABLE Customer (
	FirstName varchar(20)
		MASKED WITH (FUNCTION='partial(1, "...", 0)'),
	Phone varchar(20)
		MASKED WITH (FUNCTION='default()'),
	Email varchar(200)
		MASKED WITH (FUNCTION='email()'),
	Balance money
		MASKED WITH (FUNCTION='random(1000, 5000)')
)

ALTER TABLE Customer
	ALTER COLUMN LastName
		ADD MASKED WITH (FUNCTION='default()')
```

`sys.columns`

- is_masked
- masking_function

`sys.masked_columns`

- inherits from `sys.columns`
- filters to show only masked columns


based on user permissions:

- no permission neede to create a table with masked columns
- `ALTER ANY MASK` permission required to change and existing table mask
- `UNMASK` permission required to see masked data
- no permission needed to update or insert data in a masked column

```SQL
EXECUTE AS USER = 'TestUser'
...
REVERT
```

### DDM can't mask

- varchar(max) decorated with `FILESTREAM` attribute
- COLUMN_SET, or sparse columns that are part of a COLUMN_SET
- computed columns
- key for FULLTEXT index
- encrypted columns (always encrypted)

## Row-level Security (RLS)

restricts access to individual rows in a table

### _filter_ predicate

- SELECT, UPDATE, DELETE

can't select, update or delete rows that violate the predicate

### _block_ predicate

- AFTER INSERT, AFTER UPDATE
	* can't insert or update rows to values that would violate the predicate

- BEFORE UPDATE, BEFORE DELETE
	* can't update or delete rows that violate the predicate
	* implied when combined with filter predicate

| predicate | SELECT/UPDATE/DELETE rows that violate the predicate     | INSERT rows with violating values	| UPDATE rows with violating values	|
| :------------- | :------------- | :------------- |
| Filter 							|	no 													|	yes								|	yes								|
| AFTER INSERT block 	|	yes													|	no								|	yes								|
| AFTER UPDATE block 	|	yes													|	yes								|	no								|
| BEFORE UPDATE block	|	no													| N/A								| N/A								|
| BEFORE DELETE block	|	no													| N/A								| N/A								|

###	security predicate functions

* ordinary inline table-valued functions (TVF)
	- must be schema-bound
* accept parameters of any type
	- map these parameters to column values
* examine the row via  the columns passed parameters
	- determine if the access should be allowed or denied
* return scalar 1 (allow) or nothing at all (deny)
* encapsulate logic inside WHERE clause of a single SELECT statement inside the TVF

```SQL
CREATE FUNCTION sec.fn_MySecurityPredicate(@Param1 as int,...)
	RETURNS TABLE
	WITH SCHEMABINDING
AS
	-- SQL Server passes  in column values of each row via parameters
	RETURN
		SELECT 1 as RESULT
		WHERE ...
			-- custom logic here examines the parameters (column values) passed in and determines the row accesibility
```

### security policies

bind a security predicate to a particular table

```SQL
CREATE SECURITY POLICY sec.MySecurityPolicy
	ADD FILTER PREDICATE sec.fn_MySecurityPredicate(Col1, ...)
		ON dbo.MyTable
	ADD BLOCK PREDICATE sec.fn_MySecurityPredicate(Col1, ...)
		ON dbo.MyTable AFTER INSERT
	ADD BLOCK PREDICATE sec.fn_MySecurityPredicate(Col1, ...)
		ON dbo.MyTable AFTER UPDATE
	WITH (STATE = ON)
```

example

```SQL
CREATE FUNCTION sec.fn_MySecurityPredicate(@Username as varchar(50))
	RETURNS TABLE
	WITH SCHEMABINDING
AS
	-- SQL Server passes  in column values of each row via parameters
	RETURN
		SELECT 1 as RESULT
		WHERE
			DATABASE_PRINCIPAL_ID() = DATABASE_PRINCIPAL_ID(@Username) OR
			DATABASE_PRINCIPAL_ID() = DATABASE_PRINCIPAL_ID('ManagerUser')

-- test the function
SELECT * FROM sec.fn_MySecurityPredicate('dbo')

CREATE SECURITY POLICY sec.MyPolicyFilter
	ADD FILTER PREDICATE sec.fn_MySecurityPredicate(SalesUserName)
	ON dbo.Sales
	WITH (STATE = ON)

--disable the security policy
ALTER SECURITY POLICY sec.MyPolicyFilter WITH (STATE = OFF)

```

### identifying users for RLS
* credentials supplied for the database connection
	* SQL Server login (username and password)
	* windows authentication
	* obtain the username from DATABASE_PRINCIPAL_ID
* for n-tier applications (that connect to the DB user the same credentials for all users)
	* use SESSION_CONTEXT to store the user id for the session (as a readonly value, to prevent malicious change)

## always encrypted

- based on keys managed outside of the database
- keys are never revealed to SQL Server
- uses client-side drivers to decrypt it (separates who owns the data and who decrypts it)
- always encrypted in flight

### encryption types
- randomized
	- unpredictable, more secure
	- no support for equality searches, joins, grouping, indexing
	- use for data that is returned but not queried
- deterministic
	- predictable, less secure
	- for data that must be queried (equality only)
	- easier to guess by examining encryption patterns
		- increased risk for small value sets (e.g. True/False)

### column encryption keys (CEK)
- used to encrypt values in specific columns

### column master keys (CMK)
- used to encrypt all the CEKs
- must be stored externally:
	- key store providers: Azure Key Vault, Certificate Store, Hardware Security Modules

`sys.column_master_keys`
	- identifies each CMK
	- contains external path to CMK location

`sys.column_encryption_keys`
	- identifies each CEK

`sys.column_encryption_key_values`
	- contains CMK-encrypted values for each CEK

`sys.columns`
	- new metadata columns to identify encrypted columns

to view encrypted data change connection (SSMS: Additional Connection Parameters) to contain: `column encryption setting=enabled`

### CMK rotation
to mitigate the CMK compromising risks

generate a new CMK and re-encrypt all CEKs

### unsupported data types:
- xml
- rowversion
- image
- text
- ntext
- sql_variant
- hierarchyid
- geography
- geometry

- FILESTREAM
- ROWGUIDCOL
- IDENTITY
- computed
- sparse
- partitioning columns
- fulltext indexes
- columns with default constraints
- temporal tables
- stretch database

https://blogs.sentryone.com/aaronbertrand/t-sql-tuesday-69-always-encrypted-limitations/

## stretch database
Remote Data Archive
- store portions of database in the cloud (cold data)
- store portions of data on premises (hot data)

### terminology
- local database
	- the on-premises SQL Server database being stretched
	- own responsibility to maintain and backup
- eligible data
	- cold data in the local database that has not been yet moved to the cloud
- remote endpoint
	- the Azure SQL Data Warehouse database in the cloud
	- automatic backups by the stretch database service on Azure

### workflow

local db -> eligible data -> cloud db

can break the connection temporarily using `sys.sp_rda_deauthorize_db` and `sys.sp_rda_reauthorize_db`


```SQL
-- configure the local server to enable stretch
EXEC sp_configure `remote data archive`, 1
RECONFIGURE

-- enable stretch on the local database

CREATE MASTER KEY
	ENCRYPTION BY PASSWORD = 'Big$ecret'

CREATE DATABASE SCOPED CREDENTIAL Credential_Name
	WITH INDEITY = 'BLABLA', SECRET = 'WHAT A SECRET'

ALTER DATABASE MyStretchedDb
	SET REMOTE_DATA_ARCHIVE = ON(
		SERVER = 'something.database.windows.net',
		CREDENTIAL = Credential_Name
	)

SELECT name, is_remote_data_archive_enabled FROM sys.databases

SELECT * FROM sys.remote_data_archive_databases

ALTER TABLE Stretched_Test
	SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = PAUSED))

SELECT * FROM sys.dm_db_rda_migration_status WHERE migrated_rows > 0

EXEC sp_spaceused @objname = 'dbo.Stretched_Test'
EXEC sp_spaceused @objname = 'dbo.Stretched_Test', @mode = 'LOCAL_ONLY'
EXEC sp_spaceused @objname = 'dbo.Stretched_Test', @mode = 'REMOTE_ONLY'


ALTER TABLE Stretched_Test
	SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = OUTBOUND))

--unmigrate before changing schema

ALTER TABLE Stretched_Test
	SET (REMOTE_DATA_ARCHIVE = ON (MIGRATION_STATE = INBOUND))

```

can stretch only particular rows:
- a predicate funciton to filter what is to be mgrated
- filter by date, status or other constant
- expression must be deterministic
- can;t inherently implement a window function
- signature must not change when changing the function
- new function must be less restrictive than the current one

```SQL
CREATE FUNCTION dbo.fnStretchPredicate(@IsOld bit)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
	SELECT 1 AS is_eligible
	WHERE @IsOld = 1
GO

ALTER TABLE StretchTest
	SET (REMOTE_DATA_ARCHIVE = ON (
		FILTER_PREDICATE = dbo.fnStretchPredicate(IsOld),
		MIGRATION_STATE = OUTBOUND))
```
	
### unsupported table types
- memory optimized tables
- replicated tables
- FILESTREAM/FileTable
- tables enabled for change tracking or change data capture
- tables with more than 1023 columns or 998 indexes

### unsupported data types and column properties
- timestamp, sql_variant, xml, geography, geometry, hierarchyid, CLR UDTs, COLUMN_SET, computed columns

### unsupported indexes
- XML, full-text, spatial, indexes views into the table

### unsupported constraints
- check constraints
- default constraints
- foreign key constraints out of the table (parent tables)
- uniqueness not enforced UNIQUE constraints
- UPDATE and DELETE not supported on migrated rows
- ALTER TABLE not supported
- can't create index on the view of a migrated table
- can't update or delete from the view (insertion is supported)

## temporal tables

- create and ordinary table
	* primary key column
	* two period (start and end date) datetime2 columns
- enable the table for temporal
	* history table with the same schema, without constraints

SQL automatically records updates and deletes to the history table

### query point in time
```SQL
FOR SYSTEM_TIME AS OF -- in the select statement
```

ALTER TABLE automatically updates the history table (some changes need history to be turned off, make the changes and then reestablishing the history)

```SQL

CREATE TABLE Department (
	DepartmentId		int	NOT NULL	IDENTITY(1,1)	PRIMARY KEY,
	DepartmentName	varchar(50)	NOT NULL,
	ManagerId 			int NULL,
	ValidFrom				datetime2	GENERATED ALWAYS AS ROW START	NOT NULL,
	ValidTo					datetime2	GENERATED ALWAYS AS ROW END		NOT NULL,
	PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = DepartmentHistory))

```

querying a temporal table

```SQL
DECLARE @ThirtyDaysAgo datetime2 = DATEADD(d, -30, SYSDATETIME())

SELECT *
	FROM Department
	FOR SYSTEM_TIME AS OF @ThirtyDaysAgo
	ORDER BY DepartmentId
```

breaking the history

```SQL
ALTER TABLE DEPARTMENT SET (SYSTEM_VERSIONING = OFF)
```

PERIOD columns can be HIDDEN

### limitations

- triggers
	- INSTEAD OF triggers are not supported
	- AFTER triggers are supported on the current table only
- cascading updates and deletes are not supported
- in-memory OLTP (Hekaton) not supported
- FILESTREAM/FileTable not supported
- INSERT and UPDATE cannot explicitly reference perdiod columns (unless history is disabled temporarily)

## JSON support

adding `FOR JSON` at the end of queries would produce JSON results directly

`ISJSON`, `JSON_VALUE`, `JSON-QUERY` functions

`OPENJSON` will transfor JSON text to table

* `FOR JSON AUTO`
	* creates nested structure based on table hierarchy
* `FOR JSON PATH`
	* creates nested structure based on column aliases
* `WITHOUT_ARRAY_WRAPPER`
	* won't generate [] syntax
* `ROOT`
	* will generate a single root object wrapper around the results
* `INCLUDE_NULL_VALUES`
	* generates properties for NULL columns

* `ISJSON`
 	- validates well formed JSON
 	- use in check constraints for NVARCHAR columns containing JSON
* `JSON_QUERY`
	- queries by path expressions and returns a nested object/array
	- similar to _xml.query_
* `JSON_VALUE`
	- queries by path expression and returns a scalar value
	- similar to _xml.value_

```SQL
$ --entire json document
$.prop1.prop2[5].p3.p5 -- drills down into the json structure

SELECT
	Id,
	OrderNumber,
	OrderDate,
	JSON_VALUE(OrderDetails, '$.Order.ShipDate')
FROM
	SalesOrderRecord
WHERE
	ISJSON(OrderDetails) = 1 AND
	JSON_VALUE(OrderDetails, '$.Order.Type') = 'C'
	
	
CREATE TABLE OrdersJson(
	OrdersId int PRIMARY KEY,
	OrdersDoc nvarchar(max) NOT NULL DEFAULT '[]',
	CONSTRAINT [CK_OrdersJson_OrdersDoc] CHECK (ISJSON(OrdersDoc) = 1)
)
```

### querying JSON

```SQL
ALTER TABLE OrdersJson
	ADD BookCategory AS JSON_VALUE(BookDoc, '$.category')
	
CREATE INDEX IX_OrdersJson_BookCategory
	ON OrdersJson(BookCategory)
```

### shredding JSON

`OPENJSON` table-valued function (TVF) that shreds a JSON document into multiple rows by iterating through the objects (if array) or properties (if object) and generates a row for each object/property with key, value and type

```SQL
SELECT *
	FROM	OPENJSON(@BooksJson, '$') as b
	WHERE	JSON_VALUE(b.value, '$.category') = 'ITPro'
	ORDER BY JSON_VALUE(b.value, '$.author') DESC
	
-- can use 	OPENJSON(@BooksJson, '$') to shred a particular object from the array
```

property types:

- 0 = null
- 1 = string
- 2 = int
- 3 = bool
- 4 = array
- 5 = object

```SQL

SELECT *
 	FROM OPENJSON(@json, '$.Orders')
	WITH(
		OrderNumber	varchar(200),
		OrderDate		datetime,
		Customer		varchar(200)	'$.AccountNumber',
		Quantity		int						'$.Item.Quantity',
		Price				money					'$.Item.Price'
	)