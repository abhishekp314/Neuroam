# Neuroam Scratch

### Things TODO
- [x] Json interface to load/store data on disk
- [x] Dictionary of words in query (include words, words with special characters...)
- [x] Store the combination of id of words from dictionary
- [x] Periodic flush of dictionary to disk in case of changes
- [] Weighing the transactions
- [] Ranking the transactions
- [] Parallel search queries
- [] Caching fixed buffer in memory and seeking mechanism of partial data from disk

### Dictonary
A dictionary is collection of words with a unique id. It also maps partial matches and creates two-way binding based on String Contains()

### Queries
A query is itself a search/query. 
* Every query made is stored into the database, if it doesn't exist already. This also creates new partial matches between the words
* Find is triggered on the query made. This includes both full and partial matches.

### Transactions
A transaction is something that could be stored/retrevied from a json structure.
#### Attributes
* Datetime
* Ids of Query (referenced from Dictionary)
* Rank/Weight

### Versioning of Query data