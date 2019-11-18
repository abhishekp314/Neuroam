# Neuroam Scratch

### Things TODO
- [] Json interface to load/store data on disk
- [x] Dictionary of words in query (include words, words with special characters...)
- [x] Store the combination of id of words from dictionary
- [] Weighing the transactions
- [] Ranking the transactions
- [] Parallel search queries
- [] Caching fixed buffer in memory and seeking mechanism of partial data from disk

### Dictonary
A dictionary is collection of words with a unique id.

### Transactions
A transaction is something that could be stored/retrevied from a json structure.
### Attributes
* Datetime
* Ids of Query (referenced from Dictionary)
* Rank/Weight